using System;

namespace E.Service.Resource.Data.Interface.Asset.DTO
{
    public class AssetTransactionDTO
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public string RequestName { get; set; }
        public int? OfficeRoomId { get; set; }
        public string OfficeRoomName { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? AssetConditionId { get; set; }
        public string AssetConditionName { get; set; }
        public int? AssetId { get; set; }
        public string AssetName { get; set; }

        public int? OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string ProcessName { get; set; }
        public bool Depreciated { get; set; }
        public string RequestNo { get; set; }
        public int RequestId { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public DateTime Requestdate { get; set; }

    }
}
