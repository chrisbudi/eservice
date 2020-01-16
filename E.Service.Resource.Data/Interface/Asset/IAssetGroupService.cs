using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset
{
    public interface IAssetGroupService
    {
        //Main Group
        Task<AssetMainGroupTypes> SaveGroup(AssetMainGroupTypes entity);

        Task<Control<AssetGroupDTO>> GetGroupData(int start, int take, string filter, string order, bool showActive);

        Task<AssetGroupDTO> GetGroup(int id);


        //Sub Group
        Task<AssetSubGroupTypes> SaveSubGroup(AssetSubGroupTypes entity);

        Task<Control<AssetSubGroupDTO>> GetSubGroup(int start, int take, string filter, string order, bool showActive);

        Task<AssetSubGroupDTO> GetSubGroup(int id);
    }
}
