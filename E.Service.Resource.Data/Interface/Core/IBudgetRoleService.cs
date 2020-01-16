using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IBudgetRoleService
    {
        Task<Budget> Save(Budget entity);

        Task<Control<BudgetRoleDTO>> Get(int start, int take, string filter, string order, bool showActive, string roleId);

        Task<BudgetRoleDTO> Get(int id);

        Task<string> GetRoleId(EBudgetRole budgetRole);
        Task<Control<BudgetRoleDTO>> GetName(int start, int take, string filter, string order, bool showActive, EBudgetRole budgetRole);
    }
}
