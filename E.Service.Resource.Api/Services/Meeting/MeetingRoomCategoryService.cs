using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class MeetingRoomCategoryService : IMeetingRoomCategoryService
    {
        private EservicesdbContext db;

        public MeetingRoomCategoryService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<MeetingRoomCategoryDTO>> Get(int start, int take , string filter, string order,bool showActive)
        {
            var repos = db.MeetingRoomsCategory.AsQueryable();
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

            return new Control<MeetingRoomCategoryDTO>()
            {
                ListClass = await repos.Skip(start * take).Take(take).
                        Select(m => new MeetingRoomCategoryDTO
                        {
                            Description = m.Description,
                            Id = m.Id,
                            Name = m.Name,
                            MaxPerson = m.MaxPerson,
                            MinPerson = m.MinPerson,
                            Active = m.Active
                        }).ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
                
        }

        public async Task<MeetingRoomsCategory> Get(int id)
        {
            return await db.MeetingRoomsCategory.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IList<MeetingRoomsCategory>> GetCategoryPerson(int total)
        {
            return await db.MeetingRoomsCategory
                .Where(m => m.MinPerson <= total && m.MaxPerson >= total && m.Active == true).ToListAsync();
        }

        public async Task<Control<MeetingRoomsDTO>> GetCategoryRoom(int start, int take, string filter, string order, 
            int jumlahpeserta, int idLocation)
        {
            var repos = db.MeetingRooms.Where(m => 
                m.RoomCategory.MaxPerson >= jumlahpeserta && 
                m.RoomCategory.MinPerson <= jumlahpeserta && 
                m.Room.OfficeLocationId == idLocation &&
                m.Active == true && m.Room.Active == true).AsQueryable();

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

            return new Control<MeetingRoomsDTO>()
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

        public async Task<MeetingRoomsCategory> Save(MeetingRoomsCategory entity)
        {
            if (entity.Id == 0)
            {
                await db.MeetingRoomsCategory.AddAsync(entity);
            }
            else
            {
                db.MeetingRoomsCategory.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
