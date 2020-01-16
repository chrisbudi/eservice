using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services
{
    public class LogService : ILogService
    {

        private EservicesdbContext db;
        public LogService(EservicesdbContext db)
        {
            this.db = db;
        }

        public IEnumerable<ChangeLog> GetChangeLog(int start, int take, string filter)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.ChangeLog.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in split)
                {
                    //repos = repos.Where(m => m.ToLower().Contains(item.ToLower()));
                }
            }

            return repos.Skip(start * take).Take(take);
        }

        public IEnumerable<Log> GetLog(int start, int take, string filter)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.Log.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.Message.ToLower().Contains(item.ToLower()));
                }
            }

            return repos.Skip(start * take).Take(take);
        }
    }
}
