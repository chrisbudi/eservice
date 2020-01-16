using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.OrderReloadRequestDetailDTO.cs;
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
    public class ReportOrderReloadService : IOrderReloadReport
    {

        private EservicesdbContext _db;

        public ReportOrderReloadService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportOrderReload>> GetReport(int regionalId)
        {
            var carreport = await _db.OrderReloadDetail
                .Select(m => new ReportOrderReload()
                {
                    Wilayah = m.OrderReload.OfficeLocation.Region.Name,
                    Deskripsi = m.OrderReload.Description,
                    No = 0,
                    Lokasi = m.OrderReload.OfficeLocation.Name,
                    StatusRequest = m.OrderReload.Requester.DepartmentId.ToString(),
                    UserPengaju = m.OrderReload.Requester.Name,
                    Budget = m.OrderReload.TotalBudget.ToString("N2"),
                    MaxStock = (m.OrderItem.OrderItemStock.MaxStock ?? 0).ToString("N0"),
                    MinStock = (m.OrderItem.OrderItemStock.MinStock ?? 0).ToString("N0"),
                    Harga = m.Budget.ToString("N2"),
                    NamaItem = m.OrderItem.Name,
                    NoRequest = m.OrderReload.ReloadNo,
                    Pic = m.OrderReload.OrderReloadAccountability.Pic.Name,
                    Quantity = ((m.Qty ?? 0) + ((m.Qty2 ?? 0) * (m.OrderItem.OrderItemStock.Konv1ke2 ?? 0))).ToString("N0"),
                    Quantity1 = (m.Qty ?? 0).ToString("N0"),
                    Quantity2 = (m.Qty2 ?? 0).ToString("N0"),
                    Realisasi = m.OrderReload.OrderReloadAccountability.TotalBudget.ToString("N2"),
                    SatuanKerja = m.OrderReload.Requester.DepartmentId.ToString(),
                    TanggalPermintaan = m.OrderReload.RequiredAt.Value.ToString("dd-MM-yyyy"),
                    TanggalPermintaanSelesaiEksekusi = m.OrderReload.OrderReloadAccountability.AccountabilityDate.ToString("dd-MM-yyyy"),
                    Stock = "0",
                    TanggalReload = m.OrderReload.RequiredAt.Value.ToString("dd-MM-yyyy"),
                    Total = ((m.Qty ?? 0) + ((m.Qty2 ?? 0) * (m.OrderItem.OrderItemStock.Konv1ke2 ?? 0) * m.Budget)).ToString("N2")
                }).ToListAsync();
            return carreport;
        }

        public async Task<ReportOrderReloadRequestDetailDTO> GetReportDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.OrderReload
                              join res in _db.OrderReloadAccountability on p.Id equals res.OrderReloadId
                              into acca
                              from acc in acca.DefaultIfEmpty()
                              where p.Id == Id
                              select new ReportOrderReloadRequestDetailDTO()
                              {
                                  Id = p.Id,
                                  Budget = p.OrderReloadDetail.Sum(m => m.Budget).ToString("N2"),
                                  RequesterName = p.Requester.Name,
                                  SatuanKerjaId = p.Requester.DepartmentId ?? 0,
                                  StatusTransaksi = p.Request.Currentstate.Name,
                                  TanggalPemesanan = (p.CreateAt ?? defaultDate).ToString("dd-MM-yyyy"),
                                  Title = p.Description,
                                  TransactionNo = p.ReloadNo
                              }).SingleAsync();
            return data;
        }

        public async Task<List<OrderReloadApprovalDetailDTO>> GetReportDetailApproval(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.OrderReload
                              join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                              where p.Id == Id
                              select new OrderReloadApprovalDetailDTO()
                              {
                                  Id = requestHistory.Id,
                                  Approval = requestHistory.HistoryType,
                                  NamaAprover = requestHistory.User.Name,
                                  StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<List<OrderReloadFilesDetailDTO>> GetReportDetailFiles(int Id)
        {

            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await _db.OrderRequestAccountabilityImage
                .Where(m => m.OrderRequestId == Id)
                .Select(m => new OrderReloadFilesDetailDTO()
                {
                    Id = m.Id,
                    MasterId = m.OrderRequest.RequestId,
                    FilePath = m.Image.FilePath
                }).ToListAsync();

            return data;
        }

        public async Task<List<OrderReloadItemDetailDTO>> GetReportDetailItem(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.OrderReloadDetail
                              where p.Id == Id
                              select new OrderReloadItemDetailDTO()
                              {
                                  Id = p.Id,
                                  Kuantity1 = (p.Qty ?? 0).ToString(),
                                  Kuantity2 = (p.Qty2 ?? 0).ToString(),
                                  Lokasi = p.StockTransaction.Stock.OfficeLocation.Name,
                                  NamaWilayah = p.StockTransaction.Stock.OfficeLocation.Region.Name,
                                  Stock = p.StockTransaction.Qty.ToString("N0"),
                                  MaxStock = (p.OrderItem.OrderItemStock.MaxStock ?? 0).ToString("N0"),
                                  MinStock = (p.OrderItem.OrderItemStock.MinStock ?? 0).ToString("N0"),
                                  NamaItem = p.OrderItem.Name,
                                  Total = ((p.Qty ?? 0) + ((p.Qty2 ?? 0) * (p.OrderItem.OrderItemStock.Konv1ke2??0))).ToString("N0"),
                              }).ToListAsync();
            return data;
        }


        public async Task<List<ReportOrderReload>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId,
            string jenisOrderId)
        {
            var defaultDate = DateTime.Parse("1900-01-01");

            var repos = _db.OrderReloadDetail.AsQueryable();


            if (locationId != 0)
            {
                repos = repos.Where(m => m.StockTransaction.Stock.OfficeLocationId == locationId);
            }

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.StockTransaction.Stock.OfficeLocation.RegionId == regionalId);
            }


            if (startDate != null && startDate != defaultDate)
            {
                repos = repos.Where(m => m.StockTransaction.StockDate.Value > startDate);
            }


            if (endDate != null && endDate != defaultDate)
            {
                repos = repos.Where(m => m.StockTransaction.StockDate.Value < endDate);
            }

            var reportData = await repos
                            .Select(m => new ReportOrderReload()
                            {
                                Wilayah = m.OrderReload.OfficeLocation.Region.Name,
                                Deskripsi = m.OrderReload.Description,
                                No = 0,
                                Lokasi = m.OrderReload.OfficeLocation.Name,
                                StatusRequest = m.OrderReload.Requester.DepartmentId.ToString(),
                                UserPengaju = m.OrderReload.Requester.Name,
                                Budget = m.OrderReload.TotalBudget.ToString("N2"),
                                MaxStock = (m.OrderItem.OrderItemStock.MaxStock ?? 0).ToString("N0"),
                                MinStock = (m.OrderItem.OrderItemStock.MinStock ?? 0).ToString("N0"),
                                Harga = m.Budget.ToString("N2"),
                                NamaItem = m.OrderItem.Name,
                                NoRequest = m.OrderReload.ReloadNo,
                                Pic = m.OrderReload.OrderReloadAccountability.Pic.Name,
                                Quantity = ((m.Qty ?? 0) + ((m.Qty2 ?? 0) * (m.OrderItem.OrderItemStock.Konv1ke2 ?? 0))).ToString("N0"),
                                Quantity1 = (m.Qty ?? 0).ToString("N0"),
                                Quantity2 = (m.Qty2 ?? 0).ToString("N0"),
                                Realisasi = m.OrderReload.OrderReloadAccountability.TotalBudget.ToString("N2"),
                                SatuanKerja = m.OrderReload.Requester.DepartmentId.ToString(),
                                TanggalPermintaan = m.OrderReload.RequiredAt.Value.ToString("dd-MM-yyyy"),
                                TanggalPermintaanSelesaiEksekusi = m.OrderReload.OrderReloadAccountability.AccountabilityDate.ToString("dd-MM-yyyy"),
                                Stock = "0",
                                TanggalReload = m.OrderReload.RequiredAt.Value.ToString("dd-MM-yyyy"),
                                Total = ((m.Qty ?? 0) + ((m.Qty2 ?? 0) * (m.OrderItem.OrderItemStock.Konv1ke2 ?? 0) * m.Budget)).ToString("N2")
                            }).ToListAsync();
            return reportData;


        }
    }
}
