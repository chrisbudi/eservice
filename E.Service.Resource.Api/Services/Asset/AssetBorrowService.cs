using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Asset
{
    public class AssetBorrowService : IAssetBorrowService
    {

        public EservicesdbContext db;


        IRequestService _requestService;
        IUserService _userService;

        public AssetBorrowService(EservicesdbContext db,
            IRequestService requestService,
            IUserService userService)
        {
            this.db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<Control<AssetBorrowDTO>> Get(int start, int take, string filter, string order)
        {
            var repos = db.AssetBorrow.AsQueryable();

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Asset.Name.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<AssetBorrowDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetBorrowDTO
                    {
                        Id = m.Id,
                        Description = m.Description,
                        Title = m.Title,
                        AssetId = m.AssetId,
                        AssetName = m.Asset.Name,
                        BorrowDate = m.BorrowDate,
                        JabatanId = m.JabatanId,
                        JabatanName = m.Jabatan.Name,
                        OrganizationId = m.OrganizationId,
                        RequestDate = m.RequestDate,
                        RequestId = m.RequestId,
                        ReturnDate = m.ReturnDate,
                        AssetNumber = m.Asset.AssetNumber,
                        Status = m.Request.Currentstate.Name,
                        RoomId = m.RoomId,
                        RoomName = m.Room != null ? m.Room.Name : ""
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetBorrowDTO> Get(int id)
        {
            return await db.AssetBorrow.Select(m => new AssetBorrowDTO()
            {
                Id = m.Id,
                Description = m.Description,
                AssetNumber = m.Asset.AssetNumber,
                Title = m.Title,
                AssetId = m.AssetId,
                AssetName = m.Asset.Name,
                BorrowDate = m.BorrowDate,
                JabatanId = m.JabatanId,
                JabatanName = m.Jabatan.Name,
                OrganizationId = m.OrganizationId,
                RequestDate = m.RequestDate,
                RequestId = m.RequestId,
                ReturnDate = m.ReturnDate,
                Status = m.Request.Currentstate.Name,
                RoomId = m.RoomId,
                RoomName = m.Room != null ? m.Room.Name : ""
            }).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Control<AssetBorrowDTO>> GetAssetId(int start, int take, string filter, string order, int assetId)
        {
            var repos = db.AssetBorrow.Where(m => m.AssetId == assetId);

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Asset.Name.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            return new Control<AssetBorrowDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetBorrowDTO
                    {

                        Id = m.Id,
                        Description = m.Description,
                        AssetNumber = m.Asset.AssetNumber,
                        Title = m.Title,
                        AssetId = m.AssetId,
                        AssetName = m.Asset.Name,
                        BorrowDate = m.BorrowDate,
                        JabatanId = m.JabatanId,
                        JabatanName = m.Jabatan.Name,
                        OrganizationId = m.OrganizationId,
                        RequestDate = m.RequestDate,
                        RequestId = m.RequestId,
                        ReturnDate = m.ReturnDate,
                        Status = m.Request.Currentstate.Name,
                        RoomId = m.RoomId,
                        RoomName = m.Room != null ? m.Room.Name : ""
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetBorrowDTO> GetByRequestId(int id)
        {
            return await db.AssetBorrow.Select(m => new AssetBorrowDTO()
            {
                Id = m.Id,
                AssetNumber = m.Asset.AssetNumber,
                Description = m.Description,
                Title = m.Title,
                AssetId = m.AssetId,
                AssetName = m.Asset.Name,
                BorrowDate = m.BorrowDate,
                JabatanId = m.JabatanId,
                JabatanName = m.Jabatan.Name,
                OrganizationId = m.OrganizationId,
                RequestDate = m.RequestDate,
                RequestId = m.RequestId,
                ReturnDate = m.ReturnDate,
                Status = m.Request.Currentstate.Name,
                RoomId = m.RoomId,
                RoomName = m.Room != null ? m.Room.Name : ""
            }).SingleOrDefaultAsync(m => m.RequestId == id);
        }

        public async Task<AssetBorrow> Save(AssetBorrow entity, bool submit)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {

                var user = await _userService.GetUserById(entity.RequesterId.Value);
                var prFixrequest = $"04{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.RequestBorrowNo = await GenerateNo(prFixrequest);

                entity.RequestDate = DateTime.Now;
                entity.JabatanId = user.JabatanId;

                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.AssetBorrow),
                    Title = entity.Title,
                    Note = entity.Description,
                    Processid = await _requestService.ProgressId(ERequestType.AssetBorrow),
                    Userid = entity.RequesterId.ToString(),
                    Url = "asset/borrow/dashboard/request/"
                };

                var transitionList = await _requestService.
                    TransitionList(ERequestType.AssetBorrow);
                bool firstInput = true;

                foreach (var transition in transitionList)
                {
                    var raNext = new Requestaction()
                    {
                        Actionid = transition.Transitionaction.Actonid,
                        Isactive = firstInput,
                        Iscomplete = false,
                        Transitionid = transition.Transitionid
                    };
                    request.Requestaction.Add(raNext);
                    firstInput = false;
                }

                entity.Request = request;
                await db.AssetBorrow.AddAsync(entity);
            }
            else
            {
                db.AssetBorrow.Update(entity);
                db.Entry(entity).Property(x => x.RequestDate).IsModified = false;
                db.Entry(entity).Property(x => x.RequestBorrowNo).IsModified = false;

                request = db.RequestFlow.Single(m => m.Requestid == entity.RequestId);
            }
            await db.SaveChangesAsync();

            if (submit == true)
            {
                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.RequesterId.Value
                    });
            }


            await db.SaveChangesAsync();
            return entity;
        }

        private async Task<string> GenerateNo(string prFixrequest)
        {
            var latestNo = await db.AssetBorrow.OrderByDescending(m => m.RequestBorrowNo).
             FirstOrDefaultAsync(m => m.RequestBorrowNo.StartsWith(prFixrequest));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.RequestBorrowNo.Substring(latestNo.RequestBorrowNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFixrequest + noSeries.ToString("0000");
        }
    }
}
