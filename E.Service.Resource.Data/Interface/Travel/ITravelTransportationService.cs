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
    public interface ITravelTransportationService
    {
        Task<TravelTransportationName> Save(TravelTransportationName entity);

        Task<Control<TravelTransportationDTO>> Get(int start, int take, string filter, string order, bool active);

        Task<TravelTransportationDTO> Get(int id);

    }
}
