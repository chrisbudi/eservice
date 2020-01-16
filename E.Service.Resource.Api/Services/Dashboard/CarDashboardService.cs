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
    public class CarDashboardService : ICarDashboardService
    {
        private EservicesdbContext db;

        public CarDashboardService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<IList<CarRequestDashboardDTO>> GetListCar(DateTime dateTime)
        {
            return await db.CarRequests.Where(m => m.StartTime.Value.Date == dateTime.Date && m.EndTime.Value.Date == dateTime.Date)
                .Select(m => new CarRequestDashboardDTO()
            {
                Id = m.Id,
                Group = m.CarPoolId,
                Title = m.Requester.Name,
                EndDateTime = m.EndTime.Value,
                StartDateTime = m.StartTime.Value,
                GroupName = m.CarPool.Name
            }).ToListAsync();
        }
    }
}
