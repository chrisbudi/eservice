using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Report.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Asset
{
    public class AssetService : IAssetService
    {
        private EservicesdbContext db;

        IRequestService _requestService;
        IUserService _userService;

        public AssetService(EservicesdbContext db, IRequestService requestService, IUserService userService)
        {
            this.db = db;
            _requestService = requestService;
            _userService = userService;
        }

        public async Task<Control<AssetDTO>> Get(int start, int take, string filter, string order,
            bool showActive, bool showComplete = false,
            EAssetTypeService assetTypeService = EAssetTypeService.Current, bool borrow = false)
        {
            var repos = db.Assets.AsQueryable();
            if (!showActive)
            {
                //repos = repos.Where(m => m.Budget.Active == true);
            }


            var complete = EAssetRequest.COMPLETE.Description();

            if (borrow)
            {
                repos = repos.Where(m => m.CanBorrow == true);
                repos = repos.Where(m => m.AssetRequests.
                  Any(r => r.Request.Currentstate.Name == complete && r.Depreciated == false));
            }

            int totalData = repos.Count();
            int totalFilterData = totalData;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Name.ToLower().Contains(item.ToLower()));
                    totalFilterData = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
                repos = repos.OrderBy(order);


            if (showComplete)
            {
                repos = repos.Where(m => m.AssetRequests.
                    Any(r => r.Request.Currentstate.Name == complete && r.Depreciated == false));
            }

            var assetData = new List<AssetDTO>();

            if (assetTypeService == EAssetTypeService.AssetAdd)
            {
                assetData = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        AssetNumber = m.AssetNumber,
                        Barcode = m.Barcode,
                        BrandSeriesId = m.BrandSeriesId,
                        BrandSeriesName = m.BrandSeries.Name,
                        CanBorrow = m.CanBorrow,
                        GroupId = m.GroupId,
                        GroupName = m.Group.Name,
                        PriceAcquired = m.PriceAcquired,
                        RunningNumber = m.RunningNumber,
                        SerialNumber = m.SerialNumber,
                        VendorId = m.VendorId,
                        YearAcquired = m.YearAcquired,
                        AssetTransactionDTO = m.AssetRequests.Select(a => new AssetTransactionDTO()
                        {
                            AssetConditionId = a.AssetConditionId,
                            OfficeRoomName = a.OfficeRoom.Name,
                            OfficeRoomId = a.OfficeRoomId,
                            Requestdate = a.RequestDate,
                            AssetName = m.Name,
                            AssetId = m.Id,
                            AssetConditionName = a.AssetCondition.JenisNama,
                            Description = a.Description,
                            Title = a.Title,
                            RequesterId = a.RequesterId,
                            Status = a.Request.Currentstate.Name,
                            TypeId = a.TypeId,
                            TypeName = a.Type.Name,
                            OrganizationId = a.DepartementId,
                            RegionId = a.RegionalId ?? 0,
                            ProcessName = a.Request.Process.Nama,
                            RequestNo = a.RequestNo,
                            Id = a.Id,
                            Depreciated = a.Depreciated,
                            RequestId = a.RequestId ?? 0,
                            OwnerName = a.Asset.AssetOwner.JenisNama,
                            OwnerId = a.Asset.AssetOwnerId
                        }).SingleOrDefault(sa => sa.ProcessName ==
                        ERequestType.AssetAdd.Description())
                    }).ToListAsync();

            }
            else
            {
                assetData = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetDTO
                    {

                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        AssetNumber = m.AssetNumber,
                        Barcode = m.Barcode,
                        BrandSeriesId = m.BrandSeriesId,
                        BrandSeriesName = m.BrandSeries.Name,
                        CanBorrow = m.CanBorrow,
                        GroupId = m.GroupId,
                        GroupName = m.Group.Name,
                        PriceAcquired = m.PriceAcquired,
                        RunningNumber = m.RunningNumber,
                        SerialNumber = m.SerialNumber,
                        VendorId = m.VendorId,
                        YearAcquired = m.YearAcquired,
                        AssetTransactionDTO = m.AssetRequests.Select(a => new AssetTransactionDTO()
                        {
                            AssetConditionId = a.AssetConditionId,
                            OfficeRoomName = a.OfficeRoom.Name,
                            OfficeRoomId = a.OfficeRoomId,
                            Requestdate = a.RequestDate,
                            AssetName = m.Name,
                            AssetId = m.Id,
                            AssetConditionName = a.AssetCondition.JenisNama,
                            Description = a.Description,
                            Title = a.Title,
                            RequesterId = a.RequesterId,
                            Status = a.Request.Currentstate.Name,
                            TypeId = a.TypeId,
                            TypeName = a.Type.Name,
                            OrganizationId = a.DepartementId,
                            RegionId = a.RegionalId ?? 0,
                            ProcessName = a.Request.Process.Nama,
                            Id = a.Id,
                            RequestNo = a.RequestNo,
                            Depreciated = a.Depreciated,
                            RequestId = a.RequestId ?? 0,
                            OwnerName = a.Asset.AssetOwner.JenisNama,
                            OwnerId = a.Asset.AssetOwnerId
                        }).SingleOrDefault(sa =>
                        sa.Status == EAssetRequest.COMPLETE.Description() &&
                        sa.Depreciated == false)
                    }).ToListAsync();
            }



            return new Control<AssetDTO>()
            {
                ListClass = assetData,
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetDTO> GetBarcode(string barcode, EAssetTypeService assetTypeService = EAssetTypeService.Current)
        {
            var assetData = await db.Assets.Select(m => new AssetDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                AssetNumber = m.AssetNumber,
                Barcode = m.Barcode,
                BrandSeriesId = m.BrandSeriesId,
                BrandSeriesName = m.BrandSeries.Name,
                CanBorrow = m.CanBorrow,
                GroupId = m.GroupId,
                GroupName = m.Group.Name,
                PriceAcquired = m.PriceAcquired,
                RunningNumber = m.RunningNumber,
                SerialNumber = m.SerialNumber,
                VendorId = m.VendorId,
                YearAcquired = m.YearAcquired,

            }).SingleOrDefaultAsync(m => m.Barcode == barcode || m.AssetNumber == barcode);

            var assetRequestData = db.AssetRequests.Where(m => m.Asset.Barcode == barcode || m.Asset.AssetNumber == barcode)
                .Select(a => new AssetTransactionDTO()
                {
                    AssetConditionId = a.AssetConditionId,
                    OfficeRoomName = a.OfficeRoom.Name,
                    OfficeRoomId = a.OfficeRoomId,
                    Requestdate = a.RequestDate,
                    AssetName = a.Asset.Name,
                    AssetConditionName = a.AssetCondition.JenisNama,
                    Description = a.Description,
                    Title = a.Title,
                    RequesterId = a.RequesterId,
                    Status = a.Request.Currentstate.Name,
                    TypeId = a.TypeId,
                    TypeName = a.Type.Name,
                    OrganizationId = a.DepartementId,
                    RegionId = a.RegionalId ?? 0,
                    Id = a.Id,
                    AssetId = a.Asset.Id,
                    Depreciated = a.Depreciated,
                    RequestId = a.RequestId ?? 0,
                    OwnerName = a.Asset.AssetOwner.JenisNama,
                    OwnerId = a.Asset.AssetOwnerId
                });

            if (assetTypeService == EAssetTypeService.AssetAdd)
            {
                assetData.AssetTransactionDTO = assetRequestData.
                    SingleOrDefault(m => m.ProcessName == ERequestType.AssetAdd.Description());
            }
            else
            {
                assetData.AssetTransactionDTO = assetRequestData.
                    SingleOrDefault(m => m.Status == EAssetRequest.COMPLETE.Description()
                    && m.Depreciated == false);
            }


            return assetData;

        }

        public async Task<AssetDTO> Get(int id, EAssetTypeService assetTypeService = EAssetTypeService.Current)
        {

            var assetData = await db.Assets.Select(m => new AssetDTO()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                AssetNumber = m.AssetNumber,
                Barcode = m.Barcode,
                BrandSeriesId = m.BrandSeriesId,
                BrandSeriesName = m.BrandSeries.Name,
                CanBorrow = m.CanBorrow,
                GroupId = m.GroupId,
                GroupName = m.Group.Name,
                PriceAcquired = m.PriceAcquired,
                RunningNumber = m.RunningNumber,
                SerialNumber = m.SerialNumber,
                VendorId = m.VendorId,
                YearAcquired = m.YearAcquired,

            }).SingleOrDefaultAsync(m => m.Id == id);

            var assetRequestData = db.AssetRequests.Where(m => m.AssetId == id)
                .Select(a => new AssetTransactionDTO()
                {
                    AssetConditionId = a.AssetConditionId,
                    OfficeRoomName = a.OfficeRoom.Name,
                    OfficeRoomId = a.OfficeRoomId,
                    Requestdate = a.RequestDate,
                    AssetName = a.Asset.Name,
                    AssetConditionName = a.AssetCondition.JenisNama,
                    Description = a.Description,
                    Title = a.Title,
                    RequesterId = a.RequesterId,
                    Status = a.Request.Currentstate.Name,
                    TypeId = a.TypeId,
                    TypeName = a.Type.Name,
                    OrganizationId = a.DepartementId,
                    RegionId = a.RegionalId ?? 0,
                    Id = a.Id,
                    AssetId = a.Asset.Id,
                    Depreciated = a.Depreciated,
                    ProcessName = a.Request.Process.Nama,
                    RequestNo = a.RequestNo,
                    RequestId = a.RequestId ?? 0,
                    OwnerName = a.Asset.AssetOwner.JenisNama,
                    OwnerId = a.Asset.AssetOwnerId
                });

            if (assetTypeService == EAssetTypeService.AssetAdd)
            {
                assetData.AssetTransactionDTO = assetRequestData.
                    SingleOrDefault(m => m.ProcessName == ERequestType.AssetAdd.Description());
            }
            else
            {
                assetData.AssetTransactionDTO = assetRequestData.
                    SingleOrDefault(m => m.Status == EAssetRequest.COMPLETE.Description() &&
                    m.Depreciated == false);
            }


            return assetData;


        }

        public async Task<Control<AssetTransactionDTO>> GetHistory(int id, int start, int take, string filter, string order)
        {
            var repos = db.AssetRequests.Where(m => m.AssetId == id);

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
            {
                repos = repos.OrderBy(order);
            }

            return new Control<AssetTransactionDTO>()
            {

                ListClass = await repos.Skip(start * take).Take(take).
                    Select(m => new AssetTransactionDTO()
                    {
                        Id = m.Id,
                        AssetConditionId = m.AssetConditionId,
                        AssetConditionName = m.AssetCondition.JenisNama,
                        AssetId = m.AssetId,
                        AssetName = m.Asset.Name,
                        Description = m.Description,
                        OfficeRoomId = m.OfficeRoomId,
                        OfficeRoomName = m.OfficeRoom.Name,
                        Requestdate = m.RequestDate,
                        RequesterId = m.RequesterId,
                        Title = m.Title,
                        TypeId = m.Type.Id,
                        TypeName = m.Type.Name,
                        OrganizationId = m.DepartementId,
                        RegionId = m.RegionalId.Value,
                        RegionName = m.Regional.Name,
                        Status = m.Request.Currentstate.Name,
                        RequestId = m.RequestId.Value,
                        ProcessName = m.Request.Process.Nama,
                        RequestNo = m.RequestNo,
                        OwnerName = m.Asset.AssetOwner.JenisNama,
                        OwnerId = m.Asset.AssetOwnerId
                    }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<AssetTransactionDTO> GetHistoryId(int histId)
        {
            return await db.AssetRequests.Where(m => m.Id == histId)
                 .Select(m => new AssetTransactionDTO()
                 {
                     Id = m.Id,
                     AssetConditionId = m.AssetConditionId,
                     AssetConditionName = m.AssetCondition.JenisNama,
                     AssetId = m.AssetId,
                     AssetName = m.Asset.Name,
                     Description = m.Description,
                     OfficeRoomId = m.OfficeRoomId,
                     OfficeRoomName = m.OfficeRoom.Name,
                     Requestdate = m.RequestDate,
                     RequesterId = m.RequesterId,
                     Title = m.Title,
                     TypeId = m.Type.Id,
                     TypeName = m.Type.Name,
                     OrganizationId = m.DepartementId,
                     RegionId = m.RegionalId.Value,
                     RegionName = m.Regional.Name,
                     Status = m.Request.Currentstate.Name,
                     RequestId = m.RequestId.Value,
                     ProcessName = m.Request.Process.Nama,
                     RequestNo = m.RequestNo,
                     OwnerName = m.Asset.AssetOwner.JenisNama,
                     OwnerId = m.Asset.AssetOwnerId
                 }).SingleAsync();
        }

        public async Task<AssetInsertDTO> Save(AssetInsertDTO entity, bool submit, EAssetRequestType requestType)
        {
            var request = new RequestFlow();

            if (entity.Assets.Id == 0)
            {
                var user = await _userService.GetUserById(entity.AssetRequest.RequesterId);
                entity.Assets.CreatedAt = DateTime.Now;


                var prFixrequest = $"02{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.AssetRequest.RequestNo = await GenerateNo(prFixrequest);


                var subGroup = db.AssetSubGroupTypes.Include(m => m.MainGroup).AsNoTracking().Single(m => m.Id == entity.Assets.GroupId);

                var prFixNumber = $"{subGroup.MainGroup.Kode}{subGroup.Kode}{entity.Assets.YearAcquired.ToString().Substring(entity.Assets.YearAcquired.ToString().Length - 2)}";
                entity.Assets.AssetNumber = await GenerateAssetNo(prFixNumber);
                entity.Assets.Barcode = entity.Assets.AssetNumber;

                entity.AssetRequest.RequestDate = DateTime.Now;

                string url = "";

                if (requestType == EAssetRequestType.REQADD)
                {
                    entity.AssetRequest.Status = EAssetRequestType.REQADD.Description();
                    url = "asset/add/dashboard/request/";
                }
                if (requestType == EAssetRequestType.REQCHANGE)
                {
                    entity.AssetRequest.Status = EAssetRequestType.REQCHANGE.Description();
                    url = "asset/change/dashboard/request/";
                }
                if (requestType == EAssetRequestType.REQEDIT)
                {
                    entity.AssetRequest.Status = EAssetRequestType.REQEDIT.Description();
                    url = "asset/edit/dashboard/request/";
                }

                entity.Assets.AssetRequests.Add(entity.AssetRequest);

                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(ERequestType.AssetAdd),
                    Title = entity.AssetRequest.RequestNo,
                    Note = entity.Assets.Name,
                    Processid = await _requestService.ProgressId(ERequestType.AssetAdd),
                    Userid = entity.AssetRequest.RequesterId.ToString(),
                    Url = url
                };

                var transitionList = await _requestService.
                    TransitionList(ERequestType.AssetAdd);
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
                entity.AssetRequest.Request = request;
                await db.Assets.AddAsync(entity.Assets);
            }
            else
            {
                entity.Assets.AssetRequests.Add(entity.AssetRequest);

                var propAsset = db.Assets.Update(entity.Assets);
                propAsset.Property(m => m.CreatedAt).IsModified = false;
                propAsset.Property(m => m.AssetNumber).IsModified = false;


                var prop = db.Entry(entity.AssetRequest);
                prop.Property(m => m.RequestDate).IsModified = false;
                prop.Property(m => m.RequestNo).IsModified = false;

                request = db.RequestFlow.Single(m => m.Requestid == entity.AssetRequest.RequestId);
            }

            await db.SaveChangesAsync();

            if (submit == true)
            {
                await _requestService.SetStateRequest(request.Requestid,
                    ETransitionType.Next, new RequestActionHistory()
                    {
                        Datetime = DateTime.Now,
                        HistoryType = "Approve",
                        UserId = entity.AssetRequest.RequesterId
                    });
            }

            return entity;
        }

        public async Task<AssetRequests> SaveAssetRequest(AssetRequests entity, bool submit, EAssetType assetType)
        {
            var request = new RequestFlow();

            ERequestType asset = (ERequestType)((int)assetType);
            if (entity.Id == 0)
            {
                string url = "";
                entity.RequestDate = DateTime.Now;

                var user = await _userService.GetUserById(entity.RequesterId);


                var prFixrequest = $"02{(user.DepartmentId ?? 0).ToString("00")}{user.LocationId.ToString("00")}{DateTime.Now.ToString("yMM")}";
                entity.RequestNo = await GenerateNo(prFixrequest);


                if (assetType == EAssetType.AssetChange)
                {
                    entity.Status = EAssetType.AssetChange.Description();
                    url = "asset/change/dashboard/request/";
                }

                if (assetType == EAssetType.AssetEdit)
                {
                    entity.Status = EAssetType.AssetEdit.Description();
                    url = "asset/edit/dashboard/request/";
                }


                request = new RequestFlow()
                {
                    Daterequest = DateTime.Now,
                    Currentstateid = await _requestService.BeginStateId(asset),
                    Title = entity.RequestNo,
                    Note = entity.Description,
                    Processid = await _requestService.ProgressId(asset),
                    Userid = entity.RequesterId.ToString(),
                    Url = url
                };

                var transitionList = await _requestService.
                    TransitionList(asset);
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
                await db.AssetRequests.AddAsync(entity);
            }
            else
            {
                entity.RequestChangeAt = DateTime.Now;
                db.AssetRequests.Update(entity);
                db.Entry(entity).Property(m => m.RequestDate).IsModified = false;
                db.Entry(entity).Property(m => m.RequestNo).IsModified = false;
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
                        UserId = entity.RequesterId
                    });
            }

            return entity;
        }

        private async Task<string> GenerateNo(string prFix)
        {
            var latestNo = await db.AssetRequests.OrderByDescending(m => m.RequestNo).
              FirstOrDefaultAsync(m => m.RequestNo.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.RequestNo.Substring(latestNo.RequestNo.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }

        private async Task<string> GenerateAssetNo(string prFix)
        {
            var latestNo = await db.Assets.OrderByDescending(m => m.AssetNumber).
              FirstOrDefaultAsync(m => m.AssetNumber.StartsWith(prFix));
            var noSeries = 0;
            if (latestNo != null)
                noSeries = int.Parse(latestNo.AssetNumber.Substring(latestNo.AssetNumber.Length - 4)) + 1;
            else
                noSeries = 1;

            return prFix + noSeries.ToString("0000");
        }

        public async Task<AssetDTO> GetByRequestId(int id)
        {
            var assetData = await db.AssetRequests.Where(m => m.RequestId == id)
                .Select(m => new AssetDTO()
                {
                    Id = m.Asset.Id,
                    Name = m.Asset.Name,
                    Description = m.Asset.Description,
                    AssetNumber = m.Asset.AssetNumber,
                    Barcode = m.Asset.Barcode,
                    BrandSeriesId = m.Asset.BrandSeriesId,
                    BrandSeriesName = m.Asset.BrandSeries.Name,
                    CanBorrow = m.Asset.CanBorrow,

                    PriceAcquired = m.Asset.PriceAcquired,
                    RunningNumber = m.Asset.RunningNumber,
                    SerialNumber = m.Asset.SerialNumber,
                    VendorId = m.Asset.VendorId,
                    YearAcquired = m.Asset.YearAcquired,
                    GroupId = m.Asset.Group.Id,
                    GroupName = m.Asset.Group.MainGroup.Name,
                    SubGroupId = m.Asset.Group.Id,
                    SubGroupName = m.Asset.Group.Name,
                    AssetTransactionDTO = new AssetTransactionDTO()
                    {
                        RegionId = m.RegionalId.Value,
                        AssetConditionId = m.AssetConditionId,
                        RequestId = m.RequestId.Value,
                        AssetConditionName = m.AssetCondition.JenisNama,
                        AssetId = m.AssetId,
                        AssetName = m.Asset.Name,
                        Description = m.Description,
                        Id = m.Id,
                        OfficeRoomId = m.OfficeRoomId,
                        OfficeRoomName = m.OfficeRoom.Name,
                        OrganizationId = m.DepartementId,
                        ProcessName = m.Request.Process.Nama,
                        RegionName = m.Regional.Name,
                        Requestdate = m.RequestDate,
                        RequesterId = m.RequestId.Value,
                        RequestName = m.Requester.Name,
                        Status = m.Request.Currentstate.Name,
                        Title = m.Title,
                        TypeId = m.TypeId,
                        TypeName = m.Type.Name,
                        RequestNo = m.RequestNo,
                    }
                }).SingleOrDefaultAsync();

            return assetData;
        }

        public async Task UpdateAssetRequestToDepreciated(int requestId)
        {
            var assetRequest = await db.AssetRequests.SingleAsync(m => m.RequestId == requestId);

            var stateName = await db.RequestFlow.Where(m => m.Requestid == requestId)
                .Select(m => m.Currentstate.Name)
                .SingleOrDefaultAsync();

            if (stateName == EAssetRequest.COMPLETE.Description())
            {
                var assets = await db.AssetRequests.Where(m =>
                    m.AssetId == assetRequest.AssetId &&
                    m.RequestId != requestId).ToListAsync();
                foreach (var asset in assets)
                {
                    asset.Depreciated = true;
                    db.Update(asset);
                }

                await db.SaveChangesAsync();
            }
        }




        public async Task<List<ReportAsset>> GetReportAsset(int locationId, int regionalId, string status,
            DateTime? startDate, DateTime? endDate, int departmentId,
            int merk, int type)
        {
            var repos = db.Assets.AsQueryable();

            var complete = EAssetRequest.COMPLETE.Description();


            repos = repos.Where(m => m.CanBorrow == true);
            repos = repos.Where(m => m.AssetRequests.
              Any(r => r.Request.Currentstate.Name == complete && r.Depreciated == false));



            if (locationId != 0)
            {
                repos = repos.Where(m => m.AssetRequests.Any(a => a.OfficeRoomId == locationId));
            }

            if (regionalId != 0)
            {
                repos = repos.Where(m => m.AssetRequests.Any(a => a.RegionalId == regionalId));
            }

            if (departmentId != 0)
            {
                repos = repos.Where(m => m.AssetRequests.Any(a => a.DepartementId == departmentId));
            }

            if (startDate != null && endDate != null)
            {
                repos = repos.Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate);
            }


            var dataExport = await
                (from m in repos
                 select new ReportAsset()
                 {
                     Id = m.Id,
                     Group = m.Group.MainGroup.Name,
                     SubGroup = m.Group.Name,
                     HargaBeli = m.PriceAcquired,
                     HargaSaatIni = 0,
                     Jenis = m.AssetRequests.FirstOrDefault().Type.Name,
                     Type = m.AssetRequests.FirstOrDefault().Type.Name,
                     Merk = m.BrandSeries.Name,
                     Lokasi = m.AssetRequests.FirstOrDefault().OfficeRoom.OfficeLocation.Name,
                     Wilayah = m.AssetRequests.FirstOrDefault().Regional.Name,
                     NamaHBB = m.Name,
                     NoHBB = m.AssetNumber,
                     DepartmentId = m.AssetRequests.FirstOrDefault().DepartementId.Value,
                     TanggalPembelian = m.YearAcquired.ToString(),
                 }).ToListAsync();

            return dataExport;
        }

        public async Task UpdateAssetRequestLastCancelledId(int requestId)
        {
            var assetRequest = await db.AssetRequests.SingleAsync(m => m.RequestId == requestId);


            var assets = await db.AssetRequests.Where(m =>
                m.AssetId == assetRequest.AssetId).OrderByDescending(m => m.Id).FirstOrDefaultAsync();


            assets.Depreciated = false;
            db.Update(assets);

            await db.SaveChangesAsync();

        }
    }
}
