using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface.DTO
{
    public class DepartementDTO
    {
        public int DepartemenId { get; set; }
        public string DepartemenNama { get; set; }
        public bool? FlagActive { get; set; }
        public string Keterangan { get; set; }
        public string Satker { get; set; }
        public string Kantor { get; set; }
        public string NamaAlur { get; set; }
        public string AlurApproval { get; set; }
        public int kodePusatBiaya { get; set; }


    }
}
