using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset
{
    public class AssetBorrowDTO
    {
        public int Id { get; set; }
        public int? AssetId { get; set; }
        public string AssetNumber { get; set; }
        public string AssetName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime RequestDate { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int RequestId { get; set; }
        public int? JabatanId { get; set; }
        public string JabatanName { get; set; }

        public int? RoomId { get; set; }
        public string RoomName { get; set; }

        public string Status { get; set; }
        public string StatusType { get; set; }
    }
}
