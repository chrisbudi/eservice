using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Travel.DTO.Form;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel
{
    public interface ITravelRequestAccountabilityService
    {

        Task<TravelRequestAccountability> Save(TravelRequestAccountability request, bool submit);

        Task<Control<TravelRequestConfirmationDTO>> GetList(int start, int take, string filter, string order);

        Task<TravelRequestConfirmationDTO> Get(int id);

        Task<TravelRequestConfirmationDTO> GetByRequestId(int requestId);

    }
}
