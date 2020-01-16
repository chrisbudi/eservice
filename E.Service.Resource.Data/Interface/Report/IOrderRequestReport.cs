using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.OrderReloadRequestDetailDTO.cs;
using E.Service.Resource.Data.Interface.Report.DTO.OrderRequestDetailDTO.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface IOrderRequestReport
    {
        Task<List<ReportOrderRequest>> GetReport(int regionalId);
        Task<List<ReportOrderRequest>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId, string jenisOrderId);
        Task<ReportOrderRequestDetailDTO> GetReportDetail(int Id);
        Task<List<OrderRequestItemDetailDTO>> GetReportDetailItem(int Id);
        Task<List<OrderRequestApprovalDetailDTO>> GetReportDetailApproval(int Id);
        Task<List<OrderRequestFilesDetailDTO>> GetReportDetailFiles(int Id);


    }
}
