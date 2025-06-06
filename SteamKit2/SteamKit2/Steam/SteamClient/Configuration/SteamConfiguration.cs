﻿/*
 * This file is subject to the terms and conditions defined in
 * file 'license.txt', which is part of this source code package.
 */


using System;
using System.Net.Http;
using SteamKit2.Discovery;

namespace SteamKit2
{
    /// <summary>
    /// Specifies the intended purpose or target for an HttpClient to enable appropriate configuration.
    /// </summary>
    public enum HttpClientPurpose
    {
        /// <summary>
        /// HttpClient configured for WebSocket communication with CM services.
        /// </summary>
        /// <see cref="WebSocketConnection"/>
        CMWebSocket,
        /// <summary>
        /// HttpClient configured for interacting with the Steam Web API.
        /// </summary>
        /// <see cref="SteamKit2.WebAPI"/>
        WebAPI,
        /// <summary>
        /// HttpClient configured for downloading game content from the Steam servers.
        /// </summary>
        /// <see cref="CDN.Client"/>
        CDN,
    }

    /// <summary>
    /// Factory function to create a user-configured HttpClient based on its intended purpose.
    /// The HttpClient will be disposed of after use.
    /// </summary>
    /// <param name="purpose">The purpose for which the HttpClient will be used.</param>
    /// <returns>A new <see cref="HttpClient"/> to be used to send HTTP requests.</returns>
    public delegate HttpClient HttpClientFactory( HttpClientPurpose purpose );

    /// <summary>
    /// Configuration object to use.
    /// This object should not be mutated after it is passed to one or more <see cref="SteamClient"/> objects.
    /// </summary>
    public sealed class SteamConfiguration
    {
        /// <summary>
        /// Do not use directly - create a SteamConfiguration object by using a builder or helper method.
        /// </summary>
        internal SteamConfiguration(SteamConfigurationState state)
        {
            this.state = state;
            ServerList = new SmartCMServerList(this);
        }

        /// <summary>
        /// Creates a <see cref="SteamConfiguration" />, allowing for configuration.
        /// </summary>
        /// <param name="configurator">A method which is used to configure the configuration.</param>
        /// <returns>A configuration object.</returns>
        public static SteamConfiguration Create(Action<ISteamConfigurationBuilder> configurator)
        {
            ArgumentNullException.ThrowIfNull( configurator );

            var builder = new SteamConfigurationBuilder();
            configurator(builder);
            return builder.Build();
        }

        internal static SteamConfiguration CreateDefault()
            => new(SteamConfigurationBuilder.CreateDefaultState());

        readonly SteamConfigurationState state;

        /// <summary>
        /// Whether or not to use the Steam Directory to discover available servers.
        /// </summary>
        public bool AllowDirectoryFetch => state.AllowDirectoryFetch;

        /// <summary>
        /// The Steam Cell ID to prioritize when connecting.
        /// </summary>
        public uint CellID => state.CellID;

        /// <summary>
        /// The connection timeout used when connecting to Steam serves.
        /// </summary>
        public TimeSpan ConnectionTimeout => state.ConnectionTimeout;

        /// <summary>
        /// The default persona state flags used when requesting information for a new friend, or
        /// when calling <c>SteamFriends.RequestFriendInfo</c> without specifying flags.
        /// </summary>
        public EClientPersonaStateFlag DefaultPersonaStateFlags => state.DefaultPersonaStateFlags;

        /// <summary>
        /// Factory function to create a user-configured HttpClient.
        /// </summary>
        public HttpClientFactory HttpClientFactory => state.HttpClientFactory;

        /// <summary>
        /// The machine info provider to be used during the login process. This can be substituted with a custom implementation
        /// when running SteamKit2 in new and novel environments that do not have an appropriate default implementation.
        /// </summary>
        public IMachineInfoProvider MachineInfoProvider => state.MachineInfoProvider;

        /// <summary>
        /// The supported protocol types to use when attempting to connect to Steam.
        /// </summary>
        public ProtocolTypes ProtocolTypes => state.ProtocolTypes;

        /// <summary>
        /// The server list provider to use.
        /// </summary>
        public IServerListProvider ServerListProvider => state.ServerListProvider;

        /// <summary>
        /// The Universe to connect to. This should always be <see cref="EUniverse.Public"/> unless
        /// you work at Valve and are using this internally. If this is you, hello there.
        /// </summary>
        public EUniverse Universe => state.Universe;

        /// <summary>
        /// The base address of the Steam Web API to connect to.
        /// Use of "partner.steam-api.com" requires a Partner API key.
        /// </summary>
        public Uri WebAPIBaseAddress => state.WebAPIBaseAddress;

        /// <summary>
        /// An  API key to be used for authorized requests.
        /// Keys can be obtained from https://steamcommunity.com/dev or the Steamworks Partner site.
        /// </summary>
        public string WebAPIKey => state.WebAPIKey;

        /// <summary>
        /// The server list used for this configuration.
        /// If this configuration is used by multiple <see cref="SteamClient"/> instances, they all share the server list.
        /// </summary>
        public SmartCMServerList ServerList { get; }
    }
}
