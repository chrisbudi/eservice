using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace E.Proc.Resource.Data.Model
{
    public partial class GeiContext : DbContext
    {
        public GeiContext()
        {
        }

        public GeiContext(DbContextOptions<GeiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Departemen> Departemen { get; set; }
        public virtual DbSet<DepartmentAkun> DepartmentAkun { get; set; }
        public virtual DbSet<FpbjStatusAnggaran> FpbjStatusAnggaran { get; set; }
        public virtual DbSet<MasterAkun> MasterAkun { get; set; }
        public virtual DbSet<MstAnggaranGagas> MstAnggaranGagas { get; set; }
        public virtual DbSet<Rekanan> Rekanan { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=eproc_gei;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Departemen>(entity =>
            {
                entity.ToTable("departemen");

                entity.Property(e => e.DepartemenId)
                    .HasColumnName("departemen_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AlurApproval)
                    .HasColumnName("alur_approval")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DepartemenNama)
                    .HasColumnName("departemen_nama")
                    .HasMaxLength(100);

                entity.Property(e => e.FlagActive).HasColumnName("flag_active");

                entity.Property(e => e.Kantor)
                    .HasColumnName("kantor")
                    .HasMaxLength(300);

                entity.Property(e => e.Keterangan)
                    .HasColumnName("keterangan")
                    .HasMaxLength(300);

                entity.Property(e => e.NamaAlur)
                    .HasColumnName("nama_alur")
                    .HasMaxLength(300);

                entity.Property(e => e.Satker)
                    .HasColumnName("satker")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<DepartmentAkun>(entity =>
            {
                entity.HasKey(e => e.Iddepartmentakun);

                entity.ToTable("department_akun");

                entity.Property(e => e.Iddepartmentakun).HasColumnName("iddepartmentakun");

                entity.Property(e => e.Departmentid).HasColumnName("departmentid");

                entity.Property(e => e.Idmasterakun).HasColumnName("idmasterakun");

                entity.Property(e => e.Parent).HasColumnName("parent");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DepartmentAkun)
                    .HasForeignKey(d => d.Departmentid)
                    .HasConstraintName("FK_department_akun_departemen");

                entity.HasOne(d => d.MasterAkun)
                    .WithMany(p => p.DepartmentAkun)
                    .HasForeignKey(d => new { d.Idmasterakun, d.Parent })
                    .HasConstraintName("FK_department_akun_master_akun1");
            });

            modelBuilder.Entity<FpbjStatusAnggaran>(entity =>
            {
                entity.HasKey(e => e.IdStatusAnggaran)
                    .HasName("PK_status_fpbj_anggaran");

                entity.ToTable("fpbj_status_anggaran");

                entity.Property(e => e.IdStatusAnggaran).HasColumnName("id_status_anggaran");

                entity.Property(e => e.Booked).HasColumnName("booked");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.FinalBooked).HasColumnName("final_booked");

                entity.Property(e => e.FlagActive).HasColumnName("flag_active");

                entity.Property(e => e.FundAvailable).HasColumnName("fund_available");

                entity.Property(e => e.Hps).HasColumnName("hps");

                entity.Property(e => e.NoAccount)
                    .HasColumnName("no_account")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoFpbj)
                    .IsRequired()
                    .HasColumnName("no_fpbj")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PicPengaju).HasColumnName("pic_pengaju");

                entity.Property(e => e.SelisihBooked).HasColumnName("selisih_booked");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Tahun)
                    .HasColumnName("tahun")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<MasterAkun>(entity =>
            {
                entity.HasKey(e => new { e.IdMasterAkun, e.Parent });

                entity.ToTable("master_akun");

                entity.Property(e => e.IdMasterAkun).HasColumnName("id_master_akun");

                entity.Property(e => e.Parent).HasColumnName("parent");

                entity.Property(e => e.Akun).HasColumnName("akun");

                entity.Property(e => e.CreateBy).HasColumnName("create_by");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.DepartemenId)
                    .HasColumnName("departemen_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FlagActive).HasColumnName("flag_active");

                entity.Property(e => e.NamaMasterAkun)
                    .IsRequired()
                    .HasColumnName("nama_master_akun")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MstAnggaranGagas>(entity =>
            {
                entity.HasKey(e => e.AnggaranId);

                entity.ToTable("mst_anggaran_gagas");

                entity.Property(e => e.AnggaranId).HasColumnName("anggaran_id");

                entity.Property(e => e.AkunCadangan1)
                    .HasColumnName("akun_cadangan1")
                    .HasMaxLength(10);

                entity.Property(e => e.AkunCadangan2)
                    .HasColumnName("akun_cadangan2")
                    .HasMaxLength(10);

                entity.Property(e => e.Booked)
                    .HasColumnName("booked")
                    .HasColumnType("money");

                entity.Property(e => e.Budget)
                    .HasColumnName("budget")
                    .HasColumnType("money");

                entity.Property(e => e.ElBiaya)
                    .HasColumnName("el_biaya")
                    .HasMaxLength(10);

                entity.Property(e => e.FlagActive).HasColumnName("flag_active");

                entity.Property(e => e.FundAvailable)
                    .HasColumnName("fund_available")
                    .HasColumnType("money");

                entity.Property(e => e.JudulPaket)
                    .HasColumnName("judul_paket")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KodeAkun)
                    .HasColumnName("kode_akun")
                    .HasMaxLength(10);

                entity.Property(e => e.KodeAnggaran)
                    .HasColumnName("kode_anggaran")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KodeOrg)
                    .HasColumnName("kode_org")
                    .HasMaxLength(10);

                entity.Property(e => e.NamaAkun)
                    .HasColumnName("nama_akun")
                    .HasMaxLength(100);

                entity.Property(e => e.NamaElbiaya)
                    .HasColumnName("nama_elbiaya")
                    .HasMaxLength(100);

                entity.Property(e => e.NamaOrg)
                    .HasColumnName("nama_org")
                    .HasMaxLength(100);

                entity.Property(e => e.NamaPbiaya)
                    .HasColumnName("nama_pbiaya")
                    .HasMaxLength(100);

                entity.Property(e => e.NomorPaket)
                    .HasColumnName("nomor_paket")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.PusatBiaya)
                    .HasColumnName("pusat_biaya")
                    .HasMaxLength(10);

                entity.Property(e => e.Release)
                    .HasColumnName("release")
                    .HasColumnType("money");

                entity.Property(e => e.Tahun)
                    .HasColumnName("tahun")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.TipeAngg).HasColumnName("tipe_angg");
            });

            modelBuilder.Entity<Rekanan>(entity =>
            {
                entity.ToTable("rekanan");

                entity.Property(e => e.RekananId).HasColumnName("rekanan_id");

                entity.Property(e => e.ActivatedDate)
                    .HasColumnName("activated_date")
                    .HasMaxLength(50);

                entity.Property(e => e.AkhirUbahData)
                    .HasColumnName("akhir_ubah_data")
                    .HasMaxLength(20);

                entity.Property(e => e.Alamat)
                    .HasColumnName("alamat")
                    .HasMaxLength(200);

                entity.Property(e => e.AlasanBlacklist)
                    .HasColumnName("alasan_blacklist")
                    .HasColumnType("ntext");

                entity.Property(e => e.AlasanUbahData)
                    .HasColumnName("alasan_ubah_data")
                    .HasColumnType("ntext");

                entity.Property(e => e.AwalUbahData)
                    .HasColumnName("awal_ubah_data")
                    .HasMaxLength(20);

                entity.Property(e => e.BankAccountName)
                    .HasColumnName("bank_account_name")
                    .HasMaxLength(50);

                entity.Property(e => e.BankAccountNo)
                    .HasColumnName("bank_account_no")
                    .HasMaxLength(30);

                entity.Property(e => e.BisaMengubahData)
                    .HasColumnName("bisa_mengubah_data")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasMaxLength(100);

                entity.Property(e => e.DokumenAgreement)
                    .HasColumnName("dokumen_agreement")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.EmailPribadi)
                    .HasColumnName("email_pribadi")
                    .HasMaxLength(50);

                entity.Property(e => e.Fax)
                    .HasColumnName("fax")
                    .HasMaxLength(50);

                entity.Property(e => e.FileBlacklist)
                    .HasColumnName("file_blacklist")
                    .HasMaxLength(200);

                entity.Property(e => e.FlagActive).HasColumnName("flag_active");

                entity.Property(e => e.FlagBlacklist)
                    .HasColumnName("flag_blacklist")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdBidangUsaha).HasColumnName("id_bidang_usaha");

                entity.Property(e => e.IdSubBidang).HasColumnName("id_sub_bidang");

                entity.Property(e => e.IsActivated).HasColumnName("is_activated");

                entity.Property(e => e.IsRegistered).HasColumnName("is_registered");

                entity.Property(e => e.IsVerified).HasColumnName("is_verified");

                entity.Property(e => e.JabatanPj)
                    .HasColumnName("jabatan_pj")
                    .HasMaxLength(255);

                entity.Property(e => e.JenisPerusahaan)
                    .HasColumnName("jenis_perusahaan")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.KantorCabang).HasColumnName("kantor_cabang");

                entity.Property(e => e.Keterangan).HasColumnName("keterangan");

                entity.Property(e => e.KodeRekanan)
                    .HasColumnName("kode_rekanan")
                    .HasMaxLength(20);

                entity.Property(e => e.Kodepos)
                    .HasColumnName("kodepos")
                    .HasMaxLength(20);

                entity.Property(e => e.KotaId).HasColumnName("kota_id");

                entity.Property(e => e.MasaBlacklist)
                    .HasColumnName("masa_blacklist")
                    .HasMaxLength(30);

                entity.Property(e => e.MasaakhirBlacklist)
                    .HasColumnName("masaakhir_blacklist")
                    .HasMaxLength(50);

                entity.Property(e => e.MasaawalBlacklist)
                    .HasColumnName("masaawal_blacklist")
                    .HasMaxLength(50);

                entity.Property(e => e.MintaUbahData)
                    .HasColumnName("minta_ubah_data")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NamaPerusahaan)
                    .HasColumnName("nama_perusahaan")
                    .HasMaxLength(100);

                entity.Property(e => e.NamaPj)
                    .HasColumnName("nama_pj")
                    .HasMaxLength(255);

                entity.Property(e => e.NoPonsel)
                    .HasColumnName("no_ponsel")
                    .HasMaxLength(50);

                entity.Property(e => e.Npwp)
                    .HasColumnName("npwp")
                    .HasMaxLength(50);

                entity.Property(e => e.NpwpImgurl)
                    .HasColumnName("npwp_imgurl")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PaktaintegritasAgreement).HasColumnName("paktaintegritas_agreement");

                entity.Property(e => e.PernyataanminatAgreement).HasColumnName("pernyataanminat_agreement");

                entity.Property(e => e.Pkp)
                    .HasColumnName("pkp")
                    .HasMaxLength(100);

                entity.Property(e => e.PkpImgurl)
                    .HasColumnName("pkp_imgurl")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ProvinsiId).HasColumnName("provinsi_id");

                entity.Property(e => e.Pswd)
                    .HasColumnName("pswd")
                    .HasMaxLength(255);

                entity.Property(e => e.TanggalSetuju)
                    .HasColumnName("tanggal_setuju")
                    .HasMaxLength(50);

                entity.Property(e => e.Telepon)
                    .HasColumnName("telepon")
                    .HasMaxLength(50);

                entity.Property(e => e.TglBlacklist)
                    .HasColumnName("tgl_blacklist")
                    .HasMaxLength(50);

                entity.Property(e => e.TolakDate)
                    .HasColumnName("tolak_date")
                    .HasMaxLength(50);

                entity.Property(e => e.TolakVerifikasi).HasColumnName("tolak_verifikasi");

                entity.Property(e => e.TriggerVerified).HasColumnName("trigger_verified");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasMaxLength(30);

                entity.Property(e => e.UrlSertifikat)
                    .HasColumnName("url_sertifikat")
                    .HasMaxLength(100);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(70);

                entity.Property(e => e.VerifiedDate)
                    .HasColumnName("verified_date")
                    .HasMaxLength(50);

                entity.Property(e => e.VerifiedSendDate)
                    .HasColumnName("verified_send_date")
                    .HasMaxLength(50);

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}