using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class JabatanService : IJabatanService
    {

        private EservicesdbContext db;

        public JabatanService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<Jabatan>> Get(int start, int take, string filter, string order, bool showActive)
        {
            var repos = db.Jabatan.AsQueryable();
            if (showActive)
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

            var data = repos.Skip(start * take).Take(take);
            return new Control<Jabatan>()
            {
                ListClass = await data.ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<JabatanDTO> Get(int id)
        {
            return await db.Jabatan.Where(m => m.JabatanId == id)
                .Select(m => new JabatanDTO()
                {
                    JabatanId = m.JabatanId,
                    Name = m.Name,
                    Active = m.Active,
                    Description = m.Description,
                    Parent = m.JabatanChildJabatanChildNavigation.Select(c => new JabatanChildDTO()
                    {
                        JabatanId = c.ParentJabatanId,
                        Name = c.ParentJabatan.Name
                    }).ToList(),
                    Child = m.JabatanChildParentJabatan.Select(c => new JabatanChildDTO()
                    {
                        JabatanId = c.JabatanChildId,
                        Name = c.JabatanChildNavigation.Name
                    }).ToList()
                }).SingleOrDefaultAsync();
        }

        public async Task<Jabatan> Save(Jabatan entity)
        {
            if (entity.JabatanId == 0)
            {
                await db.Jabatan.AddAsync(entity);
            }
            else
            {
                db.Jabatan.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }


        public async Task<JabatanDTO> SaveChild(JabatanDTO entity)
        {
            var userroleremove = db.JabatanChild.Where(m => m.ParentJabatanId == entity.JabatanId);
            db.JabatanChild.RemoveRange(userroleremove);

            foreach (var data in entity.Child)
            {
                var userrole = new JabatanChild()
                {
                    JabatanChildId = data.JabatanId,
                    ParentJabatanId = entity.JabatanId
                };
                await db.JabatanChild.AddAsync(userrole);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
