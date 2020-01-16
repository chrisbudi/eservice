using E.Service.Resource.Data.Interface.Report;
using E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO;
using E.Service.Resource.Data.Interface.Report.DTO.OrderReloadRequestDetailDTO.cs;
using E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Report
{
    public class ReportMeetingRequestService : IMeetingRequestReportService
    {

        private EservicesdbContext db;


        public ReportMeetingRequestService(EservicesdbContext db)
        {
            this.db = db;
        }



        public async Task<ReportMeetingRequestDetailDTO> GetReportMeetingDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await (from p in db.MeetingRequests
                              join q in db.MeetingRequestAccountability on p.Id equals q.MeetingRequestId into macc
                              from acc in macc.DefaultIfEmpty()
                              where p.Id == Id
                              select new ReportMeetingRequestDetailDTO()
                              {
                                  Id = p.Id,
                                  budgetReal = acc != null ? (acc.TotalBudgetReal ?? 0).ToString("N2") : "",
                                  JumlahPesertaAwal = p.NumOfParticipant.ToString(),
                                  JumlahPesertaReal = acc != null ? (acc.NumOfPartisipant ?? 0).ToString("N0") : "",
                                  MeetingRequestNo = p.MeetingRequestNo,
                                  NamaPic = acc != null ? acc.Pic.Name : "",
                                  RequesterName = p.Requester.Name,
                                  RuangRapat = p.MeetingRoom.Name,
                                  TanggalPemesanan = (p.CreatedAt ?? defaultDate).ToString("dd-MM-yyyy"),
                                  TanggalPertanggungJawaban = acc != null ? (acc.CreatedAt ?? defaultDate).ToString("dd-MM-yyyy") : "",
                                  TipeRapat = p.MeetingType.JenisNama,
                                  Title = p.MeetingTitle
                              }).SingleAsync();


            return data;
        }

        public async Task<List<ReportMeetingRequestTimeDetailDTO>> GetReportMeetingTimeDetail(int Id)
        {
            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await db.MeetingRequestTime
                .Where(m => m.MeetingRequestId == Id)
                .Select(m => new ReportMeetingRequestTimeDetailDTO()
                {
                    Id = m.Id,
                    MeetingRequestId = m.MeetingRequestId ?? 0,
                    TanggalPesan = (m.StartDate ?? defaultDate).ToString("dd-MM-yyyy"),
                    WaktuMulai = (m.StartDate ?? defaultDate).ToString("HH:mm"),
                    WaktuSelesai = (m.EndDate ?? defaultDate).ToString("HH:mm")
                }).ToListAsync();

            return data;
        }
        public async Task<List<ReportMeetingRequestFilesDetailDTO>> GetReportMeetingFilesDetail(int Id)
        {

            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await db.MeetingRequestAccountabilityFiles
                .Where(m => m.MeetingRequestAccountability.MeetingRequestId == Id)
                .Select(m => new ReportMeetingRequestFilesDetailDTO()
                {
                    Id = m.Id,
                    MeetingRequestId = m.MeetingRequestAccountability.MeetingRequestId ?? 0,
                    FilePath = m.UploadFiles
                }).ToListAsync();

            return data;
        }

        public async Task<List<ReportMeetingRequestJamuanDetailDTO>> GetReportMeetingJamuanDetail(int Id)
        {

            var defaultDate = DateTime.Parse("1900-01-01");
            var data = await db.MeetingRequestBudgets
                .Where(m => m.MeetingRequestId == Id)
                .Select(m => new ReportMeetingRequestJamuanDetailDTO()
                {
                    Id = m.Id,
                    MeetingRequestId = m.MeetingRequestId ?? 0,
                    Amount = m.Amount.ToString("N2"),
                    NamaJamuan = m.MeetingBudget.BudgetName,
                    TotalAmount = m.TotalAmount.ToString("N2")

                }).ToListAsync();

            return data;
        }


        public async Task<List<ReportMeetingRequest>> GetReportMeetingRequest(
            int locationId, int regionId, string Status,
            DateTime? startDate, DateTime? endDate, int DepartmentId)
        {
            var defaultDate = DateTime.Parse("1900-01-01");

            var repos = db.MeetingRequests.AsQueryable();

            if (locationId != 0)
            {
                repos = repos.Where(m => m.MeetingRoom.Room.OfficeLocationId == locationId);
            }

            if (regionId != 0)
            {
                repos = repos.Where(m => m.MeetingRoom.Room.OfficeLocation.RegionId == regionId);
            }

            if (DepartmentId != 0)
            {
                repos = repos.Where(m => m.DepartmentId == DepartmentId);
            }


            if (startDate != null && startDate != defaultDate)
            {
                repos = repos.Where(m => m.CreatedAt >= startDate);

            }

            if (endDate != null && endDate != defaultDate)
            {
                repos = repos.Where(m => m.CreatedAt <= endDate);
            }

            var dataExport = await
                (from m in repos
                 join mt in db.MeetingRequestTime on m.Id equals mt.MeetingRequestId
                 join req in db.MeetingRequestFlow on m.Id equals req.MeetingRequestId
                 join temAcc in db.MeetingRequestAccountability on m.Id equals temAcc.MeetingRequestId
                 into temAccs
                 from acc in temAccs.DefaultIfEmpty()
                 join accFlow in db.MeetingRequestAccountabilityFlow on acc.Id equals accFlow.MeetingRequestAccountabilityId
                 select new ReportMeetingRequest()
                 {
                     Id = m.Id,
                     BudgetAwal = m.TotalBudgetBook.Value.ToString("N2"),
                     DepartmentId = m.DepartmentId.Value,
                     JamMulai = mt.StartDate.Value.ToString("HH:mm"),
                     JamSelesai = mt.EndDate.Value.ToString("HH:mm"),
                     JumlahPeserta = m.NumOfParticipant,
                     Location = m.MeetingRoom.Room.OfficeLocation.Name,
                     Wilayah = m.MeetingRoom.Room.OfficeLocation.Region.Name,
                     Title = m.MeetingTitle,
                     PICRuangRapat = m.MeetingRoom.Pic.Name,
                     RuangRapat = m.MeetingRoom.Name,
                     TanggalPemesanan = m.CreatedAt.Value.ToString("dd-MM-yyyy"),
                     TanggalRapat = mt.StartDate.Value.ToString("dd-MM-yyyy"),
                     Realisasi = (acc.TotalBudgetReal.Value).ToString("N2"),
                     StatusRequest = req.Request.Currentstate.Name,
                     StatusRequestAccountability = accFlow.Request.Currentstate.Name,
                     MeetingRequestNo = m.MeetingRequestNo,
                     MeetingType = m.MeetingType.JenisNama,
                     NO = 0
                 }).ToListAsync();

            return dataExport;
        }

    }
}
