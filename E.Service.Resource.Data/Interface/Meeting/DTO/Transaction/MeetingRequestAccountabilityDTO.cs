using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO.Transaction
{
    public class MeetingRequestAccountabilityDTO
    {
        public int MeetingRequestId { get; set; }
        public string MeetingRequestNo { get; set; }
        public string MeetingTitle { get; set; }
        public string MeetingDesc { get; set; }
        public int PicID { get; set; }
        public string PicName { get; set; }
        public decimal TotalBudgetReal { get; set; }
        public int NumOfParticipant { get; set; }
        public string State { get; set; }
        public string StateType { get; set; }
        public int Id { get; set; }

        public int RequestId { get; set; }
        public IList<IFormFile> Files { get; set; }

        public MeetingRequestDTO MeetingRequest { get; set; }
        public IList<MeetingRequestFilesDTO> MeetingRequestFilesDTO { get; set; }

    }


    public class MeetingRequestFilesDTO
    {
        public int Id { get; set; }
        public string FilesLocation { get; set; }
    }
}
