using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface ITravelRequestReport
    {
        Task<List<ReportTravel>> GetReport(int regionalId);
        Task<List<ReportTravel>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId);
        Task<TravelRequestDetailDTO> GetReportDetail(int id);
        Task<List<TravelInOutDetailDTO>> GetReportDetailInOut(int id);
        Task<TravelHotelRequestDetailDTO> GetReportDetailHotel(int id);
        Task<List<ReportCarDetailApprovalDTO>> GetReportDetailApproval(int id);
        Task<List<TravelDetailFilesDTO>> GetReportDetailFiles(int id);
    }
}
