using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Repair;
using E.Service.Resource.Data.Interface.Repair.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Repair
{
    public class RepairItemRequestService : IRepairItemRequestService
    {

        public EservicesdbContext _db;
        IRequestService _requestService;
        IUserService _userService;

        public RepairItemRequestService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            _db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<RepairItemRequestDTO> Get(int id)
        {
            return await _db.RepairItemRequests.Where(m => m.Id == id).Select(m => new RepairItemRequestDTO()
            {
                Description = m.Description,
                Id = m.Id,
                RepairItemId = m.RepairItemId,
                RepairItemName = m.RepairItem.Name,
                RepairItemType = m.RepairItem.RepairType,
                RepairItemTypeIT = m.RepairItem.ItItem,
                RequestNo = m.RequestNo,
                RequesterId = m.RequesterId,
                RequesterName = m.Requester.Name,
                RequestId = m.RequestId,
                Office = m.OfficeLocation.Name,
                Region = m.OfficeLocation.Region.Name,
                RequestDate = m.RequestDate.Value,
                Title = m.Title,
                Status = m.Request.Currentstate.Name,
                StatusType = m.Request.Currentstate.Statetype.Name,
                JabatanId = m.JabatanId.Value,
                OfficeLocationId = m.OfficeLocationId.Value,
                RepairItemImage = m.RepairItemRequestImage.Select(d => new RepairItemRequestImageDTO()
                {
                    FileUrl = d.FilePath,
                    Id = d.Id
                }).ToList()
            }).SingleOrDefaultAsync();
        }

        public async Task<RepairItemRequestAccountabilityDTO> GetAccountability(int id)
        {

            return await _db.RepairItemRequestAccountablity.Select(m => new RepairItemRequestAccountabilityDTO()
            {
                CreateDate = m.CreateDate,
                PicId = m.PicId,
                PicName = m.Pic.Name,
                RepairItemRequestId = m.RepairItemRequestId,
                TotalBudgetReal = m.TotalBudgetReal,
                RepairItemRequestDate = m.RepairItemRequest.RequestDate ?? DateTime.MinValue,
                RepairItemRequestDescription = m.RepairItemRequest.Description,
                RepairItemRequestTitle = m.RepairItemRequest.Title,
                RequestId = m.RepairItemRequest.RequestId ?? 0,
                RepairItemImage = m.RepairItemRequestAccountablitiyImage.Select(d => new RepairItemRequestAccountabiltyImageDTO()
                {
                    FileUrl = d.FilePath,
                    Id = d.Id
                }).ToList()
            }).SingleOrDefaultAsync(m => m.RepairItemRequestId == id);
        }

        public async Task<Control<RepairItemRequestAccountabilityDTO>> GetAccountabilityRequestList(int start, int take, string filter, string order)
        {
            var repos = _db.RepairItemRequestAccountablity.AsQueryable();


            //if(locationId == 0)
            //{
            //    repos = repos.Where(m => m.r)
            //}



            int totalData = repos.Count();




            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.RepairItemRequest.RepairItem.Name.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);

            return new Control<RepairItemRequestAccountabilityDTO>()
            {
                ListClass = await data.Select(m => new RepairItemRequestAccountabilityDTO
                {

                    CreateDate = m.CreateDate,
                    PicId = m.PicId,
                    RequestNo = m.RepairItemRequest.RequestNo,
                    PicName = m.Pic.Name,
                    RepairItemRequestId = m.RepairItemRequestId,
                    TotalBudgetReal = m.TotalBudgetReal,
                    RepairItemRequestDate = m.RepairItemRequest.RequestDate ?? DateTime.MinValue,
                    RepairItemRequestDescription = m.RepairItemRequest.Description,
                    RepairItemRequestTitle = m.RepairItemRequest.Title,
                    RequestId = m.RepairItemRequest.RequestId ?? 0,
                    RepairItemImage = m.RepairItemRequestAccountablitiyImage.Select(d => new RepairItemRequestAccountabiltyImageDTO()
                    {
                        FileUrl = d.FilePath,
                        Id = d.Id
                    }).ToList()
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };


        }

        public async Task<Control<RepairItemRequestDTO>> GetList(int start, int take, string filter, string order)
        {
            var repos = _db.RepairItemRequests.AsQueryable();

            int totalData = repos.Count();

            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.RepairItem.Name.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<RepairItemRequestDTO>()
            {
                ListClass = await data.Select(m => new RepairItemRequestDTO
                {

                    Description = m.Description,
                    Id = m.Id,
                    RepairItemId = m.RepairItemId,
                    RequestNo = m.RequestNo,
                    RepairItemName = m.RepairItem.Name,
                    RepairItemType = m.RepairItem.RepairType,
                    RepairItemTypeIT = m.RepairItem.ItItem,
                    RequesterId = m.RequesterId,
                    RequesterName = m.Requester.Name,
                    RequestId = m.RequestId,
                    Title = m.Title,
                    Office = m.OfficeLocation.Name,
                    Region = m.OfficeLocation.Region.Name,
                    RequestDate = m.RequestDate.Value,
                    Status = m.Request.Currentstate.Name,
                    JabatanId = m.JabatanId.Value,
                    StatusType = m.Request.Currentstate.Statetype.Name,
                    OfficeLocationId = m.OfficeLocationId.Value,
                    RepairItemImage = m.RepairItemRequestImage.Select(d => new RepairItemRequestImageDTO()
                    {
                        FileUrl = d.FilePath,
                        Id = d.Id
                    }).ToList()

                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<RepairItemRequests> Save(RepairItemRequests entity, bool submit, ERepairItTypes itTypes)
        {
            var request = new RequestFlow();
            if (entity.Id == 0)
            {
                var user = await _userService.GetUserById(entity.RequesterId);


                entity.DepartmentId = user.DepartmentId;
                entity.RequestDate = DateTime.Now;
                entity.JabatanId = user.JabatanId;
                entity.OfficeLocationId = user.LocationId;
                var prFix = $"08{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.RequestNo = await GenerateNo(prFix);
                List<Transition> transitionList = new List<Transition>();
                if (itTypes == ERepairItTypes.IT)
                {
                    request = new RequestFlow()
                    {
                        Daterequest = DateTime.Now,
                        Currentstateid = await _requestService.BeginStateId(ERequestType.RepairIT),
                        Title = entity.RequestNo,
                        Note = entity.Title,
                        Processid = await _requestService.ProgressId(ERequestType.RepairIT),
                        Userid = entity.RequesterId.ToString(),
                        Url = "repair/dashboard/request/"
                    };
                    transitionList = await _requestService.TransitionList(ERequestType.RepairIT);

                }
                else if (itTypes == ERepairItTypes.NONIT)
                {
                    request = new RequestFlow()
                    {
                        Daterequest = DateTime.Now,
                        Currentstateid = await _requestService.BeginStateId(ERequestType.RepairNonIT),
                        Title = entity.RequestNo,
                        Note = entity.Title,
                        Processid = await _requestService.ProgressId(ERequestType.RepairNonIT),
                        Userid = entity.RequesterId.ToString(),
                        Url = "repair/dashboard/request/"
                    };

                    transitionList = await _requestService.TransitionList(ERequestType.RepairNonIT);
                }


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
                    firstInput = false;
                    request.Requestaction.Add(raNext);
                }

                entity.Request = request;

                await _db.RepairItemRequests.AddAsync(entity);
            }
            else
            {

                var dataimageDB = _db.RepairItemRequestImage.Where(m => m.RepairItemRequestId == entity.Id
                            && entity.RepairItemRequestImage.Any(t => t.Id != m.Id));


                _db.RepairItemRequestImage.RemoveRange(dataimageDB);


                _db.RepairItemRequests.Update(entity);
                request = _db.RequestFlow.Single(m => m.Requestid ==
                       entity.RequestId);
            }


            await _db.SaveChangesAsync();

            if (submit == true)
            {
                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.RequesterId
                    });
            }

            return entity;
        }

        private async Task<string> GenerateNo(string prFix)
        {
            var latestNo = await _db.RepairItemRequests.OrderByDescending(m => m.RequestNo).
              FirstOrDefaultAsync(m => m.RequestNo.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.RequestNo.Substring(latestNo.RequestNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }

        public async Task<RepairItemRequestAccountablity> SaveAccountablity(RepairItemRequestAccountablity entity, bool submit)
        {

            entity.CreateDate = DateTime.Now;

            if (_db.RepairItemRequestAccountablity.Any(m => m.RepairItemRequestId == entity.RepairItemRequestId))
            {
                _db.RepairItemRequestAccountablity.Update(entity);

            }
            else
            {
                await _db.AddAsync(entity);
            }
            try
            {
                await _db.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString(), ex.InnerException);
            }




            if (submit == true)
            {
                var idreq = await _db.RepairItemRequests.SingleOrDefaultAsync(m => m.Id == entity.RepairItemRequestId);

                var request = await _db.RequestFlow
                   .SingleAsync(m => m.Requestid == idreq.RequestId);

                var requestFlowUrl = new RequestFlowUrl()
                {
                    Url = "repair/confirm/dashboard/request/",
                    Type = "confirm"
                };

                request.RequestFlowUrl.Add(requestFlowUrl);
                _db.RequestFlow.Update(request);
                await _db.SaveChangesAsync();

                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next,
                    new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.PicId
                    });
            }
            return entity;
        }

        public async Task<RepairItemRequestDTO> GetApprovalId(int id)
        {

            return await _db.RepairItemRequests.Where(m => m.RequestId == id)
                .Select(m => new RepairItemRequestDTO()
                {
                    Description = m.Description,
                    Id = m.Id,
                    RepairItemId = m.RepairItemId,
                    RepairItemName = m.RepairItem.Name,
                    RepairItemType = m.RepairItem.RepairType,
                    RepairItemTypeIT = m.RepairItem.ItItem,
                    RequesterId = m.RequesterId,
                    RequestNo = m.RequestNo,
                    RequesterName = m.Requester.Name,
                    RequestId = m.RequestId,
                    Title = m.Title,
                    Status = m.Request.Currentstate.Name,
                    Office = m.OfficeLocation.Name,
                    Region = m.OfficeLocation.Region.Name,
                    RequestDate = m.RequestDate.Value,
                    JabatanId = m.JabatanId.Value,
                    StatusType = m.Request.Currentstate.Statetype.Name,
                    OfficeLocationId = m.OfficeLocationId.Value,
                    RepairItemImage = m.RepairItemRequestImage.Select(d => new RepairItemRequestImageDTO()
                    {
                        FileUrl = d.FilePath,
                        Id = d.Id
                    }).ToList()

                }).SingleOrDefaultAsync();
        }

        public async Task<RepairItemRequestAccountabilityDTO> GetAccountabilityApprovalId(int id)
        {
            return await _db.RepairItemRequestAccountablity.Where(m => m.RepairItemRequest.RequestId == id).Select(m => new RepairItemRequestAccountabilityDTO()
            {
                CreateDate = m.CreateDate,
                PicId = m.PicId,
                PicName = m.Pic.Name,
                RepairItemRequestId = m.RepairItemRequestId,
                TotalBudgetReal = m.TotalBudgetReal,
                RepairItemRequestDate = m.RepairItemRequest.RequestDate ?? DateTime.MinValue,
                RepairItemRequestDescription = m.RepairItemRequest.Description,
                RepairItemRequestTitle = m.RepairItemRequest.Title,
                RequestId = m.RepairItemRequest.RequestId ?? 0,
                RepairItemImage = m.RepairItemRequestAccountablitiyImage.Select(d => new RepairItemRequestAccountabiltyImageDTO()
                {
                    FileUrl = d.FilePath,
                    Id = d.Id
                }).ToList()
            }).SingleOrDefaultAsync();
        }

        public async Task<Control<RepairItemRequestDTO>> GetListComplete(int start, int take, string filter, string order)
        {
            var repos = _db.RepairItemRequests.Where(m => m.RepairItemRequestAccountablity == null && m.Request.Currentstate.Statetype.Name == "Complete Editable").AsQueryable();

            int totalData = repos.Count();

            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.RepairItem.Name.ToLower().Contains(item.ToLower()));

                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<RepairItemRequestDTO>()
            {
                ListClass = await data.Select(m => new RepairItemRequestDTO
                {

                    Description = m.Description,
                    Id = m.Id,
                    RepairItemId = m.RepairItemId,
                    RequestNo = m.RequestNo,
                    RepairItemName = m.RepairItem.Name,
                    RepairItemType = m.RepairItem.RepairType,
                    RepairItemTypeIT = m.RepairItem.ItItem,
                    RequesterId = m.RequesterId,
                    RequesterName = m.Requester.Name,
                    RequestId = m.RequestId,
                    Title = m.Title,
                    Office = m.OfficeLocation.Name,
                    Region = m.OfficeLocation.Region.Name,
                    RequestDate = m.RequestDate.Value,
                    Status = m.Request.Currentstate.Name,
                    StatusType = m.Request.Currentstate.Statetype.Name,
                    JabatanId = m.JabatanId.Value,
                    OfficeLocationId = m.OfficeLocationId.Value,
                    RepairItemImage = m.RepairItemRequestImage.Select(d => new RepairItemRequestImageDTO()
                    {
                        FileUrl = d.FilePath,
                        Id = d.Id
                    }).ToList()

                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }
    }
}
