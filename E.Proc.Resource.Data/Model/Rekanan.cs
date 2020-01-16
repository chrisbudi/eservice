using System;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Model
{
    public partial class Rekanan
    {
        public int RekananId { get; set; }
        public string NamaPerusahaan { get; set; }
        public int? JenisPerusahaan { get; set; }
        public string Npwp { get; set; }
        public string Alamat { get; set; }
        public string Telepon { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Kodepos { get; set; }
        public bool? KantorCabang { get; set; }
        public bool? FlagBlacklist { get; set; }
        public bool? FlagActive { get; set; }
        public string Username { get; set; }
        public string Pswd { get; set; }
        public int? ProvinsiId { get; set; }
        public int? KotaId { get; set; }
        public int? UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public int? IdBidangUsaha { get; set; }
        public int? IdSubBidang { get; set; }
        public bool? IsActivated { get; set; }
        public string EmailPribadi { get; set; }
        public string NoPonsel { get; set; }
        public bool? IsVerified { get; set; }
        public string Keterangan { get; set; }
        public string CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string NamaPj { get; set; }
        public string JabatanPj { get; set; }
        public string Pkp { get; set; }
        public string NpwpImgurl { get; set; }
        public string PkpImgurl { get; set; }
        public string DokumenAgreement { get; set; }
        public string AlasanBlacklist { get; set; }
        public string MasaBlacklist { get; set; }
        public string FileBlacklist { get; set; }
        public string TglBlacklist { get; set; }
        public string MasaawalBlacklist { get; set; }
        public string MasaakhirBlacklist { get; set; }
        public bool? TriggerVerified { get; set; }
        public bool? BisaMengubahData { get; set; }
        public bool? MintaUbahData { get; set; }
        public string AlasanUbahData { get; set; }
        public string AwalUbahData { get; set; }
        public string AkhirUbahData { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAccountName { get; set; }
        public string KodeRekanan { get; set; }
        public int? IsRegistered { get; set; }
        public bool? PaktaintegritasAgreement { get; set; }
        public bool? PernyataanminatAgreement { get; set; }
        public string TanggalSetuju { get; set; }
        public string ActivatedDate { get; set; }
        public string VerifiedDate { get; set; }
        public string VerifiedSendDate { get; set; }
        public string TolakDate { get; set; }
        public bool? TolakVerifikasi { get; set; }
        public string UrlSertifikat { get; set; }
    }
}