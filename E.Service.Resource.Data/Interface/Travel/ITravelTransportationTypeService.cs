using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Travel.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel
{
    public interface ITravelTransportationTypeService
    {
        Task<TravelTransportationType> Save(TravelTransportationType entity);

        Task<Control<TravelTransportationTypeDTO>> Get(int start, int take, string filter, string order, bool showActive);

        Task<TravelTransportationTypeDTO> Get(int id);
    }
}
