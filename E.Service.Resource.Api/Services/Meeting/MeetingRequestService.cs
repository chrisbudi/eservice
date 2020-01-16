using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Interface.Meeting.DTO.Transaction;
using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class MeetingRequestService : IMeetingRequest
    {
        private EservicesdbContext db;

        IRequestService _requestService;
        IUserService _userService;

        public MeetingRequestService(EservicesdbContext db, IRequestService request, IUserService userService)
        {
            this.db = db;
            _requestService = request;
            _userService = userService;
        }


        public async Task<MeetingRequestDTO> Get(int id)
        {
            return await db.MeetingRequestFlow
                .Include(m => m.MeetingRequest)
                .ThenInclude(m => m.MeetingRequestTime)
                .Include(m => m.MeetingRequest)
                .ThenInclude(m => m.MeetingRequestBudgets)
                .Select(m => new MeetingRequestDTO
                {
                    Id = m.MeetingRequest.Id,
                    BudgetId = m.MeetingRequest.BudgetsId,
                    MeetingRoomId = m.MeetingRequest.MeetingRoomId,
                    MeetingTypeId = m.MeetingRequest.MeetingTypeId,
                    NumOfPartisipant = m.MeetingRequest.NumOfParticipant,
                    TotalBudgetBook = m.MeetingRequest.TotalBudgetBook,
                    Requester = m.MeetingRequest.Requester.Name,
                    MeetingRoomsName = m.MeetingRequest.MeetingRoom.Name,
                    MeetingTypeName = m.MeetingRequest.MeetingType.JenisNama,
                    RequesterId = m.MeetingRequest.RequesterId,
                    MeetingTitle = m.MeetingRequest.MeetingTitle,
                    MeetingDesc = m.MeetingRequest.MeetingDesc,
                    CreateDate = m.MeetingRequest.CreatedAt.Value,
                    MeetingRequestNo = m.MeetingRequest.MeetingRequestNo,
                    PicId = m.MeetingRequest.PicId,
                    FundAvailable = m.MeetingRequest.FundAvailable,
                    NoAkun = m.MeetingRequest.NoAkun,
                    DepartmentId = m.MeetingRequest.DepartmentId.Value,
                    LocationId = m.MeetingRequest.MeetingRoom.Room.OfficeLocationId,
                    Location = m.MeetingRequest.MeetingRoom.Room.OfficeLocation.Name,
                    MeetingRoomCategoryId = m.MeetingRequest.MeetingRoom.RoomCategoryId.Value,
                    MeetingRoomCategoryName = m.MeetingRoom.RoomCategory.Name,
                    StateName = m.Request.Currentstate.Name,
                    StateNameType = m.Request.Currentstate.Statetype.Name,
                    RequestId = m.RequestId ?? 0,
                    MeetingRequestTime = m.MeetingRequest.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                    {
                        Id = t.Id,
                        StartDate = t.StartDate.Value,
                        EndDate = t.EndDate.Value,
                    }).ToList(),
                    MeetingRequestBudgets = m.MeetingRequest.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                    {
                        Amount = b.Amount,
                        QtyDays = b.QtyDays,
                        QtyPerson = b.QtyPerson,
                        TotalAmount = b.TotalAmount,
                        Id = b.Id,
                        MeetingBudgetId = b.MeetingBudgetId.Value,
                        Name = b.MeetingBudget.BudgetName
                    }).ToList()
                }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<MeetingRequestAccountabilityDTO>> GetAccountablityList(int start, int take, string filter, string order)
        {
            var repos = db.MeetingRequestAccountabilityFlow
                .Include(m => m.MeetingRequestAccountability)
                    .ThenInclude(m => m.MeetingRequest)
                        .ThenInclude(m => m.MeetingRequestTime)
                .Include(m => m.MeetingRequestAccountability)
                    .ThenInclude(m => m.MeetingRequest)
                        .ThenInclude(m => m.MeetingRequestBudgets)
                .AsQueryable();

            int totalData = repos.Count();
            int totalFilterData = totalData;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m =>
                        m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Name.ToLower().Contains(item.ToLower()) ||
                        m.MeetingRequestAccountability.MeetingRequest.MeetingRequestNo.ToLower().Contains(item.ToLower()) ||
                        m.MeetingRequestAccountability.MeetingRequest.MeetingTitle.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                repos = repos.OrderBy(order);
            }

            return new Control<MeetingRequestAccountabilityDTO>()
            {

                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new MeetingRequestAccountabilityDTO
                    {
                        MeetingRequestId = m.MeetingRequestAccountability.MeetingRequestId ?? 0,
                        NumOfParticipant = m.MeetingRequestAccountability.NumOfPartisipant ?? 0,
                        PicID = m.MeetingRequestAccountability.PicId ?? 0,
                        PicName = m.MeetingRequestAccountability.Pic.Name,
                        TotalBudgetReal = m.MeetingRequestAccountability.TotalBudgetReal ?? 0,
                        MeetingRequestNo = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestNo,
                        MeetingTitle = m.MeetingRequestAccountability.MeetingRequest.MeetingTitle,
                        MeetingDesc = m.MeetingRequestAccountability.MeetingRequest.MeetingDesc,
                        State = m.Request.Currentstate.Name,
                        Id = m.MeetingRequestAccountability.Id,
                        RequestId = m.RequestId.Value
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<IList<MeetingBudget>> MeetingButget(int start, int take, string filter)
        {

            var repos = db.MeetingBudget.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Name.ToLower().Contains(item.ToLower()));
                }
            }

            return await repos.Skip(start * take).Take(take).ToListAsync();



        }

        public async Task<Control<MeetingRequestDTO>> GetList(int start, int take, string filter, string order)
        {


            var repos = db.MeetingRequestFlow
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRoom)
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRequestTime)
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRequestBudgets)
                        .ThenInclude(m => m.MeetingBudget)
                .AsQueryable();

            int totalData = repos.Count();
            int totalFilterData = totalData;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');


                foreach (var item in split)
                {
                    repos = repos.Where(m => m.MeetingRoom.Name.ToLower().Contains(item.ToLower()) ||
                            m.MeetingRequest.MeetingRequestNo.ToLower().Contains(item.ToLower()) ||
                            m.MeetingRequest.MeetingTitle.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                repos = repos.OrderBy(order);
            }

            return new Control<MeetingRequestDTO>()
            {

                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new MeetingRequestDTO
                    {

                        Id = m.MeetingRequest.Id,
                        BudgetId = m.MeetingRequest.BudgetsId,
                        MeetingRoomId = m.MeetingRequest.MeetingRoomId,
                        MeetingTypeId = m.MeetingRequest.MeetingTypeId,
                        NumOfPartisipant = m.MeetingRequest.NumOfParticipant,
                        TotalBudgetBook = m.MeetingRequest.TotalBudgetBook,
                        Requester = m.MeetingRequest.Requester.Name,
                        MeetingRoomsName = m.MeetingRequest.MeetingRoom.Name,
                        MeetingTypeName = m.MeetingRequest.MeetingType.JenisNama,
                        RequesterId = m.MeetingRequest.RequesterId,
                        MeetingTitle = m.MeetingRequest.MeetingTitle,
                        MeetingDesc = m.MeetingRequest.MeetingDesc,
                        CreateDate = m.MeetingRequest.CreatedAt.Value,
                        MeetingRequestNo = m.MeetingRequest.MeetingRequestNo,
                        PicId = m.MeetingRequest.PicId,
                        FundAvailable = m.MeetingRequest.FundAvailable,
                        NoAkun = m.MeetingRequest.NoAkun,
                        DepartmentId = m.MeetingRequest.DepartmentId.Value,
                        LocationId = m.MeetingRequest.MeetingRoom.Room.OfficeLocationId,
                        Location = m.MeetingRequest.MeetingRoom.Room.OfficeLocation.Name,
                        MeetingRoomCategoryId = m.MeetingRequest.MeetingRoom.RoomCategoryId.Value,
                        MeetingRoomCategoryName = m.MeetingRequest.MeetingRoom.RoomCategory.Name,
                        StateName = m.Request.Currentstate.Name,
                        StateNameType = m.Request.Currentstate.Statetype.Name,
                        RequestId = m.RequestId ?? 0,
                        MeetingRequestTime = m.MeetingRequest.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                        {
                            Id = t.Id,
                            StartDate = t.StartDate.Value,
                            EndDate = t.EndDate.Value
                        }).ToList(),
                        MeetingRequestBudgets = m.MeetingRequest.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                        {
                            Amount = b.Amount,
                            QtyDays = b.QtyDays,
                            QtyPerson = b.QtyPerson,
                            TotalAmount = b.TotalAmount,
                            Id = b.Id,
                            MeetingBudgetId = b.MeetingBudgetId.Value,
                            Name = b.MeetingBudget.BudgetName
                        }).ToList()
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }



        public async Task<IList<MeetingRoomsDTO>> Rooms(MeetingRequestRoomBinding meetingRequestBinding)
        {
            var repos = db.MeetingRooms.AsQueryable()
                .Where(m =>
                    m.RoomCategoryId == meetingRequestBinding.CategoryId &&
                    m.Room.OfficeLocationId == meetingRequestBinding.LocationId);

            var requestDate = meetingRequestBinding.RequestDate;

            foreach (var date in requestDate)
            {
                var currentStart = date.StartDate;
                var currentEnd = date.EndDate;

                repos = repos.
                    Where(m =>
                        m.MeetingRequests.Any(r =>
                        !r.MeetingRequestTime.Any(t =>
                        t.StartDate >= date.StartDate && t.EndDate <= date.EndDate)));
            }

            return await repos.Select(m => new MeetingRoomsDTO
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                PicId = m.PicId,
                PersonPICName = m.Pic.Name,
                RoomId = m.RoomId,
                RoomName = m.Room.Name,
                RoomCategoryId = m.RoomCategoryId,
                RoomCategoryName = m.RoomCategory.Name
            }).ToListAsync();

        }

        public async Task<MeetingRequests> Save(MeetingRequests entity, bool submit, int kodePusatBiaya)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {
                var user = await _userService.GetUserById(entity.RequesterId);
                entity.CreatedAt = DateTime.Now;
                var prFix = $"01{kodePusatBiaya.ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.MeetingRequestNo = await GenerateNoMeetingNo(prFix);
                await db.MeetingRequests.AddAsync(entity);
                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.MeetingRequest),
                    Title = entity.MeetingRequestNo,
                    Note = entity.MeetingTitle,
                    Processid = await _requestService.ProgressId(ERequestType.MeetingRequest),
                    Userid = entity.RequesterId.ToString(),
                    Url = "meeting/dashboard/request/"
                };

                var requestFlow = new MeetingRequestFlow();

                requestFlow.MeetingRoomId = entity.MeetingRoomId;

                var transitionList = await _requestService.
                    TransitionList(ERequestType.MeetingRequest);
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
                    request.Requestaction.Add(raNext);
                    firstInput = false;
                }


                requestFlow.Request = request;

                entity.MeetingRequestFlow.Add(requestFlow);

            }
            else
            {
                //removing data delete meeting request time
                var datadb = db.MeetingRequestTime.Where(m => m.MeetingRequestId == entity.Id
                                && entity.MeetingRequestTime.Any(t => t.Id != m.Id));
                db.MeetingRequestTime.RemoveRange(datadb);

                //removing data delete meeting request time
                var dataBudget = db.MeetingRequestBudgets.Where(m => m.MeetingRequestId == entity.Id
                                && entity.MeetingRequestBudgets.Any(t => t.Id != m.Id));
                db.MeetingRequestBudgets.RemoveRange(dataBudget);




                entity.UpdatedAt = DateTime.Now;


                db.MeetingRequests.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;

                request = db.MeetingRequestFlow.Include(m => m.Request)
                    .Single(m => m.MeetingRequestId == entity.Id).Request;
            }

            await db.SaveChangesAsync();


            if (submit == true)
            {
                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.RequesterId
                    });
            }

            return entity;
        }

        public async Task<MeetingRequests> UpdateEntity(int entityId, int anggaranId)
        {

            var data = await db.MeetingRequests.SingleAsync(m => m.Id == entityId);

            data.StatusAnggaranId = anggaranId;
            db.Update(data);
            await db.SaveChangesAsync();
            return data;

        }


        private async Task<string> GenerateNoMeetingNo(string prFix)
        {
            var latestNo = await db.MeetingRequests.OrderByDescending(m => m.MeetingRequestNo).
                FirstOrDefaultAsync(m => m.MeetingRequestNo.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.MeetingRequestNo.Substring(latestNo.MeetingRequestNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }

        public async Task<MeetingRequestAccountability> SaveAccountablity(MeetingRequestAccountability entity, bool submit)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;

                await db.MeetingRequestAccountability.AddAsync(entity);

                var meetingRequest = await db.MeetingRequests.SingleAsync(m => m.Id == entity.MeetingRequestId);

                var reg = db.MeetingRequests.Where(m => m.Id == entity.MeetingRequestId).Select(m => m.MeetingRoom.Room.OfficeLocation.RegionId).SingleOrDefault();

                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.MeetingRequestConfirmation),
                    Title = meetingRequest.MeetingRequestNo,
                    Note = meetingRequest.MeetingTitle,
                    Processid = await _requestService.ProgressId(ERequestType.MeetingRequestConfirmation),
                    Userid = meetingRequest.RequesterId.ToString(),
                    Url = "meeting/dashboard/request/confirm/"
                };

                var requestFlow = new MeetingRequestAccountabilityFlow();
                var transitionList = await _requestService.TransitionList(
                    ERequestType.MeetingRequestConfirmation);

                requestFlow.RegionalId = reg;

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
                    request.Requestaction.Add(raNext);
                    firstInput = false;
                }
                requestFlow.Request = request;
                entity.MeetingRequestAccountabilityFlow.Add(requestFlow);
            }
            else
            {
                entity.UpdatedAt = DateTime.Now;

                //removing data delete meeting request time
                var datadb = db.MeetingRequestAccountabilityFiles.Where(m => m.MeetingRequestAccountabilityId == entity.Id
                                && entity.MeetingRequestAccountabilityFiles.Any(t => t.Id != m.Id));
                db.MeetingRequestAccountabilityFiles.RemoveRange(datadb);


                db.MeetingRequestAccountability.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;

                request = db.MeetingRequestAccountabilityFlow.Include(m => m.Request).Single(m => m.MeetingRequestAccountabilityId == entity.Id).Request;
            }
            await db.SaveChangesAsync();



            if (submit == true)
            {
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

        public async Task<MeetingRequestAccountabilityDTO> GetAccountablity(int id)
        {
            return await db.MeetingRequestAccountabilityFlow
                 .Include(m => m.MeetingRequestAccountability)
                    .ThenInclude(m => m.MeetingRequest)
                        .ThenInclude(m => m.MeetingRequestTime)
                .Include(m => m.MeetingRequestAccountability)
                    .ThenInclude(m => m.MeetingRequest)
                        .ThenInclude(m => m.MeetingRequestBudgets)
                .Select(m => new MeetingRequestAccountabilityDTO()
                {
                    MeetingRequest = new MeetingRequestDTO()
                    {
                        Id = m.Id,
                        BudgetId = m.MeetingRequestAccountability.MeetingRequest.BudgetsId,
                        MeetingRoomId = m.MeetingRequestAccountability.MeetingRequest.MeetingRoomId,
                        MeetingTypeId = m.MeetingRequestAccountability.MeetingRequest.MeetingTypeId,
                        NumOfPartisipant = m.MeetingRequestAccountability.MeetingRequest.NumOfParticipant,
                        TotalBudgetBook = m.MeetingRequestAccountability.MeetingRequest.TotalBudgetBook,
                        Requester = m.MeetingRequestAccountability.MeetingRequest.Requester.Name,
                        MeetingRoomsName = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Name,
                        MeetingTypeName = m.MeetingRequestAccountability.MeetingRequest.MeetingType.JenisNama,
                        RequesterId = m.MeetingRequestAccountability.MeetingRequest.RequesterId,
                        MeetingTitle = m.MeetingRequestAccountability.MeetingRequest.MeetingTitle,
                        MeetingDesc = m.MeetingRequestAccountability.MeetingRequest.MeetingDesc,
                        CreateDate = m.MeetingRequestAccountability.MeetingRequest.CreatedAt.Value,
                        MeetingRequestNo = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestNo,
                        PicId = m.MeetingRequestAccountability.MeetingRequest.PicId,
                        FundAvailable = m.MeetingRequestAccountability.MeetingRequest.FundAvailable,
                        NoAkun = m.MeetingRequestAccountability.MeetingRequest.NoAkun,
                        DepartmentId = m.MeetingRequestAccountability.MeetingRequest.DepartmentId.Value,
                        LocationId = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Room.OfficeLocationId,
                        Location = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Room.OfficeLocation.Name,
                        MeetingRoomCategoryId = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.RoomCategoryId.Value,
                        MeetingRoomCategoryName = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.RoomCategory.Name,
                        RequestId = m.RequestId ?? 0,
                        MeetingRequestTime = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                        {
                            Id = t.Id,
                            StartDate = t.StartDate.Value,
                            EndDate = t.EndDate.Value,
                        }).ToList(),
                        MeetingRequestBudgets = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                        {
                            Amount = b.Amount,
                            QtyDays = b.QtyDays,
                            QtyPerson = b.QtyPerson,
                            TotalAmount = b.TotalAmount,
                            Id = b.Id,
                            MeetingBudgetId = b.MeetingBudgetId.Value,
                            Name = b.MeetingBudget.BudgetName
                        }).ToList()

                    },
                    MeetingRequestId = m.MeetingRequestAccountability.MeetingRequestId ?? 0,
                    NumOfParticipant = m.MeetingRequestAccountability.NumOfPartisipant ?? 0,
                    PicID = m.MeetingRequestAccountability.PicId.Value,
                    PicName = m.MeetingRequestAccountability.Pic.Name,
                    State = m.Request.Currentstate.Name,
                    TotalBudgetReal = m.MeetingRequestAccountability.TotalBudgetReal ?? 0,
                    MeetingRequestFilesDTO = m.MeetingRequestAccountability
                    .MeetingRequestAccountabilityFiles.Select(f => new MeetingRequestFilesDTO()
                    {
                        Id = f.Id,
                        FilesLocation = f.UploadFiles
                    }).ToList()
                }).SingleOrDefaultAsync(m => m.Id == id);


        }

        public async Task<IList<MeetingRequestDTO>> GetRoomId(int id)
        {
            return await db.MeetingRequests.Where(m => m.MeetingRoomId == id)
                .Select(m => new MeetingRequestDTO()
                {

                    Id = m.Id,
                    BudgetId = m.BudgetsId,
                    MeetingRoomId = m.MeetingRoomId,
                    MeetingTypeId = m.MeetingTypeId,
                    NumOfPartisipant = m.NumOfParticipant,
                    TotalBudgetBook = m.TotalBudgetBook,
                    Requester = m.Requester.Name,
                    MeetingRoomsName = m.MeetingRoom.Name,
                    MeetingTypeName = m.MeetingType.JenisNama,
                    RequesterId = m.RequesterId,
                    MeetingTitle = m.MeetingTitle,
                    MeetingDesc = m.MeetingDesc,
                    CreateDate = m.CreatedAt.Value,
                    MeetingRequestNo = m.MeetingRequestNo,
                    PicId = m.PicId,
                    FundAvailable = m.FundAvailable,
                    NoAkun = m.NoAkun,
                    DepartmentId = m.DepartmentId.Value,
                    LocationId = m.MeetingRoom.Room.OfficeLocationId,
                    Location = m.MeetingRoom.Room.OfficeLocation.Name,
                    MeetingRoomCategoryId = m.MeetingRoom.RoomCategoryId.Value,
                    MeetingRoomCategoryName = m.MeetingRoom.RoomCategory.Name,
                    MeetingRequestTime = m.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                    {
                        Id = t.Id,
                        StartDate = t.StartDate.Value,
                        EndDate = t.EndDate.Value,
                    }).ToList(),
                    MeetingRequestBudgets = m.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                    {
                        Amount = b.Amount,
                        TotalAmount = b.TotalAmount,
                        QtyDays = b.QtyDays,
                        QtyPerson = b.QtyPerson,
                        Id = b.Id,
                        MeetingBudgetId = b.MeetingBudgetId.Value,
                        Name = b.MeetingBudget.BudgetName
                    }).ToList()
                }).ToListAsync();

        }

        public async Task<IList<MeetingRequestTimeDTO>> GetRequestRoomId(MeetingRequestRoomId meetingRequest)
        {
            return await db.MeetingRequestTime.Include(m => m.MeetingRequest)
                .Where(m =>
                m.MeetingRequest.MeetingRoomId == meetingRequest.RoomId &&
                m.StartDate >= meetingRequest.RequestDate.StartDate &&
                m.EndDate <= meetingRequest.RequestDate.EndDate).Select(m => new MeetingRequestTimeDTO()
                {
                    MeetingAgendaNo = m.MeetingRequest.MeetingRequestNo,
                    EndDate = m.EndDate.Value,
                    StartDate = m.StartDate.Value,
                    Id = m.Id,
                    Sequence = m.Sequence ?? 0
                }).ToListAsync();
        }

        async Task<Control<MeetingRequestDTO>> IMeetingRequest.GetAccountabilityRequestList(int start, int take, string filter, string order)
        {
            var repos = db.MeetingRequestFlow
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRequestTime)
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRequestBudgets)
                .Include(m => m.Request)
                    .ThenInclude(m => m.Currentstate)
                .AsQueryable();

            var transitionList = await _requestService.TransitionList(
                ERequestType.MeetingRequest);

            var transition = transitionList.Single(n => n.TransitionEnd == true && n.Flowtype == "Next");

            repos = repos.Where(m =>
            m.Request.Currentstateid == transition.Nextstateid &&
            m.MeetingRequest.MeetingRequestAccountability.Count() == 0);


            int totalData = repos.Count();
            int totalFilterData = totalData;


            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.MeetingRequest.MeetingRoom.Name.ToLower().Contains(item.ToLower()) ||
                            m.MeetingRequest.MeetingRequestNo.ToLower().Contains(item.ToLower()) ||
                            m.MeetingRequest.MeetingTitle.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                repos = repos.OrderBy(order);
            }

            return new Control<MeetingRequestDTO>()
            {

                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new MeetingRequestDTO
                    {

                        Id = m.MeetingRequestId.Value,
                        BudgetId = m.MeetingRequest.BudgetsId,
                        MeetingRoomId = m.MeetingRequest.MeetingRoomId,
                        MeetingTypeId = m.MeetingRequest.MeetingTypeId,
                        NumOfPartisipant = m.MeetingRequest.NumOfParticipant,
                        TotalBudgetBook = m.MeetingRequest.TotalBudgetBook,
                        Requester = m.MeetingRequest.Requester.Name,
                        MeetingRoomsName = m.MeetingRequest.MeetingRoom.Name,
                        MeetingTypeName = m.MeetingRequest.MeetingType.JenisNama,
                        RequesterId = m.MeetingRequest.RequesterId,
                        MeetingTitle = m.MeetingRequest.MeetingTitle,
                        MeetingDesc = m.MeetingRequest.MeetingDesc,
                        CreateDate = m.MeetingRequest.CreatedAt.Value,
                        MeetingRequestNo = m.MeetingRequest.MeetingRequestNo,
                        PicId = m.MeetingRequest.PicId,
                        FundAvailable = m.MeetingRequest.FundAvailable,
                        NoAkun = m.MeetingRequest.NoAkun,
                        DepartmentId = m.MeetingRequest.DepartmentId.Value,
                        LocationId = m.MeetingRequest.MeetingRoom.Room.OfficeLocationId,
                        Location = m.MeetingRequest.MeetingRoom.Room.OfficeLocation.Name,
                        MeetingRoomCategoryId = m.MeetingRequest.MeetingRoom.RoomCategoryId.Value,
                        MeetingRoomCategoryName = m.MeetingRequest.MeetingRoom.RoomCategory.Name,
                        RequestId = m.RequestId ?? 0,
                        MeetingRequestTime = m.MeetingRequest.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                        {
                            Id = t.Id,
                            StartDate = t.StartDate.Value,
                            EndDate = t.EndDate.Value,
                        }).ToList(),
                        MeetingRequestBudgets = m.MeetingRequest.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                        {
                            Amount = b.Amount,
                            QtyDays = b.QtyDays,
                            QtyPerson = b.QtyPerson,
                            TotalAmount = b.TotalAmount,
                            Id = b.Id,
                            MeetingBudgetId = b.MeetingBudgetId.Value,
                            Name = b.MeetingBudget.BudgetName
                        }).ToList()

                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<MeetingRequests> GetMeetingRequestId(int requestId)
        {
            return await db.MeetingRequestFlow.Where(m => m.RequestId == requestId)
                .Select(m => m.MeetingRequest).SingleAsync();
        }

        public async Task<MeetingRequestDTO> GetByRequestId(int requestId)
        {
            return await db.MeetingRequestFlow
                .Include(m => m.Request)
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRequestBudgets)
                .Include(m => m.MeetingRequest)
                    .ThenInclude(m => m.MeetingRequestTime)
                .Where(m => m.RequestId == requestId)
                .Select(m => new MeetingRequestDTO()
                {

                    Id = m.Id,
                    BudgetId = m.MeetingRequest.BudgetsId,
                    MeetingRoomId = m.MeetingRequest.MeetingRoomId,
                    MeetingTypeId = m.MeetingRequest.MeetingTypeId,
                    NumOfPartisipant = m.MeetingRequest.NumOfParticipant,
                    TotalBudgetBook = m.MeetingRequest.TotalBudgetBook,
                    Requester = m.MeetingRequest.Requester.Name,
                    MeetingRoomsName = m.MeetingRequest.MeetingRoom.Name,
                    MeetingTypeName = m.MeetingRequest.MeetingType.JenisNama,
                    RequesterId = m.MeetingRequest.RequesterId,
                    MeetingTitle = m.MeetingRequest.MeetingTitle,
                    MeetingDesc = m.MeetingRequest.MeetingDesc,
                    CreateDate = m.MeetingRequest.CreatedAt.Value,
                    MeetingRequestNo = m.MeetingRequest.MeetingRequestNo,
                    PicId = m.MeetingRequest.PicId,
                    FundAvailable = m.MeetingRequest.FundAvailable,
                    NoAkun = m.MeetingRequest.NoAkun,
                    DepartmentId = m.MeetingRequest.DepartmentId.Value,
                    LocationId = m.MeetingRequest.MeetingRoom.Room.OfficeLocationId,
                    Location = m.MeetingRequest.MeetingRoom.Room.OfficeLocation.Name,
                    MeetingRoomCategoryId = m.MeetingRequest.MeetingRoom.RoomCategoryId.Value,
                    MeetingRoomCategoryName = m.MeetingRequest.MeetingRoom.RoomCategory.Name,
                    RequestId = m.RequestId ?? 0,
                    StateName = m.Request.Currentstate.Name,
                    StateNameType = m.Request.Currentstate.Statetype.Name,
                    MeetingRequestTime = m.MeetingRequest.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                    {
                        Id = t.Id,
                        StartDate = t.StartDate.Value,
                        EndDate = t.EndDate.Value,
                    }).ToList(),
                    MeetingRequestBudgets = m.MeetingRequest.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                    {
                        Amount = b.Amount,
                        TotalAmount = b.TotalAmount,
                        QtyDays = b.QtyDays,
                        QtyPerson = b.QtyPerson,
                        Id = b.Id,
                        MeetingBudgetId = b.MeetingBudgetId.Value,
                        Name = b.MeetingBudget.BudgetName
                    }).ToList()
                }).SingleAsync();
        }


        public async Task<MeetingRequestAccountabilityDTO> GetByRequestConfirmId(int requestId)
        {
            return await db.MeetingRequestAccountabilityFlow
                .Include(m => m.Request)
                .Include(m => m.MeetingRequestAccountability)
                    .ThenInclude(m => m.MeetingRequest)
                        .ThenInclude(m => m.MeetingRequestBudgets)
                .Include(m => m.MeetingRequestAccountability)
                    .ThenInclude(m => m.MeetingRequest)
                        .ThenInclude(m => m.MeetingRequestTime)
                .Where(m => m.RequestId == requestId)
                .Select(m => new MeetingRequestAccountabilityDTO()
                {
                    MeetingRequest = new MeetingRequestDTO()
                    {
                        Id = m.MeetingRequestAccountability.MeetingRequestId.Value,
                        BudgetId = m.MeetingRequestAccountability.MeetingRequest.BudgetsId,
                        MeetingRoomId = m.MeetingRequestAccountability.MeetingRequest.MeetingRoomId,
                        MeetingTypeId = m.MeetingRequestAccountability.MeetingRequest.MeetingTypeId,
                        NumOfPartisipant = m.MeetingRequestAccountability.MeetingRequest.NumOfParticipant,
                        TotalBudgetBook = m.MeetingRequestAccountability.MeetingRequest.TotalBudgetBook,
                        Requester = m.MeetingRequestAccountability.MeetingRequest.Requester.Name,
                        MeetingRoomsName = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Name,
                        MeetingTypeName = m.MeetingRequestAccountability.MeetingRequest.MeetingType.JenisNama,
                        RequesterId = m.MeetingRequestAccountability.MeetingRequest.RequesterId,
                        MeetingTitle = m.MeetingRequestAccountability.MeetingRequest.MeetingTitle,
                        MeetingDesc = m.MeetingRequestAccountability.MeetingRequest.MeetingDesc,
                        CreateDate = m.MeetingRequestAccountability.MeetingRequest.CreatedAt.Value,
                        MeetingRequestNo = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestNo,
                        PicId = m.MeetingRequestAccountability.MeetingRequest.PicId,
                        FundAvailable = m.MeetingRequestAccountability.MeetingRequest.FundAvailable,
                        NoAkun = m.MeetingRequestAccountability.MeetingRequest.NoAkun,
                        DepartmentId = m.MeetingRequestAccountability.MeetingRequest.DepartmentId.Value,
                        LocationId = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Room.OfficeLocationId,
                        Location = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.Room.OfficeLocation.Name,
                        MeetingRoomCategoryId = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.RoomCategoryId.Value,
                        MeetingRoomCategoryName = m.MeetingRequestAccountability.MeetingRequest.MeetingRoom.RoomCategory.Name,
                        MeetingRequestTime = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestTime.Select(t => new MeetingRequestTimeDTO()
                        {
                            Id = t.Id,
                            StartDate = t.StartDate.Value,
                            EndDate = t.EndDate.Value,
                        }).ToList(),
                        MeetingRequestBudgets = m.MeetingRequestAccountability.MeetingRequest.MeetingRequestBudgets.Select(b => new MeetingRequestBudgetDTO()
                        {
                            Amount = b.Amount,
                            QtyDays = b.QtyDays,
                            QtyPerson = b.QtyPerson,
                            TotalAmount = b.TotalAmount,
                            Id = b.Id,
                            MeetingBudgetId = b.MeetingBudgetId.Value,
                            Name = b.MeetingBudget.BudgetName
                        }).ToList()

                    },
                    MeetingRequestId = m.MeetingRequestAccountability.MeetingRequestId ?? 0,
                    NumOfParticipant = m.MeetingRequestAccountability.NumOfPartisipant ?? 0,
                    PicID = m.MeetingRequestAccountability.PicId.Value,
                    PicName = m.MeetingRequestAccountability.Pic.Name,
                    State = m.Request.Currentstate.Name,
                    TotalBudgetReal = m.MeetingRequestAccountability.TotalBudgetReal ?? 0,
                    Id = m.MeetingRequestAccountability.Id,
                    RequestId = m.RequestId.Value,
                    MeetingRequestFilesDTO = m.MeetingRequestAccountability
                    .MeetingRequestAccountabilityFiles.Select(f => new MeetingRequestFilesDTO()
                    {
                        Id = f.Id,
                        FilesLocation = f.UploadFiles
                    }).ToList()
                }).SingleAsync();
        }

        public async Task<IList<MeetingRequestTime>> GetRoomValidate(DateTime startTime, DateTime endTime, int roomId)
        {
            return await db.MeetingRequestTime.Where(m =>
            m.StartDate >= startTime &&
            m.EndDate <= endTime &&
            m.MeetingRequest.MeetingRoomId == roomId).ToListAsync();
        }

    }
}
