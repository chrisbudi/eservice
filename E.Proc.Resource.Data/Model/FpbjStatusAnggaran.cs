using System;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Model
{
    public partial class FpbjStatusAnggaran
    {
        public int IdStatusAnggaran { get; set; }
        public string NoFpbj { get; set; }
        public double? Booked { get; set; }
        public bool? Status { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public int PicPengaju { get; set; }
        public decimal? Tahun { get; set; }
        public bool? FlagActive { get; set; }
        public double? FundAvailable { get; set; }
        public double? Hps { get; set; }
        public double? SelisihBooked { get; set; }
        public double? FinalBooked { get; set; }
        public string NoAccount { get; set; }
    }
}