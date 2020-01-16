using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Asset
{
    public class AssetTypeService : IAssetTypeService
    {
        private EservicesdbContext db;

        public AssetTypeService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<AssetTypesDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AssetTypes.AsQueryable();
            if (!showActive)
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

            return new Control<AssetTypesDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetTypesDTO
                    {
                        Depreciation = m.Depreciation,
                        Description = m.Description,
                        Id = m.Id,
                        Name = m.Name,
                        UsagePeriod = m.UsagePeriod,
                        Active = m.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetTypesDTO> Get(int id)
        {
            return await db.AssetTypes.Select(m => new AssetTypesDTO()
            {

                Depreciation = m.Depreciation,
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                UsagePeriod = m.UsagePeriod,
                Active = m.Active
            }).SingleAsync(m => m.Id == id);
        }

        public async Task<AssetTypes> Save(AssetTypes entity)
        {

            if (entity.Id == 0)
            {
                await db.AssetTypes.AddAsync(entity);
            }
            else
            {
                db.AssetTypes.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
