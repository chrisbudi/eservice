using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core
{
    public interface IGroupService
    {
        Task<AspNetGroups> Save(AspNetGroups entity);

        Task<Control<GroupDTO>> GetList(int start, int take, string filter, string order);

        Task<GroupDTO> Get(string id);
    }
}
