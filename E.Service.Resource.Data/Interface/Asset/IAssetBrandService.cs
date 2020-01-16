using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Models;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset
{
    public interface IAssetBrandService
    {
        //brand
        Task<AssetBrands> SaveBrand(AssetBrands entity);

        Task<Control<AssetBrandDTO>> GetBrandData(int start, int take, string filter, string order, bool showActive);

        Task<AssetBrandDTO> GetBrand(int id);


        //brand series
        Task<AssetBrandSeries> SaveBrandSeries(AssetBrandSeries entity);

        Task<Control<AssetBrandSeriesDTO>> GetBrandSeries(int start, int take, string filter, string order, bool showActive);

        Task<AssetBrandSeriesDTO> GetBrandSeries(int id);
    }
}
