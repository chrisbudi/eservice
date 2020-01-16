using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum ERequestType
    {
        [Description("Meeting Request")]
        MeetingRequest = 1,
        [Description("Meeting Request Confirmation")]
        MeetingRequestConfirmation = 2,
        [Description("Asset Add")]
        AssetAdd = 3,
        [Description("Asset Edit")]
        AssetEdit = 4,
        [Description("Asset Change Dept")]
        AssetChange = 5,
        [Description("Asset Borrow")]
        AssetBorrow = 6,
        [Description("Car Request")]
        CarRequest = 7,

        [Description("Travel")]
        Travel = 8,

        [Description("Order Inventory IT")]
        OrderInventoryIT = 9,
        [Description("Order Inventory NON IT")]
        OrderInventoryNonIT = 10,

        [Description("Order ATK")]
        OrderATK = 11,

        [Description("Reload ATK")]
        ReloadATK = 12,

        [Description("Order Printing")]
        OrderPrinting = 13,


        [Description("Repair IT")]
        RepairIT = 14,
        [Description("Repair Non IT")]
        RepairNonIT = 15,
        [Description("Order Accountability")]
        OrderAccountability = 16,

        [Description("Reload Accountability")]
        ReloadAccountability = 17

    }

    public enum EAssetType
    {
        [Description("Asset Edit")]
        AssetEdit = 4,
        [Description("Asset Change Dept")]
        AssetChange = 5
    }

    public enum EAssetTypeService
    {
        AssetAdd,
        Current
    }

    public enum EStateType
    {
        [Description("Start")]
        Start = 1,
        [Description("Normal")]
        Normal = 2,
        [Description("Complete")]
        Complete = 3,
        [Description("Cancelled")]
        Cancelled = 4,
        [Description("Reject")]
        Reject = 5
    }

    public enum ETransitionType
    {
        [Description("Next")]
        Next,
        [Description("Cancel")]
        Cancel,
        [Description("Reject")]
        Reject
    }

    public enum EActionType
    {
        [Description("Approve")]
        Approve = 1,
        [Description("Deny")]
        Deny = 2,
        [Description("Cancel")]
        Cancel = 3,
        [Description("Resolve")]
        Resolve = 3,
    }
}
