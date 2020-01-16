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
    public class UserRoleService : IUserRoleService
    {

        private EservicesdbContext db;

        public UserRoleService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<UserRoleDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AspNetUsers.AsQueryable();

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.UserName.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<UserRoleDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new UserRoleDTO
                    {
                        UserId = m.Id,
                        UserName = m.UserName,
                        Roles = db.AspNetUserRoles.Where(r => r.UserId == m.Id).Select(ur => new Role()
                        {
                            RoleId = ur.RoleId,
                            RoleName = ur.Role.Name
                        }).ToList()
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<UserRoleDTO> Get(string id)
        {
            var repos = db.AspNetUsers.Single(m => m.Id == id);

            return new UserRoleDTO()
            {
                UserId = repos.Id,
                UserName = repos.UserName,
                Roles = db.AspNetUserRoles.Where(m => m.UserId == id).Select(m => new Role()
                {
                    RoleId = m.RoleId,
                    RoleName = m.Role.Name
                }).ToList()
            };
        }

        public async Task<Control<Role>> GetRole(int start, int take, string filter, string order, string type)
        {
            var repos = db.AspNetRoles.Where(m => m.Name.StartsWith(type)).AsQueryable();


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

            return new Control<Role>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new Role
                    {
                        RoleId = m.Id,
                        RoleName = m.Name
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<UserRoleDTO> Save(UserRoleDTO entity)
        {
            var userroleremove = db.AspNetUserRoles.Where(m => m.UserId == entity.UserId);
            db.AspNetUserRoles.RemoveRange(userroleremove);


            foreach (var data in entity.Roles)
            {
                var userrole = new AspNetUserRoles()
                {
                    RoleId = data.RoleId,
                    UserId = entity.UserId
                };
                await db.AspNetUserRoles.AddAsync(userrole);
            }
            await db.SaveChangesAsync();

            return entity;
        }
    }
}
