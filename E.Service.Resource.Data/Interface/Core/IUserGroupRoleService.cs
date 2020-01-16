using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IUserGroupRoleService
    {
        //User group

        Task<Control<UserDTO>> UserList(int start, int take, string filter, string order);
        Task<List<GroupDTO>> GetAvaliableGroup();
        Task<List<GroupDTO>> GetselectedGroup(string userId);

        Task<UserGroupDTO> UserGroup(UserGroupDTO userGroup);


        //group role
        Task<Control<GroupDTO>> GroupList(int start, int take, string filter, string order);
        Task<List<RoleDTO>> GetAvaliableRole();
        Task<List<RoleDTO>> GetselectedRole(string groupId);
        Task<GroupRoleDTO> GroupRole(GroupRoleDTO groupRole);
    }
}
