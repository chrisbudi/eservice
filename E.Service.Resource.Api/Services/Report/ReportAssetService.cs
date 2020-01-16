using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Report
{
    public class ReportAssetService : IAssetReport
    {

        private EservicesdbContext _db;
        public ReportAssetService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<List<AssetRequestApprovalDetailDTO>> GetDetailApproval(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.AssetRequests
                              join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                              where p.Id == Id
                              select new AssetRequestApprovalDetailDTO()
                              {
                                  Id = requestHistory.Id,
                                  Approval = requestHistory.HistoryType,
                                  NamaAprover = requestHistory.User.Name,
                                  StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<List<AssetChangeRequestDetailDTO>> GetDetailHistory(int Id)
        {

            var carreport = await (from m in _db.Assets
                                   join res in _db.AssetRequests on m.Id equals res.AssetId
                                   into acca
                                   from acc in acca.DefaultIfEmpty()
                                   where m.Id == Id
                                   select new AssetChangeRequestDetailDTO()
                                   {
                                       Deskripsi = m.Description,
                                       HargaPembelian = m.PriceAcquired.ToString("dd-MM-yyyy"),
                                       Id = m.Id,
                                       JenisKondisiAsset = acc.AssetCondition.JenisNama,
                                       MainGroup = m.Group.MainGroup.Name,
                                       SubGroup = m.Group.Name,
                                       MerkBarang = m.BrandSeries.Name,
                                       NoSeri = m.SerialNumber,
                                       NamaAsset = m.Name,
                                       Regional = acc.Regional.Name,
                                       Ruangan = acc.OfficeRoom.Name,
                                       TahunPembelian = m.YearAcquired.ToString(),
                                       TipeBarang = acc.Type.Name,
                                       VendorId = m.VendorId ?? 0,
                                       DepartmentId = acc.DepartementId ?? 0,
                                       Status = acc.Status
                                   }).ToListAsync();


            return carreport;
        }

        public async Task<List<ReportAsset>> GetReport(int regionalId)
        {
            var carreport = await _db.AssetRequests
                        .Where(m => m.Depreciated == false)
                        .Select(m => new ReportAsset()
                        {
                            Lokasi = m.OfficeRoom.OfficeLocation.Name,
                            Wilayah = m.OfficeRoom.OfficeLocation.Region.Name,
                            DepartmentId = m.DepartementId.Value,
                            Group = m.Asset.Group.Name,
                            HargaBeli = m.Asset.PriceAcquired,
                            HargaSaatIni = 0,
                            Id = m.Id,
                            Jenis = m.Type.Name,
                            Merk = m.Asset.BrandSeries.Brand.Name,
                            NamaHBB = m.Asset.Name,
                            NoHBB = m.Asset.AssetNumber,
                            SatuanBarang = m.Asset.SerialNumber,
                            SubGroup = m.Asset.Group.Name,
                            TanggalPembelian = m.Asset.YearAcquired.ToString(),
                            Type = m.Type.Name
                        }).ToListAsync();
            return carreport;
        }

        public async Task<AssetRequestDetailDTO> GetReportDetail(int Id)
        {
            var carreport = await (from m in _db.Assets
                                   join res in _db.AssetRequests on m.Id equals res.AssetId
                                   into acca
                                   from acc in acca.DefaultIfEmpty()
                                   where m.Id == Id && acc.Depreciated == false
                                   select new AssetRequestDetailDTO()
                                   {
                                       Deskripsi = m.Description,
                                       HargaPembelian = m.PriceAcquired.ToString("dd-MM-yyyy"),
                                       Id = m.Id,
                                       JenisKondisiAsset = acc.AssetCondition.JenisNama,
                                       MainGroup = m.Group.MainGroup.Name,
                                       SubGroup = m.Group.Name,
                                       MerkBarang = m.BrandSeries.Name,
                                       NoSeri = m.SerialNumber,
                                       NamaAsset = m.Name,
                                       NamaProsess = acc.Status,
                                       Regional = acc.Regional.Name,
                                       RequesterName = acc.Requester.Name,
                                       Ruangan = acc.OfficeRoom.Name,
                                       StatusTransaksi = acc.Request.Currentstate.Name,
                                       TahunPembelian = m.YearAcquired.ToString(),
                                       TanggalPemesanan = acc.RequestDate.ToString("dd-MM-yyyy"),
                                       TipeBarang = acc.Type.Name,
                                       Title = acc.Title,
                                       TransactionNo = acc.RequestNo,
                                       VendorId = m.VendorId ?? 0,
                                       DepartmentId = acc.DepartementId ?? 0
                                   }).SingleAsync();





            return carreport;
        }
        public async Task<List<ReportAsset>> GetReportSummary(int regionalId, int locationId,
        int tahunPembelian, int departmentId, int statusBarangId, int merkId, int typeId)
        {

            var repos = _db.AssetRequests.AsQueryable();


            if (regionalId != 0)
            {
                repos = repos.Where(m => m.RegionalId == regionalId);
            }


            if (locationId != 0)
            {
                repos = repos.Where(m => m.OfficeRoom.OfficeLocationId == locationId);
            }


            if (tahunPembelian != 0)
            {
                repos = repos.Where(m => m.Asset.YearAcquired == tahunPembelian);
            }


            if (departmentId != 0)
            {
                repos = repos.Where(m => m.DepartementId == departmentId);
            }

            if (merkId != 0)
            {
                repos = repos.Where(m => m.Asset.BrandSeriesId == merkId);
            }


            if (typeId != 0)
            {
                repos = repos.Where(m => m.TypeId == typeId);
            }



            var reportData = await repos
                        .Select(m => new ReportAsset()
                        {
                            Lokasi = m.OfficeRoom.OfficeLocation.Name,
                            Wilayah = m.OfficeRoom.OfficeLocation.Region.Name,
                            DepartmentId = m.DepartementId.Value,
                            StatusTransaksi = m.Request.Currentstate.Name,
                            Group = m.Asset.Group.Name,
                            HargaBeli = m.Asset.PriceAcquired,
                            HargaSaatIni = 0,
                            Id = m.Id,
                            NoAssetRequest = m.RequestNo,
                            Jenis = m.Type.Name,
                            Merk = m.Asset.BrandSeries.Brand.Name,
                            NamaHBB = m.Asset.Name,
                            NoHBB = m.Asset.AssetNumber,
                            SatuanBarang = m.Asset.SerialNumber,
                            SubGroup = m.Asset.Group.Name,
                            TanggalPembelian = m.Asset.YearAcquired.ToString(),
                            Type = m.Type.Name,
                            Process = m.Status,
                            PIC = m.Requester.Name,
                            StatusBarang = m.AssetCondition.JenisNama
                        }).ToListAsync();

            return reportData;
        }
    }
}
