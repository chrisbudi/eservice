using E.Proc.Resource.Data.Interface;
using E.Proc.Resource.Data.List;
using E.Proc.Resource.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Service
{
    public class RekananService : IRekananService
    {
        private GeiContext db;

        public RekananService(GeiContext db)
        {
            this.db = db;
        }


        public Control<Rekanan> Get(int start, int take, string filter, string order)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.Rekanan.AsQueryable();
            var totalData = repos.Count();
            var totalFilter = repos.Count();
            if (filter != "")
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.NamaPerusahaan.ToLower().Contains(item.ToLower()) ||
                    m.Keterangan.ToLower().Contains(item.ToLower()));
                    totalFilter = repos.Count();
                }
            }

            if (!string.IsNullOrEmpty(order))
            {
                repos = repos.OrderBy(order);
            }

            return new Control<Rekanan>()
            {
                ListClass = repos.Skip(start * take).Take(take).ToList(),
                Total = totalData,
                TotalFilter = totalFilter
            };
        }

        public Rekanan Get(int id)
        {
            return db.Rekanan.Single(m => m.RekananId == id);
        }
    }
}
