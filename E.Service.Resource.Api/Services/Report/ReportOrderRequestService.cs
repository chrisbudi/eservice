using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.OrderRequestDetailDTO.cs;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Report
{
    public class ReportOrderRequestService : IOrderRequestReport
    {
        private EservicesdbContext _db;

        public ReportOrderRequestService(EservicesdbContext db)
        {
            _db = db;
        }

        public async Task<List<ReportOrderRequest>> GetReport(int regionalId)
        {
            var carreport = await _db.OrderRequest
                .Select(m => new ReportOrderRequest()
                {
                    Wilayah = m.OfficeLocation.Region.Name,
                    Deskripsi = m.Description,
                    No = 0,
                    Lokasi = m.OfficeLocation.Name,
                    StatusRequest = m.Requester.DepartmentId.ToString(),
                    UserPengaju = m.Requester.Name,
                    Budget = m.TotalBudget.ToString("N2"),
                    Harga = m.OrderRequestsDetail.Budget.ToString("N2"),
                    NamaItem = m.OrderRequestsDetail.OrderItem.Name,
                    NoRequest = m.RequestNo,
                    Pic = m.OrderRequestAccountability.Pic.Name,
                    Quantity = m.OrderRequestsDetail.Qty ?? 0,
                    Realisasi = m.OrderRequestAccountability.TotalBudget.ToString("N2"),
                    SatuanKerjaId = m.Requester.DepartmentId.ToString(),
                    SatuanKerjaName = "",
                    TanggalPermintaan = m.RequiredAt.Value.ToString("dd-MM-yyyy"),
                    TanggalPermintaanSelesaiEksekusi = m.OrderRequestAccountability.AccountabilityDate.ToString("dd-MM-yyyy"),
                }).ToListAsync();
            return carreport;
        }

        public async Task<ReportOrderRequestDetailDTO> GetReportDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await(from p in _db.OrderRequest
                             join res in _db.OrderReloadAccountability on p.Id equals res.OrderReloadId
                             into acca
                             from acc in acca.DefaultIfEmpty()
                             where p.Id == Id
                             select new ReportOrderRequestDetailDTO()
                             {
                                 Id = p.Id,
                                 RequesterName = p.Requester.Name,
                                 SatuanKerjaId = p.Requester.DepartmentId ?? 0,
                                 StatusTransaksi = p.Request.Currentstate.Name,
                                 TanggalPemesanan = (p.CreateAt ?? defaultDate).ToString("dd-MM-yyyy"),
                                 Title = p.Description,
                                 TransactionNo = p.RequestNo,
                                 Lokasi = p.OfficeLocation.Name,
                                 NamaBarang = p.OrderRequestsDetail.OrderItem.Name
                             }).SingleAsync();
            return data;
        }

        public async Task<List<OrderRequestApprovalDetailDTO>> GetReportDetailApproval(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in _db.OrderRequest
                              join requestHistory in _db.RequestActionHistory on p.Request.Requestid equals requestHistory.RequestId
                              where p.Id == Id
                              select new OrderRequestApprovalDetailDTO()
                              {
                                  Id = requestHistory.Id,
                                  Approval = requestHistory.HistoryType,
                                  NamaAprover = requestHistory.User.Name,
                                  StatusApprover = requestHistory.RequestAction.Transition.Currentstate.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<List<OrderRequestFilesDetailDTO>> GetReportDetailFiles(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await _db.OrderRequestAccountabilityImage
                .Where(m => m.OrderRequestId == Id)
                .Select(m => new OrderRequestFilesDetailDTO()
                {
                    Id = m.Id,
                    MasterId = m.OrderRequest.RequestId,
                    FilePath = m.Image.FilePath
                }).ToListAsync();

            return data;
        }

        public async Task<List<OrderRequestItemDetailDTO>> GetReportDetailItem(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await(from p in _db.OrderReloadDetail
                             where p.Id == Id
                             select new OrderRequestItemDetailDTO()
                             {
                                 Id = p.Id,
                                 Kuantity1 = (p.Qty ?? 0).ToString("dd-MM-yyyy"),
                                 Kuantity2 = (p.Qty2 ?? 0).ToString("dd-MM-yyyy"),
                                 Lokasi = p.StockTransaction.Stock.OfficeLocation.Name,
                                 NamaWilayah = p.StockTransaction.Stock.OfficeLocation.Region.Name,
                                 Stock = p.StockTransaction.Qty.ToString("N0"),
                                 MaxStock = (p.OrderItem.OrderItemStock.MaxStock ?? 0).ToString("N0"),
                                 MinStock = (p.OrderItem.OrderItemStock.MinStock ?? 0).ToString("N0"),
                                 Total = ((p.Qty ?? 0) + ((p.Qty2 ?? 0) * (p.OrderItem.OrderItemStock.Konv1ke2 ?? 0))).ToString("N0"),
                             }).ToListAsync();
            return data;
        }

        public async Task<List<ReportOrderRequest>> GetReportSummary(int locationId, int regionalId, DateTime? startDate, DateTime? endDate, int departmentId, string jenisOrderId)
        {

            var defaultDate = DateTime.Parse("1900-01-01");


            var repos = _db.OrderRequest.AsQueryable();


            if (locationId != 0)
            {
                repos = repos.Where(m => m.OrderRequestsDetail.StockTransaction.Stock.OfficeLocationId == locationId);
            }

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.OrderRequestsDetail.StockTransaction.Stock.OfficeLocation.RegionId == regionalId);
            }


            if (startDate != null && startDate != defaultDate)
            {
                repos = repos.Where(m => m.OrderRequestsDetail.StockTransaction.StockDate.Value > startDate);
            }


            if (endDate != null && endDate != defaultDate)
            {
                repos = repos.Where(m => m.OrderRequestsDetail.StockTransaction.StockDate.Value < endDate);
            }



            var reportData = await repos
                .Select(m => new ReportOrderRequest()
                {
                    Wilayah = m.OfficeLocation.Region.Name,
                    Deskripsi = m.Description,
                    No = 0,
                    Lokasi = m.OfficeLocation.Name,
                    StatusRequest = m.Request.Currentstate.Name,

                    UserPengaju = m.Requester.Name,
                    Budget = m.TotalBudget.ToString("N2"),
                    Harga = m.OrderRequestsDetail.Budget.ToString("N2"),
                    NamaItem = m.OrderRequestsDetail.OrderItem.Name,
                    NoRequest = m.RequestNo,
                    Pic = m.OrderRequestAccountability.Pic.Name,
                    Quantity = m.OrderRequestsDetail.Qty ?? 0,
                    Quantity2 = m.OrderRequestsDetail.Qty2 ?? 0,
                    Realisasi = m.OrderRequestAccountability.TotalBudget.ToString("N2"),
                    SatuanKerjaId = m.Requester.DepartmentId.ToString(),
                    SatuanKerjaName = "",
                    TanggalPermintaan = m.RequiredAt.Value.ToString("dd-MM-yyyy"),
                    TanggalPermintaanSelesaiEksekusi = m.OrderRequestAccountability.AccountabilityDate.ToString("dd-MM-yyyy"),
                }).ToListAsync();
            return reportData;




        }
    }
}
