using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset
{
    public interface IAssetService
    {
        //brand
        Task<AssetInsertDTO> Save(AssetInsertDTO entity, bool submit, EAssetRequestType requestType);

        Task<Control<AssetDTO>> Get(int start, int take, string filter, string order,
            bool showActive, bool showComplete = false, EAssetTypeService assetTypeService = EAssetTypeService.Current, bool borrow = false);

        Task<AssetDTO> Get(int id, EAssetTypeService assetTypeService = EAssetTypeService.Current);

        Task<Control<AssetTransactionDTO>> GetHistory(int id, int start, int take, string filter, string order);
        Task<AssetTransactionDTO> GetHistoryId(int histId);

        Task<AssetRequests> SaveAssetRequest(AssetRequests brands, bool submit, EAssetType assetType);
        Task<AssetDTO> GetBarcode(string barcode, EAssetTypeService current);
        Task<List<ReportAsset>> GetReportAsset(int locationId, int regionalId, string v, DateTime? startDate, DateTime? endDate, int departmentId, int merk, int type);
        Task<AssetDTO> GetByRequestId(int id);
        Task UpdateAssetRequestToDepreciated(int requestId);
        Task UpdateAssetRequestLastCancelledId(int requestId);
    }
}
