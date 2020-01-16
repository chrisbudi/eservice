using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class RegionService : IRegionService
    {
        private EservicesdbContext db;


        public RegionService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<RegionDTO>> Get(int start, int take, string filter, string order,bool showActive)
        {
            var repos = db.OfficeLocationRegions.AsQueryable();
            if (!showActive)
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
            {
                repos = repos.OrderBy(order);
            }


            return new Control<RegionDTO>
            {
                ListClass = await repos.Skip(start * take).Take(take)
                .Select(m => new RegionDTO()
                {
                    Active = m.Active,
                    Description = m.Description,
                    Id = m.Id,
                    Name = m.Name
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<OfficeLocationRegions> Get(int id)
        {
            return await db.OfficeLocationRegions.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<OfficeLocationRegions> Save(OfficeLocationRegions entity)
        {
            if (entity.Id == 0)
            {
                await db.OfficeLocationRegions.AddAsync(entity);
            }
            else
            {
                db.OfficeLocationRegions.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
