using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Interface.Travel.DTO.Form;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Travel
{
    public class TravelRequestAccountabilityService : ITravelRequestAccountabilityService
    {
        public EservicesdbContext _db;
        IRequestService _requestService;
        IUserService _userService;

        public TravelRequestAccountabilityService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            _db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<TravelRequestConfirmationDTO> Get(int id)
        {
            return await _db.TravelRequestAccountability
                  .Where(m => m.TravelRequestId == id)
                  .Select(m => new TravelRequestConfirmationDTO()
                  {
                      BudgetDecimal = m.TravelRequest.TotalBudget,
                      TotalAmountHotel = m.TotalAmountHotel,
                      TotalAmountTransportation = m.TotalAmountTransportation,
                      Note = m.Note,
                      TravelRequestId = m.TravelRequestId,
                      Status = m.TravelRequest.Request.Currentstate.Name,
                      StatusType = m.TravelRequest.Request.Currentstate.Statetype.Name,
                  }).SingleAsync();

        }

        public async Task<TravelRequestConfirmationDTO> GetByRequestId(int requestId)
        {
            return await _db.TravelRequestAccountability
                  .Where(m => m.TravelRequest.RequestId == requestId)
                  .Select(m => new TravelRequestConfirmationDTO()
                  {
                      BudgetDecimal = m.TravelRequest.TotalBudget,
                      TotalAmountHotel = m.TotalAmountHotel,
                      TotalAmountTransportation = m.TotalAmountTransportation,
                      Note = m.Note,
                      TravelRequestId = m.TravelRequestId,
                      Status = m.TravelRequest.Request.Currentstate.Name,
                      StatusType = m.TravelRequest.Request.Currentstate.Statetype.Name,
                  }).SingleAsync();
        }

        public async Task<Control<TravelRequestConfirmationDTO>> GetList(int start, int take, string filter, string order)
        {
            var repos = _db.TravelRequestAccountability.AsQueryable();
            int totalData = repos.Count();

            int totalFilterData = totalData;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.TravelRequest.NoRequest.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<TravelRequestConfirmationDTO>()
            {
                ListClass = await data.Select(m => new TravelRequestConfirmationDTO
                {
                    BudgetDecimal = m.TravelRequest.TotalBudget,
                    TotalAmountHotel = m.TotalAmountHotel,
                    TotalAmountTransportation = m.TotalAmountTransportation,
                    Note = m.Note,
                    TravelRequestId = m.TravelRequestId,
                    Status = m.TravelRequest.Request.Currentstate.Name,
                    StatusType = m.TravelRequest.Request.Currentstate.Statetype.Name,
                    TravelRequest = new TravelRequestDTO()
                    {
                        Description = m.TravelRequest.Description,
                        Id = m.TravelRequest.Id,
                        NoRequest = m.TravelRequest.NoRequest,
                        RequesterId = m.TravelRequest.RequesterId,
                        RequesterName = m.TravelRequest.Requester.Name,
                        RequestId = m.TravelRequest.RequestId,
                        RequestType = m.TravelRequest.TravelHotelRequests != null && m.TravelRequest.TravelTransportationRequests != null ?
                    "Travel and Hotel" :
                    (m.TravelRequest.TravelHotelRequests != null && m.TravelRequest.TravelTransportationRequests == null ? "Hotel" :
                    (m.TravelRequest.TravelHotelRequests == null && m.TravelRequest.TravelTransportationRequests != null ? "Travel" : "")),
                        StatusRequest = m.TravelRequest.Request.Currentstate.Name,
                        Title = m.TravelRequest.Title,
                        TransactionDate = m.TravelRequest.TransactionDate
                    }
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<TravelRequestAccountability> Save(TravelRequestAccountability entity, bool submit)
        {


            var user = await _userService.GetUserById(entity.PicId.Value);
            entity.OfficeLocationId = user.LocationId;


            if (!_db.TravelRequestAccountability.Any(m => m.TravelRequestId == entity.TravelRequestId))
            {

                await _db.TravelRequestAccountability.AddAsync(entity);
            }
            else
            {
                _db.TravelRequestAccountability.Update(entity);
            }

            await _db.SaveChangesAsync();

            if (submit == true)
            {
                var idreq = await _db.TravelRequest.SingleOrDefaultAsync(m => m.Id == entity.TravelRequestId);

                var request = await _db.RequestFlow
                   .SingleAsync(m => m.Requestid == idreq.RequestId);

                var requestFlowUrl = new RequestFlowUrl()
                {
                    Url = "travel/confirm/dashboard/request/",
                    Type = "confirm"
                };

                request.RequestFlowUrl.Add(requestFlowUrl);
                _db.RequestFlow.Update(request);
                await _db.SaveChangesAsync();

                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.PicId.Value
                    });
            }




            return entity;
        }
    }
}
