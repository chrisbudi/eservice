using E.Service.Resource.Data.Interface.Dashboard.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Dashboard
{
    public interface IHBBDashboardService
    {
        Task<HBBDashboardDTO> GetListHBB();
        Task<List<int>> GetListYear();
        Task<List<AssetData>> GetListTotal(int year);
        Task<List<AssetDataTotalValue>> GetListValue(int year);
        Task<List<AssetDataTotalMoveValue>> GetListMove(int year);
    }
}
