using E.Service.Resource.Data.Interface.Dashboard;
using E.Service.Resource.Data.Interface.Dashboard.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Dashboard
{
    public class MeetingRequestDashboardService : IMeetingRequestDashboardService
    {
        private EservicesdbContext db;
        public MeetingRequestDashboardService(EservicesdbContext db)
        {
            this.db = db;
        }


        public async Task<IList<MeetingRequestDashboardDTO>> GetListMeetingRequest(DateTime dateTime)
        {

            return await db.MeetingRequestTime.Where(m => m.StartDate.Value.Date == dateTime.Date && m.EndDate.Value.Date == dateTime.Date).Select(m => new MeetingRequestDashboardDTO()
            {
                Id = m.Id,
                Group = m.MeetingRequest.MeetingRoomId,
                Title = m.MeetingRequest.MeetingTitle,
                EndDateTime = m.EndDate.Value,
                StartDateTime = m.StartDate.Value,
                GroupName = m.MeetingRequest.MeetingRoom.Name
            }).ToListAsync();
        }
    }
}
