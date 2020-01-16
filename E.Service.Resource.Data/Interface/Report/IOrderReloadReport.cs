using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.OrderReloadRequestDetailDTO.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface IOrderReloadReport
    {
        Task<List<ReportOrderReload>> GetReport(int regionalId);
        Task<List<ReportOrderReload>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId, string jenisOrderId);
        Task<ReportOrderReloadRequestDetailDTO> GetReportDetail(int Id);
        Task<List<OrderReloadItemDetailDTO>> GetReportDetailItem(int Id);
        Task<List<OrderReloadApprovalDetailDTO>> GetReportDetailApproval(int Id);
        Task<List<OrderReloadFilesDetailDTO>> GetReportDetailFiles(int Id);
    }
}
