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
    public class UserTargetService : IUserTargetService
    {

        private EservicesdbContext db;

        public UserTargetService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<List<TargetDTO>> GetAvaliableTarget()
        {
            return await db.Target.Select(m => new TargetDTO()
            {
                TargetId = m.Targetid,
                TargetName = m.Name
            }).ToListAsync();
        }

        public async Task<List<TargetDTO>> GetselectedTarget(string userId)
        {

            return await db.TargetUser.Where(t => t.UserId == userId).Select(m => new TargetDTO()
            {
                TargetId = m.TargetId.Value,
                TargetName = m.Target.Name
            }).ToListAsync();
        }

            public async Task<UserTargetDTO> UserTarget(UserTargetDTO entity)
        {
            var userroleremove = db.TargetUser.Where(m => m.UserId == entity.UserId);
            db.TargetUser.RemoveRange(userroleremove);


            foreach (var data in entity.Targets)
            {
                var userrole = new TargetUser()
                {
                    UserId = entity.UserId,
                    TargetId = data.TargetId

                };
                await db.TargetUser.AddAsync(userrole);
            }
            await db.SaveChangesAsync();

            return entity;
        }

    }
}
