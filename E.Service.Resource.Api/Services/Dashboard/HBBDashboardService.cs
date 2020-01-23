using E.Service.Resource.Data.Interface.Dashboard;
using E.Service.Resource.Data.Interface.Dashboard.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Dashboard
{
    public class HBBDashboardService : IHBBDashboardService
    {

        private EservicesdbContext db;


        string[] months = new string[] {"Januari", "Febuari", "Maret", "April", "Mei",
                "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Disember"};
        public HBBDashboardService(EservicesdbContext db)
        {
            this.db = db;
        }



        public async Task<HBBDashboardDTO> GetListHBB()
        {
            var assethbb = await db.Assets//.Where(m => m.StartTime.Value.Date == dateTime.Date && m.EndTime.Value.Date == dateTime.Date)
                .Select(m => new HBBDashboardDTO()
                {
                    Broken = m.AssetRequests.Count(r => r.Asset.AssetRequests.Any(rd => rd.AssetCondition.JenisNama == "Rusak")),
                    BrokenAll = m.AssetRequests.Count(r => r.Asset.AssetRequests.Any(rd => rd.AssetCondition.JenisNama == "Rusak Sekali")),
                    Missing = m.AssetRequests.Count(r => r.Asset.AssetRequests.Any(rd => rd.AssetCondition.JenisNama == "Hilang")),
                    Good = m.AssetRequests.Count(r => r.Asset.AssetRequests.Any(rd => rd.AssetCondition.JenisNama == "Baik")),
                    GoodEnough = m.AssetRequests.Count(r => r.Asset.AssetRequests.Any(rd => rd.AssetCondition.JenisNama == "Cukup Baik")),
                }).ToListAsync();

            var assetLatestVal = new HBBDashboardDTO
            {
                Broken = assethbb.Sum(m => m.Broken),
                BrokenAll = assethbb.Sum(m => m.BrokenAll),
                Missing = assethbb.Sum(m => m.Missing),
                Good = assethbb.Sum(m => m.Good),
                GoodEnough = assethbb.Sum(m => m.GoodEnough)
            };


            return assetLatestVal;

        }

        public async Task<List<AssetDataTotalMoveValue>> GetListMove(int year)
        {
            var asset = db.AssetRequests.Where(m => (m.RequestDate.Year) == year);
            var listAsset = new List<AssetDataTotalMoveValue>();
            for (var i = 0; i <= months.Length - 1; i++)
            {
                AssetDataTotalMoveValue ad = new AssetDataTotalMoveValue()
                {
                    Name = months[i],
                    Value = await asset.CountAsync(m => m.RequestDate.Month == i + 1)
                };
                listAsset.Add(ad);
            }

            return listAsset.ToList();
        }

        public async Task<List<AssetData>> GetListTotal(int year)
        {
            var asset = db.Assets.Where(m => (m.CreatedAt.HasValue ? m.CreatedAt.Value.Year : DateTime.Now.Year) == year);
            var listAsset = new List<AssetData>();
            for (var i = 0; i <= months.Length - 1; i++)
            {
                AssetData ad = new AssetData()
                {
                    Name = months[i],
                    Value = await asset.CountAsync(m => m.CreatedAt.Value.Month == i + 1)
                };
                listAsset.Add(ad);
            }

            return listAsset.ToList();
        }

        public async Task<List<AssetDataTotalValue>> GetListValue(int year)
        {
            var assets = await db.Assets.Where(m => (m.CreatedAt.HasValue ? m.CreatedAt.Value.Year : DateTime.Now.Year) <= year).ToListAsync();
            var listAsset = new List<AssetDataTotalValue>();
            foreach(var asset in assets)
            {

                var susutTahun = DateTime.Now.Year - asset.YearAcquired;
                var susutVal = (asset.PriceAcquired / (susutTahun * 12));
                AssetDataTotalValue ad = new AssetDataTotalValue()
                {
                    Name = asset.Name,
                    Value = asset.PriceAcquired -susutVal
                };
                listAsset.Add(ad);
            }
            return listAsset;


        }

        public async Task<List<int>> GetListYear()
        {
            return await db.Assets.Select(m => m.CreatedAt.HasValue ? m.CreatedAt.Value.Year : DateTime.Now.Year).Distinct().ToListAsync();
        }
    }
}
