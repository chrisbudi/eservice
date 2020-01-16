using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel.DTO.Form
{
    public class TravelRequestDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string NoRequest { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? RequestId { get; set; }
        public int? RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string Title { get; set; }
        public string StatusRequest { get; set; }
        public string StatusType { get; set; }
        public bool Editable { get; set; }
        public string RequestType { get; set; }
        public decimal TotalBudget { get; set; }

        public int? StatusAnggaranId { get; set; }
        public decimal FundAvailable { get; set; }
        public string NoAccount { get; set; }
        public decimal BudgetLeft { get; set; }
        public int? BudgetId { get; set; }

        public TravelRequestTransportationDTO RequestTransportation { get; set; }
        public TravelRequestHotelDTO RequestHotel { get; set; }
    }




    public class TravelRequestTransportationDTO
    {
        public int TravelRequestId { get; set; }
        public bool? Done { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? TravelCitiesId { get; set; }
        public string TravelCitiesName { get; set; }
        public int? PersonTotal { get; set; }

        public decimal BudgetTravelNominal { get; set; }
        public ICollection<TravelRequestTransportationDetailDTO> RequestTransportationDetail { get; set; }

    }



    public class TravelRequestUploadFileDTO
    {
        public List<TravelRequestTransportationDetailDTO> TransportationDetail { get; set; }
        public TravelRequestHotelDTO Hotel { get; set; }
    }

    public class TravelRequestTransportationDetailDTO
    {
        public int Id { get; set; }
        public int? TravelTransportatonIdRequestId { get; set; }
        public int? TravelOutbondCategoryId { get; set; }
        public string TravelOutbondName { get; set; }
        public int? TravelTransportationNameId { get; set; }
        public string TravelTransportationName { get; set; }
        public int? FromCity { get; set; }
        public string FromCityName { get; set; }
        public int? ToCity { get; set; }
        public string ToCityName { get; set; }
        public DateTime? DepartDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public string FilePath { get; set; }
        public int? BudgetId { get; set; }
        public string BudgetName { get; set; }
        public decimal? BudgetNominal { get; set; }
        public IFormFile Image { get; set; }
    }

    public class TravelRequestHotelDTO
    {
        public int TravelRequestId { get; set; }
        public int? TravelHotelId { get; set; }
        public string TravelHotelName { get; set; }
        public int? TravelCityId { get; set; }
        public string TravelCityName { get; set; }
        public DateTime CheckinAt { get; set; }
        public DateTime CheckoutAt { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? RoomTotal { get; set; }
        public decimal? TotalPrice { get; set; }
        public IFormFile Image { get; set; }
    }
}
