﻿/*
 * This file is subject to the terms and conditions defined in
 * file 'license.txt', which is part of this source code package.
 */



using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SteamKit2.Discovery;

namespace SteamKit2.Internal
{
    /// <summary>
    /// This base client handles the underlying connection to a CM server. This class should not be use directly, but through the <see cref="SteamClient"/> class.
    /// </summary>
    public abstract class CMClient : ILogContext
    {
        /// <summary>
        /// The configuration for this client.
        /// </summary>
        public SteamConfiguration Configuration { get; }

        /// <summary>
        /// A unique identifier for this client instance.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Bootstrap list of CM servers.
        /// </summary>
        public SmartCMServerList Servers => Configuration.ServerList;

        /// <summary>
        /// Returns the local IP of this client.
        /// </summary>
        /// <returns>The local IP.</returns>
        public IPAddress? LocalIP => connection?.GetLocalIP();

        /// <summary>
        /// Returns the current endpoint this client is connected to.
        /// </summary>
        /// <returns>The current endpoint.</returns>
        public EndPoint? CurrentEndPoint => connection?.CurrentEndPoint;

        /// <summary>
        /// Gets the public IP address of this client. This value is assigned after a logon attempt has succeeded.
        /// This value will be <c>null</c> if the client is logged off of Steam.
        /// </summary>
        /// <value>The SteamID.</value>
        public IPAddress? PublicIP { get; private set; }

        /// <summary>
        /// Gets the country code of our public IP address according to Steam. This value is assigned after a logon attempt has succeeded.
        /// This value will be <c>null</c> if the client is logged off of Steam.
        /// </summary>
        /// <value>The SteamID.</value>
        public string? IPCountryCode { get; private set; }

        /// <summary>
        /// Gets the universe of this client.
        /// </summary>
        /// <value>The universe.</value>
        public EUniverse Universe => Configuration.Universe;

        /// <summary>
        /// Gets a value indicating whether this instance is connected to the remote CM server.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets the session token assigned to this client from the AM.
        /// </summary>
        public ulong SessionToken { get; private set; }

        /// <summary>
        /// Gets the Steam recommended Cell ID of this client. This value is assigned after a logon attempt has succeeded.
        /// This value will be <c>null</c> if the client is logged off of Steam.
        /// </summary>
        public uint? CellID { get; private set; }

        /// <summary>
        /// Gets the session ID of this client. This value is assigned after a logon attempt has succeeded.
        /// This value will be <c>null</c> if the client is logged off of Steam.
        /// </summary>
        /// <value>The session ID.</value>
        public int? SessionID { get; private set; }
        /// <summary>
        /// Gets the SteamID of this client. This value is assigned after a logon attempt has succeeded.
        /// This value will be <c>null</c> if the client is logged off of Steam.
        /// </summary>
        /// <value>The SteamID.</value>
        public SteamID? SteamID { get; private set; }

        /// <summary>
        /// Gets or sets the connection timeout used when connecting to the Steam server.
        /// </summary>
        /// <value>
        /// The connection timeout.
        /// </value>
        public TimeSpan ConnectionTimeout => Configuration.ConnectionTimeout;

        /// <summary>
        /// Gets or sets the network listening interface. Use this for debugging only.
        /// For your convenience, you can use <see cref="NetHookNetworkListener"/> class.
        /// </summary>
        public IDebugNetworkListener? DebugNetworkListener { get; set; }

        internal bool ExpectDisconnection { get; set; }

        // connection lock around the setup and tear down of the connection task
        object connectionLock = new();
        CancellationTokenSource? connectionCancellation;
        Task? connectionSetupTask;
        volatile IConnection? connection;

        ScheduledFunction heartBeatFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CMClient"/> class with a specific configuration.
        /// </summary>
        /// <param name="configuration">The configuration to use for this client.</param>
        /// <param name="identifier">A specific identifier to be used to uniquely identify this instance.</param>
        /// <exception cref="ArgumentNullException">The configuration object or identifier is <c>null</c></exception>
        /// <exception cref="ArgumentException">The identifier is an empty string</exception>
        public CMClient( SteamConfiguration configuration, string identifier )
        {
            Configuration = configuration ?? throw new ArgumentNullException( nameof( configuration ) );

            ArgumentNullException.ThrowIfNull( identifier );
            if ( identifier.Length == 0 ) throw new ArgumentException( "Identifer must not be empty.", nameof( identifier ) );

            ID = identifier;

            heartBeatFunc = new ScheduledFunction( () =>
            {
                Send( new ClientMsgProtobuf<CMsgClientHeartBeat>( EMsg.ClientHeartBeat ) );
            } );
        }

