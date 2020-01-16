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
    public interface IAssetTypeService
    {
        Task<AssetTypes> Save(AssetTypes entity);

        Task<Control<AssetTypesDTO>> Get(int start, int take, string filter, string order, bool showActive);

        Task<AssetTypesDTO> Get(int id);
    }
}
