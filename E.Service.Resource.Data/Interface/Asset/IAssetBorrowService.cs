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
    public interface IAssetBorrowService
    {
        Task<AssetBorrow> Save(AssetBorrow entity, bool submit);

        Task<Control<AssetBorrowDTO>> Get(int start, int take, string filter, string order);

        Task<AssetBorrowDTO> Get(int id);
        Task<AssetBorrowDTO> GetByRequestId(int id);

        Task<Control<AssetBorrowDTO>> GetAssetId(int start, int take, string filter, string order, int assetId);
    }
}
