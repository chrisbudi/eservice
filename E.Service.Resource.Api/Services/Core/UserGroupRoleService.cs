using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
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
    public class UserGroupRoleService : IUserGroupRoleService
    { 
        private EservicesdbContext db;

        public UserGroupRoleService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<List<GroupDTO>> GetAvaliableGroup()
        {
            return await db.AspNetGroups.Select(m => new GroupDTO()
            {
                GroupId = m.Id,
                GroupName = m.Name
            }).ToListAsync();
        }

        public async Task<List<RoleDTO>> GetAvaliableRole()
        {

            return await db.AspNetRoles.Where(m =>
            m.Name.StartsWith("Create") ||
            m.Name.StartsWith("View") ||
            m.Name.StartsWith("Cancel") ||
            m.Name.StartsWith("Approval") ||
            m.Name.StartsWith("Dashboard") ||
            m.Name.StartsWith("Can") ||
            m.Name.StartsWith("History")).OrderBy(m => m.Name)
                .Select(m => new RoleDTO()
                {
                    Name = m.Name,
                    Id = m.Id
                }).ToListAsync();
        }


        public async Task<List<GroupDTO>> GetselectedGroup(string userId)
        {
            return await db.AspNetUserGroup.Where(m => m.UserId == userId).Select(m => new GroupDTO()
            {
                GroupId = m.GroupId,
                GroupName = m.Group.Name,
                GroupDesc = m.Group.Description
            }).ToListAsync();
        }

        public async Task<List<RoleDTO>> GetselectedRole(string groupId)
        {
            return await db.AspNetRoleGroup.Where(m => m.GroupId == groupId).Select(m => new RoleDTO()
            {
                Name = m.Role.Name,
                Id = m.Role.Id
            }).ToListAsync();
        }


        public async Task<Control<GroupDTO>> GroupList(int start, int take, string filter, string order)
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
                        GroupName = m.Name
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<GroupRoleDTO> GroupRole(GroupRoleDTO groupRole)
        {
            var itemInId = db.AspNetRoleGroup.Where(m => m.GroupId == groupRole.GroupId).AsNoTracking();

            foreach (var item in itemInId)
            {
                var newrole = new AspNetRoleGroup()
                {
                    GroupId = groupRole.GroupId,
                    RoleId = item.RoleId
                };


                db.Remove(newrole);

            }

            foreach (var user in groupRole.Roles)
            {

                var uRole = new AspNetRoleGroup()
                {
                    RoleId = user.Id,
                    GroupId = groupRole.GroupId
                };

                await db.AspNetRoleGroup.AddAsync(uRole);
            }

            await db.SaveChangesAsync();

            return groupRole;
        }

        public async Task<Control<UserDTO>> UserList(int start, int take, string filter, string order)
        {
            var repos = db.Users.AsQueryable();


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

            return new Control<UserDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new UserDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        LocationId = m.LocationId ?? 0,
                        JabatanName = m.Jabatan.Name,
                        JabatanId = m.JabatanId,
                        DepartmentId = m.DepartmentId,
                        Email = m.User.Email,
                        UserId = m.UserId,
                        LocationName = m.Location.Name,
                        RegionId = m.Location.RegionId ?? 0,
                        RegionName = m.Location.Region.Name,
                        Active = m.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<UserGroupDTO> UserGroup(UserGroupDTO userGroup)
        {
            var itemInId = db.AspNetUserGroup.Where(m => m.UserId == userGroup.UserId).AsNoTracking();

            foreach (var item in itemInId)
            {
                var newrole = new AspNetUserGroup()
                {
                    GroupId = item.GroupId,
                    UserId = userGroup.UserId
                };


                db.Remove(newrole);

            }

            foreach (var group in userGroup.Group)
            {

                var uRole = new AspNetUserGroup()
                {
                    UserId = userGroup.UserId,
                    GroupId = group.GroupId
                };

                await db.AspNetUserGroup.AddAsync(uRole);
            }

            await db.SaveChangesAsync();

            return userGroup;
        }
    }
}
