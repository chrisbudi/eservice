using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface IAssetReport
    {
        Task<List<ReportAsset>> GetReport(int regionalId);
        Task<List<ReportAsset>> GetReportSummary(int regionalId, int locationId, int tahunPembelian, int departmentId, int statusBarangId, int merkId, int typeId);

        Task<AssetRequestDetailDTO> GetReportDetail(int Id);
        Task<List<AssetRequestApprovalDetailDTO>> GetDetailApproval(int Id);
        Task<List<AssetChangeRequestDetailDTO>> GetDetailHistory(int Id);

    }
}
