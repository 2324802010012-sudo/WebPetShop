using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebPetShop.Models;

namespace WebPetShop.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<BaiDangNhanNuoi> BaiDangNhanNuois { get; set; }

    public virtual DbSet<BaoCaoDoanhThu> BaoCaoDoanhThus { get; set; }

    public virtual DbSet<ChiTietChamSoc> ChiTietChamSocs { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<DanhGium> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhoHang> KhoHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<KyGuiThuCung> KyGuiThuCungs { get; set; }

    public virtual DbSet<LichLamViec> LichLamViecs { get; set; }

    public virtual DbSet<LichSuHeThong> LichSuHeThongs { get; set; }

    public virtual DbSet<LichSuTrangThaiDonHang> LichSuTrangThaiDonHangs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<PhiGiaoHang> PhiGiaoHangs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<ThanhToanTrucTuyen> ThanhToanTrucTuyens { get; set; }

    public virtual DbSet<ThuCungNhanNuoi> ThuCungNhanNuois { get; set; }

    public virtual DbSet<YeuThich> YeuThiches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-JB4U48JA\\SQLEXPRESS01;Database=PetShopDB;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<BaiDangNhanNuoi>(entity =>
        {
            entity.HasKey(e => e.MaBaiDang).HasName("PK__BaiDangN__BF5D50C56E2D950B");

            entity.ToTable("BaiDangNhanNuoi");

            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.NgayDang).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TieuDe).HasMaxLength(100);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ duyệt");

            entity.HasOne(d => d.MaNguoiTaoNavigation).WithMany(p => p.BaiDangNhanNuois)
                .HasForeignKey(d => d.MaNguoiTao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BaiDangNh__MaNgu__2BFE89A6");

            entity.HasOne(d => d.MaThuCungNhanNuoiNavigation).WithMany(p => p.BaiDangNhanNuois)
                .HasForeignKey(d => d.MaThuCungNhanNuoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BaiDangNh__MaThu__2B0A656D");
        });

        modelBuilder.Entity<BaoCaoDoanhThu>(entity =>
        {
            entity.HasKey(e => e.MaBaoCao).HasName("PK__BaoCaoDo__25A9188C72F9778F");

            entity.ToTable("BaoCaoDoanhThu");

            entity.Property(e => e.LoiNhuan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongChiPhi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.NguoiLapNavigation).WithMany(p => p.BaoCaoDoanhThus)
                .HasForeignKey(d => d.NguoiLap)
                .HasConstraintName("FK__BaoCaoDoa__Nguoi__245D67DE");
        });

        modelBuilder.Entity<ChiTietChamSoc>(entity =>
        {
            entity.HasKey(e => e.MaCtcs).HasName("PK__ChiTietC__1E4E48C4744EFA0F");

            entity.ToTable("ChiTietChamSoc");

            entity.Property(e => e.MaCtcs).HasColumnName("MaCTCS");
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TinhTrang).HasMaxLength(200);

            entity.HasOne(d => d.MaKyGuiNavigation).WithMany(p => p.ChiTietChamSocs)
                .HasForeignKey(d => d.MaKyGui)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietCh__MaKyG__71D1E811");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaCtdh).HasName("PK__ChiTietD__1E4E40F0CB3D44C1");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.MaCtdh).HasColumnName("MaCTDH");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaDH__60A75C0F");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaSP__619B8048");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.MaCtgh).HasName("PK__ChiTietG__1E4FAF546172E42D");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.MaCtgh).HasColumnName("MaCTGH");
            entity.Property(e => e.MaGh).HasColumnName("MaGH");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaGhNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietGio__MaGH__5812160E");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietGio__MaSP__59063A47");
        });

        modelBuilder.Entity<DanhGium>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGia__AA9515BF7A434DCF");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.NgayDanhGia)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(500);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DanhGia__MaNguoi__0F624AF8");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.DanhGia)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("FK__DanhGia__MaSP__10566F31");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMuc__B37508872BE94D27");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("PK__DonHang__27258661FDAAA8B1");

            entity.ToTable("DonHang");

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaKm).HasColumnName("MaKM");
            entity.Property(e => e.MaVanDon).HasMaxLength(50);
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDonViGiao).HasMaxLength(100);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ duyệt");

            entity.HasOne(d => d.MaKmNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKm)
                .HasConstraintName("FK__DonHang__MaKM__02FC7413");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonHang__MaNguoi__5DCAEF64");

            entity.HasOne(d => d.MaPhiNavigation).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPhi)
                .HasConstraintName("FK__DonHang__MaPhi__06CD04F7");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGh).HasName("PK__GioHang__2725AE8533881736");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGh).HasColumnName("MaGH");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GioHang__MaNguoi__5441852A");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HoaDon__2725A6E0FD89A372");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.HinhThuc).HasMaxLength(50);
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("FK__HoaDon__MaDH__7B5B524B");

            entity.HasOne(d => d.MaKeToanNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKeToan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaKeToan__7A672E12");

            entity.HasOne(d => d.MaKyGuiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKyGui)
                .HasConstraintName("FK__HoaDon__MaKyGui__7C4F7684");
        });

        modelBuilder.Entity<KhoHang>(entity =>
        {
            entity.HasKey(e => e.MaKho).HasName("PK__KhoHang__3BDA9350B366DC9B");

            entity.ToTable("KhoHang");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.LoaiGiaoDich).HasMaxLength(50);
            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.NgayThucHien)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.KhoHangs)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK__KhoHang__MaNCC__68487DD7");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.KhoHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhoHang__MaSP__6754599E");

            entity.HasOne(d => d.NguoiThucHienNavigation).WithMany(p => p.KhoHangs)
                .HasForeignKey(d => d.NguoiThucHien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhoHang__NguoiTh__693CA210");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKm).HasName("PK__KhuyenMa__2725CF159BBEBD14");

            entity.ToTable("KhuyenMai");

            entity.HasIndex(e => e.MaCode, "UQ__KhuyenMa__152C7C5CE4736415").IsUnique();

            entity.Property(e => e.MaKm).HasColumnName("MaKM");
            entity.Property(e => e.GiaTriToiDa).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaCode).HasMaxLength(20);
            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.SoLanSuDungToiDa).HasDefaultValue(1);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<KyGuiThuCung>(entity =>
        {
            entity.HasKey(e => e.MaKyGui).HasName("PK__KyGuiThu__2B9FA9BEAFEA1E7D");

            entity.ToTable("KyGuiThuCung");

            entity.Property(e => e.GiongLoai).HasMaxLength(100);
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.NgayGui).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PhiKyGui).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenThuCung).HasMaxLength(100);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang ký gửi");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.KyGuiThuCungs)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KyGuiThuCu__MaKH__6E01572D");
        });

        modelBuilder.Entity<LichLamViec>(entity =>
        {
            entity.HasKey(e => e.MaLichLam).HasName("PK__LichLamV__0A7F064A0FED0DE0");

            entity.ToTable("LichLamViec");

            entity.Property(e => e.CaLam).HasMaxLength(20);
            entity.Property(e => e.GhiChu).HasMaxLength(255);

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.LichLamViecs)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichLamVi__MaNha__17F790F9");
        });

        modelBuilder.Entity<LichSuHeThong>(entity =>
        {
            entity.HasKey(e => e.MaLichSu).HasName("PK__LichSuHe__C443222AA4C7E898");

            entity.ToTable("LichSuHeThong");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.HanhDong).HasMaxLength(255);
            entity.Property(e => e.NgayThucHien)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.LichSuHeThongs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuHeT__MaNgu__1BC821DD");
        });

        modelBuilder.Entity<LichSuTrangThaiDonHang>(entity =>
        {
            entity.HasKey(e => e.MaLichSuDh).HasName("PK__LichSuTr__1EBCBDED00E7051A");

            entity.ToTable("LichSuTrangThaiDonHang");

            entity.Property(e => e.MaLichSuDh).HasColumnName("MaLichSuDH");
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThaiCu).HasMaxLength(50);
            entity.Property(e => e.TrangThaiMoi).HasMaxLength(50);

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.LichSuTrangThaiDonHangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuTran__MaDH__1F98B2C1");

            entity.HasOne(d => d.NguoiThucHienNavigation).WithMany(p => p.LichSuTrangThaiDonHangs)
                .HasForeignKey(d => d.NguoiThucHien)
                .HasConstraintName("FK__LichSuTra__Nguoi__208CD6FA");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762FC376884");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC092605992").IsUnique();

            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
            entity.Property(e => e.VaiTro).HasMaxLength(30);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB15CA175D");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("TenNCC");
        });

        modelBuilder.Entity<PhiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.MaPhi).HasName("PK__PhiGiaoH__3AE0AA6574C45B95");

            entity.ToTable("PhiGiaoHang");

            entity.Property(e => e.DonViGiao).HasMaxLength(100);
            entity.Property(e => e.KhuVuc).HasMaxLength(100);
            entity.Property(e => e.PhiCoDinh).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PhiTheoKm)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThoiGianUocTinh).HasMaxLength(50);
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__2725081C2B6FE3D9");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.Gia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TenSp)
                .HasMaxLength(100)
                .HasColumnName("TenSP");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SanPham__MaDanhM__5070F446");
        });

        modelBuilder.Entity<ThanhToanTrucTuyen>(entity =>
        {
            entity.HasKey(e => e.MaTttt).HasName("PK__ThanhToa__44843A5D938BBDC7");

            entity.ToTable("ThanhToanTrucTuyen");

            entity.Property(e => e.MaTttt).HasColumnName("MaTTTT");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaGiaoDich).HasMaxLength(100);
            entity.Property(e => e.NgayThanhToan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ThanhToanTrucTuyens)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToanT__MaDH__0A9D95DB");
        });

        modelBuilder.Entity<ThuCungNhanNuoi>(entity =>
        {
            entity.HasKey(e => e.MaNhanNuoi).HasName("PK__ThuCungN__A03CFC07E2869939");

            entity.ToTable("ThuCungNhanNuoi");

            entity.Property(e => e.NgayChuyen).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa có người nhận");

            entity.HasOne(d => d.MaKyGuiNavigation).WithMany(p => p.ThuCungNhanNuois)
                .HasForeignKey(d => d.MaKyGui)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThuCungNh__MaKyG__76969D2E");
        });

        modelBuilder.Entity<YeuThich>(entity =>
        {
            entity.HasKey(e => e.MaYt).HasName("PK__YeuThich__272330D4E7581D41");

            entity.ToTable("YeuThich");

            entity.Property(e => e.MaYt).HasColumnName("MaYT");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.NgayThem)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__YeuThich__MaNguo__14270015");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__YeuThich__MaSP__151B244E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
