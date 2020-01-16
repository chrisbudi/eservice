using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Report
{
    public class ReportBorrowService : IAssetBorrowReport
    {

        private EservicesdbContext _db;
        public ReportBorrowService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportAssetBorrow>> GetReport(int regionalId)
        {
            var carreport = await _db.AssetBorrow
                .Include(m => m.Asset)
                    .ThenInclude(m => m.AssetRequests)
                        .ThenInclude(m => m.OfficeRoom)
                            .ThenInclude(m => m.OfficeLocation)
                                .ThenInclude(m => m.Region)
                .Select(m => new ReportAssetBorrow()
                {
                    Id = m.Id,
                    Deskripsi = m.Description,
                    No = 0,
                    StatusRequest = m.Requester.DepartmentId.ToString(),
                    UserPengaju = m.Requester.Name,
                    RoomId = m.RoomId.Value,
                    RoomName = m.Room.Name,
                    NamaItem = m.Asset.Name,
                    NoRequest = m.RequestBorrowNo,
                    Pic = m.Requester.Name,
                    TanggalPermintaan = m.RequestDate.ToString("dd-MM-yyyy"),
                    TanggalKembali = m.ReturnDate.ToString("dd-MM-yyyy"),
                    TanggalPinjam = m.BorrowDate.ToString("dd-MM-yyyy"),
                    Lokasi = m.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).OfficeRoomId.HasValue ?
                    m.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).OfficeRoom.OfficeLocationId.ToString() : "",
                    Wilayah = m.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).OfficeRoomId.HasValue ?
                    "" : "",
                }).ToListAsync();
            return carreport;
        }

        public async Task<List<AssetBorrowApprovalDetailDTO>> GetReportAssetBorrowApprovalDetail(int Id)
        {

            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await(from p in _db.AssetBorrow
                             join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                             where p.Id == Id
                             select new AssetBorrowApprovalDetailDTO()
                             {
                                 Id = requestHistory.Id,
                                 Approval = requestHistory.HistoryType,
                                 NamaAprover = requestHistory.User.Name,
                                 StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                             }).ToListAsync();
            return data;
        }

        public async Task<AssetBorrowRequestDetailDTO> GetReportAssetBorrowDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.AssetBorrow
                              join q in _db.AssetRequests on p.Id equals q.AssetId
                                   
                              where p.Id == Id && q.Depreciated == false
                              select new AssetBorrowRequestDetailDTO()
                              {
                                  Id = p.Id,
                                  RequesterName = p.Requester.Name,
                                  DepartmentId = q.DepartementId ?? 0,
                                  TransactionNo = p.RequestBorrowNo,
                                  NamaAsset = p.Asset.Name,
                                  Ruangan = q.OfficeRoom.Name,
                                  TanggalKembali = p.ReturnDate.ToString("dd-MM-yyyy"),
                                  TanggalPinjam = p.BorrowDate.ToString("dd-MM-yyyy"),
                                  TanggalPemesanan= p.RequestDate.ToString("dd-MM-yyyy"),
                                  Title = p.Title,
                                  Description = p.Description
                             }).SingleAsync();

            return data;
        }


        public async Task<List<ReportAssetBorrow>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId)
        {


            var defaultDate = DateTime.Parse("1900-01-01");

            var repos = from p in _db.AssetBorrow
                        join q in _db.AssetRequests on p.AssetId equals q.AssetId
                        join r in _db.Assets on p.AssetId equals r.Id
                        select new { borrow = p, request = q, asset = r };


            if (locationId != 0)
            {
                repos = repos.Where(m => m.request.OfficeRoom.OfficeLocationId == locationId);
            }

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.request.OfficeRoom.OfficeLocation.RegionId == regionalId);
            }


            if (startDate != null && startDate != defaultDate)
            {
                repos = repos.Where(m => m.borrow.BorrowDate > startDate);
            }


            if (endDate != null && endDate != defaultDate)
            {
                repos = repos.Where(m => m.borrow.BorrowDate < endDate);
            }


            var carreport = await repos
                .Select(m => new ReportAssetBorrow()
                {
                    Id = m.borrow.Id,
                    Deskripsi = m.borrow.Description,
                    Title = m.borrow.Title,
                    No = 0,
                    RoomId = m.borrow.RoomId.Value,
                    RoomName = m.borrow.Room.Name,
                    OrganizationId = m.borrow.OrganizationId ?? 0,
                    StatusRequest = m.borrow.Requester.DepartmentId.ToString(),
                    UserPengaju = m.borrow.Requester.Name,
                    NamaItem = m.borrow.Asset.Name,
                    NoRequest = m.borrow.RequestBorrowNo,
                    Pic = m.borrow.Requester.Name,
                    TanggalPermintaan = m.borrow.RequestDate.ToString("dd-MM-yyyy"),
                    TanggalKembali = m.borrow.ReturnDate.ToString("dd-MM-yyyy"),
                    TanggalPinjam = m.borrow.BorrowDate.ToString("dd-MM-yyyy"),
                    DepartmentAssetId = m.borrow.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).DepartementId.HasValue ?
                    m.borrow.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).DepartementId.Value : 0,
                    LokasiId = m.borrow.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).OfficeRoomId.HasValue ?
                    m.borrow.Asset.AssetRequests.FirstOrDefault(d => d.Depreciated == false).OfficeRoom.OfficeLocationId : 0,

                }).ToListAsync();

            foreach (var report in carreport)
            {
                if (report.LokasiId != 0)
                {
                    var lokasi = _db.OfficeLocations.Include(m => m.Region).Single(m => m.Id == report.LokasiId);

                    report.Lokasi = lokasi.Name;
                    report.WilayahId = lokasi.RegionId ?? 0;
                    report.Wilayah = lokasi.Region.Name;

                }
            }

            return carreport;

        }
    }
}
