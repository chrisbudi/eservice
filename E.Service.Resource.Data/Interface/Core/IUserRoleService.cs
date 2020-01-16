using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IUserRoleService
    {
        Task<UserRoleDTO> Save(UserRoleDTO entity);

        Task<Control<UserRoleDTO>> Get(int start, int take, string filter, string order, bool showActive);

        Task<UserRoleDTO> Get(string id);


        Task<Control<Role>> GetRole(int start, int take, string filter, string order, string type);
    }
}
