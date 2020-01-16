using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface IAssetBorrowReport
    {
        Task<List<ReportAssetBorrow>> GetReport(int regionalId);
        Task<List<ReportAssetBorrow>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId);

        Task<AssetBorrowRequestDetailDTO> GetReportAssetBorrowDetail(int Id);
        Task<List<AssetBorrowApprovalDetailDTO>> GetReportAssetBorrowApprovalDetail(int Id);
        
    }
}
