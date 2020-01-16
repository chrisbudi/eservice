using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class RoomsService : IRoomsService
    {
        private EservicesdbContext db;

        public RoomsService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<RoomsDTO>> Get(int start, int take, string filter, string order, bool showActive, int departmentId, int locationId)
        {
            var repos = db.OfficeRooms.AsQueryable();
            if (!showActive)
            {
                repos = repos.Where(m => m.Active == true);
            }

            if (departmentId != 0)
            {
                repos = repos.Where(m => m.DepartmentId == departmentId);
            }

            if (locationId != 0)
            {
                repos = repos.Where(m => m.OfficeLocationId == locationId);
            }


            int totalData = repos.Count();
            int totalFilterData = totalData;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');

                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Name.ToLower().Contains(item.ToLower()) ||
                        m.OfficeLocation.Name.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
            {
                repos = repos.OrderBy(order);
            }


            return new Control<RoomsDTO>
            {
                ListClass = await repos.Skip(start * take).Take(take).
                Select(m => new RoomsDTO
                {
                    Description = m.Description,
                    Id = m.Id,
                    Name = m.Name,
                    LocationId = m.OfficeLocationId,
                    LocationName = m.OfficeLocation.Name,
                    DepartementId = m.DepartmentId,
                    Active = m.Active
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<RoomsDTO> Get(int id)
        {
            return await db.OfficeRooms.Select(m => new RoomsDTO()
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                LocationId = m.OfficeLocationId,
                LocationName = m.OfficeLocation.Name,
                DepartementId = m.DepartmentId,
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<OfficeRooms> Save(OfficeRooms entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await db.OfficeRooms.AddAsync(entity);
            }
            else
            {
                db.OfficeRooms.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
