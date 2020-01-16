using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car
{
    public interface ICarRequestService
    {
        Task<CarRequests> Save(CarRequests entity, bool submit);
        Task<Control<CarRequestDTO>> Get(int start, int take, string filter, string order, bool showActive);
        Task<CarRequestDTO> Get(int id);
        Task<CarBudgetDetail> SaveDriverBudget(CarBudgetDetail car, EDriverBudgetStatusType add);
        Task<List<ReportCarRequest>> GetReport(int regionalId);
        Task<List<ReportCarRequest>> GetReportSummary(int regionalId);
        Task<CarRequestBudget> SaveRequestBudget(CarRequestBudget car, bool submit);
        Task<Control<CarDetilBudgetDTO>> GetDriverBudgetList(int id, int start, int take, string filter, string order, bool showActive);

        Task<CarRequestBudget> PostEnd(int id);
        Task<CarRequestBudget> PostStart(int id);
        Task<CarRequestDTO> GetByRequestId(int id);
        Task<CarRequestBudgetDTO> GetByConfirmRequestId(int id);
        Task<Control<CarRequestBudgetDTO>> GetBudgets(int start, int take, string filter, string order, bool showActive);

        Task<CarRequestBudgetDTO> GetBudgetId(int carRequestId);
        Task<Control<CarRequestDTO>> GetDriverJobList(int start, int take, string filter, string order, int driverId, bool showComplete, bool check);

        Task UpdateDriverId(int requestId, int driverId);
        Task<CarRequestBudgetDTO> GetConfirmId(int id);
        Task<CarCoordinateInsertDTO> PostCoordinate(CarCoordinateInsertDTO coordinate);


        decimal GetCurrentDriverBudget(int driverId);
        Task UpdateEntity(int id, int anggaranstatusId);


        Task<ReportCarDetailLocationDTO> GetReportDetailLocation(int id);
        Task<List<ReportCarUsageDetailDTO>> GetReportDetailUsage(int id);
        Task<List<ReportCarDetailApprovalDTO>> GetReportDetailApproval(int id);

        Task<ReportCarDetailDateDTO> GetReportDetailDate(int id);

    }
}
