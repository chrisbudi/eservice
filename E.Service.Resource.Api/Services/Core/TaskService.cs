using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Approval;
using E.Service.Resource.Data.Interface.Approval.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Core
{
    public class TaskService : ITaskService
    {
        private EservicesdbContext db;
        IUserService _userService;
     
        public TaskService(EservicesdbContext db, IUserService userService)
        {
            this.db = db;
            this._userService = userService;
        }

        public async Task<Control<UserTaskRequestDTO>> GetTaskList(int start, int take, string filter, string order, string userId)
        {

            var userData = await _userService.GetUser(userId);
            var data = db.RequestFlow.Where(m => m.Userid == userData.Id.ToString() && 
            m.Currentstate.Statetype.Name.ToLower() != "complete" &&
            m.Currentstate.Statetype.Name.ToLower() != "cancelled");



            int totalData = data.Count();
            int totalFilterData = totalData;


            if (!string.IsNullOrEmpty(order))
                data = data.OrderBy(order);

            data = data.Skip(start * take).Take(take);

            return new Control<UserTaskRequestDTO>()
            {
                ListClass = await data.Select(m => new UserTaskRequestDTO()
                {
                    ProcessName = m.Process.Nama,
                    RequestDate = m.Daterequest.Value,
                    RequestNote = m.Note,
                    RequestId = m.Requestid,
                    RequestTitle = m.Title,
                    StateName = m.Currentstate.Name,
                    Url = m.Process.Url.Replace("?", m.Requestid.ToString())
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }
    }
}
