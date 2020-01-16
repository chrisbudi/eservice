using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Asset
{
    public class AssetBrandService : IAssetBrandService
    {
        private EservicesdbContext db;
        public AssetBrandService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<AssetBrandDTO> GetBrand(int id)
        {
            return await db.AssetBrands.Select(m => new AssetBrandDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Active = m.Active,
                AssetBrand = m.AssetBrandSeries.Select(a => new AssetBrandSeriesDTO()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Active = a.Active
                }).ToList()
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<AssetBrandDTO>> GetBrandData(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AssetBrands.AsQueryable();

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

            return new Control<AssetBrandDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetBrandDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Active = m.Active,
                        AssetBrand = m.AssetBrandSeries.Select(a => new AssetBrandSeriesDTO()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Active = a.Active
                        }).ToList()
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<Control<AssetBrandSeriesDTO>> GetBrandSeries(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AssetBrandSeries.AsQueryable();

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

            return new Control<AssetBrandSeriesDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetBrandSeriesDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Active = m.Active,
                        AssetBrand = new AssetBrandDTO()
                        {
                            Id = m.Brand.Id,
                            Name = m.Brand.Name,
                            Description = m.Brand.Description,
                            Active = m.Brand.Active
                        }
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetBrandSeriesDTO> GetBrandSeries(int id)
        {
            return await db.AssetBrandSeries.Select(m => new AssetBrandSeriesDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Active = m.Active,
                AssetBrand = new AssetBrandDTO()
                {
                    Id = m.Brand.Id,
                    Name = m.Brand.Name,
                    Description = m.Brand.Description,
                    Active = m.Brand.Active
                }
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<AssetBrands> SaveBrand(AssetBrands entity)
        {
            if (entity.Id == 0)
            {
                await db.AssetBrands.AddAsync(entity);
            }
            else
            {
                db.AssetBrands.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<AssetBrandSeries> SaveBrandSeries(AssetBrandSeries entity)
        {
            if (entity.Id == 0)
            {
                await db.AssetBrandSeries.AddAsync(entity);
            }
            else
            {
                db.AssetBrandSeries.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
