using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Travel.DTO.Form;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel
{
    public interface ITravelRequestService
    {
        Task<TravelRequest> Save(TravelRequest request, bool submit);

        Task<Control<TravelRequestDTO>> GetList(int start, int take, string filter, string order, ETravelAccountabilityType eTravelAccountability);

        Task<TravelRequestDTO> Get(int id);

        Task<TravelRequestDTO> GetByRequestId(int requestId);
        Task<List<TravelRequestTransportationDetailDTO>> GetDetailById(int travelRequestId);
        Task<List<TravelTransportationRequestDetails>> UpdateDetail(List<TravelTransportationRequestDetails> detailEntity, TravelHotelRequests detailEntityHotel, int requesterId, bool submit);
        Task<TravelHotelRequests> UpdateHotelDetail(TravelHotelRequests detailEntityHotel);
        Task<TravelTransportationRequestDetails> UpdateTransportDetail(TravelTransportationRequestDetails detailEntityTransport);
        Task UpdateEntity(int id, int anggaranstatusId);
    }
}
