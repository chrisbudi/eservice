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
    public interface IDriverService
    {
        Task<CarDrivers> Save(CarDrivers entity);

        Task<Control<CarDriverDTO>> Get(int start, int take, string filter, 
            string order, bool showActive, bool active, int regionId);

        Task<CarDriverDTO> Get(int id);
        Task<CarDriverDTO> GetUserID(int id);
    }
}
