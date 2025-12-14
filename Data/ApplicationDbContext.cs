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

    public virtual DbSet<BaiDangNhanNuoi> BaiDangNhanNuois { get; set; }

    public virtual DbSet<BaoCaoDoanhThu> BaoCaoDoanhThus { get; set; }

    public virtual DbSet<ChiTietChamSoc> ChiTietChamSocs { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public DbSet<DanhGia> DanhGia { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DeXuatNhanNuoi> DeXuatNhanNuois { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GiaoHang> GiaoHangs { get; set; }

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

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<ThanhToanTrucTuyen> ThanhToanTrucTuyens { get; set; }

    public virtual DbSet<ThuCungNhanNuoi> ThuCungNhanNuois { get; set; }

    public virtual DbSet<VDanhSachThuCungNhanNuoi> VDanhSachThuCungNhanNuois { get; set; }

    public virtual DbSet<YeuThich> YeuThiches { get; set; }
    public virtual DbSet<VDanhSachThuCungNhanNuoi> DanhSachThuCungNhanNuoi { get; set; } = null!;
    public DbSet<PhieuXuat> PhieuXuats { get; set; }
    public DbSet<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }

    public DbSet<HinhThucThanhToanThucTe> HinhThucThanhToanThucTes { get; set; }
    public DbSet<YeuCauNhanNuoi> YeuCauNhanNuois { get; set; } = null!;




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiDangNhanNuoi>(entity =>
        {
            entity.HasKey(e => e.MaBaiDang).HasName("PK__BaiDangN__BF5D50C5D2EBC439");

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
                .HasConstraintName("FK__BaiDangNh__MaNgu__1332DBDC");

            entity.HasOne(d => d.MaThuCungNhanNuoiNavigation).WithMany(p => p.BaiDangNhanNuois)
                .HasForeignKey(d => d.MaThuCungNhanNuoi)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BaiDangNh__MaThu__123EB7A3");
        });

        modelBuilder.Entity<BaoCaoDoanhThu>(entity =>
        {
            entity.HasKey(e => e.MaBaoCao).HasName("PK__BaoCaoDo__25A9188C59576223");

            entity.ToTable("BaoCaoDoanhThu");

            entity.Property(e => e.LoiNhuan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongChiPhi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.NguoiLapNavigation).WithMany(p => p.BaoCaoDoanhThus)
                .HasForeignKey(d => d.NguoiLap)
                .HasConstraintName("FK__BaoCaoDoa__Nguoi__282DF8C2");
        });

        modelBuilder.Entity<ChiTietChamSoc>(entity =>
        {
            entity.HasKey(e => e.MaCtcs).HasName("PK__ChiTietC__1E4E48C420DC318F");

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
                .HasConstraintName("FK__ChiTietCh__MaKyG__08B54D69");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => e.MaCtdh).HasName("PK__ChiTietD__1E4E40F05C8E5744");

            entity.ToTable("ChiTietDonHang", tb => tb.HasTrigger("trg_CapNhatTonKho_AfterDonHang"));

            entity.Property(e => e.MaCtdh).HasColumnName("MaCTDH");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaDH__6B24EA82");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDon__MaSP__6C190EBB");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.MaCtgh).HasName("PK__ChiTietG__1E4FAF54BA8ECC03");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.MaCtgh).HasColumnName("MaCTGH");
            entity.Property(e => e.MaGh).HasColumnName("MaGH");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaGhNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaGh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietGio__MaGH__60A75C0F");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietGio__MaSP__619B8048");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaCtpn).HasName("PK__ChiTietP__1E4E607516DCFB70");

            entity.ToTable("ChiTietPhieuNhap", tb => tb.HasTrigger("trg_CapNhatTonKhoSauNhapHang"));

            entity.Property(e => e.MaCtpn).HasColumnName("MaCTPN");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaPn).HasColumnName("MaPN");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");

            entity.HasOne(d => d.MaPnNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaPn)
                .HasConstraintName("FK__ChiTietPhi__MaPN__7F2BE32F");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("FK__ChiTietPhi__MaSP__00200768");
        });
        modelBuilder.Entity<DanhGia>(entity =>
        {
            entity.ToTable("DanhGia");

            entity.HasKey(e => e.MaDanhGia);

            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.NgayDanhGia).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.NoiDung).HasMaxLength(500);

            // 🔥 FIX QUAN TRỌNG: khai báo đúng FK
            entity.HasOne(d => d.MaNguoiDungNavigation)
                  .WithMany(p => p.DanhGia)
                  .HasForeignKey(d => d.MaNguoiDung)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_DanhGia_NguoiDung");

            entity.HasOne(d => d.MaSpNavigation)
                  .WithMany(p => p.DanhGia)
                  .HasForeignKey(d => d.MaSp)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_DanhGia_SanPham");
        });


        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMuc__B3750887FD221FCC");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(100);
        });

        modelBuilder.Entity<DeXuatNhanNuoi>(entity =>
        {
            entity.HasKey(e => e.MaDeXuat).HasName("PK__DeXuatNh__22244765A11BC5AF");

            entity.ToTable("DeXuatNhanNuoi");

            entity.Property(e => e.LyDo).HasMaxLength(255);
            entity.Property(e => e.NgayDeXuat).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ duyệt");

            entity.HasOne(d => d.MaKyGuiNavigation).WithMany(p => p.DeXuatNhanNuois)
                .HasForeignKey(d => d.MaKyGui)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeXuatNha__MaKyG__17F790F9");

            entity.HasOne(d => d.MaNhanVienChamSocNavigation).WithMany(p => p.DeXuatNhanNuois)
                .HasForeignKey(d => d.MaNhanVienChamSoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DeXuatNha__MaNha__18EBB532");
        });
        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDh).HasName("PK__DonHang__27258661E6A07298");

            entity.ToTable("DonHang", tb =>
            {
                tb.HasTrigger("trg_LuuLichSuTrangThaiDonHang");
                tb.HasTrigger("trg_TaoHoaDonTuDong");
            });

            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.DiaChiGiao).HasMaxLength(200);
            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.HoTenNhan).HasMaxLength(100);
            entity.Property(e => e.MaKm).HasColumnName("MaKM");
            entity.Property(e => e.MaTTTT).HasColumnName("MaTTTT");
            entity.Property(e => e.MaHTTT).HasColumnName("MaHTTT");
            entity.Property(e => e.MaVanDon).HasMaxLength(50);
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(50);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenDonViGiao).HasMaxLength(100);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chờ duyệt");

            // Khuyến mãi
            entity.HasOne(d => d.MaKmNavigation)
                .WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaKm);

            // Người dùng
            entity.HasOne(d => d.MaNguoiDungNavigation)
                .WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Phí giao hàng
            entity.HasOne(d => d.MaPhiNavigation)
                .WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaPhi);

            // 🔗 1–1: Thanh toán trực tuyến
            entity.HasOne(d => d.ThanhToanTrucTuyenNavigation)
                .WithOne(p => p.MaDhNavigation)
                .HasForeignKey<ThanhToanTrucTuyen>(p => p.MaDh)
                .HasConstraintName("FK_DonHang_ThanhToanTrucTuyen");

            // 🔗 1–1: Hình thức thanh toán thực tế
            entity.HasOne(d => d.HinhThucThanhToanThucTeNavigation)
                .WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.MaHTTT)
                .HasConstraintName("FK_DonHang_HTThanhToanThucTe");
        });

        // =========================
        // Hình Thức Thanh Toán Thực Tế (COD tiền mặt / chuyển khoản)
        // =========================
        modelBuilder.Entity<HinhThucThanhToanThucTe>(entity =>
        {
            entity.HasKey(e => e.MaHTTT) // Primary key
                  .HasName("PK_HinhThucThanhToanThucTe");

            entity.ToTable("HinhThucThanhToanThucTe");

            entity.Property(e => e.MaHTTT)
                  .HasColumnName("MaHTTT");

            entity.Property(e => e.TenHinhThuc)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.MoTa)
                  .HasMaxLength(255);

            // QUAN HỆ 1 - N: Một hình thức có nhiều đơn hàng
            entity.HasMany(e => e.DonHangs)
                  .WithOne(d => d.HinhThucThanhToanThucTeNavigation)
                  .HasForeignKey(d => d.MaHTTT)
                  .HasConstraintName("FK_DonHang_HTThanhToanThucTe");
        });

        modelBuilder.Entity<GiaoHang>(entity =>
        {
            entity.HasKey(e => e.MaGiaoHang).HasName("PK__GiaoHang__81CCF4FD3D95E6AC");

            entity.ToTable("GiaoHang");

            entity.Property(e => e.DonViGiao).HasMaxLength(100);
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayGiao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang giao");

            entity.HasOne(d => d.MaDhNavigation).WithMany(p => p.GiaoHangs)
                .HasForeignKey(d => d.MaDh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiaoHang__MaDH__1DB06A4F");

            entity.HasOne(d => d.MaNhanVienGiaoNavigation).WithMany(p => p.GiaoHangs)
                .HasForeignKey(d => d.MaNhanVienGiao)
                .HasConstraintName("FK__GiaoHang__MaNhan__1EA48E88");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.MaGh).HasName("PK__GioHang__2725AE85CB90290A");

            entity.ToTable("GioHang");

            entity.Property(e => e.MaGh).HasColumnName("MaGH");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GioHang__MaNguoi__5CD6CB2B");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HoaDon__2725A6E05503E113");

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
                .HasConstraintName("FK__HoaDon__MaDH__236943A5");

            entity.HasOne(d => d.MaKeToanNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKeToan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaKeToan__22751F6C");

            entity.HasOne(d => d.MaKyGuiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKyGui)
                .HasConstraintName("FK__HoaDon__MaKyGui__245D67DE");
        });

        modelBuilder.Entity<KhoHang>(entity =>
        {
            entity.HasKey(e => e.MaKho).HasName("PK__KhoHang__3BDA9350AA34B1D9");

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
                .HasConstraintName("FK__KhoHang__MaNCC__76969D2E");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.KhoHangs)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhoHang__MaSP__75A278F5");

            entity.HasOne(d => d.NguoiThucHienNavigation).WithMany(p => p.KhoHangs)
                .HasForeignKey(d => d.NguoiThucHien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhoHang__NguoiTh__778AC167");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.HasKey(e => e.MaKm);
            entity.ToTable("KhuyenMai");
            entity.HasIndex(e => e.MaCode).IsUnique();
            entity.Property(e => e.GiaTriToiDa).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MaCode).HasMaxLength(20);
            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.SoLanSuDungToiDa).HasDefaultValue(1);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            // 🔹 Quan hệ với SanPham
            entity.HasOne(k => k.SanPham)
                  .WithMany(s => s.KhuyenMais)
                  .HasForeignKey(k => k.MaSP)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<KyGuiThuCung>(entity =>
        {
            entity.HasKey(e => e.MaKyGui).HasName("PK__KyGuiThu__2B9FA9BE7DC16F18");

            entity.ToTable("KyGuiThuCung", tb => tb.HasTrigger("trg_CapNhatTrangThaiKyGui"));

            entity.Property(e => e.GhiChu).HasMaxLength(500);
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
                .HasConstraintName("FK__KyGuiThuCu__MaKH__04E4BC85");
        });

        modelBuilder.Entity<LichLamViec>(entity =>
        {
            entity.HasKey(e => e.MaLichLam).HasName("PK__LichLamV__0A7F064AE10E18A7");

            entity.ToTable("LichLamViec");

            entity.Property(e => e.CaLam).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.LichLamViecs)
                .HasForeignKey(d => d.MaNhanVien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichLamVi__MaNha__4A8310C6");
        });

        modelBuilder.Entity<LichSuHeThong>(entity =>
        {
            entity.HasKey(e => e.MaLichSu).HasName("PK__LichSuHe__C443222AB55B2896");

            entity.ToTable("LichSuHeThong");

            entity.Property(e => e.GhiChu).HasMaxLength(255);
            entity.Property(e => e.HanhDong).HasMaxLength(255);
            entity.Property(e => e.NgayThucHien)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.LichSuHeThongs)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuHeT__MaNgu__367C1819");
        });

        modelBuilder.Entity<LichSuTrangThaiDonHang>(entity =>
        {
            entity.HasKey(e => e.MaLichSuDh).HasName("PK__LichSuTr__1EBCBDEDFA0A6A31");

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
                .HasConstraintName("FK__LichSuTran__MaDH__3A4CA8FD");

            entity.HasOne(d => d.NguoiThucHienNavigation).WithMany(p => p.LichSuTrangThaiDonHangs)
                .HasForeignKey(d => d.NguoiThucHien)
                .HasConstraintName("FK__LichSuTra__Nguoi__3B40CD36");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D7625505B874");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC05F4EA5A2").IsUnique();

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
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB011488BD");

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
            entity.HasKey(e => e.MaPhi).HasName("PK__PhiGiaoH__3AE0AA65C312140C");

            entity.ToTable("PhiGiaoHang");

            entity.Property(e => e.DonViGiao).HasMaxLength(100);
            entity.Property(e => e.KhuVuc).HasMaxLength(100);
            entity.Property(e => e.PhiCoDinh).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PhiTheoKm)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThoiGianUocTinh).HasMaxLength(50);
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPn).HasName("PK__PhieuNha__2725E7F08E9D30DE");

            entity.ToTable("PhieuNhap");

            entity.Property(e => e.MaPn).HasColumnName("MaPN");
            entity.Property(e => e.MaNcc).HasColumnName("MaNCC");
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK__PhieuNhap__MaNCC__7B5B524B");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__PhieuNhap__MaNha__7C4F7684");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__2725081C3EEE9CD1");

            entity.ToTable("SanPham", tb => tb.HasTrigger("trg_LichSuHeThong"));

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
            entity.ToTable("ThanhToanTrucTuyen");   //  ⬅ FIX QUAN TRỌNG

            entity.HasKey(e => e.MaTttt);
            entity.Property(e => e.MaTttt).HasColumnName("MaTTTT");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
        });



        modelBuilder.Entity<ThuCungNhanNuoi>(entity =>
        {
            entity.HasKey(e => e.MaNhanNuoi).HasName("PK__ThuCungN__A03CFC07E64A7684");

            entity.ToTable("ThuCungNhanNuoi");

            entity.Property(e => e.NgayChuyen).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa có người nhận");

            entity.HasOne(d => d.MaKyGuiNavigation).WithMany(p => p.ThuCungNhanNuois)
                .HasForeignKey(d => d.MaKyGui)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThuCungNh__MaKyG__0D7A0286");
        });

        modelBuilder.Entity<VDanhSachThuCungNhanNuoi>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_DanhSachThuCungNhanNuoi");

            entity.Property(e => e.GiongLoai).HasMaxLength(100);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenThuCung).HasMaxLength(100);
            entity.Property(e => e.TieuDe).HasMaxLength(100);
            entity.Property(e => e.TrangThaiBaiDang).HasMaxLength(50);
            entity.Property(e => e.TrangThaiKyGui).HasMaxLength(50);
        });

        modelBuilder.Entity<YeuThich>(entity =>
        {
            entity.HasKey(e => e.MaYt).HasName("PK__YeuThich__272330D453F09BBA");

            entity.ToTable("YeuThich");

            entity.Property(e => e.MaYt).HasColumnName("MaYT");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.NgayThem)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__YeuThich__MaNguo__31B762FC");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__YeuThich__MaSP__32AB8735");
        });
        // ======================================================
        // 📤 PHIẾU XUẤT
        // ======================================================
        modelBuilder.Entity<PhieuXuat>(entity =>
        {
            entity.HasKey(e => e.MaPx).HasName("PK__PhieuXuat__2725E7CACE181046");
            entity.ToTable("PhieuXuat");

            entity.Property(e => e.MaPx).HasColumnName("MaPX");
            entity.Property(e => e.MaDh).HasColumnName("MaDH");
            entity.Property(e => e.NgayXuat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TongTien)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");

            // ✅ Nhân viên xuất kho
            entity.HasOne(d => d.MaNhanVienNavigation)
                .WithMany()
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK_PhieuXuat_NhanVien");

            // ✅ Khách hàng nhận hàng
            entity.HasOne(d => d.MaKhachHangNavigation)
                .WithMany()
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK_PhieuXuat_KhachHang");

            // ✅ Liên kết đến đơn hàng (nếu có)
            entity.HasOne(d => d.MaDhNavigation)
                .WithMany(p => p.PhieuXuats)
                .HasForeignKey(d => d.MaDh)
                .HasConstraintName("FK_PhieuXuat_DonHang");
        });

        // ======================================================
        // 📦 CHI TIẾT PHIẾU XUẤT
        // ======================================================
        modelBuilder.Entity<ChiTietPhieuXuat>(entity =>
        {
            entity.HasKey(e => e.MaCtpx).HasName("PK__ChiTietPhieuXuat");
            entity.ToTable("ChiTietPhieuXuat");

            entity.Property(e => e.MaCtpx).HasColumnName("MaCTPX");
            entity.Property(e => e.MaPx).HasColumnName("MaPX");
            entity.Property(e => e.MaSp).HasColumnName("MaSP");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

            // 🔗 FK đến PhieuXuat
            entity.HasOne(d => d.MaPxNavigation)
                .WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(d => d.MaPx)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietPhieuXuat_PhieuXuat");

            // 🔗 FK đến SanPham
            entity.HasOne(d => d.MaSpNavigation)
                .WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietPhieuXuat_SanPham");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    // ==========================================================
    // 🧩 FIX TRIGGER + COMPATIBILITY
    // ==========================================================
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=PetShopDB;Trusted_Connection=True;TrustServerCertificate=True;",
                sqlOptions =>
                {
                    sqlOptions.UseCompatibilityLevel(120);
                });
        }
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
