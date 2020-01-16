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
    public interface ITravelCitiesService
    {
        Task<TravelCities> Save(TravelCities entity);

        Task<Control<TravelCitiesDTO>> Get(int start, int take, string filter, string order, bool showActive);

        Task<TravelCitiesDTO> Get(int id);

    }
}
