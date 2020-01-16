using E.Service.Resource.Data.Interface.Dashboard;
using E.Service.Resource.Data.Interface.Dashboard.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Dashboard
{
    public class HBBDashboardService : IHBBDashboardService
    {

        private EservicesdbContext db;
        public HBBDashboardService(EservicesdbContext db)
        {
            this.db = db;
        }



        public Task<IList<MeetingRequestDashboardDTO>> GetListHBB(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
