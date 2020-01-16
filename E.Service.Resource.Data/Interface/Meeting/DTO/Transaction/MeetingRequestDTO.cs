using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO.Transaction
{
    public class MeetingRequestDTO
    {
        public int Id { get; set; }
        public string MeetingRequestNo { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingDesc { get; set; }
        public int? RequesterId { get; set; }
        public string Requester { get; set; }
        public int? MeetingRoomId { get; set; }
        public string MeetingRoomsName { get; set; }
        public int? MeetingTypeId { get; set; }
        public string MeetingTypeName { get; set; }
        public int? NumOfPartisipant { get; set; }
        public int? BudgetId { get; set; }
        public string BudgetName { get; set; }
        public decimal? TotalBudgetBook { get; set; }
        public int? PicId { get; set; }
        public decimal? FundAvailable { get; set; }
        public string NoAkun { get; set; }
        public string StateName { get; set; }
        public string StateNameType { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int KodePusatBiayaId { get; set; }
        public string KodePusatBiayaName { get; set; }

        public int KodeElementBiayaId { get; set; }
        public string KodeElementBiayaName { get; set; }

        public string Location { get; set; }
        public int LocationId { get; set; }

        public int MeetingRoomCategoryId { get; set; }
        public string MeetingRoomCategoryName { get; set; }
        public int RequestId { get; set; }

        public DateTime CreateDate { get; set; }
        public IList<MeetingRequestTimeDTO> MeetingRequestTime { get; set; }

        public IList<MeetingRequestBudgetDTO> MeetingRequestBudgets { get; set; }
    }


    public class MeetingRequestBudgetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MeetingBudgetId { get; set; }
        public decimal Amount { get; set; }
        public int QtyDays { get; set; }
        public int QtyPerson { get; set; }
        public decimal TotalAmount { get; set; }

    }


    public class MeetingRequestTimeDTO
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MeetingAgendaNo { get; set; }

    }
}
