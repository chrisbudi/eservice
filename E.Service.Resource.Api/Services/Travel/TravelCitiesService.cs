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
    public class TravelCitiesService : ITravelCitiesService
    {
        private EservicesdbContext _db;
        public TravelCitiesService(EservicesdbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Control<TravelCitiesDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = _db.TravelCities.AsQueryable();

            if (showActive)
            {
                repos = repos.Where(m => m.Active ==  true);
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
            return new Control<TravelCitiesDTO>()
            {
                ListClass = await data.Select(m => new TravelCitiesDTO
                {
                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id,
                    
                    
                    Active = m.Active
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<TravelCitiesDTO> Get(int id)
        {
            return await _db.TravelCities.Select(m => new TravelCitiesDTO()
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);

        }

        public async Task<TravelCities> Save(TravelCities entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await _db.TravelCities.AddAsync(entity);
            }
            else
            {
                _db.TravelCities.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;
            }
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
