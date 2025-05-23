// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: gcsystemmsgs.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace SteamKit2.GC.Deadlock.Internal
{

    [global::ProtoBuf.ProtoContract()]
    public enum ESOMsg
    {
        k_ESOMsg_Create = 21,
        k_ESOMsg_Update = 22,
        k_ESOMsg_Destroy = 23,
        k_ESOMsg_CacheSubscribed = 24,
        k_ESOMsg_CacheUnsubscribed = 25,
        k_ESOMsg_UpdateMultiple = 26,
        k_ESOMsg_CacheSubscriptionRefresh = 28,
        k_ESOMsg_CacheSubscribedUpToDate = 29,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum EGCBaseClientMsg
    {
        k_EMsgGCPingRequest = 3001,
        k_EMsgGCPingResponse = 3002,
        k_EMsgGCToClientPollConvarRequest = 3003,
        k_EMsgGCToClientPollConvarResponse = 3004,
        k_EMsgGCCompressedMsgToClient = 3005,
        k_EMsgGCCompressedMsgToClient_Legacy = 523,
        k_EMsgGCToClientRequestDropped = 3006,
        k_EMsgGCClientWelcome = 4004,
        k_EMsgGCServerWelcome = 4005,
        k_EMsgGCClientHello = 4006,
        k_EMsgGCServerHello = 4007,
        k_EMsgGCClientConnectionStatus = 4009,
        k_EMsgGCServerConnectionStatus = 4010,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
