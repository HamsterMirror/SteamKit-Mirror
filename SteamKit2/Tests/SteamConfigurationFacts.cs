﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SteamKit2;
using SteamKit2.Discovery;
using Xunit;

namespace Tests
{
    public class SteamConfigurationDefaultFacts
    {
        public SteamConfigurationDefaultFacts()
        {
            configuration = SteamConfiguration.Create(_ => { });
        }

        readonly SteamConfiguration configuration;

        [Fact]
        public void AllowsDirectoryFetch()
        {
            Assert.True(configuration.AllowDirectoryFetch);
        }

        [Fact]
        public void CellIDIsZero()
        {
            Assert.Equal(0u, configuration.CellID);
        }

        [Fact]
        public void ConnectionTimeoutIsFiveSeconds()
        {
            Assert.Equal(TimeSpan.FromSeconds(5), configuration.ConnectionTimeout);
        }

        [Fact]
        public void DefaultPersonaStateFlags()
        {
            var expected = EClientPersonaStateFlag.PlayerName | EClientPersonaStateFlag.Presence |
                    EClientPersonaStateFlag.SourceID | EClientPersonaStateFlag.GameExtraInfo |
                    EClientPersonaStateFlag.LastSeen;

            Assert.Equal(expected, configuration.DefaultPersonaStateFlags);
        }

        [Fact]
        public void DefaultHttpClientFactory()
        {
            using var client = configuration.HttpClientFactory( HttpClientPurpose.WebAPI );
            Assert.NotNull( client );
            Assert.IsType<HttpClient>( client );

            var steamKitAssemblyVersion = typeof( SteamClient ).Assembly.GetName().Version;
            Assert.Equal( "SteamKit/" + steamKitAssemblyVersion.ToString( fieldCount: 3 ), client.DefaultRequestHeaders.UserAgent.ToString() );
        }

#if DEBUG
        [Fact]
        public void DefaultMachineInfoProvider()
        {
            Assert.NotNull(configuration.MachineInfoProvider);
            Assert.IsNotType<DefaultMachineInfoProvider>(configuration.MachineInfoProvider);
        }
#endif

        [Fact]
        public void ServerListProviderIsNothingFancy()
        {
            Assert.IsType<MemoryServerListProvider>(configuration.ServerListProvider);
        }

        [Fact]
        public void ServerListIsNotNull()
        {
            Assert.NotNull(configuration.ServerList);
        }

        [Fact]
        public void DefaultProtocols()
        {
            Assert.Equal(ProtocolTypes.Tcp | ProtocolTypes.WebSocket, configuration.ProtocolTypes);
        }

        [Fact]
        public void PublicUniverse()
        {
            Assert.Equal(EUniverse.Public, configuration.Universe);
        }

        [Fact]
        public void WebAPIAddress()
        {
            Assert.Equal("https://api.steampowered.com/", configuration.WebAPIBaseAddress?.AbsoluteUri);
        }

        [Fact]
        public void NoWebAPIKey()
        {
            Assert.Null(configuration.WebAPIKey);
        }
    }

    public class SteamConfigurationConfiguredObjectFacts
    {
        public SteamConfigurationConfiguredObjectFacts()
        {
            configuration = SteamConfiguration.Create(b =>
                b.WithDirectoryFetch(false)
                 .WithCellID(123)
                 .WithConnectionTimeout(TimeSpan.FromMinutes(1))
                 .WithDefaultPersonaStateFlags(EClientPersonaStateFlag.SourceID)
                 .WithHttpClientFactory((purpose) => { var c = new HttpClient(); c.DefaultRequestHeaders.Add("X-SteamKit-Tests", "true"); return c; })
                 .WithMachineInfoProvider(new CustomMachineInfoProvider())
                 .WithProtocolTypes(ProtocolTypes.WebSocket | ProtocolTypes.Udp)
                 .WithServerListProvider(new CustomServerListProvider())
                 .WithUniverse(EUniverse.Internal)
                 .WithWebAPIBaseAddress(new Uri("http://foo.bar.com/api/"))
                 .WithWebAPIKey("T0PS3kR1t"));
        }

        readonly SteamConfiguration configuration;

        [Fact]
        public void DirectoryFetchIsConfigured()
        {
            Assert.False(configuration.AllowDirectoryFetch);
        }

        [Fact]
        public void CellIDIsConfigured()
        {
            Assert.Equal(123u, configuration.CellID);
        }

        [Fact]
        public void ConnectionTimeoutIsConfigured()
        {
            Assert.Equal(TimeSpan.FromMinutes(1), configuration.ConnectionTimeout);
        }

        [Fact]
        public void HttpClientFactoryIsConfigured()
        {
            using var client = configuration.HttpClientFactory( HttpClientPurpose.WebAPI );
            Assert.Equal( "true", client.DefaultRequestHeaders.GetValues( "X-SteamKit-Tests" ).FirstOrDefault() );
        }

        [Fact]
        public void MachineInfoProviderIsConfigured()
        {
            Assert.IsType<CustomMachineInfoProvider>(configuration.MachineInfoProvider);
            Assert.Same(configuration.MachineInfoProvider, configuration.MachineInfoProvider);
        }

        [Fact]
        public void PersonaStateFlagsIsConfigured()
        {
            Assert.Equal(EClientPersonaStateFlag.SourceID, configuration.DefaultPersonaStateFlags);
        }

        [Fact]
        public void ServerListProviderIsConfigured()
        {
            Assert.IsType<CustomServerListProvider>(configuration.ServerListProvider);
        }

        [Fact]
        public void ServerListIsNotNull()
        {
            Assert.NotNull(configuration.ServerList);
        }

        [Fact]
        public void ProtocolsAreConfigured()
        {
            Assert.Equal(ProtocolTypes.WebSocket | ProtocolTypes.Udp, configuration.ProtocolTypes);
        }

        [Fact]
        public void UniverseIsConfigured()
        {
            Assert.Equal(EUniverse.Internal, configuration.Universe);
        }

        [Fact]
        public void WebAPIAddress()
        {
            Assert.Equal("http://foo.bar.com/api/", configuration.WebAPIBaseAddress?.AbsoluteUri);
        }

        [Fact]
        public void NoWebAPIKey()
        {
            Assert.Equal("T0PS3kR1t", configuration.WebAPIKey);
        }

        class CustomMachineInfoProvider : IMachineInfoProvider
        {
            byte[] IMachineInfoProvider.GetDiskId()
                => throw new NotImplementedException();
            byte[] IMachineInfoProvider.GetMacAddress()
                => throw new NotImplementedException();
            byte[] IMachineInfoProvider.GetMachineGuid()
                => throw new NotImplementedException();
        }

        class CustomServerListProvider : IServerListProvider
        {
            public DateTime LastServerListRefresh => throw new NotImplementedException();

            Task<IEnumerable<ServerRecord>> IServerListProvider.FetchServerListAsync()
                => throw new NotImplementedException();

            Task IServerListProvider.UpdateServerListAsync(IEnumerable<ServerRecord> endpoints)
                => throw new NotImplementedException();
        }
    }
}
