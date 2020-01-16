using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel.DTO.Form
{
    public class TravelRequestConfirmationDTO
    {
        public int TravelRequestId { get; set; }
        public string Note { get; set; }
        public decimal? BudgetDecimal { get; set; }
        public decimal? TotalAmountTransportation { get; set; }
        public decimal? TotalAmountHotel { get; set; }
        public int RequesterId { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
        public TravelRequestDTO TravelRequest { get; set; }
        public List<IFormFile> Files { get; set; }
        public IList<TravelRequestConfirmationFilesDTO> filesDTO { get; set; }

    }

    public class TravelRequestConfirmationFilesDTO
    {
        public int TravelRequestAccountabilityFilesId { get; set; }
        public int TravelRequestId { get; set; }
        public string FilePath { get; set; }
    }
}
