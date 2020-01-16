using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class MeetingRoomService : IMeetingRoomService
    {
        private EservicesdbContext db;

        public MeetingRoomService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<MeetingRoomsDTO>> Get(int start, int take, string filter, string order,bool showActive)
        {

            var repos = db.MeetingRooms.AsQueryable();
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

            return new Control<MeetingRoomsDTO>
            {
                ListClass = await repos.Skip(start * take).Take(take).
                Select(m => new MeetingRoomsDTO
                {
                    Description = m.Description,
                    Id = m.Id,
                    Name = m.Name,
                    PicId = m.PicId,
                    PersonPICName = m.Pic.Name,
                    RoomId = m.RoomId,
                    RoomName = m.Room.Name,
                    RoomCategoryId = m.RoomCategoryId,
                    RoomCategoryName = m.RoomCategory.Name,
                    Active = m.Active
                }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };

        }

        public async Task<MeetingRoomsDTO> Get(int id)
        {
            return await db.MeetingRooms.Select(m => new MeetingRoomsDTO()
            {
                Description = m.Description,
                Id = m.Id,
                Name = m.Name,
                PicId = m.PicId,
                PersonPICName = m.Pic.Name,
                RoomId = m.RoomId,
                RoomName = m.Room.Name,
                RoomCategoryId = m.RoomCategoryId,
                RoomCategoryName = m.RoomCategory.Name,
                Active = m.Active
            }).SingleOrDefaultAsync(m => m.Id == id);
        }
        public async Task<MeetingRooms> Save(MeetingRooms entity)
        {
            if (entity.Id == 0)
            {
                entity.CreatedAt = DateTime.Now;
                await db.MeetingRooms.AddAsync(entity);
            }
            else
            {
                entity.UpdatedAt = DateTime.Now;
                db.MeetingRooms.Update(entity)
                    .Property(m => m.CreatedAt).IsModified = false;
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
