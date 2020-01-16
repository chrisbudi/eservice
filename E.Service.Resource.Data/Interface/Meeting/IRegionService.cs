using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IRegionService
    {
        Task<OfficeLocationRegions> Save(OfficeLocationRegions entity);

        Task<Control<RegionDTO>> Get(int start, int take, string filter, string order,bool showActive);

        Task<OfficeLocationRegions> Get(int id);
    }
}
