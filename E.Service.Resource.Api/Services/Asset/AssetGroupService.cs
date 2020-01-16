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
    public class AssetGroupService : IAssetGroupService
    {
        private EservicesdbContext db;

        public AssetGroupService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<AssetGroupDTO> GetGroup(int id)
        {
            return await db.AssetMainGroupTypes.Select(m => new AssetGroupDTO()
            {
                Id = m.Id,
                Name = m.Name,
                kode = m.Kode,
                Description = m.Description,
                Active = m.Active,
                AssetSubGroup = m.AssetSubGroupTypes.Select(a => new AssetSubGroupDTO()
                {
                    Id = a.Id,
                    kode = a.Kode,
                    Description = a.Description,
                    Name = a.Name,
                    Active = a.Active
                }).ToList()
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<AssetGroupDTO>> GetGroupData(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AssetMainGroupTypes.AsQueryable();

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

            return new Control<AssetGroupDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetGroupDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        kode = m.Kode,
                        Description = m.Description,
                        Active = m.Active,
                        AssetSubGroup = m.AssetSubGroupTypes.Select(a => new AssetSubGroupDTO()
                        {
                            Id = a.Id,
                            kode = a.Kode,
                            Description = a.Description,
                            Name = a.Name,
                            Active = a.Active
                        }).ToList()
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<Control<AssetSubGroupDTO>> GetSubGroup(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AssetSubGroupTypes.AsQueryable();

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

            return new Control<AssetSubGroupDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetSubGroupDTO
                    {
                        Id = m.Id,
                        kode = m.Kode,
                        Name = m.Name,
                        Description = m.Description,
                        GroupId = m.MainGroup.Id,
                        GroupName = m.MainGroup.Name,
                        Active = m.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetSubGroupDTO> GetSubGroup(int id)
        {
            return await db.AssetSubGroupTypes.Select(m => new AssetSubGroupDTO()
            {
                Id = m.Id,
                Name = m.Name,
                kode = m.Kode,
                Description = m.Description,
                GroupId = m.MainGroup.Id,
                GroupName = m.MainGroup.Name,
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<AssetMainGroupTypes> SaveGroup(AssetMainGroupTypes entity)
        {
            if (entity.Id == 0)
            {
                await db.AssetMainGroupTypes.AddAsync(entity);
            }
            else
            {
                db.AssetMainGroupTypes.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<AssetSubGroupTypes> SaveSubGroup(AssetSubGroupTypes entity)
        {
            if (entity.Id == 0)
            {
                await db.AssetSubGroupTypes.AddAsync(entity);
            }
            else
            {
                db.AssetSubGroupTypes.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
