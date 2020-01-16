using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car.DTO
{
    public class CarRequestBudgetDTO
    {
        public int CarRequestId { get; set; }
        public string CarRequestBudgetNo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? RequestStart { get; set; }
        public DateTime? RequestEnd { get; set; }
        public int? RequesterId { get; set; }
        public int? RequestId { get; set; }
        public string Description { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
        public ICollection<CarDetilBudgetDTO> CarBudgetDetil { get; set; }

        public bool DriverInputableData { get; set; }
    }

    public class CarDetilBudgetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarRequestBudgetId { get; set; }
        public int CarBudgetId { get; set; }
        public string Description { get; set; }
        public decimal Nominal { get; set; }
        public string Filelocation { get; set; }
        public bool Done { get; set; }
        public int CarRequestDetailStatusId { get; set; }
        public string CarRequestDetailStatusName { get; set; }
        public IFormFile Image { get; set; }
    }
}
