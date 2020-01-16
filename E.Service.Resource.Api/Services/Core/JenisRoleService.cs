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
using E.Service.Resource.Data.Types;

namespace E.Service.Resource.Api.Services.Core
{
    public class JenisRoleService : IJenisRoleService
    {

        private EservicesdbContext db;

        public JenisRoleService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<JenisRoleDTO>> Get(int start, int take, string filter, string order, bool showActive, string roleId)
        {
            var repos = db.JenisRole.Where(m => m.RoleId == roleId).AsQueryable();
            if (!showActive)
            {
                repos = repos.Where(m => m.Jenis.Active == true);
            }
            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Jenis.JenisNama.ToLower().Contains(item.ToLower()) ||
                        m.Jenis.JenisDesc.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<JenisRoleDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new JenisRoleDTO
                    {

                        RoleId = m.RoleId,
                        RoleName = m.Role.Name,
                        JenisId = m.JenisId.Value,
                        JenisDesc = m.Jenis.JenisDesc,
                        JenisName = m.Jenis.JenisNama,
                        JenisRoleId = m.JenisRoleId,
                        Active = m.Jenis.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<JenisRoleDTO> Get(int id)
        {
            return await db.JenisRole.Select(m => new JenisRoleDTO()
            {

                RoleId = m.RoleId,
                RoleName = m.Role.Name,
                JenisId = m.JenisId.Value,
                JenisDesc = m.Jenis.JenisDesc,
                JenisName = m.Jenis.JenisNama,
                JenisRoleId = m.JenisRoleId,
                Active = m.Jenis.Active
            }).SingleAsync(m => m.JenisId == id);
        }

        public async Task<Control<JenisRoleDTO>> GetName(int start, int take, string filter, string order, bool showActive, EJenisRole jenisRole)
        {
            var repos = db.JenisRole.Where(m => m.Role.Name.ToUpper() == jenisRole.Description().ToUpper()).AsQueryable();

            if (!showActive)
            {
                repos = repos.Where(m => m.Jenis.Active == true);
            }
            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Jenis.JenisNama.ToLower().Contains(item.ToLower()) ||
                        m.Jenis.JenisDesc.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<JenisRoleDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new JenisRoleDTO
                    {

                        RoleId = m.RoleId,
                        RoleName = m.Role.Name,
                        JenisId = m.JenisId.Value,
                        JenisDesc = m.Jenis.JenisDesc,
                        JenisName = m.Jenis.JenisNama,
                        JenisRoleId = m.JenisRoleId,
                        Active = m.Jenis.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<string> GetRoleId(EJenisRole jenisRole)
        {
            var role = jenisRole.Description();
            var data = await db.AspNetRoles.SingleAsync(m => m.Name.Equals(role));
            return data.Id;
        }

        public async Task<Jenis> Save(Jenis entity)
        {
            if (entity.JenisId == 0)
            {
                await db.Jenis.AddAsync(entity);
            }
            else
            {
                db.Jenis.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