        /// <summary>
        /// Connects this client to a Steam3 server.
        /// This begins the process of connecting and encrypting the data channel between the client and the server.
        /// Results are returned asynchronously in a <see cref="SteamClient.ConnectedCallback"/>.
        /// If the server that SteamKit attempts to connect to is down, a <see cref="SteamClient.DisconnectedCallback"/>
        /// will be posted instead.
        /// SteamKit will not attempt to reconnect to Steam, you must handle this callback and call Connect again
        /// preferrably after a short delay.
        /// </summary>
        /// <param name="cmServer">
        /// The <see cref="IPEndPoint"/> of the CM server to connect to.
        /// If <c>null</c>, SteamKit will randomly select a CM server from its internal list.
        /// </param>
        public void Connect( ServerRecord? cmServer = null )
        {
            lock ( connectionLock )
            {
                Disconnect( userInitiated: true );
                DebugLog.Assert( connection == null, nameof( CMClient ), "Connection is not null" );
                DebugLog.Assert( connectionSetupTask == null, nameof( CMClient ), "Connection setup task is not null" );
                DebugLog.Assert( connectionCancellation == null, nameof( CMClient ), "Connection cancellation token is not null" );

                connectionCancellation = new CancellationTokenSource();
                var token = connectionCancellation.Token;

                ExpectDisconnection = false;

                Task<ServerRecord?> recordTask;

                if ( cmServer == null )
                {
                    recordTask = Servers.GetNextServerCandidateAsync( Configuration.ProtocolTypes );
                }
                else
                {
                    recordTask = Task.FromResult( ( ServerRecord? )cmServer );
                }

                connectionSetupTask = recordTask.ContinueWith( t =>
                {
                    if ( token.IsCancellationRequested )
                    {
                        LogDebug( nameof( CMClient ), "Connection cancelled before a server could be chosen." );
                        OnClientDisconnected( userInitiated: true );
                        return;
                    }
                    else if ( t.IsFaulted || t.IsCanceled )
                    {
                        LogDebug( nameof( CMClient ), "Server record task threw exception: {0}", t.Exception );
                        OnClientDisconnected( userInitiated: false );
                        return;
                    }

                    var record = t.Result;

                    if ( record is null )
                    {
                        LogDebug( nameof( CMClient ), "Server record task returned no result." );
                        OnClientDisconnected( userInitiated: false );
                        return;
                    }

                    var newConnection = CreateConnection( record.ProtocolTypes & Configuration.ProtocolTypes );

                    var connectionRelease = Interlocked.Exchange( ref connection, newConnection );
                    DebugLog.Assert( connectionRelease == null, nameof( CMClient ), "Connection was set during a connect, did you call CMClient.Connect() on multiple threads?" );

                    newConnection.NetMsgReceived += NetMsgReceived;
                    newConnection.Connected += Connected;
                    newConnection.Disconnected += Disconnected;
                    newConnection.Connect( record.EndPoint, ( int )ConnectionTimeout.TotalMilliseconds );
                }, TaskContinuationOptions.ExecuteSynchronously ).ContinueWith( t =>
                {
                    if ( t.IsFaulted )
                    {
                        LogDebug( nameof( CMClient ), "Unhandled exception when attempting to connect to Steam: {0}", t.Exception );
                        OnClientDisconnected( userInitiated: false );
                    }

                    connectionSetupTask = null;
                }, TaskContinuationOptions.ExecuteSynchronously );
            }
        }

        /// <summary>
        /// Disconnects this client.
        /// </summary>
        public void Disconnect() => Disconnect( userInitiated: true );

        private protected void Disconnect( bool userInitiated )
        {
            lock ( connectionLock )
            {
                heartBeatFunc.Stop();

                if ( connectionCancellation != null )
                {
                    connectionCancellation.Cancel();
                    connectionCancellation.Dispose();
                    connectionCancellation = null;
                }

                var connectionSetupTaskToWait = Interlocked.Exchange( ref connectionSetupTask, null );

                // though it's ugly, we want to wait for the completion of this task and keep hold of the lock
                connectionSetupTaskToWait?.GetAwaiter().GetResult();

                // Connection implementations are required to issue the Disconnected callback before Disconnect() returns
                connection?.Disconnect( userInitiated );
                DebugLog.Assert( connection == null, nameof( CMClient ), "Connection was not released in disconnect." );
            }
        }

