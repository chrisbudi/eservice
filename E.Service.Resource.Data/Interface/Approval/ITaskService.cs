using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Approval.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Approval
{
    public interface ITaskService
    {

        Task<Control<UserTaskRequestDTO>> GetTaskList(int start, int take, string filter, string order, string userId);

    }
}
