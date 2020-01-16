using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car.DTO
{
    public class CarRequestDTO
    {
        public int Id { get; set; }
        public string CarRequestNo { get; set; }
        public int RequesterId { get; set; }
        public string RequesterName { get; set; }
        public int CarPoolId { get; set; }
        public string carPoolName { get; set; }
        public string CarPoolLicenseNo { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
        public bool? Done { get; set; }
        public DateTime RequiredAt { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? RequestStartTime { get; set; }
        public DateTime? RequestEndTime { get; set; }
        public int? RequestId { get; set; }
        public int? RegionalId { get; set; }
        public string RegionalName { get; set; }
        public decimal Balance { get; set; }
        public bool? UsingDriver { get; set; }
        public string CurrentDriverName { get; set; }
        public int CurrentDriverId { get; set; }

        public decimal TotalBudget { get; set; }
        public int? StatusAnggaranId { get; set; }
        public decimal FundAvailable { get; set; }
        public string NoAccount { get; set; }
        public decimal BudgetLeft { get; set; }


    }
}
