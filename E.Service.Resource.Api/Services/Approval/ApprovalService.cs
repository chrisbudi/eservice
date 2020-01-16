using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Approval;
using E.Service.Resource.Data.Interface.Approval.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Approval
{
    public class ApprovalService : IApprovalService
    {

        private EservicesdbContext db;
        IUserService _userService;
        public ApprovalService(EservicesdbContext db, IUserService userService)
        {
            this.db = db;
            this._userService = userService;
        }

        public async Task<Control<RequestActionHistoryDTO>> GetRequestActionHistory(int requestId, int start, int take, string filter, string order)
        {
            var repos = db.RequestActionHistory.Where(m => m.RequestId == requestId);
            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.User.Name.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<RequestActionHistoryDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new RequestActionHistoryDTO
                    {
                        Id = m.Id,
                        CurrentState = m.RequestAction.Transition.Nextstate.Name,
                        BeforeState = m.RequestAction.Transition.Currentstate.Name,
                        ActionName = m.RequestAction.Action.Name,
                        Datetime = m.Datetime,
                        Note = m.Note,
                        UserPIC = m.User.Name,
                        UserId = m.UserId,
                        RequestId = m.RequestId,
                        RequestActionId = m.RequestActionId

                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<Control<UserRequestDTO>> GetUserApprovalUser(int start, int take, string filter, string order, string userId)
        {

            var user = await _userService.GetUser(userId);


            var repos = db.RequestFlow.Where(p => p.Userid == user.Id.ToString()).AsQueryable();

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Title.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<UserRequestDTO>()
            {
                ListClass = null,
                Total = totalData,
                TotalFilter = totalFilterData
            };


        }

        public async Task<Control<UserRequestDTO>> GetUserLoginRequest(int start,
            int take, string filter, string order, string userId)
        {
            var target = db.TargetUser.Where(m => m.UserId == userId);

            if (!target.Any())
                return null;

            var user = await _userService.GetUser(userId);


            var stateTypeNormalId = (int)EStateType.Normal;

            var stateTypeReject = (int)EStateType.Reject;

            var action = db.Transitionaction.Where(m =>
                target.Any(t => m.Acton.Actiontarget.Any(act => act.Targetid == t.TargetId)) &&
                (m.Transition.Currentstate.Statetypeid == stateTypeNormalId ||
                m.Transition.Currentstate.Statetypeid == stateTypeReject)).
                Select(m => new
                {
                    m.Transitionid,
                    m.Actonid
                });




            var repos = db.Requestaction.Where(m =>
                action.Any(a => a.Actonid == m.Actionid && a.Transitionid == m.Transitionid && m.Isactive && !m.Iscomplete) &&

                m.Iscomplete == false &&
                m.Action.Actiontarget.Any(t => t.Target.TargetUser.Any(tu => tu.UserId == user.UserId)) &&

                (m.Request.Currentstate.Statetypeid == stateTypeNormalId || m.Request.Currentstate.Statetypeid == stateTypeReject) &&
                (

                ////meeting request
                m.Request.MeetingRequestFlow.Any(f => user.MeetingRoomIds.Contains(f.MeetingRoomId.Value)) ||

                ////meeting request accountablity
                m.Request.MeetingRequestAccountabilityFlow.Any(f => f.RegionalId == user.RegionId) ||

                //asset request -->  region
                m.Request.AssetRequests.Any(a => a.RegionalId == user.RegionId) ||

                //////asset borrow jabatan or parent id
                (m.Request.AssetBorrow.Any(a => a.JabatanId == user.JabatanId) ||
                m.Request.AssetBorrow.Any(a => a.Jabatan.JabatanChildJabatanChildNavigation.Any(c => c.ParentJabatanId == user.JabatanId))) &&
                m.Request.AssetBorrow.Any(a => a.Room.OfficeLocation.RegionId == user.RegionId) ||

                ////Car Request
                (m.Request.CarRequests.Any(a => a.CarPool.OfficeLocation.RegionId == user.RegionId)) ||

                ////travel request
                (m.Request.TravelRequest.Any(a =>
                a.JabatanId == user.JabatanId ||
                a.Jabatan.JabatanChildJabatanChildNavigation.Any(c => c.ParentJabatanId == user.JabatanId))) &&
                m.Request.TravelRequest.Any(a => a.OfficeLocation.RegionId == user.RegionId) ||

                //repair request
                (m.Request.RepairItemRequests.Any(a => a.JabatanId == user.JabatanId ||
                a.Jabatan.JabatanChildJabatanChildNavigation.Any(c => c.ParentJabatanId == user.JabatanId))) &&
                m.Request.RepairItemRequests.Any(a => a.OfficeLocation.RegionId == user.RegionId) ||

                //order request
                (m.Request.OrderRequest.Any(a => a.JabatanId == user.JabatanId ||
                a.Jabatan.JabatanChildJabatanChildNavigation.Any(c => c.ParentJabatanId == user.JabatanId))) &&
                m.Request.OrderRequest.Any(a => a.OfficeLocation.RegionId == user.RegionId) ||

                //order reload
                (m.Request.OrderReload.Any(a => a.JabatanId == user.JabatanId ||
                a.Jabatan.JabatanChildJabatanChildNavigation.Any(c => c.ParentJabatanId == user.JabatanId))) &&
                m.Request.OrderReload.Any(a => a.OfficeLocation.RegionId == user.RegionId) ||


                m.Request.OrderRequestAccountability.Any(a => a.Pic.Location.Region.Id == user.RegionId) ||

                m.Request.OrderReloadAccountability.Any(a => a.Pic.Location.Region.Id == user.RegionId)
                )

                )

                .Select(m => m.Request).AsQueryable();


            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Title.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<UserRequestDTO>()
            {
                ListClass = await data.Select(m => new UserRequestDTO()
                {
                    ProcessName = m.Process.Nama,
                    RequestDate = m.Daterequest.Value,
                    RequestId = m.Requestid,
                    RequestNote = m.Note,
                    RequestTitle = m.Title,
                    StateName = m.Currentstate.Name,
                    Url = //car request
                          ((m.Process.Nama == "Car Request" &&
                           m.Currentstate.Name == "Admin review") ||
                           //travel
                           (m.Process.Nama == "Travel" &&
                           m.Currentstate.Name == "In Admin Approval") ||
                           //repair IT and non it
                           ((m.Process.Nama == "Repair IT" || m.Process.Nama == "Repair Non IT") &&
                           m.Currentstate.Name.ToLower() == "in admin approval") ?
                           (
                           m.RequestFlowUrl.SingleOrDefault(r => r.Type == "confirm") == null ? m.Url :
                           m.RequestFlowUrl.SingleOrDefault(r => r.Type == "confirm").Url) :
                           m.Url)

                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }
    }
}
