using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Car.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car
{
    public interface ICarCoordinateService
    {
        Task<IList<CarCoordinateViewDTO>> GetAll(int carRequestId);
        Task<IList<CarCoordinateViewDTO>> Get(int carrequestId, int statusID);
        Task<string> GetLastStatus(int carRequestId);
    }
}
