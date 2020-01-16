using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Car
{
    public class CarPoolService : ICarPoolService
    {
        private EservicesdbContext db;
        public CarPoolService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<CarPoolsDTO>> Get(int start, int take, string filter,
            string order, bool showActive, int regionId)
        {
            var repos = db.CarPools.AsQueryable();

            if (!showActive)
            {
                repos = repos.Where(m => m.Active == true);
            }

            if (regionId != 0)
            {
                repos = repos.Where(m => m.OfficeLocation.RegionId == regionId);
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

            return new Control<CarPoolsDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarPoolsDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        LicenseNo = m.LicensePlate,
                        OfficeLocationId = m.OfficeLocationId,
                        OfficeLocationName = m.OfficeLocation.Name,
                        Active = m.Active,
                        RegionId = m.OfficeLocation.RegionId ?? 0,
                        RegionName = m.OfficeLocation.Region.Name
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<CarPoolsDTO> Get(int id)
        {
            return await db.CarPools.Select(m => new CarPoolsDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                LicenseNo = m.LicensePlate,
                OfficeLocationId = m.OfficeLocationId,
                OfficeLocationName = m.OfficeLocation.Name,
                Active = m.Active,
                RegionId = m.OfficeLocation.RegionId ?? 0,
                RegionName = m.OfficeLocation.Region.Name
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<CarPoolsDTO>> GetActive(int start, int take, string filter,
            string order, bool showActive, int regionId,
            DateTime? startDate, DateTime? endDate)
        {
            var repos = db.CarPools.AsQueryable();

            if (startDate != null && endDate != null)
            {
                repos = repos.Where(m => !m.CarRequests.Any(r =>
                    r.StartTime >= startDate &&
                    r.EndTime <= endDate &&
                    r.Request.Currentstate.Name.ToLower() == "pic approved"));
            }

            if (!showActive)
            {
                repos = repos.Where(m => m.Active == true);
            }

            if (regionId != 0)
            {
                repos = repos.Where(m => m.OfficeLocation.RegionId == regionId);
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

            return new Control<CarPoolsDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarPoolsDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        LicenseNo = m.LicensePlate,
                        OfficeLocationId = m.OfficeLocationId,
                        OfficeLocationName = m.OfficeLocation.Name,
                        RegionId = m.OfficeLocation.RegionId ?? 0,
                        RegionName = m.OfficeLocation.Region.Name,
                        Active = m.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<CarPools> Save(CarPools entity)
        {
            if (entity.Id == 0)
            {
                await db.CarPools.AddAsync(entity);
            }
            else
            {
                db.CarPools.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
