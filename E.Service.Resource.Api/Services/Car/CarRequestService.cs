using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Car
{
    public class CarRequestService : ICarRequestService
    {

        private EservicesdbContext db;

        IRequestService _requestService;
        IUserService _userService;

        public CarRequestService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            this.db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<Control<CarRequestDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.CarRequests.AsQueryable();
            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Description.ToLower().Contains(item.ToLower()) ||
                        m.Destination.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<CarRequestDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarRequestDTO
                    {
                        Id = m.Id,
                        Description = m.Description,
                        CarPoolId = m.CarPoolId,
                        CarPoolLicenseNo = m.CarPool.LicensePlate,
                        carPoolName = m.CarPool.Name,
                        Destination = m.Destination,
                        Done = m.Done ?? false,
                        EndTime = m.EndTime,
                        RegionalId = m.RegionalId,
                        RegionalName = m.Regional.Name,
                        RequesterId = m.RequesterId,
                        RequiredAt = m.RequiredAt,
                        StartTime = m.StartTime,

                        RequestEndTime = m.CarRequestBudget.RequestEnd,
                        RequestStartTime = m.CarRequestBudget.RequestStart,
                        Status = m.Request.Currentstate.Name,
                        StatusType = m.Request.Currentstate.Statetype.Name,
                        RequestId = m.RequestId,
                        RequesterName = m.Requester.Name,
                        UsingDriver = m.UsingDriver,
                        CarRequestNo = m.CarRequestNo,
                        Balance = m.CarRequestBudget.Balance ?? 0,
                        CurrentDriverId = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.DriverId ?? 0 : 0,
                        CurrentDriverName = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.Driver.Name : ""

                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }


        public async Task<Control<CarRequestDTO>> GetDriverJobList(int start, int take, string filter, string order, int driverId, bool showComplete, bool check)
        {
            var completePIC = ECarRequest.COMPLETEPIC.Description();

            var repos = db.CarRequests.Where(m =>
                    m.Request.Currentstate.Name == ECarRequest.COMPLETEPIC.Description() ||
                    m.Request.Currentstate.Name == ECarRequest.ADMINREVIEW.Description() ||
                    m.Request.Currentstate.Name == ECarRequest.COMPLETE.Description() ||
                    m.Request.Currentstate.Name == "Admin Revisi")
                    .AsQueryable();


            if (showComplete)
            {
                repos = repos.Where(m => m.CarRequestBudget.Checked == check);
                repos = repos.Where(m =>
                m.CarRequestBudget.RequestEnd != null &&
                m.CarRequestBudget.RequestStart != null);
            }


            if (driverId != 0)
            {
                repos = repos.Where(m => m.CarRequestBudget.Checked == check);
                repos = repos.Where(m => m.CarRequestBudget.DriverId == driverId);
            }

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Description.ToLower().Contains(item.ToLower()) ||
                        m.Destination.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<CarRequestDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarRequestDTO
                    {
                        Id = m.Id,
                        Description = m.Description,
                        CarPoolId = m.CarPoolId,
                        CarPoolLicenseNo = m.CarPool.LicensePlate,
                        carPoolName = m.CarPool.Name,
                        Destination = m.Description,
                        Done = m.Done ?? false,
                        EndTime = m.EndTime,
                        RegionalId = m.RegionalId,
                        RegionalName = m.Regional.Name,
                        RequesterId = m.RequesterId,
                        RequiredAt = m.RequiredAt,
                        StartTime = m.StartTime,
                        RequestEndTime = m.CarRequestBudget.RequestEnd,
                        RequestStartTime = m.CarRequestBudget.RequestStart,
                        Status = m.Request.Currentstate.Name,
                        StatusType = m.Request.Currentstate.Statetype.Name,
                        RequestId = m.RequestId,
                        RequesterName = m.Requester.Name,
                        UsingDriver = m.UsingDriver,
                        Balance = m.CarRequestBudget.Balance ?? 0,
                        CarRequestNo = m.CarRequestNo,
                        CurrentDriverId = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.DriverId ?? 0 : 0,
                        CurrentDriverName = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.Driver.Name : ""
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }


        public async Task<CarRequestDTO> Get(int id)
        {
            return await db.CarRequests.Select(m => new CarRequestDTO()
            {
                Id = m.Id,
                CarRequestNo = m.CarRequestNo,
                Description = m.Description,
                CarPoolId = m.CarPoolId,
                CarPoolLicenseNo = m.CarPool.LicensePlate,
                carPoolName = m.CarPool.Name,
                Destination = m.Destination,
                Done = m.Done ?? false,
                EndTime = m.EndTime,
                RegionalId = m.RegionalId,
                RegionalName = m.Regional.Name,
                RequesterId = m.RequesterId,
                RequiredAt = m.RequiredAt,
                StartTime = m.StartTime,
                RequesterName = m.Requester.Name,
                Status = m.Request.Currentstate.Name,
                StatusType = m.Request.Currentstate.Statetype.Name,
                RequestId = m.RequestId,
                UsingDriver = m.UsingDriver,
                Balance = m.CarRequestBudget.Balance ?? 0,
                CurrentDriverId = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.DriverId ?? 0 : 0,
                CurrentDriverName = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.Driver.Name : ""
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<CarRequestBudgetDTO>> GetBudgets(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.CarRequests.Where(m =>
            m.CarRequestBudget.RequestStart != null &&
            m.CarRequestBudget.RequestEnd != null).AsQueryable();

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m =>
                        m.Description.ToLower().Contains(item.ToLower()) ||
                        m.Destination.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<CarRequestBudgetDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarRequestBudgetDTO
                    {
                        RequestId = m.RequestId,
                        CarRequestId = m.Id,
                        RequestStart = m.CarRequestBudget.RequestStart,
                        RequestEnd = m.CarRequestBudget.RequestEnd,
                        TransactionDate = m.CarRequestBudget.TransactionDate,
                        Status = m.Request.Currentstate.Name,
                        CarRequestBudgetNo = m.CarRequestBudget.CarRequestBudgetNo,
                        DriverId = m.CarRequestBudget.DriverId,
                        Description = m.Description,
                        DriverName = m.CarRequestBudget.Driver != null ? m.CarRequestBudget.Driver.Name : null,
                        DriverPhone = m.CarRequestBudget.Driver != null ? m.CarRequestBudget.Driver.PhoneNumber : null,
                        CarBudgetDetil = m.CarRequestBudget.CarBudgetDetail
                            .Select(c => new CarDetilBudgetDTO()
                            {
                                Description = c.Description,
                                Filelocation = c.FileLocation,
                                Name = c.Name,
                                Nominal = c.Nominal ?? 0,
                                Id = c.Id,
                                Done = c.Done,
                                CarRequestBudgetId = m.Id,
                                CarRequestDetailStatusId = c.CarRequestDetailStatusId.Value,
                                CarRequestDetailStatusName = c.CarRequestDetailStatus.Name
                            }).ToList()
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<CarRequestBudgetDTO> GetBudgetId(int carRequestId)
        {
            var data = await db.CarRequestBudget.Where(m => m.CarRequestId == carRequestId)
                .Select(m => new CarRequestBudgetDTO()
                {
                    CarRequestId = m.CarRequestId,
                    RequestStart = m.RequestStart,
                    RequestEnd = m.RequestEnd,
                    RequestId = m.CarRequest.RequestId.Value,
                    TransactionDate = m.TransactionDate.Value,
                    Status = m.CarRequest.Request.Currentstate.Name,
                    CarRequestBudgetNo = m.CarRequestBudgetNo,
                    DriverId = m.DriverId,
                    Description = m.Description,
                    DriverName = m.Driver != null ? m.Driver.Name : null,
                    DriverPhone = m.Driver != null ? m.Driver.PhoneNumber : null,
                    CarBudgetDetil = m.CarBudgetDetail.Select(c =>
                    new CarDetilBudgetDTO()
                    {
                        Description = c.Description,
                        Filelocation = c.FileLocation,
                        Id = c.Id,
                        Name = c.Name,
                        Nominal = c.Nominal ?? 0,
                        Done = c.Done,
                        CarRequestBudgetId = m.CarRequestId,
                        CarRequestDetailStatusId = c.CarRequestDetailStatusId.Value,
                        CarRequestDetailStatusName = c.CarRequestDetailStatus.Name
                    }
                    ).ToList()
                }).SingleOrDefaultAsync();
            return data;
        }

        public async Task<CarRequestBudgetDTO> GetByConfirmRequestId(int id)
        {
            var carRequestBudgetData = await db.CarRequestBudget
                .Where(m => m.CarRequest.RequestId == id)
                .Select(m => new CarRequestBudgetDTO()
                {
                    CarRequestId = m.CarRequestId,
                    RequestStart = m.RequestStart,
                    RequestEnd = m.RequestEnd,
                    RequestId = m.CarRequest.RequestId.Value,
                    TransactionDate = m.TransactionDate.Value,
                    Status = m.CarRequest.Request.Currentstate.Name,
                    CarRequestBudgetNo = m.CarRequestBudgetNo,
                    DriverId = m.DriverId,
                    Description = m.Description,
                    DriverName = m.Driver != null ? m.Driver.Name : null,
                    DriverPhone = m.Driver != null ? m.Driver.PhoneNumber : null,
                    CarBudgetDetil = m.CarBudgetDetail.Select(md => new CarDetilBudgetDTO()
                    {
                        Description = md.Description,
                        Filelocation = md.FileLocation,
                        Id = md.Id,
                        Name = md.Name,
                        Nominal = md.Nominal ?? 0,
                        Done = md.Done
                    }).ToList()
                }).SingleAsync();

            return carRequestBudgetData;
        }

        public async Task<CarRequestDTO> GetByRequestId(int id)
        {
            var carRequestData = await db.CarRequests.Where(m => m.RequestId == id)
               .Select(m => new CarRequestDTO()
               {
                   RequestId = m.RequestId,
                   Id = m.Id,
                   CarPoolId = m.CarPoolId,
                   CarPoolLicenseNo = m.CarPool.LicensePlate,
                   carPoolName = m.CarPool.Name,
                   Description = m.Description,
                   Destination = m.Destination,
                   Done = m.Done,
                   EndTime = m.EndTime,
                   RegionalId = m.RegionalId,
                   RegionalName = m.Regional.Name,
                   RequesterId = m.RequesterId,
                   RequesterName = m.Requester.Name,
                   RequiredAt = m.RequiredAt,
                   StartTime = m.StartTime,
                   RequestEndTime = m.CarRequestBudget.RequestEnd,
                   RequestStartTime = m.CarRequestBudget.RequestStart,
                   Status = m.Request.Currentstate.Name,
                   StatusType = m.Request.Currentstate.Statetype.Name,
                   UsingDriver = m.UsingDriver,
                   CarRequestNo = m.CarRequestNo,
                   CurrentDriverId = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.DriverId ?? 0 : 0,
                   CurrentDriverName = m.CarRequestBudget != null ?
                                       m.CarRequestBudget.Driver.Name : "",

               }).SingleOrDefaultAsync();

            return carRequestData;
        }

        public async Task<Control<CarDetilBudgetDTO>> GetDriverBudgetList(int id, int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.CarBudgetDetail.Where(m => m.CarRequestBudgetId == id);

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Description.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<CarDetilBudgetDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                  Select(m => new CarDetilBudgetDTO
                  {
                      Id = m.Id,
                      Name = m.Name,
                      Done = m.Done,
                      Filelocation = m.FileLocation,
                      Nominal = m.Nominal ?? 0,
                      CarRequestBudgetId = m.CarRequestBudgetId
                  }).ToListAsync(),
                Total = repos.Count(),
                TotalFilter = repos.Count()
            };

        }

        public async Task<CarRequestBudget> PostEnd(int id)
        {

            var carRequest = await db.CarRequestBudget.SingleAsync(m => m.CarRequestId == id);

            carRequest.RequestEnd = DateTime.Now;
            db.CarRequestBudget.Update(carRequest);
            await db.SaveChangesAsync();

            return carRequest;
        }

        public async Task<CarRequestBudget> PostStart(int id)
        {
            var carRequest = await db.CarRequestBudget.SingleOrDefaultAsync(m => m.CarRequestId == id);

            if (carRequest != null)
            {
                carRequest.RequestStart = DateTime.Now;
                db.CarRequestBudget.Update(carRequest);
            }
            else
            {
                var car = new CarRequestBudget()
                {
                    CarRequestId = id,
                    RequestStart = DateTime.Now
                };

                db.CarRequestBudget.Add(car);
            }
            await db.SaveChangesAsync();

            return carRequest;
        }

        public async Task<CarRequests> Save(CarRequests entity, bool submit)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {
                var user = await _userService.GetUserById(entity.RequesterId);
                entity.CreatedAt = DateTime.Now;
                var prFix = $"04{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";


                entity.CarRequestNo = await GenerateNo(prFix);

                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.CarRequest),
                    Title = entity.CarRequestNo,
                    Note = entity.Description,
                    Processid = await _requestService.ProgressId(ERequestType.CarRequest),
                    Userid = entity.RequesterId.ToString(),
                    Url = "car/dashboard/request/"
                };

                var transitionList = await _requestService.
                    TransitionList(ERequestType.CarRequest);
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
                entity.Request = request;
                await db.CarRequests.AddAsync(entity);
            }
            else
            {
                db.CarRequests.Update(entity);
                request = db.RequestFlow.Single(m => m.Requestid ==
                        entity.RequestId);
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

        public async Task<CarBudgetDetail> SaveDriverBudget(CarBudgetDetail car, EDriverBudgetStatusType eBudgetStatusDetailType)
        {
            if (eBudgetStatusDetailType == EDriverBudgetStatusType.Add)
            {
                car.Done = true;

                if (!db.CarRequestBudget.Any(m => m.CarRequestId == car.CarRequestBudgetId))
                {
                    db.CarRequestBudget.Add(new CarRequestBudget()
                    {
                        CarRequestId = car.CarRequestBudgetId
                    });

                }
            }


            if (car.Id == 0)
            {
                car.CarRequestDetailStatusId = (int)eBudgetStatusDetailType;

                await db.CarBudgetDetail.AddAsync(car);
            }
            else
            {

                if (string.IsNullOrEmpty(car.FileLocation))
                {
                    var carbudgetdetail = db.CarBudgetDetail.AsNoTracking()
                        .Single(m => m.Id == car.Id);
                    car.FileLocation = carbudgetdetail.FileLocation;
                }


                car.CarRequestDetailStatusId = (int)eBudgetStatusDetailType;
                db.CarBudgetDetail.Update(car);
            }
            try
            {
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }
            return car;
        }

        public async Task<CarRequestBudget> SaveRequestBudget(CarRequestBudget entity, bool submit)
        {
            //no add cause it aleready inside
            string prFix = "";
            if (entity.RequesterId != null)
            {
                var user = await _userService.GetUserById(entity.RequesterId.Value);


                prFix = $"04{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";

            }
            else
            {
                prFix = $"04{DateTime.Now.ToString("yMM")}";
            }


            if (string.IsNullOrEmpty(entity.CarRequestBudgetNo))
            {
                entity.CarRequestBudgetNo = await GenerateNo(prFix);
                entity.TransactionDate = DateTime.Now;
            }

            if (entity.DriverId.HasValue)
            {
                entity.Balance = GetCurrentDriverBudget(entity.DriverId.Value);
            }

            entity.Checked = true;

            db.CarRequestBudget.Update(entity);

            await db.SaveChangesAsync();


            if (submit == true)
            {
                var idreq = await db.CarRequests.SingleOrDefaultAsync(m => m.Id == entity.CarRequestId);

                var request = await db.RequestFlow
                   .SingleAsync(m => m.Requestid == idreq.RequestId);

                var requestFlowUrl = new RequestFlowUrl()
                {
                    Url = "car/confirm/dashboard/request/",
                    Type = "confirm"
                };

                request.RequestFlowUrl.Add(requestFlowUrl);
                db.RequestFlow.Update(request);
                await db.SaveChangesAsync();

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

        private async Task<string> GenerateNo(string prFix)
        {
            var latestNo = await db.CarRequests.OrderByDescending(m => m.CarRequestNo).
              FirstOrDefaultAsync(m => m.CarRequestNo.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.CarRequestNo.Substring(latestNo.CarRequestNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }

        public async Task UpdateDriverId(int requestId, int driverId)
        {
            var car = await db.CarRequests.Include(m => m.CarRequestBudget).SingleOrDefaultAsync(m => m.RequestId == requestId);

            if (car.CarRequestBudget == null)
            {
                var cardrive = new CarRequestBudget()
                {
                    CarRequestId = car.Id,
                    DriverId = driverId,
                    Balance = GetCurrentDriverBudget(driverId)
                };

                await db.CarRequestBudget.AddAsync(cardrive);
            }
            else
            {
                var carupdate = await db.CarRequestBudget.SingleOrDefaultAsync(m => m.CarRequestId == car.Id);

                carupdate.DriverId = driverId;
                carupdate.Balance = GetCurrentDriverBudget(driverId);

                db.CarRequestBudget.Update(carupdate);

            }
            await db.SaveChangesAsync();
        }

        public async Task<CarRequestBudgetDTO> GetConfirmId(int id)
        {
            var carRequestBudgetData = await db.CarRequestBudget
                .Where(m => m.CarRequestId == id)
                .Select(m => new CarRequestBudgetDTO()
                {
                    CarRequestId = m.CarRequestId,
                    RequestStart = m.RequestStart,
                    RequestEnd = m.RequestEnd,
                    RequestId = m.CarRequest.RequestId.Value,
                    TransactionDate = m.TransactionDate.Value,
                    Status = m.CarRequest.Request.Currentstate.Name,
                    CarRequestBudgetNo = m.CarRequestBudgetNo,
                    DriverId = m.DriverId,
                    Description = m.Description,
                    DriverName = m.Driver != null ? m.Driver.Name : null,
                    DriverPhone = m.Driver != null ? m.Driver.PhoneNumber : null,
                    CarBudgetDetil = m.CarBudgetDetail.Select(md => new CarDetilBudgetDTO()
                    {
                        Description = md.Description,
                        Filelocation = md.FileLocation,
                        Id = md.Id,
                        Name = md.Name,
                        Nominal = md.Nominal ?? 0,
                        Done = md.Done,
                        CarRequestDetailStatusId = md.CarRequestDetailStatusId.Value,
                        CarRequestDetailStatusName = md.CarRequestDetailStatus.Name
                    }).ToList(),
                    DriverInputableData = !(m.RequestStart != null &&
                        m.RequestEnd != null)
                }).SingleAsync();

            return carRequestBudgetData;
        }

        public decimal GetCurrentDriverBudget(int driverId)
        {


            //var addNominal = db.CarRequestBudget
            //           .Where(m => m.DriverId == driverId && m.Checked == true)
            //           .Sum(d => d.CarBudgetDetail
            //           .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Add && dd.Done == true)
            //           .Sum(dd => dd.Nominal ?? 0));

            //var minNominal = db.CarRequestBudget
            //           .Where(m => m.DriverId == driverId && m.Checked == true)
            //           .Sum(d => d.CarBudgetDetail
            //           .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Min && dd.Done == true)
            //           .Sum(dd => dd.Nominal ?? 0));



            return (db.CarRequestBudget
                       .Where(m => m.DriverId == driverId)
                       .Sum(d => d.CarBudgetDetail
                       .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Add && dd.Done == true)
                       .Sum(dd => dd.Nominal ?? 0))) -
                       (db.CarRequestBudget
                       .Where(m => m.DriverId == driverId)
                       .Sum(d => d.CarBudgetDetail
                       .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Min && dd.Done == true)
                       .Sum(dd => dd.Nominal ?? 0)));
        }

        public async Task<CarCoordinateInsertDTO> PostCoordinate(CarCoordinateInsertDTO coordinate)
        {

            CarRequestCoordinate carcoordinate = await db.CarRequestCoordinate.OrderByDescending(m => m.Id).FirstOrDefaultAsync();


            if (carcoordinate != null && carcoordinate.CarCoordinateStatusId == coordinate.CarCoordinateStatusId)
            {
                CarRequestCoordinateDetail carDetail = new CarRequestCoordinateDetail()
                {
                    TransactionDateTime = DateTime.Now,
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude
                };
                carcoordinate.CarRequestCoordinateDetail.Add(carDetail);
                db.CarRequestCoordinate.Update(carcoordinate);

            }
            else
            {
                carcoordinate = new CarRequestCoordinate()
                {
                    CurrentDateTime = DateTime.Now,
                    CarCoordinateStatusId = coordinate.CarCoordinateStatusId,
                    CarRequestId = coordinate.CarRequestId
                };

                CarRequestCoordinateDetail carDetail = new CarRequestCoordinateDetail()
                {
                    TransactionDateTime = DateTime.Now,
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude
                };
                carcoordinate.CarRequestCoordinateDetail.Add(carDetail);
                await db.CarRequestCoordinate.AddAsync(carcoordinate);

            }

            await db.SaveChangesAsync();
            return coordinate;
        }

        public async Task<List<ReportCarRequest>> GetReport(int regionalId)
        {
            var carreport = await db.CarRequests
                .Where(m => m.RegionalId == regionalId)
                .Include(m => m.CarRequestBudget)
                    .ThenInclude(m => m.CarBudgetDetail)
                .Include(m => m.Regional)
                .Include(m => m.CarRequestCoordinate)
                    .ThenInclude(m => m.CarCoordinateStatus)
                    .Select(m => new ReportCarRequest()
                    {
                        Id = m.Id,
                        JamBerangkat = m.CarRequestBudget.RequestStart.HasValue ? m.CarRequestBudget.RequestStart.Value.ToString("HH:mm") : "",
                        JamSampai = m.CarRequestBudget.RequestEnd.HasValue ? m.CarRequestBudget.RequestEnd.Value.ToString("HH:mm") : "",
                        BudgetAwal = (m.CarRequestBudget.CarBudgetDetail.Sum(d => d.Nominal) ?? decimal.Zero).ToString("N2"),
                        PerihalPerjalanan = m.Description,
                        Realisasi = (m.CarRequestBudget.CarBudgetDetail.Where(d => d.Done).Sum(d => d.Nominal) ?? decimal.Zero).ToString("N2"),
                        StartAwal = m.CarRequestBudget.RequestStart.HasValue ? m.CarRequestBudget.RequestStart.Value.ToString("HH:mm") : "",
                        TanggalPemesanan = m.RequiredAt.ToString("dd-MM-yyyy HH:mm"),
                        Tujuan = m.Destination,
                        Wilayah = m.Regional.Name,
                        StatusDriver = m.CarRequestCoordinate.Any() ?
                        m.CarRequestCoordinate.OrderByDescending(d => d.Id).FirstOrDefault().CarCoordinateStatus.Name : "",
                        StatusPermintaan = m.CarRequestCoordinate.Any() ?
                        m.CarRequestCoordinate.OrderByDescending(d => d.Id).FirstOrDefault().CarCoordinateStatus.Name : ""

                    }).ToListAsync();

            return carreport;
        }

        public async Task UpdateEntity(int id, int anggaranstatusId)
        {
            var data = await db.CarRequests.SingleAsync(m => m.Id == id);

            data.StatusAnggaranId = anggaranstatusId;
            db.Update(data);
            await db.SaveChangesAsync();
        }

        public async Task<List<ReportCarRequest>> GetReportSummary(int regionalId)
        {
            var repos = db.CarRequests.AsQueryable();

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.RegionalId == regionalId);
            }

            var reportData = await repos.Include(m => m.CarRequestBudget)
                   .ThenInclude(m => m.CarBudgetDetail)
               .Include(m => m.Regional)
               .Include(m => m.CarRequestCoordinate)
                   .ThenInclude(m => m.CarCoordinateStatus)
                   .Select(m => new ReportCarRequest()
                   {
                       Id = m.Id,
                       JamBerangkat = m.CarRequestBudget.RequestStart.HasValue ? m.CarRequestBudget.RequestStart.Value.ToString("HH:mm") : "",
                       JamSampai = m.CarRequestBudget.RequestEnd.HasValue ? m.CarRequestBudget.RequestEnd.Value.ToString("HH:mm") : "",
                       BudgetAwal = (m.CarRequestBudget.CarBudgetDetail.Sum(d => d.Nominal) ?? decimal.Zero).ToString("N2"),
                       PerihalPerjalanan = m.Description,
                       Realisasi = (m.CarRequestBudget.CarBudgetDetail.Where(d => d.Done).Sum(d => d.Nominal) ?? decimal.Zero).ToString("N2"),
                       StartAwal = m.CarRequestBudget.RequestStart.HasValue ? m.CarRequestBudget.RequestStart.Value.ToString("HH:mm") : "",
                       TanggalPemesanan = m.RequiredAt.ToString("dd-MM-yyyy HH:mm"),
                       Tujuan = m.Destination,
                       Wilayah = m.Regional.Name,
                       StatusDriver = m.CarRequestCoordinate.Any() ?
                       m.CarRequestCoordinate.OrderByDescending(d => d.Id).FirstOrDefault().CarCoordinateStatus.Name : "",
                       StatusPermintaan = m.CarRequestCoordinate.Any() ?
                       m.CarRequestCoordinate.OrderByDescending(d => d.Id).FirstOrDefault().CarCoordinateStatus.Name : ""
                   }).ToListAsync();


            return reportData;
        }

        public async Task<ReportCarDetailDTO> GetReportDetailLocation(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in db.CarRequests
                              where p.Id == id
                              select new ReportCarDetailDTO()
                              {
                                  Id = p.Id,
                                  RequesterName = p.Requester.Name,
                                  TransactionNo = p.CarRequestNo,
                                  Description = p.Description,
                                  DriverName = p.CarRequestBudget.Driver != null ? p.CarRequestBudget.Driver.Name : "",
                                  NamaPIC = "",
                                  TanggalPemesanan = (p.CreatedAt ?? defaultDate).ToString("dd-MM-yyyy"),
                                  TanggalPertanggungjawaban = p.RequiredAt.ToString("dd-MM-yyyy"),
                                  WaktuMulai = (p.CarRequestBudget.RequestStart ?? defaultDate).ToString("dd-MM-yyyy"),
                                  WaktuSelesai = (p.CarRequestBudget.RequestEnd ?? defaultDate).ToString("dd-MM-yyyy"),
                                  Tujuan = p.Destination
                              }).SingleAsync();

            return data;
        }

        public async Task<List<ReportCarUsageDetailDTO>> GetReportDetailUsage(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in db.CarBudgetDetail
                              where p.Id == id
                              select new ReportCarUsageDetailDTO()
                              {
                                  Id = p.Id,
                                  Deskripsi = p.Description,
                                  Done = p.Done ? "done" : "not done",
                                  Gambar = p.FileLocation,
                                
                                  Nama = p.Name,
                                  Nominal = (p.Nominal ?? 0).ToString("N2")
                              }).ToListAsync();

            return data;
        }

        public async Task<List<ReportCarDetailApprovalDTO>> GetReportDetailApproval(int id)
        {

            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in db.CarRequests
                              join requestHistory in db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                              where p.Id == id
                              select new ReportCarDetailApprovalDTO()
                              {
                                  Id = requestHistory.Id,
                                  Approval = requestHistory.HistoryType,
                                  NamaAprover = requestHistory.User.Name,
                                  StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<ReportCarDetailDateDTO> GetReportDetailDate(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in db.CarRequests
                              where p.Id == id
                              select new ReportCarDetailDateDTO()
                              {
                                  Id = p.Id,
                                  TanggalDipesan = p.RequiredAt.ToString("dd-MM-yyyy"),
                                  WaktuMulai = (p.CarRequestBudget.RequestStart ?? defaultDate).ToString("dd-MM-yyyy HH:mm"),
                                  WaktuSelesai = (p.CarRequestBudget.RequestEnd ?? defaultDate).ToString("dd-MM-yyyy HH:mm"),
                              }).SingleAsync();

            return data;
        }

        async Task<ReportCarDetailLocationDTO> ICarRequestService.GetReportDetailLocation(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await(from p in db.CarRequests
                             where p.Id == id
                             select new ReportCarDetailLocationDTO()
                             {
                                 Id = p.Id,
                                 Deskripsi = p.Description,
                                 RegionalAwal = p.Regional.Name,
                                 Tujuan = p.Destination
                             }).SingleAsync();

            return data;
        }
    }
}
