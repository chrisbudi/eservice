using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Repair;
using E.Service.Resource.Data.Interface.Repair.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Repair
{
    public class RepairItemService : IRepairItemService
    {
        private EservicesdbContext _db;
        public RepairItemService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<RepairItemDTO> Get(int id)
        {
            return await _db.RepairItem.Select(m => new RepairItemDTO()
            {
                Description = m.Description,
                Name = m.Name,
                Id = m.Id,
                JenisId = m.JenisId,
                JenisName = m.Jenis.JenisNama,
                ItItem = m.ItItem,
                LocationId = m.LocationId,
                LocationName = m.Location.Name,
                RepairType = ((ERepairTypes)Enum.Parse(typeof(ERepairTypes), m.RepairType)).Description(),
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<RepairItemDTO>> Get(int start, int take, string filter, string order, bool showActive, ERepairTypes? repairTypes, int locationId = 0)
        {
            var repos = _db.RepairItem.AsQueryable();

            if (repairTypes != null)
            {
                repos = repos.Where(m => m.RepairType == repairTypes.Description());
            }

            if (locationId != 0)
            {
                repos = repos.Where(m => m.LocationId == locationId);
            }

            if (showActive == true)
            {
                repos = repos.Where(m => m.Active  == true);
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
            return new Control<RepairItemDTO>()
            {
                ListClass = await data.Select(m => new RepairItemDTO
                {

                    Description = m.Description,
                    Name = m.Name,
                    Id = m.Id,
                    JenisId = m.JenisId,
                    JenisName = m.Jenis.JenisNama,
                    ItItem = m.ItItem,
                    LocationId = m.LocationId,
                    LocationName = m.Location.Name,
                    RepairType = ((ERepairTypes)Enum.Parse(typeof(ERepairTypes), m.RepairType)).Description(),
                    Active = m.Active
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<RepairItem> Save(RepairItem entity)
        {
            if (entity.Id == 0)
            {
                await _db.RepairItem.AddAsync(entity);
            }
            else
            {
                _db.RepairItem.Update(entity);
            }

            try
            {
                await _db.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString(), ex.InnerException);
            }



            return entity;
        }
    }
}
