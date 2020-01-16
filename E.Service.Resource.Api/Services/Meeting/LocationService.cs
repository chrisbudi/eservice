using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class LocationService : ILocationService
    {
        private EservicesdbContext db;

        public LocationService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<LocationDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.OfficeLocations.AsQueryable();
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
                    repos = repos.Where(m => m.Name.ToLower().Contains(item.ToLower()) ||
                    m.LocationType.Name.ToLower().Contains(item.ToLower()));


                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
            {
                repos = repos.OrderBy(order);
            }



            return new Control<LocationDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new LocationDTO
                    {
                        Description = m.Description,
                        Id = m.Id,
                        Name = m.Name,
                        LocationTypeId = m.LocationTypeId,
                        LocationType = m.LocationType.Name,
                        Active = m.Active,
                        RegionId = m.RegionId,
                        RegionName = m.Region.Name
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<LocationDTO> Get(int id)
        {
            return await db.OfficeLocations.Select(m => new LocationDTO()
            {
                Description = m.Description,
                Id = m.Id,
                LocationType = m.LocationType.Name,
                Name = m.Name,
                LocationTypeId = m.LocationTypeId,
                RegionName = m.Region != null ? m.Region.Name : "",
                RegionId = m.RegionId,
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<OfficeLocations> Save(OfficeLocations entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await db.OfficeLocations.AddAsync(entity);
            }
            else
            {
                db.OfficeLocations.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
