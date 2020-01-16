using E.Service.Resource.Data.Interface.Repair.DTO;
using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.RepairDetailDTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Report
{
    public class ReportRepairService : IRepairReport
    {
        private EservicesdbContext _db;

        public ReportRepairService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportRepair>> GetReport(int regionalId)
        {
            var carreport = await _db.RepairItemRequests
                    .Select(m => new ReportRepair()
                    {
                        Wilayah = m.OfficeLocation.Region.Name,
                        Deskripsi = m.Description,
                        JenisBarang = m.RepairItem.Jenis.JenisNama,
                        NoRequest = m.RequestNo,
                        Id = m.Id,
                        No = 0,
                        Lokasi = m.OfficeLocation.Name,
                        JenisRepair = m.RepairItem.ItItem ? "IT" : "NON IT",
                        Perihal = m.Title,
                        PICPerbaikan = m.RepairItemRequestAccountablity != null ? m.RepairItemRequestAccountablity.Pic.Name : null,
                        RepairItem = m.RepairItem.Name,
                        SatuanKerjaPengaju = "",
                        StatusRequest = m.Requester.DepartmentId.ToString(),
                        TanggalPermintaan = m.RequestDate.Value.ToString("dd-MM-yyyy"),
                        TanggalSelesaiPermintaan = m.RepairItemRequestAccountablity != null ? m.RepairItemRequestAccountablity.CreateDate.ToString("dd-MM-yyyy") : null,
                        UserPengaju = m.Requester.Name
                    }).ToListAsync();
            return carreport;
        }

        public async Task<List<RepairItemApprovalDetailDTO>> GetReportRepairItemApprovalDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.RepairItemRequests
                              join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                              where p.Id == Id
                              select new RepairItemApprovalDetailDTO()
                              {
                                  Id = requestHistory.Id,
                                  Approval = requestHistory.HistoryType,
                                  NamaAprover = requestHistory.User.Name,
                                  StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<RepairItemRequestDetailDTO> GetReportRepairItemDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");

            var data = await (from p in _db.RepairItemRequests
                              join maina in _db.RepairItemRequestAccountablity on p.Id equals maina.RepairItemRequestId
                               into acca
                              from acc in acca.DefaultIfEmpty()
                              where p.Id == Id
                              select new RepairItemRequestDetailDTO()
                              {
                                  DepartmentId = p.DepartmentId ?? 0,
                                  Id = p.Id,
                                  NamaPic = acc.Pic.Name,
                                  RequesterName = p.Requester.Name,
                                  StatusTransaksi = p.Request.Currentstate.Name,
                                  TanggalPemesanan = (p.RequestDate ?? defaultDate).ToString("dd-MM-yyyy"),
                                  Title = p.Title,
                                  TransactionNo = p.RequestNo,

                                  Deskripsi = p.Description,
                                  ItemPerbaikan = p.RepairItem.Name,
                                  JenisPerbaikan = p.RepairItem.RepairType,
                                  Location = p.OfficeLocation.Name,
                                  Regional = p.OfficeLocation.Region.Name
                              }).SingleAsync();
            return data;
        }

        public async Task<List<RepairItemRequestFilesDetailDTO>> GetReportRepairItemFilesDetail(int Id)
        {
            var data = await (from p in _db.RepairItemRequestAccountablitiyImage
                              where p.RepairItemRequestId == Id
                              select new RepairItemRequestFilesDetailDTO()
                              {
                                  Id = p.Id,
                                  FilePath = p.FilePath,
                                  MasterId = p.RepairItemRequestId ?? 0
                              }).ToListAsync();

            return data;
        }

        public async Task<List<RepairItemApprovalDetailDTO>> GetReportRepairItemRequestApprovalDetail(int Id)
        {

            var data = await (from p in _db.RepairItemRequests
                              join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                              where p.Id == Id
                              select new RepairItemApprovalDetailDTO()
                              {
                                  Id = requestHistory.Id,
                                  Approval = requestHistory.HistoryType,
                                  NamaAprover = requestHistory.User.Name,
                                  StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<List<ReportRepair>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId, string jenisRepairId)
        {
            var defaultDate = DateTime.Parse("1900-01-01");

            var repos = _db.RepairItemRequests.AsQueryable();


            if (locationId != 0)
            {
                repos = repos.Where(m => m.OfficeLocationId == locationId);
            }

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.OfficeLocation.RegionId == regionalId);
            }
            if (departmentId != 0)
            {
                repos = repos.Where(m => m.DepartmentId == departmentId);
            }


            if (startDate != null && startDate != defaultDate)
            {
                repos = repos.Where(m => m.RequestDate.Value > startDate);
            }


            if (endDate != null && endDate != defaultDate)
            {
                repos = repos.Where(m => m.RequestDate.Value < endDate);
            }

            var carreport = await repos
                    .Select(m => new ReportRepair()
                    {
                        Wilayah = m.OfficeLocation.Region.Name,
                        Deskripsi = m.Description,
                        JenisBarang = m.RepairItem.Jenis.JenisNama,
                        NoRequest = m.RequestNo,
                        Id = m.Id,
                        No = 0,
                        Lokasi = m.OfficeLocation.Name,
                        JenisRepair = m.RepairItem.ItItem ? "IT" : "NON IT",
                        Perihal = m.Title,
                        PICPerbaikan = m.RepairItemRequestAccountablity != null ? m.RepairItemRequestAccountablity.Pic.Name : null,
                        RepairItem = m.RepairItem.Name,
                        SatuanKerjaPengaju = "",
                        StatusRequest = m.Requester.DepartmentId.ToString(),
                        TanggalPermintaan = m.RequestDate.Value.ToString("dd-MM-yyyy"),
                        TanggalSelesaiPermintaan = m.RepairItemRequestAccountablity != null ? m.RepairItemRequestAccountablity.CreateDate.ToString("dd-MM-yyyy") : null,
                        UserPengaju = m.Requester.Name,
                        DepartmentId = m.DepartmentId ?? 0
                    }).ToListAsync();
            return carreport;
        }
    }
}
