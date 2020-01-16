using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Car
{
    public class CarCoordinateStatusService : ICarCoordinateStatusService
    {

        private EservicesdbContext db;

        public CarCoordinateStatusService(EservicesdbContext db)
        {
            this.db = db;
        }


        public async Task<Control<CarCoordinateStatusDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.CarRequestCoordinateStatus.AsQueryable();

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
                repos = repos.OrderBy(order);

            return new Control<CarCoordinateStatusDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new CarCoordinateStatusDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Color = m.Color,
                        Active = m.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<CarCoordinateStatusDTO> Get(int id)
        {
            return await db.CarRequestCoordinateStatus.Where(m => m.Id == id)
                .Select(m => new CarCoordinateStatusDTO()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Active = m.Active,
                    Color = m.Color
                }).SingleOrDefaultAsync();
        }

        public async Task<CarRequestCoordinateStatus> Save(CarRequestCoordinateStatus entity)
        {
            if (entity.Id == 0)
            {
                await db.CarRequestCoordinateStatus.AddAsync(entity);
            }
            else
            {
                db.CarRequestCoordinateStatus.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