        /// <summary>
        /// Sends the specified client message to the server.
        /// This method automatically assigns the correct SessionID and SteamID of the message.
        /// </summary>
        /// <param name="msg">The client message to send.</param>
        public void Send( IClientMsg msg )
        {
            if ( msg == null )
            {
                throw new ArgumentNullException( nameof( msg ), "A value for 'msg' must be supplied" );
            }

            DebugLog.Assert( IsConnected, nameof( CMClient ), "Send() was called while not connected to Steam." );

            var sessionID = this.SessionID;

            if ( sessionID.HasValue )
            {
                msg.SessionID = sessionID.Value;
            }

            var steamID = this.SteamID;

            if ( steamID != null )
            {
                msg.SteamID = steamID;
            }

            var serialized = msg.Serialize();

            try
            {
                DebugNetworkListener?.OnOutgoingNetworkMessage( msg.MsgType, serialized );
            }
            catch ( Exception e )
            {
                LogDebug( "CMClient", "DebugNetworkListener threw an exception: {0}", e );
            }

            // we'll swallow any network failures here because they will be thrown later
            // on the network thread, and that will lead to a disconnect callback
            // down the line

            try
            {
                connection?.Send( serialized );
            }
            catch ( IOException )
            {
            }
            catch ( SocketException )
            {
            }
        }

        /// <summary>
        /// Writes a line to the debug log, informing all listeners.
        /// </summary>
        /// <param name="category">The category of the message.</param>
        /// <param name="message">A composite format string.</param>
        /// <param name="args">An array containing zero or more objects to format.</param>
        public void LogDebug( string category, string message, params object?[]? args )
        {
            if ( !DebugLog.Enabled )
            {
                return;
            }

            var fullCategory = string.Concat( ID, "/", category );
            DebugLog.WriteLine( fullCategory, message, args );
        }


        /// <summary>
        /// Called when a client message is received from the network.
        /// </summary>
        /// <param name="packetMsg">The packet message.</param>
        protected virtual bool OnClientMsgReceived( [NotNullWhen( true )] IPacketMsg? packetMsg )
        {
            if ( packetMsg == null )
            {
                LogDebug( "CMClient", "Packet message failed to parse, shutting down connection" );
                Disconnect( userInitiated: false );
                return false;
            }

            // Multi message gets logged down the line after it's decompressed
            if ( packetMsg.MsgType != EMsg.Multi )
            {
                try
                {
                    DebugNetworkListener?.OnIncomingNetworkMessage( packetMsg.MsgType, packetMsg.GetData() );
                }
                catch ( Exception e )
                {
                    LogDebug( "CMClient", "DebugNetworkListener threw an exception: {0}", e );
                }
            }

            switch ( packetMsg.MsgType )
            {
                case EMsg.Multi:
                    HandleMulti( packetMsg );
                    break;

                case EMsg.ClientLogOnResponse: // we handle this to get the SteamID/SessionID and to setup heartbeating
                    HandleLogOnResponse( packetMsg );
                    break;

                case EMsg.ClientLoggedOff: // to stop heartbeating when we get logged off
                    HandleLoggedOff( packetMsg );
                    break;

                case EMsg.ClientServerUnavailable:
                    HandleServerUnavailable( packetMsg );
                    break;

                case EMsg.ClientSessionToken: // am session token
                    HandleSessionToken( packetMsg );
                    break;
            }

            return true;
        }
        /// <summary>
        /// Called when the client is securely connected to Steam3.
        /// </summary>
        protected virtual void OnClientConnected()
        {
            var request = new ClientMsgProtobuf<CMsgClientHello>( EMsg.ClientHello );
            request.Body.protocol_version = MsgClientLogon.CurrentProtocol;

            Send( request );
        }
        /// <summary>
        /// Called when the client is physically disconnected from Steam3.
        /// </summary>
        protected virtual void OnClientDisconnected( bool userInitiated )
        {
        }

