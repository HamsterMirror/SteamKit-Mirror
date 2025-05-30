// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: econ_gcmessages.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace SteamKit2.GC.CSGO.Internal
{

    [global::ProtoBuf.ProtoContract()]
    public partial class CMsgGCGiftedItems : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint accountid
        {
            get => __pbn__accountid.GetValueOrDefault();
            set => __pbn__accountid = value;
        }
        public bool ShouldSerializeaccountid() => __pbn__accountid != null;
        public void Resetaccountid() => __pbn__accountid = null;
        private uint? __pbn__accountid;

        [global::ProtoBuf.ProtoMember(2)]
        public uint giftdefindex
        {
            get => __pbn__giftdefindex.GetValueOrDefault();
            set => __pbn__giftdefindex = value;
        }
        public bool ShouldSerializegiftdefindex() => __pbn__giftdefindex != null;
        public void Resetgiftdefindex() => __pbn__giftdefindex = null;
        private uint? __pbn__giftdefindex;

        [global::ProtoBuf.ProtoMember(3)]
        public uint max_gifts_possible
        {
            get => __pbn__max_gifts_possible.GetValueOrDefault();
            set => __pbn__max_gifts_possible = value;
        }
        public bool ShouldSerializemax_gifts_possible() => __pbn__max_gifts_possible != null;
        public void Resetmax_gifts_possible() => __pbn__max_gifts_possible = null;
        private uint? __pbn__max_gifts_possible;

        [global::ProtoBuf.ProtoMember(4)]
        public uint num_eligible_recipients
        {
            get => __pbn__num_eligible_recipients.GetValueOrDefault();
            set => __pbn__num_eligible_recipients = value;
        }
        public bool ShouldSerializenum_eligible_recipients() => __pbn__num_eligible_recipients != null;
        public void Resetnum_eligible_recipients() => __pbn__num_eligible_recipients = null;
        private uint? __pbn__num_eligible_recipients;

        [global::ProtoBuf.ProtoMember(5)]
        public global::System.Collections.Generic.List<uint> recipients_accountids { get; } = new global::System.Collections.Generic.List<uint>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CMsgGCDev_SchemaReservationRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string schema_typename
        {
            get => __pbn__schema_typename ?? "";
            set => __pbn__schema_typename = value;
        }
        public bool ShouldSerializeschema_typename() => __pbn__schema_typename != null;
        public void Resetschema_typename() => __pbn__schema_typename = null;
        private string __pbn__schema_typename;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string instance_name
        {
            get => __pbn__instance_name ?? "";
            set => __pbn__instance_name = value;
        }
        public bool ShouldSerializeinstance_name() => __pbn__instance_name != null;
        public void Resetinstance_name() => __pbn__instance_name = null;
        private string __pbn__instance_name;

        [global::ProtoBuf.ProtoMember(3)]
        public ulong context
        {
            get => __pbn__context.GetValueOrDefault();
            set => __pbn__context = value;
        }
        public bool ShouldSerializecontext() => __pbn__context != null;
        public void Resetcontext() => __pbn__context = null;
        private ulong? __pbn__context;

        [global::ProtoBuf.ProtoMember(4)]
        public ulong id
        {
            get => __pbn__id.GetValueOrDefault();
            set => __pbn__id = value;
        }
        public bool ShouldSerializeid() => __pbn__id != null;
        public void Resetid() => __pbn__id = null;
        private ulong? __pbn__id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CMsgCasketItem : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong casket_item_id
        {
            get => __pbn__casket_item_id.GetValueOrDefault();
            set => __pbn__casket_item_id = value;
        }
        public bool ShouldSerializecasket_item_id() => __pbn__casket_item_id != null;
        public void Resetcasket_item_id() => __pbn__casket_item_id = null;
        private ulong? __pbn__casket_item_id;

        [global::ProtoBuf.ProtoMember(2)]
        public ulong item_item_id
        {
            get => __pbn__item_item_id.GetValueOrDefault();
            set => __pbn__item_item_id = value;
        }
        public bool ShouldSerializeitem_item_id() => __pbn__item_item_id != null;
        public void Resetitem_item_id() => __pbn__item_item_id = null;
        private ulong? __pbn__item_item_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CMsgGCUserTrackTimePlayedConsecutively : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint state
        {
            get => __pbn__state.GetValueOrDefault();
            set => __pbn__state = value;
        }
        public bool ShouldSerializestate() => __pbn__state != null;
        public void Resetstate() => __pbn__state = null;
        private uint? __pbn__state;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CMsgGCItemCustomizationNotification : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<ulong> item_id { get; } = new global::System.Collections.Generic.List<ulong>();

        [global::ProtoBuf.ProtoMember(2)]
        public uint request
        {
            get => __pbn__request.GetValueOrDefault();
            set => __pbn__request = value;
        }
        public bool ShouldSerializerequest() => __pbn__request != null;
        public void Resetrequest() => __pbn__request = null;
        private uint? __pbn__request;

        [global::ProtoBuf.ProtoMember(3)]
        public global::System.Collections.Generic.List<ulong> extra_data { get; } = new global::System.Collections.Generic.List<ulong>();

    }

    [global::ProtoBuf.ProtoContract()]
    public enum EGCItemMsg
    {
        k_EMsgGCBase = 1000,
        k_EMsgGCSetItemPosition = 1001,
        k_EMsgGCCraft = 1002,
        k_EMsgGCCraftResponse = 1003,
        k_EMsgGCDelete = 1004,
        k_EMsgGCVerifyCacheSubscription = 1005,
        k_EMsgGCNameItem = 1006,
        k_EMsgGCUnlockCrate = 1007,
        k_EMsgGCUnlockCrateResponse = 1008,
        k_EMsgGCPaintItem = 1009,
        k_EMsgGCPaintItemResponse = 1010,
        k_EMsgGCGoldenWrenchBroadcast = 1011,
        k_EMsgGCMOTDRequest = 1012,
        k_EMsgGCMOTDRequestResponse = 1013,
        k_EMsgGCAddItemToSocket_DEPRECATED = 1014,
        k_EMsgGCAddItemToSocketResponse_DEPRECATED = 1015,
        k_EMsgGCAddSocketToBaseItem_DEPRECATED = 1016,
        k_EMsgGCAddSocketToItem_DEPRECATED = 1017,
        k_EMsgGCAddSocketToItemResponse_DEPRECATED = 1018,
        k_EMsgGCNameBaseItem = 1019,
        k_EMsgGCNameBaseItemResponse = 1020,
        k_EMsgGCRemoveSocketItem_DEPRECATED = 1021,
        k_EMsgGCRemoveSocketItemResponse_DEPRECATED = 1022,
        k_EMsgGCCustomizeItemTexture = 1023,
        k_EMsgGCCustomizeItemTextureResponse = 1024,
        k_EMsgGCUseItemRequest = 1025,
        k_EMsgGCUseItemResponse = 1026,
        k_EMsgGCGiftedItems_DEPRECATED = 1027,
        k_EMsgGCRemoveItemName = 1030,
        k_EMsgGCRemoveItemPaint = 1031,
        k_EMsgGCGiftWrapItem = 1032,
        k_EMsgGCGiftWrapItemResponse = 1033,
        k_EMsgGCDeliverGift = 1034,
        k_EMsgGCDeliverGiftResponseGiver = 1035,
        k_EMsgGCDeliverGiftResponseReceiver = 1036,
        k_EMsgGCUnwrapGiftRequest = 1037,
        k_EMsgGCUnwrapGiftResponse = 1038,
        k_EMsgGCSetItemStyle = 1039,
        k_EMsgGCUsedClaimCodeItem = 1040,
        k_EMsgGCSortItems = 1041,
        k_EMsgGC_RevolvingLootList_DEPRECATED = 1042,
        k_EMsgGCLookupAccount = 1043,
        k_EMsgGCLookupAccountResponse = 1044,
        k_EMsgGCLookupAccountName = 1045,
        k_EMsgGCLookupAccountNameResponse = 1046,
        k_EMsgGCUpdateItemSchema = 1049,
        k_EMsgGCRemoveCustomTexture = 1051,
        k_EMsgGCRemoveCustomTextureResponse = 1052,
        k_EMsgGCRemoveMakersMark = 1053,
        k_EMsgGCRemoveMakersMarkResponse = 1054,
        k_EMsgGCRemoveUniqueCraftIndex = 1055,
        k_EMsgGCRemoveUniqueCraftIndexResponse = 1056,
        k_EMsgGCSaxxyBroadcast = 1057,
        k_EMsgGCBackpackSortFinished = 1058,
        k_EMsgGCCollectItem = 1061,
        k_EMsgGCItemAcknowledged__DEPRECATED = 1062,
        k_EMsgGC_ReportAbuse = 1065,
        k_EMsgGC_ReportAbuseResponse = 1066,
        k_EMsgGCNameItemNotification = 1068,
        k_EMsgGCApplyConsumableEffects = 1069,
        k_EMsgGCConsumableExhausted = 1070,
        k_EMsgGCShowItemsPickedUp = 1071,
        k_EMsgGCClientDisplayNotification = 1072,
        k_EMsgGCApplyStrangePart = 1073,
        k_EMsgGC_IncrementKillCountAttribute = 1074,
        k_EMsgGC_IncrementKillCountResponse = 1075,
        k_EMsgGCApplyPennantUpgrade = 1076,
        k_EMsgGCSetItemPositions = 1077,
        k_EMsgGCApplyEggEssence = 1078,
        k_EMsgGCNameEggEssenceResponse = 1079,
        k_EMsgGCPaintKitItem = 1080,
        k_EMsgGCPaintKitBaseItem = 1081,
        k_EMsgGCPaintKitItemResponse = 1082,
        k_EMsgGCGiftedItems = 1083,
        k_EMsgGCUnlockItemStyle = 1084,
        k_EMsgGCUnlockItemStyleResponse = 1085,
        k_EMsgGCApplySticker = 1086,
        k_EMsgGCItemAcknowledged = 1087,
        k_EMsgGCStatTrakSwap = 1088,
        k_EMsgGCUserTrackTimePlayedConsecutively = 1089,
        k_EMsgGCItemCustomizationNotification = 1090,
        k_EMsgGCModifyItemAttribute = 1091,
        k_EMsgGCCasketItemAdd = 1092,
        k_EMsgGCCasketItemExtract = 1093,
        k_EMsgGCCasketItemLoadContents = 1094,
        k_EMsgGCTradingBase = 1500,
        k_EMsgGCTrading_InitiateTradeRequest = 1501,
        k_EMsgGCTrading_InitiateTradeResponse = 1502,
        k_EMsgGCTrading_StartSession = 1503,
        k_EMsgGCTrading_SetItem = 1504,
        k_EMsgGCTrading_RemoveItem = 1505,
        k_EMsgGCTrading_UpdateTradeInfo = 1506,
        k_EMsgGCTrading_SetReadiness = 1507,
        k_EMsgGCTrading_ReadinessResponse = 1508,
        k_EMsgGCTrading_SessionClosed = 1509,
        k_EMsgGCTrading_CancelSession = 1510,
        k_EMsgGCTrading_TradeChatMsg = 1511,
        k_EMsgGCTrading_ConfirmOffer = 1512,
        k_EMsgGCTrading_TradeTypingChatMsg = 1513,
        k_EMsgGCServerBrowser_FavoriteServer = 1601,
        k_EMsgGCServerBrowser_BlacklistServer = 1602,
        k_EMsgGCServerRentalsBase = 1700,
        k_EMsgGCItemPreviewCheckStatus = 1701,
        k_EMsgGCItemPreviewStatusResponse = 1702,
        k_EMsgGCItemPreviewRequest = 1703,
        k_EMsgGCItemPreviewRequestResponse = 1704,
        k_EMsgGCItemPreviewExpire = 1705,
        k_EMsgGCItemPreviewExpireNotification = 1706,
        k_EMsgGCItemPreviewItemBoughtNotification = 1707,
        k_EMsgGCDev_NewItemRequest = 2001,
        k_EMsgGCDev_NewItemRequestResponse = 2002,
        k_EMsgGCDev_PaintKitDropItem = 2003,
        k_EMsgGCDev_SchemaReservationRequest = 2004,
        k_EMsgGCStoreGetUserData = 2500,
        k_EMsgGCStoreGetUserDataResponse = 2501,
        k_EMsgGCStorePurchaseInit_DEPRECATED = 2502,
        k_EMsgGCStorePurchaseInitResponse_DEPRECATED = 2503,
        k_EMsgGCStorePurchaseFinalize = 2504,
        k_EMsgGCStorePurchaseFinalizeResponse = 2505,
        k_EMsgGCStorePurchaseCancel = 2506,
        k_EMsgGCStorePurchaseCancelResponse = 2507,
        k_EMsgGCStorePurchaseQueryTxn = 2508,
        k_EMsgGCStorePurchaseQueryTxnResponse = 2509,
        k_EMsgGCStorePurchaseInit = 2510,
        k_EMsgGCStorePurchaseInitResponse = 2511,
        k_EMsgGCBannedWordListRequest = 2512,
        k_EMsgGCBannedWordListResponse = 2513,
        k_EMsgGCToGCBannedWordListBroadcast = 2514,
        k_EMsgGCToGCBannedWordListUpdated = 2515,
        k_EMsgGCToGCDirtySDOCache = 2516,
        k_EMsgGCToGCDirtyMultipleSDOCache = 2517,
        k_EMsgGCToGCUpdateSQLKeyValue = 2518,
        k_EMsgGCToGCIsTrustedServer = 2519,
        k_EMsgGCToGCIsTrustedServerResponse = 2520,
        k_EMsgGCToGCBroadcastConsoleCommand = 2521,
        k_EMsgGCServerVersionUpdated = 2522,
        k_EMsgGCToGCWebAPIAccountChanged = 2524,
        k_EMsgGCRequestAnnouncements = 2525,
        k_EMsgGCRequestAnnouncementsResponse = 2526,
        k_EMsgGCRequestPassportItemGrant = 2527,
        k_EMsgGCClientVersionUpdated = 2528,
        k_EMsgGCRecurringSubscriptionStatus = 2530,
        k_EMsgGCAdjustEquipSlotsManual = 2531,
        k_EMsgGCAdjustEquipSlotsShuffle = 2532,
        k_EMsgGCNameItemAndEquip = 2533,
        k_EMsgGCOpenCrate = 2534,
        k_EMsgGCAcknowledgeRentalExpiration = 2535,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum EGCMsgResponse
    {
        k_EGCMsgResponseOK = 0,
        k_EGCMsgResponseDenied = 1,
        k_EGCMsgResponseServerError = 2,
        k_EGCMsgResponseTimeout = 3,
        k_EGCMsgResponseInvalid = 4,
        k_EGCMsgResponseNoMatch = 5,
        k_EGCMsgResponseUnknownError = 6,
        k_EGCMsgResponseNotLoggedOn = 7,
        k_EGCMsgFailedToCreate = 8,
        k_EGCMsgLimitExceeded = 9,
        k_EGCMsgCommitUnfinalized = 10,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum EUnlockStyle
    {
        k_UnlockStyle_Succeeded = 0,
        k_UnlockStyle_Failed_PreReq = 1,
        k_UnlockStyle_Failed_CantAfford = 2,
        k_UnlockStyle_Failed_CantCommit = 3,
        k_UnlockStyle_Failed_CantLockCache = 4,
        k_UnlockStyle_Failed_CantAffordAttrib = 5,
    }

    [global::ProtoBuf.ProtoContract()]
    public enum EGCItemCustomizationNotification
    {
        k_EGCItemCustomizationNotification_NameItem = 1006,
        k_EGCItemCustomizationNotification_UnlockCrate = 1007,
        k_EGCItemCustomizationNotification_XRayItemReveal = 1008,
        k_EGCItemCustomizationNotification_XRayItemClaim = 1009,
        k_EGCItemCustomizationNotification_CasketTooFull = 1011,
        k_EGCItemCustomizationNotification_CasketContents = 1012,
        k_EGCItemCustomizationNotification_CasketAdded = 1013,
        k_EGCItemCustomizationNotification_CasketRemoved = 1014,
        k_EGCItemCustomizationNotification_CasketInvFull = 1015,
        k_EGCItemCustomizationNotification_NameBaseItem = 1019,
        k_EGCItemCustomizationNotification_RemoveItemName = 1030,
        k_EGCItemCustomizationNotification_RemoveSticker = 1053,
        k_EGCItemCustomizationNotification_ApplySticker = 1086,
        k_EGCItemCustomizationNotification_StatTrakSwap = 1088,
        k_EGCItemCustomizationNotification_RemovePatch = 1089,
        k_EGCItemCustomizationNotification_ApplyPatch = 1090,
        k_EGCItemCustomizationNotification_ApplyKeychain = 1091,
        k_EGCItemCustomizationNotification_RemoveKeychain = 1092,
        k_EGCItemCustomizationNotification_ActivateFanToken = 9178,
        k_EGCItemCustomizationNotification_ActivateOperationCoin = 9179,
        k_EGCItemCustomizationNotification_GraffitiUnseal = 9185,
        k_EGCItemCustomizationNotification_GenerateSouvenir = 9204,
        k_EGCItemCustomizationNotification_ClientRedeemMissionReward = 9209,
        k_EGCItemCustomizationNotification_ClientRedeemFreeReward = 9219,
        k_EGCItemCustomizationNotification_XpShopUseTicket = 9221,
        k_EGCItemCustomizationNotification_XpShopAckTracks = 9222,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
