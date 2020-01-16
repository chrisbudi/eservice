using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Order;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Models;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;

namespace E.Service.Resource.Api.Services.Order
{
    public class OrderInventoryService : IOrderInventoryService
    {
        private EservicesdbContext _db;
        public OrderInventoryService(EservicesdbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Control<OrderItemDTO>> Get(int start, int take, string filter, string order, bool active)
        {

            var repos = _db.OrderItem.Where(m => m.RoleId == EOrderTypes.OrderInventory.Description()).AsQueryable();

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
                    JenisName = m.Jenis.JenisNama,
                    Merk = m.Merk,
                    BudgetId = m.BudgetId,
                    BudgetName = m.Budget.BudgetName,
                    BudgetNominal = m.Budget.BudgetNominal ?? 0,
                    RoleId = m.RoleId,
                    SerialNumber = m.SerialNumber,
                    OrderItemIT = new OrderItemITDTO()
                    {
                        OrderItemId = m.OrderItemInventoryIt.OrderItemId,
                        ItemIt = m.OrderItemInventoryIt.ItemIt
                    },
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<OrderItemDTO> Get(int id)
        {

            return await _db.OrderItem.Select(m => new OrderItemDTO()
            {
                Description = m.Description,
                Name = m.Name,
                Id = m.Id,
                Active = m.Active,
                JenisId = m.JenisId,
                JenisName = m.Jenis.JenisNama,
                Merk = m.Merk,
                RoleId = m.RoleId,
                BudgetId = m.BudgetId,
                BudgetName = m.Budget.BudgetName,
                BudgetNominal = m.Budget.BudgetNominal ?? 0,
                SerialNumber = m.SerialNumber,
                OrderItemIT = new OrderItemITDTO()
                {
                    OrderItemId = m.OrderItemInventoryIt.OrderItemId,
                    ItemIt = m.OrderItemInventoryIt.ItemIt
                },
                OrderItemStock = null
            }).SingleOrDefaultAsync(m => m.Id == id);

        }

        public async Task<OrderItem> Save([FromBody]OrderItem entity)
        {
            entity.RoleId = EOrderTypes.OrderInventory.Description();
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
