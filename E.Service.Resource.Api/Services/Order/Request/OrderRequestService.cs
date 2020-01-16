using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;

namespace E.Service.Resource.Api.Services.Order.Request
{
    public class OrderRequestService : IOrderRequestService
    {
        EservicesdbContext _db;

        IRequestService _requestService;
        IUserService _userService;

        public OrderRequestService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            _db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<OrderRequestDTO> Get(int id)
        {
            return await _db.OrderRequest.Select(m => new OrderRequestDTO()
            {
                Description = m.Description,
                Id = m.Id,
                CreateAt = m.CreateAt,
                OfficeLocationId = m.OfficeLocationId,
                RequesterId = m.RequesterId,
                RequestNo = m.RequestNo,
                RequiredAt = m.RequiredAt,
                RoleId = m.RoleId,
                OfficeLocationName = m.OfficeLocation.Name,
                RequesterName = m.Requester.Name,
                RoleName = m.Role.Name,
                RequestId = m.RequestId,
                RequestStatus = m.Request.Currentstate.Name,
                RequestStatusType = m.Request.Currentstate.Statetype.Name,
                BudgetNominalTotal = ((m.OrderRequestsDetail != null ?
                            (m.OrderRequestsDetail.Qty == 0 ? 1 : m.OrderRequestsDetail.Qty) : 0) +
                            (m.OrderRequestsDetail != null ?
                            m.OrderRequestsDetail.Qty2 ?? 0 * m.OrderRequestsDetail.OrderItem.OrderItemStock.Konv1ke2 ?? 0 : 0)) *
                            m.OrderRequestsDetail.OrderItem.Budget.BudgetNominal ?? 0,

                BudgetLeft = m.BudgetLeft,
                FundAvailable = m.FundAvailable,
                NoAccount = m.NoAccount,
                TotalBudget = m.TotalBudget,
                StatusAnggaranId = m.StatusAnggaranId,

                OrderRequestDetailDTO = new OrderRequestDetailDTO()
                {

                    BudgetId = m.OrderRequestsDetail.OrderItem.BudgetId,
                    BudgetName = m.OrderRequestsDetail.OrderItem.Budget.BudgetName,
                    BudgetNominal =
                         ((m.OrderRequestsDetail != null ?
                            (m.OrderRequestsDetail.Qty == 0 ? 1 : m.OrderRequestsDetail.Qty) : 0) +
                            (m.OrderRequestsDetail != null ?
                            m.OrderRequestsDetail.Qty2 ?? 0 * m.OrderRequestsDetail.OrderItem.OrderItemStock.Konv1ke2 ?? 0 : 0)) *
                            m.OrderRequestsDetail.OrderItem.Budget.BudgetNominal ?? 0,
                    Id = m.OrderRequestsDetail.OrderRequestId,
                    OrderItemId = m.OrderRequestsDetail.OrderItemId,
                    OrderItemName = m.OrderRequestsDetail.OrderItem.Name,
                    StockMin = m.OrderRequestsDetail.OrderItem.OrderItemStock != null ? m.OrderRequestsDetail.OrderItem.OrderItemStock.MinStock ?? 0 : 0,
                    StockMax = m.OrderRequestsDetail.OrderItem.OrderItemStock != null ? m.OrderRequestsDetail.OrderItem.OrderItemStock.MaxStock ?? 0 : 0,
                    OrderRequestId = m.OrderRequestsDetail.OrderRequestId,
                    OrderRequestStokDTO = new OrderRequestStokDTO()
                    {
                        OrderRequestDetailId = m.OrderRequestsDetail.OrderRequestId,
                        Qty = m.OrderRequestsDetail.Qty,
                        Qty2 = m.OrderRequestsDetail.Qty2,
                        StockTransactionId = m.OrderRequestsDetail.StockTransactionId
                    },
                    OrderRequestImageDTO = m.OrderRequestImage.Select(d => new OrderRequestImageDTO
                    {
                        Id = d.Id,
                        OrderRequestId = d.OrderRequestId.Value,
                        ImageId = d.ImageId ?? 0,
                        FilePath = d.Image.FilePath
                    }).ToList()
                }
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<OrderRequestAccountabilityDTO> GetAccountabilityByRequestId(int id)
        {
            return await _db.OrderRequestAccountability.Where(m => m.RequestId == id).Select(m => new OrderRequestAccountabilityDTO()
            {
                AccountabilityDate = m.AccountabilityDate,
                Description = m.OrderRequest.Description,
                Note = m.Note,
                OfficeLocationName = m.OrderRequest.OfficeLocation.Name,
                OrderRequestId = m.OrderRequestId,
                RequesterName = m.OrderRequest.Requester.Name,
                RequestNo = m.OrderRequest.RequestNo,
                RequiredAt = m.OrderRequest.RequiredAt,
                TotalBudget = m.TotalBudget,
                PicId = m.PicId,
                RequestId = m.RequestId,
                OrderRequestAccountabilityImages = m.OrderRequestAccountabilityImage
                      .Select(i => new OrderRequestAccountabilityImageDTO()
                      {
                          Id = i.Id,
                          file_path = i.Image.FilePath,
                          OrderRequestId = m.OrderRequestId
                      }).ToList()
            }).SingleOrDefaultAsync();

        }

        public async Task<OrderRequestAccountabilityDTO> GetAccountablity(int id)
        {

            return await _db.OrderRequestAccountability.Select(m => new OrderRequestAccountabilityDTO()
            {
                AccountabilityDate = m.AccountabilityDate,
                Description = m.OrderRequest.Description,
                Note = m.Note,
                OfficeLocationName = m.OrderRequest.OfficeLocation.Name,
                OrderRequestId = m.OrderRequestId,
                RequesterName = m.OrderRequest.Requester.Name,
                RequestNo = m.OrderRequest.RequestNo,
                RequiredAt = m.OrderRequest.RequiredAt,
                TotalBudget = m.TotalBudget,
                PicId = m.PicId,
                RequestId = m.RequestId,
                Status = m.Request.Currentstate.Name,
                OrderRequestAccountabilityImages = m.OrderRequestAccountabilityImage
                    .Select(i => new OrderRequestAccountabilityImageDTO()
                    {
                        Id = i.Id,
                        file_path = i.Image.FilePath,
                        OrderRequestId = m.OrderRequestId
                    }).ToList()
            }).SingleOrDefaultAsync(m => m.OrderRequestId == id);
        }

        public async Task<OrderRequestDTO> GetByRequestId(int id)
        {
            return await _db.OrderRequest.Where(m => m.RequestId == id).Select(m => new OrderRequestDTO()
            {
                Description = m.Description,
                Id = m.Id,
                CreateAt = m.CreateAt,
                OfficeLocationId = m.OfficeLocationId,
                RequesterId = m.RequesterId,
                RequestNo = m.RequestNo,
                RequiredAt = m.RequiredAt,
                RoleId = m.RoleId,
                OfficeLocationName = m.OfficeLocation.Name,
                RequesterName = m.Requester.Name,
                RoleName = m.Role.Name,
                RequestStatus = m.Request.Currentstate.Name,
                RequestStatusType = m.Request.Currentstate.Statetype.Name,
                BudgetNominalTotal =
                            ((m.OrderRequestsDetail != null ?
                            (m.OrderRequestsDetail.Qty == 0 ? 1 : m.OrderRequestsDetail.Qty) : 0) +
                            (m.OrderRequestsDetail != null ?
                            m.OrderRequestsDetail.Qty2 ?? 0 * m.OrderRequestsDetail.OrderItem.OrderItemStock.Konv1ke2 ?? 0 : 0)) *
                            m.OrderRequestsDetail.OrderItem.Budget.BudgetNominal ?? 0,
                BudgetLeft = m.BudgetLeft,
                FundAvailable = m.FundAvailable,
                NoAccount = m.NoAccount,
                TotalBudget = m.TotalBudget,
                StatusAnggaranId = m.StatusAnggaranId,
                RequestId = m.RequestId,
                OrderRequestDetailDTO = new OrderRequestDetailDTO()
                {

                    BudgetId = m.OrderRequestsDetail.OrderItem.BudgetId,
                    BudgetName = m.OrderRequestsDetail.OrderItem.Budget.BudgetName,
                    BudgetNominal =
                     ((m.OrderRequestsDetail != null ?
                            (m.OrderRequestsDetail.Qty == 0 ? 1 : m.OrderRequestsDetail.Qty) : 0) +
                            (m.OrderRequestsDetail != null ?
                            m.OrderRequestsDetail.Qty2 ?? 0 * m.OrderRequestsDetail.OrderItem.OrderItemStock.Konv1ke2 ?? 0 : 0)) *
                            m.OrderRequestsDetail.OrderItem.Budget.BudgetNominal ?? 0,
                    Id = m.OrderRequestsDetail.OrderRequestId,
                    OrderItemId = m.OrderRequestsDetail.OrderItemId,
                    OrderItemName = m.OrderRequestsDetail.OrderItem.Name,
                    StockMin = m.OrderRequestsDetail.OrderItem.OrderItemStock != null ? m.OrderRequestsDetail.OrderItem.OrderItemStock.MinStock ?? 0 : 0,
                    StockMax = m.OrderRequestsDetail.OrderItem.OrderItemStock != null ? m.OrderRequestsDetail.OrderItem.OrderItemStock.MaxStock ?? 0 : 0,
                    OrderRequestId = m.OrderRequestsDetail.OrderRequestId,
                    OrderRequestStokDTO = new OrderRequestStokDTO()
                    {
                        OrderRequestDetailId = m.OrderRequestsDetail.OrderRequestId,
                        Qty = m.OrderRequestsDetail.Qty,
                        Qty2 = m.OrderRequestsDetail.Qty2,
                        StockTransactionId = m.OrderRequestsDetail.StockTransactionId
                    },
                    OrderRequestImageDTO = m.OrderRequestImage.Select(d => new OrderRequestImageDTO
                    {
                        FilePath = d.Image.FilePath
                    }).ToList()
                },

            }).SingleOrDefaultAsync();
        }

        public async Task<Control<OrderRequestDTO>> GetList(int start, int take, string filter, string order, EOrderTypes? eOrderTypes, bool complete)
        {
            var repos = _db.OrderRequest.AsQueryable();
            if (eOrderTypes != null)
            {
                repos = repos.Where(m => m.RoleId == eOrderTypes.Description());
            }


            if (complete)
            {
                repos = repos.Where(m => m.OrderRequestAccountability == null &&
                m.Request.Currentstate.Statetype.Name == "Complete");
            }

            int totalData = repos.Count();


            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.RequestNo.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<OrderRequestDTO>()
            {
                ListClass = await data.Select(m => new OrderRequestDTO
                {

                    Description = m.Description,
                    Id = m.Id,
                    CreateAt = m.CreateAt,
                    OfficeLocationId = m.OfficeLocationId,
                    RequesterId = m.RequesterId,
                    RequestNo = m.RequestNo,
                    RequiredAt = m.RequiredAt,
                    RoleId = m.RoleId,
                    OfficeLocationName = m.OfficeLocation.Name,
                    RequesterName = m.Requester.Name,
                    RoleName = m.Role.Name,
                    RequestStatus = m.Request.Currentstate.Name,
                    RequestStatusType = m.Request.Currentstate.Statetype.Name,
                    RequestId = m.RequestId,
                    BudgetNominalTotal =
                     ((m.OrderRequestsDetail != null ? m.OrderRequestsDetail.Qty : 0) +
                            (m.OrderRequestsDetail != null ? m.OrderRequestsDetail.Qty2 * m.OrderRequestsDetail.OrderItem.OrderItemStock.Konv1ke2 : 0)) *
                            m.OrderRequestsDetail.OrderItem.Budget.BudgetNominal ?? 0,
                    BudgetLeft = m.BudgetLeft,
                    FundAvailable = m.FundAvailable,
                    NoAccount = m.NoAccount,
                    TotalBudget = m.TotalBudget,
                    StatusAnggaranId = m.StatusAnggaranId,
                    OrderRequestDetailDTO = new OrderRequestDetailDTO()
                    {
                        BudgetId = m.OrderRequestsDetail.OrderItem.BudgetId,
                        BudgetName = m.OrderRequestsDetail.OrderItem.Budget.BudgetName,
                        BudgetNominal =
                            ((m.OrderRequestsDetail != null ? m.OrderRequestsDetail.Qty : 0) +
                            (m.OrderRequestsDetail != null ? m.OrderRequestsDetail.Qty2 * m.OrderRequestsDetail.OrderItem.OrderItemStock.Konv1ke2 : 0)) *
                            m.OrderRequestsDetail.OrderItem.Budget.BudgetNominal ?? 0,
                        Id = m.OrderRequestsDetail.OrderRequestId,
                        OrderItemId = m.OrderRequestsDetail.OrderItemId,
                        OrderItemName = m.OrderRequestsDetail.OrderItem.Name,
                        OrderRequestId = m.OrderRequestsDetail.OrderRequestId,
                        StockMin = m.OrderRequestsDetail.OrderItem.OrderItemStock != null ? m.OrderRequestsDetail.OrderItem.OrderItemStock.MinStock ?? 0 : 0,
                        StockMax = m.OrderRequestsDetail.OrderItem.OrderItemStock != null ? m.OrderRequestsDetail.OrderItem.OrderItemStock.MaxStock ?? 0 : 0,
                        OrderRequestStokDTO = new OrderRequestStokDTO()
                        {
                            OrderRequestDetailId = m.OrderRequestsDetail.OrderRequestId,
                            Qty = m.OrderRequestsDetail.Qty,
                            Qty2 = m.OrderRequestsDetail.Qty2,
                            StockTransactionId = m.OrderRequestsDetail.StockTransactionId
                        },
                        OrderRequestImageDTO = m.OrderRequestImage.Select(d => new OrderRequestImageDTO
                        {
                            FilePath = d.Image.FilePath
                        }).ToList()
                    }
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<Control<OrderRequestAccountabilityDTO>> GetListAccountability(int start, int take, string filter, string order)
        {
            var repos = _db.OrderRequestAccountability.AsQueryable();

            int totalData = repos.Count();


            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.OrderRequest.RequestNo.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<OrderRequestAccountabilityDTO>()
            {
                ListClass = await data.Select(m => new OrderRequestAccountabilityDTO
                {

                    AccountabilityDate = m.AccountabilityDate,
                    Description = m.OrderRequest.Description,
                    Note = m.Note,
                    OfficeLocationName = m.OrderRequest.OfficeLocation.Name,
                    OrderRequestId = m.OrderRequestId,
                    RequesterName = m.OrderRequest.Requester.Name,
                    RequestNo = m.OrderRequest.RequestNo,
                    RequiredAt = m.OrderRequest.RequiredAt,
                    TotalBudget = m.TotalBudget,
                    RequestId = m.RequestId,
                    Status = m.Request.Currentstate.Name,
                    OrderRequestAccountabilityImages = m.OrderRequestAccountabilityImage
                    .Select(i => new OrderRequestAccountabilityImageDTO()
                    {
                        Id = i.Id,
                        file_path = i.Image.FilePath,
                        OrderRequestId = m.OrderRequestId
                    }).ToList()
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<OrderRequest> Save(OrderRequest entity, EOrderTypes eOrderTypes, bool submit)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {
                var user = await _userService.GetUserById(entity.RequesterId.Value);
                entity.CreateAt = DateTime.Now;
                entity.JabatanId = user.JabatanId;

                var prFix = $"06{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.RequestNo = await GenerateNo(prFix);

                request = new RequestFlow();

                List<Transition> transitionList = new List<Transition>();

                if (eOrderTypes == EOrderTypes.OrderInventory)
                {
                    var item = _db.OrderItem.Include(m => m.OrderItemInventoryIt).AsNoTracking().Single(m => m.Id == entity.OrderRequestsDetail.OrderItemId);
                    if (item.OrderItemInventoryIt.ItemIt == true)
                    {
                        request = new RequestFlow()
                        {
                            Daterequest = DateTime.Now,
                            Currentstateid = await _requestService.BeginStateId(ERequestType.OrderInventoryIT),
                            Title = entity.RequestNo,
                            Note = entity.Description,
                            Processid = await _requestService.ProgressId(ERequestType.OrderInventoryIT),
                            Userid = entity.RequesterId.ToString(),
                            Url = "Order/dashboard/request/"
                        };

                        transitionList = await _requestService.
                        TransitionList(ERequestType.OrderInventoryIT);
                    }
                    else
                    {
                        request = new RequestFlow()
                        {
                            Daterequest = DateTime.Now,
                            Currentstateid = await _requestService.BeginStateId(ERequestType.OrderInventoryNonIT),
                            Title = entity.RequestNo,
                            Note = entity.Description,
                            Processid = await _requestService.ProgressId(ERequestType.OrderInventoryNonIT),
                            Userid = entity.RequesterId.ToString(),
                            Url = "Order/dashboard/request/"
                        };
                        transitionList = await _requestService.
                        TransitionList(ERequestType.OrderInventoryNonIT);
                    }
                }
                else if (eOrderTypes == EOrderTypes.OrderPrinting)
                {
                    request = new RequestFlow()
                    {
                        Daterequest = DateTime.Now,
                        Currentstateid = await _requestService.BeginStateId(ERequestType.OrderPrinting),
                        Title = entity.RequestNo,
                        Note = entity.Description,
                        Processid = await _requestService.ProgressId(ERequestType.OrderPrinting),
                        Userid = entity.RequesterId.ToString(),
                        Url = "Order/dashboard/request/"
                    };
                    transitionList = await _requestService.
                    TransitionList(ERequestType.OrderPrinting);
                }
                else if (eOrderTypes == EOrderTypes.OrderStationary)
                {
                    request = new RequestFlow()
                    {
                        Daterequest = DateTime.Now,
                        Currentstateid = await _requestService.BeginStateId(ERequestType.OrderATK),
                        Title = entity.RequestNo,
                        Note = entity.Description,
                        Processid = await _requestService.ProgressId(ERequestType.OrderATK),
                        Userid = entity.RequesterId.ToString(),
                        Url = "Order/dashboard/request/"
                    };

                    transitionList = await _requestService.
                    TransitionList(ERequestType.OrderATK);
                }


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

                await _db.OrderRequest.AddAsync(entity);

            }
            else
            {

                //removing data delete meeting request time
                var dataImage = _db.OrderRequestImage.Where(m => m.OrderRequestId == entity.Id
                                && entity.OrderRequestImage.Any(t => t.Id != m.Id));
                _db.OrderRequestImage.RemoveRange(dataImage);


                var user = await _userService.GetUserById(entity.RequesterId.Value);
                entity.JabatanId = user.JabatanId;

                _db.OrderRequest.Update(entity);

                _db.Entry(entity).Property(x => x.CreateAt).IsModified = false;


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

                await updateStock(request.Requestid, eOrderTypes);
            }
            return entity;
        }

        private async Task updateStock(int requestid, EOrderTypes eOrderTypes)
        {

            var repos = _db.OrderRequestsDetail.Where(m => m.OrderRequest.RequestId == requestid).Single();


            if (eOrderTypes == EOrderTypes.OrderStationary)
            {

                var item = await _db.OrderItem.Include(m => m.OrderItemStock).AsNoTracking()
                    .SingleAsync(m => m.Id == repos.OrderItemId);

                var StockTransaction = new StockTransaction()
                {
                    Qty = (repos.Qty ?? 0) + ((repos.Qty2 ?? 0) *
                        (item.OrderItemStock.Konv1ke2 ?? 0)),
                    Note = "Order Stationary with ID " + repos.OrderRequestId,
                    StockDate = DateTime.Now,
                    StockTransactionStatusId = 2
                };

                var datastok = _db.Stocks.SingleOrDefault(m => m.OfficeLocationId == repos.OrderRequest.OfficeLocationId &&
                m.OrderItemId == repos.OrderItemId);

                if (datastok != null)
                {
                    StockTransaction.Stock = datastok;
                }
                else
                {
                    StockTransaction.Stock = new Stocks()
                    {
                        OrderItemId = repos.OrderItemId,
                        OfficeLocationId = repos.OrderRequest.OfficeLocationId
                    };
                }
                repos.StockTransaction = StockTransaction;
                _db.OrderRequestsDetail.Update(repos);
                await _db.SaveChangesAsync();
            }


        }

        public async Task<OrderRequestAccountability> SaveAccountability(OrderRequestAccountability entity, bool submit)
        {
            var request = new RequestFlow();
            if (!_db.OrderRequestAccountability.Any(m => m.OrderRequestId == entity.OrderRequestId))
            {

                var order = await _db.OrderRequest.Where(m => m.Id == entity.OrderRequestId).SingleAsync();
                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.OrderAccountability),
                    Title = order.RequestNo,
                    Note = entity.Note,
                    Processid = await _requestService.ProgressId(ERequestType.OrderAccountability),
                    Userid = entity.PicId.ToString(),
                    Url = "order/dashboard/request/accountability/"
                };

                var transitionList = await _requestService.TransitionList(ERequestType.OrderAccountability);

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

                await _db.OrderRequestAccountability.AddAsync(entity);
            }
            else
            {
                _db.OrderRequestAccountability.Update(entity);
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
                        UserId = entity.PicId
                    });
            }
            return entity;
        }

        private async Task<string> GenerateNo(string prFix)
        {
            var latestNo = await _db.OrderRequest.OrderByDescending(m => m.RequestNo).
                FirstOrDefaultAsync(m => m.RequestNo.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.RequestNo.Substring(latestNo.RequestNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }

        public async Task UpdateEntity(int id, int anggaranstatusId)
        {
            var data = await _db.OrderRequest.SingleAsync(m => m.Id == id);

            data.StatusAnggaranId = anggaranstatusId;
            _db.Update(data);
            await _db.SaveChangesAsync();
        }
    }
}
