using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Interface.Travel.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Travel
{
    public class TravelHotelService : ITravelHotelService
    {
        private EservicesdbContext _db;
        public TravelHotelService(EservicesdbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Control<TravelHotelDTO>> Get(int start, int take, string filter, string order, int cityId)
        {
            var repos = _db.TravelHotel.AsQueryable();

            if (cityId != 0)
            {
                repos = repos.Where(m => m.TravelCitiesId == cityId);
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
            return new Control<TravelHotelDTO>()
            {
                ListClass = await data.Select(m => new TravelHotelDTO
                {
                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id,
                    TravelCityId = m.TravelCitiesId.Value,
                    TravelCityName = m.TravelCities.Name,
                    Active = m.Active,
                    budgetId = m.BudgetId ?? 0,
                    budgetName = m.Budget != null ? m.Budget.BudgetName : "",
                    budgetNominal = m.Budget != null ? m.Budget.BudgetNominal.Value : decimal.Zero,
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<TravelHotelDTO> Get(int id)
        {

            return await _db.TravelHotel.Select(m => new TravelHotelDTO()
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                Active = m.Active,
                TravelCityId = m.TravelCitiesId.Value,
                TravelCityName = m.TravelCities.Name,
                budgetId = m.BudgetId ?? 0,
                budgetName = m.Budget != null ? m.Budget.BudgetName : "",
                budgetNominal = m.Budget != null ? m.Budget.BudgetNominal.Value : decimal.Zero,
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TravelHotel> Save(TravelHotel entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await _db.TravelHotel.AddAsync(entity);
            }
            else
            {
                _db.TravelHotel.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;
            }
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