        IConnection CreateConnection( ProtocolTypes protocol )
        {
            if ( protocol.HasFlagsFast( ProtocolTypes.WebSocket ) )
            {
                return new WebSocketConnection( this, Configuration.HttpClientFactory( HttpClientPurpose.CMWebSocket ) );
            }
            else if ( protocol.HasFlagsFast( ProtocolTypes.Tcp ) )
            {
                return new EnvelopeEncryptedConnection( new TcpConnection( this ), Universe, this, DebugNetworkListener );
            }
            else if ( protocol.HasFlagsFast( ProtocolTypes.Udp ) )
            {
                return new EnvelopeEncryptedConnection( new UdpConnection( this ), Universe, this, DebugNetworkListener );
            }

            throw new ArgumentOutOfRangeException( nameof( protocol ), protocol, "Protocol bitmask has no supported protocols set." );
        }


        void NetMsgReceived( object? sender, NetMsgEventArgs e )
        {
            OnClientMsgReceived( GetPacketMsg( e.Data, this ) );
        }

#if DEBUG
        internal void ReceiveTestPacketMsg( IPacketMsg packetMsg ) => OnClientMsgReceived( packetMsg );
        internal void SetIsConnected( bool value ) => IsConnected = value;
#endif

        void Connected( object? sender, EventArgs e )
        {
            DebugLog.Assert( connection != null, nameof( CMClient ), "No connection object after connecting." );
            DebugLog.Assert( connection.CurrentEndPoint != null, nameof( CMClient ), "No connection endpoint after connecting - cannot update server list" );

            Servers.TryMark( connection.CurrentEndPoint, connection.ProtocolTypes, ServerQuality.Good );

            IsConnected = true;

            try
            {
                OnClientConnected();
            }
            catch ( Exception ex )
            {
                DebugLog.WriteLine( nameof(CMClient), "Unhandled exception after connecting: {0}", ex );
                Disconnect(userInitiated: false);
            }
        }

        void Disconnected( object? sender, DisconnectedEventArgs e )
        {
            var connectionRelease = Interlocked.Exchange( ref connection, null );
            if ( connectionRelease == null )
            {
                return;
            }

            IsConnected = false;

            if ( !e.UserInitiated && !ExpectDisconnection )
            {
                DebugLog.Assert( connectionRelease.CurrentEndPoint != null, nameof( CMClient ), "No connection endpoint while disconnecting - cannot update server list" );
                Servers.TryMark( connectionRelease.CurrentEndPoint!, connectionRelease.ProtocolTypes, ServerQuality.Bad );
            }

            SessionID = null;
            SteamID = null;

            connectionRelease.NetMsgReceived -= NetMsgReceived;
            connectionRelease.Connected -= Connected;
            connectionRelease.Disconnected -= Disconnected;

            if ( connectionRelease is IDisposable disposableConnection )
            {
                disposableConnection.Dispose();
            }

            heartBeatFunc.Stop();

            OnClientDisconnected( userInitiated: e.UserInitiated || ExpectDisconnection );
        }

        internal static IPacketMsg? GetPacketMsg( byte[] data, ILogContext log )
        {
            if ( data.Length < sizeof( uint ) )
            {
                log.LogDebug( nameof( CMClient ), "PacketMsg too small to contain a message, was only {0} bytes. Message: 0x{1}", data.Length, Utils.EncodeHexString( data ) );
                return null;
            }

            uint rawEMsg = BitConverter.ToUInt32( data, 0 );
            EMsg eMsg = MsgUtil.GetMsg( rawEMsg );

            switch ( eMsg )
            {
                // certain message types are always MsgHdr
                case EMsg.ChannelEncryptRequest:
                case EMsg.ChannelEncryptResponse:
                case EMsg.ChannelEncryptResult:
                    return new PacketMsg( eMsg, data );
            }

            try
            {
                if ( MsgUtil.IsProtoBuf( rawEMsg ) )
                {
                    // if the emsg is flagged, we're a proto message
                    return new PacketClientMsgProtobuf( eMsg, data );
                }
                else
                {
                    // otherwise we're a struct message
                    return new PacketClientMsg( eMsg, data );
                }
            }
            catch ( Exception ex )
            {
                log.LogDebug( "CMClient", "Exception deserializing emsg {0} ({1}).\n{2}", eMsg, MsgUtil.IsProtoBuf( rawEMsg ), ex.ToString() );
                return null;
            }
        }


