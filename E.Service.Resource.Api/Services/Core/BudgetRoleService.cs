using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using E.Service.Resource.Data.Types;

namespace E.Service.Resource.Api.Services.Core
{
    public class BudgetRoleService : IBudgetRoleService
    {

        private EservicesdbContext db;

        public BudgetRoleService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<BudgetRoleDTO>> Get(int start, int take, string filter, string order, bool showActive, string roleId)
        {
            var repos = db.BudgetRole.Where(m => m.RoleId == roleId).AsQueryable();

            if (!showActive)
            {
                repos = repos.Where(m => m.Budget.Active == true);
            }
            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Budget.BudgetName.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<BudgetRoleDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new BudgetRoleDTO
                    {
                        BudgetDesc = m.Budget.BudgetDesc,
                        BudgetId = m.BudgetId.Value,
                        BudgetName = m.Budget.BudgetName,
                        BudgetNominal = m.Budget.BudgetNominal.Value,
                        RoleId = m.RoleId,
                        RoleName = m.Role.Name,
                        BudgetRoleid = m.BudgetRoleId,
                        Active = m.Budget.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<BudgetRoleDTO> Get(int id)
        {
            return await db.BudgetRole.Select(m => new BudgetRoleDTO()
            {
                BudgetDesc = m.Budget.BudgetDesc,
                BudgetId = m.BudgetId.Value,
                BudgetName = m.Budget.BudgetName,
                BudgetNominal = m.Budget.BudgetNominal.Value,
                RoleId = m.RoleId,
                RoleName = m.Role.Name,
                BudgetRoleid = m.BudgetRoleId,
                Active = m.Budget.Active
            }).SingleAsync(m => m.BudgetId == id);
        }

        public async Task<Control<BudgetRoleDTO>> GetName(int start, int take, string filter, string order, bool showActive, EBudgetRole budgetRole)
        {
            var repos = db.BudgetRole.Where(m => m.Role.Name.ToUpper() == budgetRole.Description().ToUpper()).AsQueryable();

            if (!showActive)
            {
                repos = repos.Where(m => m.Budget.Active == true);
            }
            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Budget.BudgetName.ToLower().Contains(item.ToLower()) ||
                        m.Budget.BudgetDesc.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }
            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<BudgetRoleDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new BudgetRoleDTO
                    {

                        BudgetDesc = m.Budget.BudgetDesc,
                        BudgetId = m.BudgetId.Value,
                        BudgetName = m.Budget.BudgetName,
                        BudgetNominal = m.Budget.BudgetNominal.Value,
                        RoleId = m.RoleId,
                        RoleName = m.Role.Name,
                        BudgetRoleid = m.BudgetRoleId,
                        Active = m.Budget.Active
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<string> GetRoleId(EBudgetRole budgetRole)
        {
            var role = budgetRole.Description();
            var data = await db.AspNetRoles.SingleAsync(m => m.Name.Equals(role));
            return data.Id;
        }

        public async Task<Budget> Save(Budget entity)
        {
            if (entity.BudgetId == 0)
            {
                await db.Budget.AddAsync(entity);
            }
            else
            {
                db.Budget.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
