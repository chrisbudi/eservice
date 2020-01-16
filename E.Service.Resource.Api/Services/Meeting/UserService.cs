using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class UserService : IUserService
    {
        private EservicesdbContext db;

        public UserService(EservicesdbContext db)
        {
            this.db = db;
        }

        public Task<int> GetDepartmentId(int userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<UserDTO>> GetRequestActionTargetUser(int requestActionId)
        {


            var target = db.Actiontarget.Where(m => m.Actionid == requestActionId).Select(m => m.Targetid);

            var targetUser = db.TargetUser.Where(m => target.Any(t => t.Value == m.TargetId)).Select(m => m.UserId);


            return await db.Users.Where(m => targetUser.Any(t => t == m.UserId)).Select(m => new UserDTO()
            {
                UserId = m.UserId,
                Id = m.Id,
                DepartmentId = m.DepartmentId,
                Email = m.User.Email,
                Name = m.User.FirstName + " " +  m.User.LastName,
                DeviceId = m.DeviceId   
            }).ToListAsync();
        }

        public async Task<UserDTO> GetUser(string userId)
        {
            var user = await db.Users
                .Include(m => m.Jabatan)
                .Include(m => m.Location)
                    .ThenInclude(m => m.Region)
                .Select(m => new UserDTO()
                {
                    Id = m.Id,
                    Name = m.Name,
                    DepartmentId = m.DepartmentId,
                    JabatanId = m.JabatanId,
                    JabatanName = m.Jabatan.Name,
                    UserId = m.UserId,
                    LocationId = m.LocationId.Value,
                    LocationName = m.Location.Name,
                    RegionId = m.Location != null ? m.Location.RegionId ?? 0 : 0,
                    RegionName = m.Location.Region != null ? m.Location.Region.Name : "",
                    Active = m.Active,
                    DeviceId = m.DeviceId,
                    MeetingRoomIds = m.MeetingRooms.Select(r => r.Id).ToList()
                }).SingleOrDefaultAsync(m => m.UserId == userId);

            user.Roles = (from p in db.AspNetUserRoles
                          join q in db.AspNetRoles on p.RoleId equals q.Id
                          where p.UserId == user.UserId
                          select q.NormalizedName).ToList();

            user.GroupRoles = (from p in db.AspNetRoleGroup
                               join q in db.AspNetRoles on p.RoleId equals q.Id
                               join r in db.AspNetUserGroup on p.GroupId equals r.GroupId
                               where r.UserId == user.UserId
                               select q.NormalizedName).ToList();

            return user;
        }

        public async Task<UserDTO> GetUserById(int userId)
        {

            return await db.Users
                .Include(m => m.Jabatan)
                .Include(m => m.Location)
                .Select(m => new UserDTO()
                {
                    Id = m.Id,
                    Name = m.Name,
                    DepartmentId = m.DepartmentId,
                    JabatanId = m.JabatanId,
                    JabatanName = m.Jabatan.Name,
                    UserId = m.UserId,
                    LocationId = m.LocationId.Value,
                    LocationName = m.Location.Name,
                    Email = m.User.Email,
                    Active = m.Active,
                    DeviceId = m.DeviceId
                }).SingleOrDefaultAsync(m => m.Id == userId);
        }

        public async Task<List<UserDTO>> GetUserInProcessId(int processId)
        {
            var dataUser = await (from p in db.Actiontarget
                                  join q in db.TargetUser on p.Targetid equals q.TargetId
                                  join r in db.Users on q.UserId equals r.UserId
                                  where p.Action.Processid == processId
                                  select new UserDTO()
                                  {
                                      Id = r.Id,
                                      Name = r.Name,
                                      DepartmentId = r.DepartmentId,
                                      JabatanId = r.JabatanId,
                                      JabatanName = r.Jabatan.Name,
                                      UserId = r.UserId,
                                      LocationId = r.LocationId.Value,
                                      LocationName = r.Location.Name,
                                      Email = r.User.Email
                                  }).ToListAsync();

            return dataUser;
        }

        public async Task<Control<Users>> GetUsers(int start, int take, string filter, string order, bool active)
        {
            var repos = db.Users.Include(m => m.User).AsQueryable();

            if (active == true)
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
            {
                repos = repos.OrderBy(order);
            }


            return new Control<Users>
            {
                ListClass = await repos.Skip(start * take).Take(take).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }
    }
}
