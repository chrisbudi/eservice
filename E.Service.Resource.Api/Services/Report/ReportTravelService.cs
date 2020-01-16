using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Report
{
    public class ReportTravelService : ITravelRequestReport
    {


        private EservicesdbContext _db;
        public ReportTravelService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportTravel>> GetReport(int regionalId)
        {
            var carreport = await _db.TravelTransportationRequestDetails
                                 .Select(m => new ReportTravel()
                                 {
                                     Lokasi = m.TravelTransportatonIdRequest.TravelRequest.OfficeLocation.Name,
                                     Wilayah = m.TravelTransportatonIdRequest.TravelRequest.OfficeLocation.Region.Name,
                                     Budget = m.TotalBudget.ToString("N2"),
                                     Deskripsi = m.TravelTransportatonIdRequest.TravelRequest.Description,
                                     Hotel = m.TravelTransportatonIdRequest.TravelRequest.TravelHotelRequests.TravelHotel.Name,
                                     JumlahKamar = m.TravelTransportatonIdRequest.TravelRequest.TravelHotelRequests.RoomTotal,
                                     JumlahOrang = m.TravelTransportatonIdRequest.TravelRequest.TravelTransportationRequests.PersonTotal,
                                     KotaAsal = m.FromCityNavigation.Name,
                                     KotaTujuan = m.ToCityNavigation.Name,
                                     KotaHotel = m.TravelTransportatonIdRequest.TravelRequest.TravelHotelRequests.TravelCity.Name,
                                     No = 0,
                                     NoRequest = m.TravelTransportatonIdRequest.TravelRequest.NoRequest,
                                     Realisasi = ((m.TravelTransportatonIdRequest.TravelRequest.TravelRequestAccountability.TotalAmountHotel + m.TravelTransportatonIdRequest.TravelRequest.TravelRequestAccountability.TotalAmountTransportation) ?? 0).ToString("N2"),
                                     Perihal = m.TravelTransportatonIdRequest.TravelRequest.Title,
                                     PIC = (m.TravelTransportatonIdRequest.TravelRequest.TravelRequestAccountability != null ?
                                             m.TravelTransportatonIdRequest.TravelRequest.TravelRequestAccountability.Pic.Name :
                                             ""),
                                     TanggalBerangkat = m.DepartDateTime.HasValue ? m.DepartDateTime.Value.ToString("dd-MM-yyyy") : "",
                                     TanggalSelesaiEksekusi = "",
                                     WaktuBerangkat = m.DepartDateTime.HasValue ? m.DepartDateTime.Value.ToString("HH:mm") : "",
                                     UserPengaju = m.TravelTransportatonIdRequest.TravelRequest.Requester.Name,
                                     Id = m.Id,
                                 }).ToListAsync();
            return carreport;
        }

        public async Task<TravelRequestDetailDTO> GetReportDetail(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var repos = await (from main in _db.TravelRequest
                               join maind in _db.TravelTransportationRequestDetails on main.Id equals maind.TravelTransportatonIdRequestId
                               into acct
                               from tdetail in acct.DefaultIfEmpty()
                               join mainh in _db.TravelHotelRequests on main.Id equals mainh.TravelRequestId
                               into acch
                               from hdetail in acch.DefaultIfEmpty()
                               join maina in _db.TravelRequestAccountability on main.Id equals maina.TravelRequestId
                               into acca
                               from acc in acca.DefaultIfEmpty()
                               where main.Id == id
                               select new TravelRequestDetailDTO
                               {
                                   BiayaHotel = (hdetail.TotalPrice ?? 0).ToString("N2"),
                                   BiayaTransportasi = tdetail.TravelTransportatonIdRequest.
                                                        TravelTransportationRequestDetails.Sum(m => m.TotalBudget).ToString("N2"),
                                   Description = main.Description,
                                   TransactionNo = main.NoRequest,
                                   Id = main.Id,
                                   NamaPIC = acc.Pic.Name,
                                   RequesterName = main.Requester.Name,
                                   Title = main.Title,
                                   StatusTransaksi = main.Request.Currentstate.Name,
                                   TanggalPemesanan = (main.TransactionDate ?? defaultDate).ToString("dd-MM-yyyy"),
                                   TanggalPertanggungjawaban = acc.TransactionDate.ToString("dd-MM-yyyy"),
                                   CekIn = acch.First() != null ? acch.First().CheckinAt.ToString("dd-MM-yyyy HH:mm") : "",
                                   CekOut = acch.First() != null ? acch.First().CheckoutAt.ToString("dd-MM-yyyy HH:mm") : "",
                                   City = acch.First() != null ? acch.First().TravelCity.Name : "",
                                   Deskripsi = acch.First() != null ? acch.First().Note : "",
                                   JumlahKamar = acch.First() != null ? (acch.First().RoomTotal ?? 0) : 0,
                                   NamaHotel = acch.First() != null ? acch.First().TravelHotel.Name : "",
                               }).SingleAsync();
            return repos;
        }

        public async Task<List<ReportCarDetailApprovalDTO>> GetReportDetailApproval(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await(from p in _db.TravelRequest
                             join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                             where p.Id == id
                             select new ReportCarDetailApprovalDTO()
                             {
                                 Id = requestHistory.Id,
                                 Approval = requestHistory.HistoryType,
                                 NamaAprover = requestHistory.User.Name,
                                 StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                             }).ToListAsync();
            return data;
        }

        public async Task<List<TravelDetailFilesDTO>> GetReportDetailFiles(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await _db.TravelRequestAccountabilityFiles
                .Where(m => m.TravelRequestId == id)
                .Select(m => new TravelDetailFilesDTO()
                {
                    Id = m.TravelRequestAccountabilityFilesId,
                    MasterId = m.TravelRequestId,
                    FilePath = m.FilePath
                }).ToListAsync();

            return data;
        }

        public async Task<TravelHotelRequestDetailDTO> GetReportDetailHotel(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var repos = await _db.TravelHotelRequests
                .Where(m => m.TravelRequestId == id)
                .Select(m => new TravelHotelRequestDetailDTO
                {
                    CekIn = m.CheckinAt.ToString("dd-MM-yyyy HH:mm"),
                    CekOut = m.CheckoutAt.ToString("dd-MM-yyyy HH:checkout"),
                    City = m.TravelCity.Name,
                    Id = m.TravelRequestId,
                    JumlahKamar = (m.RoomTotal ?? 0).ToString("N0"),
                    NamaHotel = m.TravelHotel.Name
                }).SingleOrDefaultAsync();
            return repos;
        }

        public async Task<List<TravelInOutDetailDTO>> GetReportDetailInOut(int id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var repos = await _db.TravelTransportationRequestDetails
                .Where(m => m.TravelTransportatonIdRequestId == id)
                .Select(m => new TravelInOutDetailDTO()
                {
                    FromCity = m.FromCityNavigation.Name,
                    ToCity = m.ToCityNavigation.Name,
                    Id = m.Id,
                    TanggalJam = (m.ArrivalDateTime ?? defaultDate).ToString("dd-MM-yyyy HH:mm"),
                    TransportationType = m.TravelTransportationName.Name
                }).ToListAsync();


            return repos;
        }

        public async Task<List<ReportTravel>> GetReportSummary(int locationId, int regionalId,
            DateTime? startDate, DateTime? endDate, int departmentId)
        {
            var defaultDate = DateTime.Parse("1900-01-01");

            var repos = from p in _db.TravelRequest
                        join q in _db.TravelTransportationRequestDetails on p.Id equals q.TravelTransportatonIdRequestId
                               into transportrequestdetail
                        from q in transportrequestdetail.DefaultIfEmpty()
                        select new
                        {
                            travelRequest = p,
                            travelRequestDetail = q
                        };

            if (locationId != 0)
            {
                repos = repos.Where(m => m.travelRequest.OfficeLocationId == locationId);
            }

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.travelRequest.OfficeLocation.RegionId == regionalId);

            }

            if (startDate != null && startDate != defaultDate)
            {
                repos = repos.Where(m => m.travelRequest.TransactionDate > startDate);

            }

            if (endDate != null && endDate != defaultDate)
            {
                repos = repos.Where(m => m.travelRequest.TransactionDate < endDate);
            }

            if (departmentId != 0)
            {
                repos = repos.Where(m => m.travelRequest.DepartmentId == departmentId);
            }


            var reportData = await repos
                                     .Select(m => new ReportTravel()
                                     {
                                         Lokasi = m.travelRequest.OfficeLocation.Name,
                                         Wilayah = m.travelRequest.OfficeLocation.Region.Name,
                                         Budget = m.travelRequest.TotalBudget.ToString("N2"),
                                         Deskripsi = m.travelRequest.Description,
                                         Hotel = m.travelRequest.TravelHotelRequests.TravelHotel.Name,
                                         JumlahKamar = m.travelRequest.TravelHotelRequests.RoomTotal,
                                         JumlahOrang = m.travelRequest.TravelTransportationRequests.PersonTotal,
                                         KotaAsal = m.travelRequestDetail.FromCityNavigation.Name,
                                         KotaTujuan = m.travelRequestDetail.ToCityNavigation.Name,
                                         KotaHotel = m.travelRequest.TravelHotelRequests.TravelCity.Name,
                                         No = 0,
                                         NoRequest = m.travelRequest.NoRequest,
                                         Realisasi = ((m.travelRequest.TravelRequestAccountability.TotalAmountHotel + m.travelRequest.TravelRequestAccountability.TotalAmountTransportation) ?? 0).ToString("N2"),
                                         Perihal = m.travelRequest.Title,
                                         PIC = (m.travelRequest.TravelRequestAccountability != null ?
                                                 m.travelRequest.TravelRequestAccountability.Pic.Name :
                                                 ""),
                                         TanggalBerangkat = m.travelRequestDetail.DepartDateTime.HasValue ? m.travelRequestDetail.DepartDateTime.Value.ToString("dd-MM-yyyy") : "",
                                         TanggalSelesaiEksekusi = "",
                                         WaktuBerangkat = m.travelRequestDetail.DepartDateTime.HasValue ? m.travelRequestDetail.DepartDateTime.Value.ToString("HH:mm") : "",
                                         UserPengaju = m.travelRequest.Requester.Name,
                                         SatuanKerjaId = m.travelRequest.DepartmentId ?? 0,
                                         Id = m.travelRequest.Id,
                                     }).ToListAsync();
            return reportData;
        }
    }
}
