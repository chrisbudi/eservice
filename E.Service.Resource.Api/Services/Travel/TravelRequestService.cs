using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Interface.Travel.DTO.Form;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Remotion.Linq.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Travel
{
    public class TravelRequestService : ITravelRequestService
    {
        private EservicesdbContext _db;

        IRequestService _requestService;
        IUserService _userService;

        public TravelRequestService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            _db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<TravelRequestDTO> Get(int id)
        {

            var travel = await _db.TravelRequest.Where(m => m.Id == id)
                .Include(m => m.Requester)
                .Include(m => m.Request)
                    .ThenInclude(m => m.Currentstate)
                        .ThenInclude(m => m.Statetype)
                .AsAsyncEnumerable().Select(m => new TravelRequestDTO()
                {
                    Description = m.Description,
                    Id = m.Id,
                    NoRequest = m.NoRequest,
                    RequesterId = m.RequesterId,
                    RequesterName = m.Requester.Name,
                    RequestId = m.RequestId,
                    Title = m.Title,
                    TransactionDate = m.TransactionDate,
                    StatusRequest = m.Request.Currentstate.Name,
                    StatusType = m.Request.Currentstate.Statetype.Name,
                    BudgetLeft = m.BudgetLeft,
                    FundAvailable = m.FundAvailable,
                    NoAccount = m.NoAccount,
                    StatusAnggaranId = m.StatusAnggaranId,
                    BudgetId = m.BudgetId,
                    TotalBudget = m.TotalBudget
                }).Single();


            if (_db.TravelHotelRequests.Any(m => m.TravelRequestId == id))
            {

                travel.RequestHotel = await _db.TravelHotelRequests.Where(m => m.TravelRequestId == id).Select(m => new TravelRequestHotelDTO()
                {
                    CheckinAt = m.CheckinAt,
                    CheckoutAt = m.CheckoutAt,
                    FilePath = m.FilePath,
                    Note = m.Note,
                    RoomTotal = m.RoomTotal,
                    TotalPrice = m.TravelHotel.Budget.BudgetNominal,
                    TravelCityId = m.TravelCityId,
                    TravelCityName = m.TravelCity.Name,
                    TravelHotelId = m.TravelHotelId,
                    TravelHotelName = m.TravelHotel.Name,
                    TravelRequestId = m.TravelRequestId,
                    CreatedAt = m.CreatedAt
                }).SingleOrDefaultAsync();
            }

            if (_db.TravelTransportationRequests.Any(m => m.TravelRequestId == id))
            {

                travel.RequestTransportation = await _db.TravelTransportationRequests.Where(m => m.TravelRequestId == id)
                    .Select(m => new TravelRequestTransportationDTO()
                    {
                        TravelRequestId = m.TravelRequestId,
                        Done = m.Done,
                        PersonTotal = m.PersonTotal,
                        TravelCitiesId = m.TravelCities == null ? null : m.TravelCitiesId,
                        TravelCitiesName = m.TravelCities == null ? null : m.TravelCities.Name,
                        CreatedAt = m.CreatedAt,
                        RequestTransportationDetail = m.TravelTransportationRequestDetails.Select(td => new TravelRequestTransportationDetailDTO()
                        {
                            ArrivalDateTime = td.ArrivalDateTime,
                            BudgetId = td.TravelTransportationName.BudgetId,
                            BudgetName = td.TravelTransportationName.Budget.BudgetName,
                            BudgetNominal = td.TravelTransportationName.Budget.BudgetNominal ?? 0,
                            DepartDateTime = td.DepartDateTime,
                            FilePath = td.FilePath,
                            FromCity = td.FromCity,
                            FromCityName = td.FromCityNavigation.Name,
                            ToCity = td.ToCity,
                            ToCityName = td.ToCityNavigation.Name,
                            TravelOutbondCategoryId = td.TravelOutbondCategoryId,
                            TravelOutbondName = td.TravelOutbondCategory.Name,
                            TravelTransportationName = td.TravelTransportationName.Name,
                            TravelTransportationNameId = td.TravelTransportationNameId,
                            TravelTransportatonIdRequestId = m.TravelRequestId,
                            Id = td.Id
                        }).ToList()

                    }).SingleOrDefaultAsync();
            }

            travel.TotalBudget = travel.RequestHotel.TotalPrice ?? 0 +
                travel.RequestTransportation.RequestTransportationDetail.Sum(m => m.BudgetNominal ?? 0);
            return travel;

        }


        public async Task<TravelRequestDTO> GetByRequestId(int requestId)
        {
            var travel = await _db.TravelRequest
                 .Where(m => m.RequestId == requestId)
                 .Select(m => new TravelRequestDTO()
                 {
                     Description = m.Description,
                     Id = m.Id,
                     NoRequest = m.NoRequest,
                     RequesterId = m.RequesterId,
                     RequesterName = m.Requester.Name,
                     RequestId = m.RequestId,
                     Title = m.Title,
                     TransactionDate = m.TransactionDate,
                     StatusRequest = m.Request.Currentstate.Name,
                     StatusType = m.Request.Currentstate.Statetype.Name,
                     TotalBudget = m.TotalBudget
                 }).SingleAsync();




            if (_db.TravelHotelRequests.Any(m => m.TravelRequestId == travel.Id))
            {

                travel.RequestHotel = await _db.TravelHotelRequests.Where(m => m.TravelRequestId == travel.Id).Select(m => new TravelRequestHotelDTO()
                {
                    CheckinAt = m.CheckinAt,
                    CheckoutAt = m.CheckoutAt,
                    FilePath = m.FilePath,
                    Note = m.Note,
                    RoomTotal = m.RoomTotal,
                    TotalPrice = m.TravelHotel.Budget.BudgetNominal ?? 0,
                    TravelCityId = m.TravelCityId,
                    TravelCityName = m.TravelCity.Name,
                    TravelHotelId = m.TravelHotelId,
                    TravelHotelName = m.TravelHotel.Name,
                    TravelRequestId = m.TravelRequestId,
                    CreatedAt = m.CreatedAt
                }).SingleOrDefaultAsync();
            }

            if (_db.TravelTransportationRequests.Any(m => m.TravelRequestId == travel.Id))
            {

                travel.RequestTransportation = await _db.TravelTransportationRequests.Where(m => m.TravelRequestId == travel.Id)
                    .Select(m => new TravelRequestTransportationDTO()
                    {
                        TravelRequestId = m.TravelRequestId,
                        Done = m.Done,
                        PersonTotal = m.PersonTotal,
                        TravelCitiesId = m.TravelCities == null ? null : m.TravelCitiesId,
                        TravelCitiesName = m.TravelCities == null ? null : m.TravelCities.Name,
                        CreatedAt = m.CreatedAt,
                        RequestTransportationDetail = m.TravelTransportationRequestDetails.Select(td => new TravelRequestTransportationDetailDTO()
                        {
                            ArrivalDateTime = td.ArrivalDateTime,
                            BudgetId = td.TravelTransportationName.BudgetId,
                            BudgetName = td.TravelTransportationName.Budget.BudgetName,
                            BudgetNominal = td.TravelTransportationName.Budget.BudgetNominal ?? 0,
                            DepartDateTime = td.DepartDateTime,
                            FilePath = td.FilePath,
                            FromCity = td.FromCity,
                            FromCityName = td.FromCityNavigation.Name,
                            ToCity = td.ToCity,
                            ToCityName = td.ToCityNavigation.Name,
                            TravelOutbondCategoryId = td.TravelOutbondCategoryId,
                            TravelOutbondName = td.TravelOutbondCategory.Name,
                            TravelTransportationName = td.TravelTransportationName.Name,
                            TravelTransportationNameId = td.TravelTransportationNameId,
                            TravelTransportatonIdRequestId = m.TravelRequestId,
                            Id = td.Id
                        }).ToList()

                    }).SingleOrDefaultAsync();
            }

            var totalPrice = travel.RequestTransportation.RequestTransportationDetail.Sum(m => m.BudgetNominal ?? 0);

            travel.TotalBudget = travel.TotalBudget;

            return travel;
        }

        public async Task<List<TravelRequestTransportationDetailDTO>> GetDetailById(int travelRequestId)
        {
            return await _db.TravelTransportationRequestDetails
                .Where(m => m.TravelTransportatonIdRequestId == travelRequestId)
                .Select(m => new TravelRequestTransportationDetailDTO()
                {
                    ArrivalDateTime = m.ArrivalDateTime,
                    TravelTransportatonIdRequestId = m.TravelTransportatonIdRequestId,
                    BudgetId = m.TravelTransportationName.BudgetId,
                    BudgetName = m.TravelTransportationName.Budget.BudgetName,
                    BudgetNominal = m.TravelTransportationName.Budget.BudgetNominal,
                    DepartDateTime = m.DepartDateTime,
                    FilePath = m.FilePath,
                    FromCity = m.FromCity,
                    FromCityName = m.FromCityNavigation.Name,
                    Id = m.Id,
                    ToCity = m.ToCity,
                    ToCityName = m.ToCityNavigation.Name,
                    TravelOutbondCategoryId = m.TravelOutbondCategoryId,
                    TravelOutbondName = m.TravelOutbondCategory.Name,
                    TravelTransportationName = m.TravelTransportationName.Name,
                    TravelTransportationNameId = m.TravelTransportationNameId
                }).ToListAsync();
        }

        public async Task<Control<TravelRequestDTO>> GetList(int start, int take, string filter, string order, ETravelAccountabilityType eTravelAccountability)
        {
            var repos = _db.TravelRequest.AsQueryable();

            if (eTravelAccountability != ETravelAccountabilityType.NONE)
            {

                if (eTravelAccountability == ETravelAccountabilityType.GAApproved)
                    repos = repos.Where(m =>
                    m.Request.Currentstateid == (int)ETravelAccountabilityType.GAApproved ||
                    m.Request.Currentstateid == (int)ETravelAccountabilityType.UserUploaded);
                else
                {
                    repos = repos.Where(m =>
                            m.Request.Currentstateid == (int)ETravelAccountabilityType.UserUploaded);

                }
            }

            int totalData = repos.Count();

            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.NoRequest.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<TravelRequestDTO>()
            {
                ListClass = await data.Select(m => new TravelRequestDTO
                {
                    Description = m.Description,
                    Id = m.Id,
                    NoRequest = m.NoRequest,
                    RequesterId = m.RequesterId,
                    RequesterName = m.Requester.Name,
                    RequestId = m.RequestId,
                    Title = m.Title,
                    TransactionDate = m.TransactionDate,
                    StatusRequest = m.Request.Currentstate.Name,
                    StatusType = m.Request.Currentstate.Statetype.Name,
                    RequestType = m.TravelHotelRequests != null && m.TravelTransportationRequests != null ?
                    "Travel and Hotel" : (m.TravelHotelRequests != null && m.TravelTransportationRequests == null ? "Hotel" :
                    (m.TravelHotelRequests == null && m.TravelTransportationRequests != null ? "Travel" : ""))

                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<TravelRequest> Save(TravelRequest entity, bool submit)
        {
            RequestFlow request;
            if (entity.Id == 0)
            {

                var user = await _userService.GetUserById(entity.RequesterId.Value);
                var nominalTotal = decimal.Zero;
                entity.TransactionDate = DateTime.Now;
                entity.JabatanId = user.JabatanId;
                entity.OfficeLocationId = user.LocationId;

                var prFix = $"05{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.NoRequest = await GenerateNo(prFix);

                if (entity.TravelHotelRequests != null)
                {

                    var budgetHotel = _db.TravelHotel.Include(m => m.Budget)
                        .AsNoTracking().SingleOrDefault(m => m.Id == entity.TravelHotelRequests.TravelHotelId).Budget.BudgetNominal;

                    entity.TravelHotelRequests.TotalPrice = budgetHotel;
                    nominalTotal = budgetHotel ?? 0;

                }

                if (entity.TravelTransportationRequests != null)
                {
                    foreach (var detail in entity.TravelTransportationRequests.TravelTransportationRequestDetails)
                    {
                        var budgetTransportation = _db.TravelTransportationName.Include(m => m.Budget)
                                           .AsNoTracking().SingleOrDefault(m => m.Id == detail.TravelTransportationNameId).Budget;

                        detail.TotalBudget = budgetTransportation.BudgetNominal ?? 0;


                        nominalTotal += (detail.TotalBudget);

                    }
                }
                entity.TotalBudget = nominalTotal;
                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.Travel),
                    Title = entity.NoRequest,
                    Note = entity.Title,
                    Processid = await _requestService.ProgressId(ERequestType.Travel),
                    Userid = entity.RequesterId.ToString(),
                    Url = "travel/dashboard/request/"
                };

                List<Transition> transitionList = await _requestService.TransitionList(ERequestType.Travel);
                bool firstInput = true;

                foreach (var transition in transitionList)
                {
                    var raNext = new Requestaction()
                    {
                        Actionid = transition.Transitionaction.Actonid,
                        Isactive = firstInput,
                        Iscomplete = false,
                        Transitionid = transition.Transitionid
                    };
                    firstInput = false;
                    request.Requestaction.Add(raNext);
                }

                entity.Request = request;

                await _db.TravelRequest.AddAsync(entity);
            }
            else
            {

                if (entity.TravelTransportationRequests != null)
                {
                    //removing data delete meeting request time
                    var datatravelDetail = _db.TravelTransportationRequestDetails.Where(m => m.TravelTransportatonIdRequestId == entity.Id
                                    && entity.TravelTransportationRequests.TravelTransportationRequestDetails.Any(t => t.Id != m.Id));

                    _db.TravelTransportationRequestDetails.RemoveRange(datatravelDetail);
                }

                var user = await _userService.GetUserById(entity.RequesterId.Value);
                entity.JabatanId = user.JabatanId;
                entity.OfficeLocationId = user.LocationId;

                _db.TravelRequest.Update(entity);

                _db.Entry(entity).Property(x => x.TransactionDate).IsModified = false;


                request = _db.RequestFlow.Single(m => m.Requestid ==
                       entity.RequestId);
            }
            await _db.SaveChangesAsync();

            if (submit == true)
            {
                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.RequesterId.Value
                    });
            }
            return entity;
        }

        public async Task<List<TravelTransportationRequestDetails>> UpdateDetail(
            List<TravelTransportationRequestDetails> detailEntity, TravelHotelRequests detailEntityHotel, int requesterId, bool submit)
        {
            _db.TravelTransportationRequestDetails.UpdateRange(detailEntity);
            _db.TravelHotelRequests.Update(detailEntityHotel);
            await _db.SaveChangesAsync();

            if (submit == true)
            {
                var dbID = detailEntity[0].TravelTransportatonIdRequestId;
                var transportrequest = await _db.TravelTransportationRequests.SingleAsync(m => m.TravelRequestId == dbID);

                await _requestService.SetStateRequest(transportrequest.TravelRequest.RequestId.Value,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = requesterId
                    });
            }

            return detailEntity;
        }

        public async Task UpdateEntity(int id, int anggaranstatusId)
        {
            var data = await _db.TravelRequest.SingleAsync(m => m.Id == id);

            data.StatusAnggaranId = anggaranstatusId;
            _db.Update(data);
            await _db.SaveChangesAsync();
        }

        public async Task<TravelHotelRequests> UpdateHotelDetail(TravelHotelRequests detailEntityHotel)
        {
            _db.TravelHotelRequests.Update(detailEntityHotel);
            await _db.SaveChangesAsync();


            return detailEntityHotel;
        }

        public async Task<TravelTransportationRequestDetails> UpdateTransportDetail(TravelTransportationRequestDetails detailEntityTransport)
        {
            var dataEntity = _db.TravelTransportationRequestDetails.Single(m => m.Id == detailEntityTransport.Id);


            dataEntity.FilePath = detailEntityTransport.FilePath;

            _db.TravelTransportationRequestDetails.Update(dataEntity);

            await _db.SaveChangesAsync();

            return detailEntityTransport;
        }

        private async Task<string> GenerateNo(string prFix)
        {
            var latestNo = await _db.TravelRequest.OrderByDescending(m => m.NoRequest).
              FirstOrDefaultAsync(m => m.NoRequest.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.NoRequest.Substring(latestNo.NoRequest.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }
    }
}
