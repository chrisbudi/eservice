using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car
{
    public interface ICarCoordinateStatusService
    {
        Task<CarRequestCoordinateStatus> Save(CarRequestCoordinateStatus entity);

        Task<Control<CarCoordinateStatusDTO>> Get(int start, int take, string filter,
            string order, bool showActive);

        Task<CarCoordinateStatusDTO> Get(int id);
    }
}
