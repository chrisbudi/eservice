using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IUserTargetService
    {
        Task<List<TargetDTO>> GetAvaliableTarget();
        Task<List<TargetDTO>> GetselectedTarget(string userId);
        Task<UserTargetDTO> UserTarget(UserTargetDTO userGroup);
    }
}