        #region ClientMsg Handlers
        void HandleMulti( IPacketMsg packetMsg )
        {
            if ( !packetMsg.IsProto )
            {
                LogDebug( "CMClient", "HandleMulti got non-proto MsgMulti!!" );
                return;
            }

            var msgMulti = new ClientMsgProtobuf<CMsgMulti>( packetMsg );
            using var payloadStream = new MemoryStream( msgMulti.Body.message_body );
            Stream stream = payloadStream;

            if ( msgMulti.Body.size_unzipped > 0 )
            {
                stream = new GZipStream( payloadStream, CompressionMode.Decompress );
            }

            using ( stream )
            {
                Span<byte> length = stackalloc byte[ sizeof( int ) ];

                while ( stream.ReadAll( length ) > 0 )
                {
                    var subSize = BitConverter.ToInt32( length );
                    var subData = new byte[ subSize ];

                    stream.ReadAll( subData );

                    if ( !OnClientMsgReceived( GetPacketMsg( subData, this ) ) )
                    {
                        break;
                    }
                }
            }
        }

        void HandleLogOnResponse( IPacketMsg packetMsg )
        {
            if ( !packetMsg.IsProto )
            {
                // a non proto ClientLogonResponse can come in as a result of connecting but never sending a ClientLogon
                // in this case, it always fails, so we don't need to do anything special here
                LogDebug( "CMClient", "Got non-proto logon response, this is indicative of no logon attempt after connecting." );
                return;
            }

            var logonResp = new ClientMsgProtobuf<CMsgClientLogonResponse>( packetMsg );
            var logonResult = ( EResult )logonResp.Body.eresult;

            if ( logonResult == EResult.OK )
            {
                SessionID = logonResp.ProtoHeader.client_sessionid;
                SteamID = logonResp.ProtoHeader.steamid;

                CellID = logonResp.Body.cell_id;
                PublicIP = logonResp.Body.public_ip.GetIPAddress();
                IPCountryCode = logonResp.Body.ip_country_code;

                int hbDelay = logonResp.Body.legacy_out_of_game_heartbeat_seconds;

                // restart heartbeat
                heartBeatFunc.Stop();
                heartBeatFunc.Delay = TimeSpan.FromSeconds( hbDelay );
                heartBeatFunc.Start();
            }
            else if ( logonResult == EResult.TryAnotherCM || logonResult == EResult.ServiceUnavailable )
            {
                if ( connection?.CurrentEndPoint != null )
                {
                    Servers.TryMark( connection.CurrentEndPoint, connection.ProtocolTypes, ServerQuality.Bad );
                }
            }
        }
        void HandleLoggedOff( IPacketMsg packetMsg )
        {
            SessionID = null;
            SteamID = null;

            CellID = null;
            PublicIP = null;
            IPCountryCode = null;

            heartBeatFunc.Stop();

            if ( packetMsg.IsProto )
            {
                var logoffMsg = new ClientMsgProtobuf<CMsgClientLoggedOff>( packetMsg );
                var logoffResult = ( EResult )logoffMsg.Body.eresult;

                if ( logoffResult == EResult.TryAnotherCM || logoffResult == EResult.ServiceUnavailable )
                {
                    DebugLog.Assert( connection != null, nameof( CMClient ), "No connection object during ClientLoggedOff." );
                    DebugLog.Assert( connection.CurrentEndPoint != null, nameof( CMClient ), "No connection endpoint during ClientLoggedOff - cannot update server list status" );
                    Servers.TryMark( connection.CurrentEndPoint, connection.ProtocolTypes, ServerQuality.Bad );
                }
            }
        }

        void HandleServerUnavailable( IPacketMsg packetMsg )
        {
            var msgServerUnavailable = new ClientMsg<MsgClientServerUnavailable>( packetMsg );

            LogDebug( "SteamClient", "A server of type '{0}' was not available for request: '{1}'",
                msgServerUnavailable.Body.EServerTypeUnavailable, ( EMsg )msgServerUnavailable.Body.EMsgSent );
            Disconnect( userInitiated: false );
        }

        void HandleSessionToken( IPacketMsg packetMsg )
        {
            var sessToken = new ClientMsgProtobuf<CMsgClientSessionToken>( packetMsg );

            SessionToken = sessToken.Body.token;
        }
        #endregion
    }
}
