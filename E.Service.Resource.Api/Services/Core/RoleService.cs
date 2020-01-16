using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using E.Service.Resource.Data.Interface;
using System.Security.Cryptography;

namespace E.Service.Resource.Api.Services.Core
{
    public class RoleService : IRoleService
    {

        private EservicesdbContext db;

        public RoleService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<RoleDTO>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.AspNetRoles.AsQueryable();

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

            return new Control<RoleDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new RoleDTO
                    {
                        Id = m.Id,
                        Name = m.Name
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<RoleDTO> Get(string id)
        {
            return await db.AspNetRoles.Select(m => new RoleDTO()
            {
                Id = m.Id,
                Name = m.Id
            }).SingleAsync(m => m.Id == id);
        }


        public async Task<List<RoleDTO>> GetAvaliableRole()
        {
            return await db.AspNetRoles.Where(m =>
            m.Name.StartsWith("Create") ||
            m.Name.StartsWith("View") ||
            m.Name.StartsWith("Cancel") ||
            m.Name.StartsWith("Approval") ||
            m.Name.StartsWith("Dashboard") ||
            m.Name.StartsWith("History")).OrderBy(m => m.Name)
                .Select(m => new RoleDTO()
                {
                    Name = m.Name,
                    Id = m.Id
                }).ToListAsync();
        }

        public async Task<List<RoleDTO>> GetselectedRole(string userid)
        {
            return await db.AspNetUserRoles.Where(m => m.UserId == userid).Select(m => new RoleDTO
            {
                Id = m.RoleId,
                Name = m.Role.Name
            }).ToListAsync();
        }
        public Task<UserDTO> Get(int id)
        {

            throw new NotImplementedException();
        }

        public async Task<Users> User(UserDTO user)
        {
            var entity = new Users()
            {
                Id = user.Id,
                DepartmentId = user.DepartmentId,
                JabatanId = user.JabatanId,
                LocationId = user.LocationId,
                Name = user.Name,
                UpdatedAt = DateTime.Now,
                Active = user.Active,
                UserId = user.UserId,
                DeviceId = user.DeviceId
            };

            db.Users.Update(entity);
                

            db.Entry(entity).Property(x => x.UserId).IsModified = false;
            db.Entry(entity).Property(x => x.CreatedAt).IsModified = false;


            var entityUser = await db.AspNetUsers.AsNoTracking().SingleAsync(m => m.Id == entity.UserId);
            entityUser.IsEnabled = user.Active;
            db.AspNetUsers.Update(entityUser);


            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<UserRoleDTO> UserRole(UserRoleDTO userRole)
        {

            var itemInId = db.AspNetUserRoles.Where(m => m.UserId == userRole.UserId).AsNoTracking();

            foreach (var item in itemInId)
            {
                var newrole = new AspNetUserRoles()
                {
                    UserId = userRole.UserId,
                    RoleId = item.RoleId
                };


                db.Remove(newrole);

            }

            foreach (var user in userRole.Roles)
            {

                var uRole = new AspNetUserRoles()
                {
                    RoleId = user.RoleId,
                    UserId = userRole.UserId
                };

                await db.AspNetUserRoles.AddAsync(uRole);
            }

            await db.SaveChangesAsync();

            return userRole;
        }
    }
}
