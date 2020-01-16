using E.Proc.Resource.Data.Interface;
using E.Proc.Resource.Data.Interface.DTO;
using E.Proc.Resource.Data.List;
using E.Proc.Resource.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Service
{
    public class DepartmentService : IDepartmentService
    {
        private GeiContext db;

        public DepartmentService(GeiContext db)
        {
            this.db = db;
        }

        public Control<Departemen> Get(int start, int take, string filter, string order)
        {
            string[] split = filter.ToLower().Split(' ');

            var repos = db.Departemen.AsQueryable();
            var totalData = repos.Count();
            var totalFilter = repos.Count();
            if (filter != "")
            {
                foreach (var item in split)
                {
                    repos = repos.Where(m => m.DepartemenNama.ToLower().Contains(item.ToLower()) ||
                    m.Keterangan.ToLower().Contains(item.ToLower()));
                    totalFilter = repos.Count();
                }
            }

            if (order != "")
            {
                repos = repos.OrderBy(order);

            }

            return new Control<Departemen>()
            {
                ListClass = repos.Skip(start * take).Take(take).ToList(),
                Total = totalData,
                TotalFilter = totalFilter
            };
        }

        public DepartementDTO Get(int id)
        {

            var repos = db.DepartmentAkun.Include(m => m.Department).Select(m => new DepartementDTO() {
                DepartemenNama = m.Department.DepartemenNama,
                Kantor = m.Department.Kantor,
                DepartemenId = m.Department.DepartemenId,
                NamaAlur = m.Department.NamaAlur,
                AlurApproval = m.Department.AlurApproval,
                FlagActive = m.Department.FlagActive,
                Keterangan = m.Department.Keterangan,
                Satker = m.Department.Satker,
                kodePusatBiaya = m.MasterAkun.IdMasterAkun
            }).Single(m => m.DepartemenId == id);
            return repos;
        }

        public IEnumerable<DepartmentAkunDTO> GetDepartmentAkun(int departmentId)
        {
            var repos = from da in db.DepartmentAkun
                        join d in db.Departemen on da.Departmentid equals d.DepartemenId
                        join pa in db.MasterAkun on da.Idmasterakun equals pa.IdMasterAkun
                        join ca in db.MasterAkun on pa.IdMasterAkun equals ca.Parent
                        select new DepartmentAkunDTO()
                        {
                            DepartemenNama = d.DepartemenNama,
                            Departmentid = d.DepartemenId,
                            Idmasterakun = pa.IdMasterAkun,
                            Iddepartmentakun = da.Iddepartmentakun,
                            ChildAkun = ca.IdMasterAkun,
                            ChildNamaAkun = ca.NamaMasterAkun,
                            Parent = pa.IdMasterAkun,
                            ParentNamaAkun = pa.NamaMasterAkun,
                            KodeDepartment = ""
                        };


            return repos;
        }
    }
}
