using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Order;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Models;
using System.Linq.Dynamic.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E.Service.Resource.Data.Types;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore.Internal;

namespace E.Service.Resource.Api.Services.Order
{
    public class OrderStationaryService : IOrderStationaryService
    {
        private EservicesdbContext _db;
        public OrderStationaryService(EservicesdbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Control<OrderItemDTO>> Get(int start, int take, string filter, string order, bool active)
        {

            var repos = _db.OrderItem.Where(m => m.RoleId == EOrderTypes.OrderStationary.Description()).AsQueryable();

            if (active)
            {
                repos = repos.Where(m => m.Active == true);
            }

            int totalData = repos.Count();

            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Name.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<OrderItemDTO>()
            {
                ListClass = await data.Select(m => new OrderItemDTO
                {
                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id,
                    Active = m.Active,
                    JenisId = m.JenisId,

                    BudgetId = m.BudgetId,
                    BudgetName = m.Budget.BudgetName,
                    BudgetNominal = m.Budget.BudgetNominal ?? 0,
                    JenisName = m.Jenis.JenisNama,
                    Merk = m.Merk,
                    RoleId = m.RoleId,
                    SerialNumber = m.SerialNumber
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<OrderItemDTO> Get(int id)
        {

            return await _db.OrderItem.Where(m => m.RoleId == EOrderTypes.OrderStationary.Description()).Select(m => new OrderItemDTO()
            {
                Description = m.Description,
                Name = m.Name,
                Id = m.Id,
                Active = m.Active,
                JenisId = m.JenisId,
                JenisName = m.Jenis.JenisNama,
                Merk = m.Merk,

                BudgetId = m.BudgetId,
                BudgetName = m.Budget.BudgetName,
                BudgetNominal = m.Budget.BudgetNominal ?? 0,
                RoleId = m.RoleId,
                SerialNumber = m.SerialNumber,
                OrderItemStock = new OrderItemStockDTO()
                {
                    Konv1ke2 = m.OrderItemStock.Konv1ke2,
                    MaxStock = m.OrderItemStock.MaxStock,
                    MinStock = m.OrderItemStock.MinStock,
                    Satuan1 = m.OrderItemStock.Satuan1,
                    Satuan2 = m.OrderItemStock.Satuan2
                },
                OrderItemIT = null
            }).SingleOrDefaultAsync(m => m.Id == id);

        }

        public async Task<Control<OrderItemDTO>> GetNONLocation(string itemId, int start, int take, string filter, string order, bool active)
        {

            var repos = _db.OrderItem.Where(m => m.RoleId == EOrderTypes.OrderStationary.Description()).AsQueryable();

            if (!string.IsNullOrEmpty(itemId))
            {

                var itemInId = itemId.Split(',').Select(Int32.Parse).ToList(); 
                if (itemInId.Count > 0)
                {
                    repos = from p in repos
                            where !itemInId.Contains(p.Id)
                            select p;
                }
            }
            if (active)
            {
                repos = repos.Where(m => m.Active == true);
            }

            int totalData = repos.Count();

            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Name.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<OrderItemDTO>()
            {
                ListClass = await data.Select(m => new OrderItemDTO
                {
                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id,
                    Active = m.Active,
                    JenisId = m.JenisId,
                    BudgetId = m.BudgetId,
                    BudgetName = m.Budget.BudgetName,
                    BudgetNominal = m.Budget.BudgetNominal ?? 0,
                    JenisName = m.Jenis.JenisNama,
                    Merk = m.Merk,
                    RoleId = m.RoleId,
                    SerialNumber = m.SerialNumber
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<OrderRequestDetailStok> GetStok(int id, int locationId)
        {
            var datastok = await (from stok in _db.Stocks
                                  join location in _db.OfficeLocations on stok.OfficeLocationId equals location.Id
                                  join reg in _db.OfficeLocationRegions on location.RegionId equals reg.Id
                                  join itemStok in _db.OrderItem on stok.OrderItemId equals itemStok.Id
                                  join itemStokInfo in _db.OrderItemStock on itemStok.Id equals itemStokInfo.OrderItemId
                                  where itemStok.Id == id && location.Id == locationId
                                  select new OrderRequestDetailStok()
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
                                  }).SingleOrDefaultAsync();

            return datastok;
        }

        public async Task<List<OrderRequestDetailStok>> GetStokById(int id)
        {
            var datastok = await (from stok in _db.Stocks
                                  join location in _db.OfficeLocations on stok.OfficeLocationId equals location.Id
                                  join reg in _db.OfficeLocationRegions on location.RegionId equals reg.Id
                                  join itemStok in _db.OrderItem on stok.OrderItemId equals itemStok.Id
                                  join itemStokInfo in _db.OrderItemStock on itemStok.Id equals itemStokInfo.OrderItemId
                                  where itemStok.Id == id
                                  select new OrderRequestDetailStok()
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
                                  }).ToListAsync();

            foreach (var data in datastok)
            {
                data.StokQtyNow = data.StokQtyAdd - data.StokQtyMin;
            }



            return datastok;
        }

        public async Task<OrderItem> Save(OrderItem entity)
        {
            entity.RoleId = EOrderTypes.OrderStationary.Description();
            if (entity.Id == 0)
            {
                await _db.OrderItem.AddAsync(entity);
            }
            else
            {
                _db.OrderItem.Update(entity);
            }
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
