using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Order;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using MailKit;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Order.Request
{
    public class OrderReloadService : IOrderReloadService
    {
        public EservicesdbContext _db;

        IRequestService _requestService;
        IUserService _userService;

        public OrderReloadService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            _db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<OrderReloadDTO> Get(int id)
        {
            var orderReloadData = await _db.OrderReload.Where(m => m.Id == id)
                .Select(m => new OrderReloadDTO()
                {
                    Description = m.Description,
                    Id = m.Id,
                    CreateAt = m.CreateAt,
                    OfficeLocationId = m.OfficeLocationId,
                    RequesterId = m.RequesterId,
                    RequiredAt = m.RequiredAt,
                    RoleId = m.RoleId,
                    OfficeLocationName = m.OfficeLocation.Name,
                    RequesterName = m.Requester.Name,
                    RoleName = m.Role.Name,
                    ReloadNo = m.ReloadNo,
                    RequestId = m.RequestId ?? 0,
                    RequestStatus = m.Request.Currentstate.Name,
                    RequestStatusType = m.Request.Currentstate.Statetype.Name,
                    BudgetLeft = m.BudgetLeft,
                    FundAvailable = m.FundAvailable,
                    NoAccount = m.NoAccount,
                    StatusAnggaranId = m.StatusAnggaranId,
                    TotalBudget = m.TotalBudget,
                    BudgetId = m.BudgetId,
                    orderReloadDetailDTOs = m.OrderReloadDetail.Select(o => new OrderReloadDetailDTO()
                    {
                        Id = o.Id,
                        OrderItemId = o.OrderItemId,
                        OrderItemaName = o.OrderItem.Name,
                        OrderItemSatuan1 = o.OrderItem.OrderItemStock.Satuan1,
                        OrderItemSatuan2 = o.OrderItem.OrderItemStock.Satuan2,
                        OrderReloadId = o.OrderReloadId,
                        Qty = o.Qty ?? 0,
                        Qty2 = o.Qty2 ?? 0,
                        Konv21 = o.OrderItem.OrderItemStock.Konv1ke2 ?? 0,
                        BudgetDetailId = o.OrderItem.BudgetId,
                        BudgetName = o.OrderItem.Budget.BudgetName,
                        BudgetNominal = o.OrderItem.Budget.BudgetNominal ?? 0,
                        StockTransactionId = o.StockTransactionId,
                        OrderReloadStok = (from stok in _db.Stocks
                                           join location in _db.OfficeLocations on stok.OfficeLocationId equals location.Id
                                           join reg in _db.OfficeLocationRegions on location.RegionId equals reg.Id
                                           join itemStok in _db.OrderItem on stok.OrderItemId equals itemStok.Id
                                           join itemStokInfo in _db.OrderItemStock on itemStok.Id equals itemStokInfo.OrderItemId
                                           where itemStok.Id == o.OrderItemId && location.Id == m.OfficeLocationId
                                           select new OrderReloadStokDTO()
                                           {
                                               ItemId = stok.OrderItemId,
                                               ItemName = itemStok.Name,
                                               itemMaxStok = itemStok.OrderItemStock.MaxStock ?? 0,
                                               itemMinStok = itemStok.OrderItemStock.MinStock ?? 0,
                                               LocationName = location.Name,
                                               Satuan2 = itemStokInfo.Satuan2,
                                               Satuan1 = itemStokInfo.Satuan1,
                                               Konv21 = itemStokInfo.Konv1ke2 ?? 0,
                                               RegionName = reg.Name,
                                               StokQtyAdd = stok.StockTransaction
                                                .Where(stok => stok.StockTransactionStatusId == 1)
                                                .Sum(stok => stok.Qty),
                                               StokQtyMin = stok.StockTransaction
                                                .Where(stok => stok.StockTransactionStatusId == 2)
                                                .Sum(stok => stok.Qty)
                                           }).SingleOrDefault()
                    }).ToList()
                }).SingleAsync();



            foreach (var detail in orderReloadData.orderReloadDetailDTOs)
            {

                detail.BudgetTotalQtyNominal = (detail.Qty + detail.Qty2 * detail.OrderReloadStok.Konv21) * detail.BudgetNominal;
                detail.OrderReloadStok.StokQtyNow = detail.OrderReloadStok.StokQtyAdd - detail.OrderReloadStok.StokQtyMin;
            }
            orderReloadData.BudgetSumTotalNominal = orderReloadData.orderReloadDetailDTOs.Sum(m => m.BudgetTotalQtyNominal);


            return orderReloadData;
        }

        public async Task<OrderReloadAccountabilityDTO> GetAccountabilityByRequestId(int id)
        {

            return await _db.OrderReloadAccountability.Where(m => m.RequestId == id).Select(m => new OrderReloadAccountabilityDTO()
            {
                AccountabilityDate = m.AccountabilityDate,
                Description = m.OrderReload.Description,
                Note = m.Note,
                OfficeLocationName = m.OrderReload.OfficeLocation.Name,
                TotalBudget = m.TotalBudget,
                PicId = m.PicId,
                ReloadNo = m.OrderReload.ReloadNo,
                RequestId = m.RequestId,
                OrderReloadId = m.OrderReloadId,
                OrderReloadAccountabilityImage = m.OrderReloadAccountabilityImage
                      .Select(i => new OrderReloadAccountabilityImageDTO()
                      {
                          Id = i.Id,
                          file_path = i.Image.FilePath,
                          OrderReloadId = i.OrderReloadId.Value
                      }).ToList()
            }).SingleOrDefaultAsync();
        }

        public async Task<OrderReloadAccountabilityDTO> GetAccountablity(int id)
        {
            return await _db.OrderReloadAccountability.Where(m => m.OrderReloadId == id).Select(m => new OrderReloadAccountabilityDTO()
            {
                AccountabilityDate = m.AccountabilityDate,
                Description = m.OrderReload.Description,
                Note = m.Note,
                OfficeLocationName = m.OrderReload.OfficeLocation.Name,
                TotalBudget = m.TotalBudget,
                PicId = m.PicId,
                ReloadNo = m.OrderReload.ReloadNo,
                RequestId = m.RequestId,
                OrderReloadId = m.OrderReloadId,
                Status = m.Request.Currentstate.Name,
                OrderReloadAccountabilityImage = m.OrderReloadAccountabilityImage
                     .Select(i => new OrderReloadAccountabilityImageDTO()
                     {
                         Id = i.Id,
                         file_path = i.Image.FilePath,
                         OrderReloadId = i.OrderReloadId.Value
                     }).ToList()
            }).SingleOrDefaultAsync();
        }

        public async Task<OrderReloadDTO> GetByRequestId(int id)
        {
            var orderReloadData = await _db.OrderReload.Where(m => m.RequestId == id).Select(m => new OrderReloadDTO()
            {
                Description = m.Description,
                Id = m.Id,
                CreateAt = m.CreateAt,
                OfficeLocationId = m.OfficeLocationId,
                RequesterId = m.RequesterId,
                RequiredAt = m.RequiredAt,
                RoleId = m.RoleId,
                OfficeLocationName = m.OfficeLocation.Name,
                RequesterName = m.Requester.Name,
                RoleName = m.Role.Name,
                ReloadNo = m.ReloadNo,
                RequestId = m.RequestId ?? 0,
                RequestStatus = m.Request.Currentstate.Name,
                RequestStatusType = m.Request.Currentstate.Statetype.Name,
                BudgetLeft = m.BudgetLeft,
                FundAvailable = m.FundAvailable,
                NoAccount = m.NoAccount,
                StatusAnggaranId = m.StatusAnggaranId,
                TotalBudget = m.TotalBudget,
                BudgetId = m.BudgetId,
                orderReloadDetailDTOs = m.OrderReloadDetail.Select(o => new OrderReloadDetailDTO()
                {
                    Id = o.Id,
                    OrderItemId = o.OrderItemId,
                    OrderItemaName = o.OrderItem.Name,
                    OrderItemSatuan1 = o.OrderItem.OrderItemStock.Satuan1,
                    OrderItemSatuan2 = o.OrderItem.OrderItemStock.Satuan2,
                    OrderReloadId = o.OrderReloadId,
                    BudgetDetailId = o.OrderItem.BudgetId,
                    BudgetName = o.OrderItem.Budget.BudgetName,
                    BudgetNominal = o.OrderItem.Budget.BudgetNominal.Value,
                    BudgetTotalQtyNominal = (o.Qty ?? 0 + ((o.Qty2 ?? 0) * (o.OrderItem.OrderItemStock.Konv1ke2 ?? 0))),
                    Qty = o.Qty ?? 0,
                    Qty2 = o.Qty2 ?? 0,
                    Konv21 = o.OrderItem.OrderItemStock.Konv1ke2 ?? 0,
                    StockTransactionId = o.StockTransactionId,
                    OrderReloadStok = (from stok in _db.Stocks
                                       join location in _db.OfficeLocations on stok.OfficeLocationId equals location.Id
                                       join reg in _db.OfficeLocationRegions on location.RegionId equals reg.Id
                                       join itemStok in _db.OrderItem on stok.OrderItemId equals itemStok.Id
                                       join itemStokInfo in _db.OrderItemStock on itemStok.Id equals itemStokInfo.OrderItemId
                                       where itemStok.Id == o.OrderItemId
                                       select new OrderReloadStokDTO()
                                       {
                                           ItemId = stok.OrderItemId,
                                           ItemName = itemStok.Name,
                                           itemMaxStok = itemStok.OrderItemStock.MaxStock ?? 0,
                                           itemMinStok = itemStok.OrderItemStock.MinStock ?? 0,
                                           LocationName = location.Name,
                                           Satuan2 = itemStokInfo.Satuan2,
                                           Satuan1 = itemStokInfo.Satuan1,
                                           Konv21 = itemStokInfo.Konv1ke2 ?? 0,
                                           RegionName = reg.Name,
                                           StokQtyAdd = stok.StockTransaction
                                            .Where(stok => stok.StockTransactionStatusId == 1)
                                            .Sum(stok => stok.Qty),
                                           StokQtyMin = stok.StockTransaction
                                            .Where(stok => stok.StockTransactionStatusId == 2)
                                            .Sum(stok => stok.Qty)
                                       }).FirstOrDefault()
                }).ToList()
            }).SingleAsync();


            foreach (var detail in orderReloadData.orderReloadDetailDTOs)
            {
                if (detail.OrderReloadStok != null)
                {
                    detail.BudgetTotalQtyNominal = (detail.Qty + detail.Qty2 * detail.OrderReloadStok.Konv21) * detail.BudgetNominal;
                    detail.OrderReloadStok.StokQtyNow = detail.OrderReloadStok.StokQtyAdd - detail.OrderReloadStok.StokQtyMin;

                }
            }
            orderReloadData.BudgetSumTotalNominal = orderReloadData.orderReloadDetailDTOs.Sum(m => m.BudgetTotalQtyNominal);


            return orderReloadData;
        }

        public async Task<Control<OrderReloadDTO>> GetList(int start, int take, string filter, string order, bool complete)
        {
            var repos = _db.OrderReload.AsQueryable();

            if (complete)
            {
                repos = repos.Where(m => m.Request.Currentstate.Statetype.Name == "Complete" && m.OrderReloadAccountability == null);
            }

            int totalData = repos.Count();



            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.ReloadNo.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<OrderReloadDTO>()
            {
                ListClass = await data.Select(m => new OrderReloadDTO
                {

                    Description = m.Description,
                    Id = m.Id,
                    CreateAt = m.CreateAt,
                    OfficeLocationId = m.OfficeLocationId,
                    RequesterId = m.RequesterId,
                    RequiredAt = m.RequiredAt,
                    RoleId = m.RoleId,
                    OfficeLocationName = m.OfficeLocation.Name,
                    RequesterName = m.Requester.Name,
                    RoleName = m.Role.Name,
                    ReloadNo = m.ReloadNo,
                    RequestId = m.RequestId ?? 0,
                    RequestStatus = m.Request.Currentstate.Name,
                    RequestStatusType = m.Request.Currentstate.Statetype.Name,
                    orderReloadDetailDTOs = m.OrderReloadDetail.Select(o => new OrderReloadDetailDTO()
                    {
                        Id = o.Id,
                        OrderItemId = o.OrderItemId,
                        OrderItemaName = o.OrderItem.Name,
                        OrderItemSatuan1 = o.OrderItem.OrderItemStock.Satuan1,
                        OrderItemSatuan2 = o.OrderItem.OrderItemStock.Satuan2,
                        BudgetDetailId = o.OrderItem.BudgetId,
                        BudgetName = o.OrderItem.Budget.BudgetName,
                        BudgetNominal = o.OrderItem.Budget.BudgetNominal ?? 0,
                        BudgetTotalQtyNominal = (o.Qty ?? 0 + ((o.Qty2) ?? 0 * (o.OrderItem.OrderItemStock.Konv1ke2 ?? 0))) * (o.OrderItem.Budget.BudgetNominal ?? 0),
                        OrderReloadId = o.OrderReloadId,
                        Qty = o.Qty ?? 0,
                        Qty2 = o.Qty2 ?? 0,
                        StockTransactionId = o.StockTransactionId

                    }).ToList()
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<Control<OrderReloadAccountabilityDTO>> GetListAccountability(int start, int take, string filter, string order)
        {
            var repos = _db.OrderReloadAccountability.AsQueryable();



            int totalData = repos.Count();


            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.OrderReload.ReloadNo.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<OrderReloadAccountabilityDTO>()
            {
                ListClass = await data.Select(m => new OrderReloadAccountabilityDTO
                {

                    AccountabilityDate = m.AccountabilityDate,
                    Description = m.OrderReload.Description,
                    Note = m.Note,
                    OfficeLocationName = m.OrderReload.OfficeLocation.Name,
                    TotalBudget = m.TotalBudget,
                    PicId = m.PicId,
                    RequesterName = m.OrderReload.Requester.Name,
                    RequiredAt = m.OrderReload.RequiredAt.Value,
                    RoleName = m.OrderReload.Role.Name,
                    ReloadNo = m.OrderReload.ReloadNo,
                    RequestId = m.RequestId,
                    OrderReloadId = m.OrderReloadId,
                    Status = m.Request.Currentstate.Name,
                    OrderReloadAccountabilityImage = m.OrderReloadAccountabilityImage
                     .Select(i => new OrderReloadAccountabilityImageDTO()
                     {
                         Id = i.Id,
                         file_path = i.Image.FilePath,
                         OrderReloadId = i.OrderReloadId.Value
                     }).ToList()
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<IList<OrderReloadStokDTO>> GetListCreate(int regionId)
        {
            var orderReloadList = await
                (from stok in _db.Stocks
                 join location in _db.OfficeLocations on stok.OfficeLocationId equals location.Id
                 join reg in _db.OfficeLocationRegions on location.RegionId equals reg.Id
                 join itemStok in _db.OrderItem on stok.OrderItemId equals itemStok.Id
                 join itemStokInfo in _db.OrderItemStock on itemStok.Id equals itemStokInfo.OrderItemId
                 where location.RegionId == regionId
                 select new OrderReloadStokDTO()
                 {
                     ItemId = stok.OrderItemId,
                     ItemName = itemStok.Name,
                     itemMaxStok = itemStok.OrderItemStock.MaxStock ?? 0,
                     itemMinStok = itemStok.OrderItemStock.MinStock ?? 0,
                     LocationName = location.Name,
                     Satuan2 = itemStokInfo.Satuan2,
                     Satuan1 = itemStokInfo.Satuan1,
                     Konv21 = itemStokInfo.Konv1ke2 ?? 0,
                     RegionName = reg.Name,
                     StokQtyAdd = stok.StockTransaction
                        .Where(m => m.StockTransactionStatusId == 1)
                        .Sum(m => m.Qty),
                     StokQtyMin = stok.StockTransaction
                        .Where(m => m.StockTransactionStatusId == 2)
                        .Sum(m => m.Qty)
                 }).ToListAsync();

            foreach (var item in orderReloadList)
            {
                item.StokQtyNow = item.StokQtyAdd - item.StokQtyMin;
                item.Show = item.StokQtyNow < item.itemMinStok;
            }

            return orderReloadList;


        }

        public async Task<OrderReload> Save(OrderReload entity, bool submit)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {
                var user = await _userService.GetUserById(entity.RequesterId.Value);
                entity.CreateAt = DateTime.Now;
                entity.JabatanId = user.JabatanId;

                var prFix = $"07{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.ReloadNo = await GenerateNo(prFix);
                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.ReloadATK),
                    Title = entity.ReloadNo,
                    Note = entity.Description,
                    Processid = await _requestService.ProgressId(ERequestType.ReloadATK),
                    Userid = entity.RequesterId.ToString(),
                    Url = "reload/dashboard/request/"
                };
                var transitionList = await _requestService.
                TransitionList(ERequestType.ReloadATK);


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

                await _db.OrderReload.AddAsync(entity);
            }
            else
            {
                _db.OrderReload.Update(entity);
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

        public async Task<OrderReloadAccountability> SaveAccountability(OrderReloadAccountability entity, bool submit)
        {
            var request = new RequestFlow();
            if (!_db.OrderReloadAccountability.Any(m => m.OrderReloadId == entity.OrderReloadId))
            {

                var reload = await _db.OrderReload.Where(m => m.Id == entity.OrderReloadId).SingleAsync();
                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.ReloadAccountability),
                    Title = reload.ReloadNo,
                    Note = entity.Note,
                    Processid = await _requestService.ProgressId(ERequestType.ReloadAccountability),
                    Userid = entity.PicId.ToString(),
                    Url = "reload/dashboard/request/accountability/"
                };

                var transitionList = await _requestService.TransitionList(ERequestType.ReloadAccountability);

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
                }

                entity.Request = request;

                await _db.OrderReloadAccountability.AddAsync(entity);
            }
            else
            {
                _db.OrderReloadAccountability.Update(entity);
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

        public async Task UpdateEntity(int id, int anggaranstatusId)
        {
            var data = await _db.OrderReload.SingleAsync(m => m.Id == id);

            data.StatusAnggaranId = anggaranstatusId;
            _db.Update(data);
            await _db.SaveChangesAsync();
        }

        public async Task updateStock(int requestId)
        {
            var repos = _db.OrderReloadDetail.Include(m => m.OrderReload).ThenInclude(m => m.OfficeLocation).Where(m => m.OrderReload.RequestId == requestId);



            foreach (var detail in repos)
            {
                var item = await _db.OrderItem.Include(m => m.OrderItemStock).AsNoTracking()
                    .SingleAsync(m => m.Id == detail.OrderItemId);

                var StockTransaction = new StockTransaction()
                {
                    Qty = (detail.Qty ?? 0) + ((detail.Qty2 ?? 0) * (item.OrderItemStock.Konv1ke2 ?? 0)),
                    Note = "Order reload with ID " + detail.Id,
                    StockDate = DateTime.Now,
                    StockTransactionStatusId = 1
                };

                var datastok = _db.Stocks
                    .SingleOrDefault(m => m.OfficeLocationId == detail.OrderReload.OfficeLocationId &&
                    m.OrderItemId == detail.OrderItemId);

                if (datastok != null)
                {
                    StockTransaction.Stock = datastok;
                }
                else
                {
                    StockTransaction.Stock = new Stocks()
                    {
                        OrderItemId = detail.OrderItemId ?? 0,
                        OfficeLocationId = detail.OrderReload.OfficeLocationId
                    };
                }
                detail.StockTransaction = StockTransaction;


                _db.OrderReloadDetail.Update(detail);
                await _db.SaveChangesAsync();
            }




        }

        private async Task<string> GenerateNo(string prFix)
        {
            var latestNo = await _db.OrderReload.OrderByDescending(m => m.ReloadNo).
                 FirstOrDefaultAsync(m => m.ReloadNo.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.ReloadNo.Substring(latestNo.ReloadNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }
    }
}
