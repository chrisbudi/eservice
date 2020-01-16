using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class LocationTypeService : ILocationTypeService
    {
        private EservicesdbContext db;

        public LocationTypeService(EservicesdbContext db)
        {
            this.db = db;
        }
        public async Task<Control<LocationTypeDTO>> Get(int start, int take, string filter, string order,bool showActive)
        {
            
            var repos = db.OfficeLocationType.AsQueryable();
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

            return new Control<LocationTypeDTO>()
            {
                ListClass = repos.Skip(start * take).Take(take).
                    Select(m => new LocationTypeDTO
                    {
                        Description = m.Description,
                        Id = m.Id,
                        Name = m.Name,
                        Active = m.Active
                    }).ToList(),
                TotalFilter = totalFilterData,
                Total = totalData
            };
        }

        public async Task<OfficeLocationType> Get(int id)
        {
            return await db.OfficeLocationType.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<OfficeLocationType> Save(OfficeLocationType entity)
        {
            if (entity.Id == 0)
            {
                await db.OfficeLocationType.AddAsync(entity);
            }
            else
            {
                db.OfficeLocationType.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
