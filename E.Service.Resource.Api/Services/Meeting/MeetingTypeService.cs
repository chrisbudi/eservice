using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class MeetingTypeService : IMeetingTypeService
    {
        private EservicesdbContext db;

        public MeetingTypeService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<MeetingTypeDTO>> Get(int start, int take, string filter, string order,bool showActive)
        {
           
            var repos = db.MeetingTypes.AsQueryable();
            if (!showActive)
            {
                repos = repos.Where(m => m.Active == true);
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
            {
                repos = repos.OrderBy(order);
            }

            return new Control<MeetingTypeDTO>
            {
                ListClass = await repos.Skip(start * take).Take(take).
                Select(m => new MeetingTypeDTO
                {
                    Description = m.Description,
                    Id = m.Id,
                    Name = m.Name,
                    Active = m.Active
                }).ToListAsync(),

                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<MeetingTypes> Get(int id)
        {
            return await db.MeetingTypes.SingleOrDefaultAsync(m => m.Id == id);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'MeetingTypeService.Save(MeetingTypes)'
        public async Task<MeetingTypes> Save(MeetingTypes entity)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'MeetingTypeService.Save(MeetingTypes)'
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await db.MeetingTypes.AddAsync(entity);
            }
            else
            {
                db.MeetingTypes.Update(entity).Property(m => m.CreatedAt).IsModified = false;
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
