using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Interface.Travel.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Travel
{
    public class TravelTransportService : ITravelTransportationService
    {
        private EservicesdbContext _db;
        public TravelTransportService(EservicesdbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Control<TravelTransportationDTO>> Get(int start, int take, string filter, string order, bool active)
        {
            var repos = _db.TravelTransportationName.AsQueryable();


            if (active == true)
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
            return new Control<TravelTransportationDTO>()
            {
                ListClass = await data.Select(m => new TravelTransportationDTO
                {
                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id,
                    TravelTypeId = m.TravelTypeId,
                    TravelTypeName = m.TravelType.Name,
                    Active = m.Active,
                    BudgetId = m.BudgetId ?? 0,
                    BudgetName = m.Budget != null ? m.Budget.BudgetName : "",
                    BudgetNominal = m.Budget != null ? m.Budget.BudgetNominal.Value : decimal.Zero,
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<TravelTransportationDTO> Get(int id)
        {

            return await _db.TravelTransportationName.Select(m => new TravelTransportationDTO()
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                Active = m.Active,
                TravelTypeId = m.TravelTypeId,
                TravelTypeName = m.TravelType.Name,
                BudgetId = m.BudgetId ?? 0,
                BudgetName = m.Budget != null ? m.Budget.BudgetName : "",
                BudgetNominal = m.Budget != null ? m.Budget.BudgetNominal.Value : decimal.Zero,
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TravelTransportationName> Save(TravelTransportationName entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await _db.TravelTransportationName.AddAsync(entity);
            }
            else
            {
                _db.TravelTransportationName.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;
            }
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
