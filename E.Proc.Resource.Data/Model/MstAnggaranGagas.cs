using System;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Model
{
    public partial class MstAnggaranGagas
    {
        public int AnggaranId { get; set; }
        public int? TipeAngg { get; set; }
        public string KodeOrg { get; set; }
        public string KodeAkun { get; set; }
        public string PusatBiaya { get; set; }
        public string ElBiaya { get; set; }
        public decimal? FundAvailable { get; set; }
        public bool? FlagActive { get; set; }
        public string KodeAnggaran { get; set; }
        public string NamaOrg { get; set; }
        public string NamaAkun { get; set; }
        public string NamaPbiaya { get; set; }
        public string NamaElbiaya { get; set; }
        public string AkunCadangan1 { get; set; }
        public string AkunCadangan2 { get; set; }
        public decimal? NomorPaket { get; set; }
        public string JudulPaket { get; set; }
        public decimal? Tahun { get; set; }
        public decimal? Booked { get; set; }
        public decimal? Release { get; set; }
        public decimal? Budget { get; set; }
    }
}