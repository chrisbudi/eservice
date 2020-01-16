using E.Proc.Resource.Data.Interface;
using E.Proc.Resource.Data.Interface.DTO;
using E.Proc.Resource.Data.List;
using E.Proc.Resource.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Service
{
    public class AkunService : IAkunService
    {
        private GeiContext db;

        public AkunService(GeiContext db)
        {
            this.db = db;
        }

        public Control<MasterAkun> Get(int start, int take, string filter, string order)

        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.MasterAkun.AsQueryable();

            repos = repos.Where(m => m.Akun == 2);
            var totalData = repos.Count();
            var totalFilter = repos.Count();
            if (filter != "")
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.NamaMasterAkun.ToLower().Contains(item.ToLower()));
                    totalFilter = repos.Count();
                }
            }

            if (order != "")
            {
                repos = repos.OrderBy(order);

            }


            return new Control<MasterAkun>()
            {
                ListClass = repos.Skip(start * take).Take(take).ToList(),
                Total = totalData,
                TotalFilter = totalFilter
            }; 
        }


        public IEnumerable<AkunDTO> GetAkunDepartment(int departmentId = 0)
        {
            var datarepos = from pa in db.MasterAkun
                            join ca in db.MasterAkun on pa.IdMasterAkun equals ca.Parent
                            where pa.DepartmentAkun.Any(m => m.Departmentid == departmentId)
                            select new AkunDTO()
                            {
                                ChildAkun = ca.IdMasterAkun,
                                ChildNamaAkun = ca.NamaMasterAkun,
                                ParentAkun = pa.IdMasterAkun,
                                ParentNamaAkun = pa.NamaMasterAkun
                            };
            return datarepos;
        }

        public IEnumerable<AkunDTO> Get(int id)
        {
            var datarepos = from pa in db.MasterAkun
                            join ca in db.MasterAkun on pa.IdMasterAkun equals ca.Parent
                            where pa.IdMasterAkun == id
                            select new AkunDTO()
                            {
                                ChildAkun = ca.IdMasterAkun,
                                ChildNamaAkun = ca.NamaMasterAkun,
                                ParentAkun = pa.IdMasterAkun,
                                ParentNamaAkun = pa.NamaMasterAkun
                            };

            return datarepos;
        }

        public IEnumerable<MasterAkun> GetKodeAkun(int start, int take, string filter)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.MasterAkun.AsQueryable();

            int[] kodeMasterAkun = { 90600, 90500, 80102 };

            repos = repos.Where(m => m.Parent == 0 && m.Akun == 2 && kodeMasterAkun.Contains(m.IdMasterAkun));

            if (filter != "")
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.NamaMasterAkun.ToLower().Contains(item.ToLower()));
                }
            }


            return repos
                .Skip(start * take).Take(take);
        }

        public IEnumerable<MasterAkun> GetKodeAkunBiaya(int start, int take, string filter)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.MasterAkun.AsQueryable();


            repos = repos.Where(m => m.DepartemenId != null);

            if (filter != "")
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.NamaMasterAkun.ToLower().Contains(item.ToLower()));
                }
            }


            return repos
                .Skip(start * take).Take(take);
        }

        public IEnumerable<MasterAkun> GetKodeAkunElement(int start, int take, string filter)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.MasterAkun.AsQueryable();


            repos = repos.Where(m => m.Parent != 0 && m.Akun == 1);

            if (filter != "")
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.NamaMasterAkun.ToLower().Contains(item.ToLower()));
                }
            }


            return repos
                .Skip(start * take).Take(take);
        }

        public MstAnggaranGagas GetBiaya(string masterAkun, string biayaEL, int tahun)
        {
            return db.MstAnggaranGagas.Single(m =>
            m.PusatBiaya == masterAkun && m.ElBiaya == biayaEL && m.Tahun == tahun);
        }

        public MasterAkun GetKodeELementBiaya(int id)
        {
            var data = (from p in db.MasterAkun
                        join q in db.MstAnggaranGagas on p.IdMasterAkun equals Convert.ToInt32(q.ElBiaya)
                       where q.AnggaranId == id
                       select p).SingleOrDefault();
            return data;


        }

        public MasterAkun GetKodeAkunId(int id)
        {
            var data = (from p in db.MasterAkun
                        join q in db.MstAnggaranGagas on p.IdMasterAkun equals Convert.ToInt32(q.KodeAkun)
                        where q.AnggaranId == id
                        select p).SingleOrDefault();
            return data;
        }
    }
}
