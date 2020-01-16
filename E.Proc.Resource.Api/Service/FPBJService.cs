using E.Proc.Resource.Data.Interface;
using E.Proc.Resource.Data.Interface.DTO;
using E.Proc.Resource.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Service
{
    public class fpbjService : IFPBJService
    {
        private GeiContext db;

        public fpbjService (GeiContext db)
        {
            this.db = db;
        }

        public IEnumerable<FpbjStatusAnggaran> Get(int start, int take, string filter)
        {
            var repos = db.FpbjStatusAnggaran.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string[] split = filter.ToLower().Split(' ');
                foreach (var item in split)
                {

                }
            }

            return repos.Skip(start * take).Take(take);
        }

        public FpbjStatusAnggaran Get(int id)
        {
            return db.FpbjStatusAnggaran.Single(m => m.IdStatusAnggaran == id);
        }

        public FpbjStatusAnggaran Save(FpbjStatusAnggaran entity)
        {

            if (entity.IdStatusAnggaran == 0)
            {
                entity.CreateDate = DateTime.Now;
                db.FpbjStatusAnggaran.Add(entity);
            }
            else
            {
                db.FpbjStatusAnggaran.Update(entity)
                    .Property(m => m.CreateDate).IsModified = false;
            }
            db.SaveChanges();
            return entity;
        }
    }
}
