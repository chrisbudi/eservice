using E.Service.Resource.Data.Interface.Dashboard.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Dashboard
{
    public interface ICarDashboardService
    {
        Task<IList<CarRequestDashboardDTO>> GetListCar(DateTime dateTime);
    }
}
