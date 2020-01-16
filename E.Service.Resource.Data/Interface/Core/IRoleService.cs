using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IRoleService
    {

        Task<Control<RoleDTO>> Get(int start, int take, string filter, string order, bool showActive);

        Task<RoleDTO> Get(string id);
        Task<List<RoleDTO>> GetAvaliableRole();
        Task<List<RoleDTO>> GetselectedRole(string userid);
        Task<UserRoleDTO> UserRole(UserRoleDTO userRole);
        Task<Users> User(UserDTO user);
        Task<UserDTO> Get(int id);
    }
}
