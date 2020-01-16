using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.OrderReloadRequestDetailDTO.cs;
using E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public interface IMeetingRequestReportService
    {
        Task<List<ReportMeetingRequest>> GetReportMeetingRequest(int locationId, int RegionId, string Status, DateTime? startDate, DateTime? endDate, int departmentId);
        Task<ReportMeetingRequestDetailDTO> GetReportMeetingDetail(int Id);
        Task<List<ReportMeetingRequestTimeDetailDTO>> GetReportMeetingTimeDetail(int Id);
        Task<List<ReportMeetingRequestJamuanDetailDTO>> GetReportMeetingJamuanDetail(int Id);
        Task<List<ReportMeetingRequestFilesDetailDTO>> GetReportMeetingFilesDetail(int Id);



    }
}
