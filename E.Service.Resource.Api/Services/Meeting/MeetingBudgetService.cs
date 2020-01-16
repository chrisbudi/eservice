using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Meeting
{
    public class MeetingBudgetService : IMeetingBudgetService
    {

        private EservicesdbContext db;

        public MeetingBudgetService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<Control<MeetingBudget>> Get(int start, int take, string filter, string order,bool showActive)
        {

            var repos = db.MeetingBudget.AsQueryable();
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
                repos = repos.OrderBy(order);

            var data = repos.Skip(start * take).Take(take);
            return new Control<MeetingBudget>()
            {
                ListClass = await data.ToListAsync(),
                Total = totalData,
                TotalFilter = totalFilterData
            };
        }

        public async Task<MeetingBudget> Get(int id)
        {
            return await db.MeetingBudget.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MeetingBudget> Save(MeetingBudget entity)
        {
            if (entity.Id == 0)
            {
                await db.MeetingBudget.AddAsync(entity);
            }
            else
            {
                db.MeetingBudget.Update(entity);
            }
            await db.SaveChangesAsync();
            return entity;

        }
    }
}
