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
    public class TravelTransportationTypeService : ITravelTransportationTypeService
    {
        private EservicesdbContext _db;
        public TravelTransportationTypeService(EservicesdbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<Control<TravelTransportationTypeDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = _db.TravelTransportationType.AsQueryable();
            if (showActive)
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
            return new Control<TravelTransportationTypeDTO>()
            {
                ListClass = await data.Select(m => new TravelTransportationTypeDTO
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

        public async Task<TravelTransportationTypeDTO> Get(int id)
        {
            return await _db.TravelTransportationType.Select(m => new TravelTransportationTypeDTO()
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TravelTransportationType> Save(TravelTransportationType entity)
        {
            if (entity.Id == 0)
            {
                await _db.TravelTransportationType.AddAsync(entity);
            }
            else
            {
                _db.TravelTransportationType.Update(entity);
            }
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
