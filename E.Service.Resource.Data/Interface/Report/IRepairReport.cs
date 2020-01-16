using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.RepairDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface IRepairReport
    {
        Task<List<ReportRepair>> GetReport(int regionalId);
        Task<List<ReportRepair>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId, string jenisRepairId);

        Task<RepairItemRequestDetailDTO> GetReportRepairItemDetail(int Id);
        Task<List<RepairItemRequestFilesDetailDTO>> GetReportRepairItemFilesDetail(int Id);
        Task<List<RepairItemApprovalDetailDTO>> GetReportRepairItemRequestApprovalDetail(int Id);




    }
}
