using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Core
{
    public class GroupService : IGroupService
    {
        private EservicesdbContext db;

        public GroupService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<GroupDTO>> GetList(int start, int take, string filter, string order)
        {
            var repos = db.AspNetGroups.AsQueryable();
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

            return new Control<GroupDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new GroupDTO
                    {
                        GroupId = m.Id,
                        GroupName = m.Name,
                        GroupDesc = m.Description
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<GroupDTO> Get(string id)
        {
            return await db.AspNetGroups.Select(m => new GroupDTO()
            {
                GroupName = m.Name,
                GroupId = m.Id,
                GroupDesc = m.Description
            }).SingleAsync(m => m.GroupId == id);
        }

        public async Task<AspNetGroups> Save(AspNetGroups entity)
        {
            if (!db.AspNetGroups.Any(m => m.Id == entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                await db.AspNetGroups.AddAsync(entity);
            }
            else
            {
                db.AspNetGroups.Update(entity);
            }

            await db.SaveChangesAsync();
            return entity;
        }
    }
}
