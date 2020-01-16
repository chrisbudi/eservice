using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IUserService
    {
        Task<Control<Users>> GetUsers(int start, int take, string filter, string order, bool active);

        Task<int> GetDepartmentId(int userId);
        Task<UserDTO> GetUser(string userId);
        Task<UserDTO> GetUserById(int userId);
        Task<List<UserDTO>> GetRequestActionTargetUser(int requestActionId);
        Task<List<UserDTO>> GetUserInProcessId(int processId);
    }
}
