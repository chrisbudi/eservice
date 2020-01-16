using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Car
{
    public class DriverService : IDriverService
    {

        private EservicesdbContext db;

        public DriverService(EservicesdbContext db)
        {
            this.db = db;
        }
        public async Task<Control<CarDriverDTO>> Get(int start, int take,
            string filter, string order, bool showActive, bool activeonly, int regionId)
        {
            var repos = db.CarDrivers.Include(m => m.User).AsQueryable();

            if (!showActive)
            {
                repos = repos.Where(m => m.Active == true);
            }

            if (activeonly)
            {
                repos = repos.Where(m => !m.CarRequestBudget.Any(c => c.RequestStart != null && c.RequestEnd != null));
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

            return new Control<CarDriverDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarDriverDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        UserId = m.Userid ?? 0,
                        UserName = m.User == null ? "" : m.User.Name,
                        LocationId = m.OfficeLocationId,
                        LocationName = m.OfficeLocation.Name,
                        PhoneNumber = m.PhoneNumber,
                        CurrentNominalSaldo = (m.CarRequestBudget
                            .Where(d => d.DriverId == m.Id)
                            .Sum(d => d.CarBudgetDetail
                            .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Add && dd.Done == true)
                            .Sum(dd => dd.Nominal ?? 0))) -
                            (m.CarRequestBudget
                            .Where(d => d.DriverId == m.Id)
                            .Sum(d => d.CarBudgetDetail
                            .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Min && dd.Done == true)
                            .Sum(dd => dd.Nominal ?? 0))),
                        Active = m.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<CarDriverDTO> Get(int id)
        {
            return await db.CarDrivers.Include(m => m.User).Select(m => new CarDriverDTO()
            {
                Id = m.Id,
                Name = m.Name,
                UserId = m.Userid ?? 0,
                UserName = m.User == null ? "" : m.User.Name,
                Description = m.Description,
                LocationId = m.OfficeLocationId,
                LocationName = m.OfficeLocation.Name,
                PhoneNumber = m.PhoneNumber,
                Active = m.Active,
                CurrentNominalSaldo = (m.CarRequestBudget
                            .Where(d => d.Checked && d.DriverId == m.Id)
                            .Sum(d => d.CarBudgetDetail
                            .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Add)
                            .Sum(dd => dd.Nominal ?? 0))) - m.CarRequestBudget
                            .Where(d => d.Checked && d.DriverId == m.Id)
                            .Sum(d => d.CarBudgetDetail
                            .Where(dd => dd.CarRequestDetailStatusId == (int)EDriverBudgetStatusType.Min)
                            .Sum(dd => dd.Nominal ?? 0)),
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<CarDriverDTO> GetUserID(int id)
        {
            return await db.CarDrivers.Where(m => m.Userid == id).Select(m => new CarDriverDTO()
            {
                Id = m.Id,
                Name = m.Name,
                UserId = m.Userid ?? 0,
                UserName = m.User == null ? "" : m.User.Name,
                Description = m.Description,
                LocationId = m.OfficeLocationId,
                LocationName = m.OfficeLocation.Name,
                PhoneNumber = m.PhoneNumber,
                Active = m.Active,
            }).SingleAsync();
        }

        public async Task<CarDrivers> Save(CarDrivers entity)
        {
            if (entity.Id == 0)
            {
                await db.CarDrivers.AddAsync(entity);
            }
            else
            {
                db.CarDrivers.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
