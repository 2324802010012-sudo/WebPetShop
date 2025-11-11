using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPetShop.Migrations
{
    /// <inheritdoc />
    public partial class Add_MaDh_To_PhieuXuat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__BaiDangNh__MaNgu__2BFE89A6",
                table: "BaiDangNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK__BaiDangNh__MaThu__2B0A656D",
                table: "BaiDangNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK__BaoCaoDoa__Nguoi__245D67DE",
                table: "BaoCaoDoanhThu");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietCh__MaKyG__71D1E811",
                table: "ChiTietChamSoc");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietDon__MaDH__60A75C0F",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietDon__MaSP__619B8048",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietGio__MaGH__5812160E",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietGio__MaSP__59063A47",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DanhGia__MaNguoi__0F624AF8",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK__DanhGia__MaSP__10566F31",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK__DonHang__MaKM__02FC7413",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DonHang__MaNguoi__5DCAEF64",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DonHang__MaPhi__06CD04F7",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__GioHang__MaNguoi__5441852A",
                table: "GioHang");

            migrationBuilder.DropForeignKey(
                name: "FK__HoaDon__MaDH__7B5B524B",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK__HoaDon__MaKeToan__7A672E12",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK__HoaDon__MaKyGui__7C4F7684",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK__KhoHang__MaNCC__68487DD7",
                table: "KhoHang");

            migrationBuilder.DropForeignKey(
                name: "FK__KhoHang__MaSP__6754599E",
                table: "KhoHang");

            migrationBuilder.DropForeignKey(
                name: "FK__KhoHang__NguoiTh__693CA210",
                table: "KhoHang");

            migrationBuilder.DropForeignKey(
                name: "FK__KyGuiThuCu__MaKH__6E01572D",
                table: "KyGuiThuCung");

            migrationBuilder.DropForeignKey(
                name: "FK__LichLamVi__MaNha__17F790F9",
                table: "LichLamViec");

            migrationBuilder.DropForeignKey(
                name: "FK__LichSuHeT__MaNgu__1BC821DD",
                table: "LichSuHeThong");

            migrationBuilder.DropForeignKey(
                name: "FK__LichSuTra__Nguoi__208CD6FA",
                table: "LichSuTrangThaiDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__LichSuTran__MaDH__1F98B2C1",
                table: "LichSuTrangThaiDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ThanhToanT__MaDH__0A9D95DB",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropForeignKey(
                name: "FK__ThuCungNh__MaKyG__76969D2E",
                table: "ThuCungNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK__YeuThich__MaNguo__14270015",
                table: "YeuThich");

            migrationBuilder.DropForeignKey(
                name: "FK__YeuThich__MaSP__151B244E",
                table: "YeuThich");

            migrationBuilder.DropPrimaryKey(
                name: "PK__YeuThich__272330D4E7581D41",
                table: "YeuThich");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ThuCungN__A03CFC07E2869939",
                table: "ThuCungNhanNuoi");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ThanhToa__44843A5D938BBDC7",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SanPham__2725081C2B6FE3D9",
                table: "SanPham");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PhiGiaoH__3AE0AA6574C45B95",
                table: "PhiGiaoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__NhaCungC__3A185DEB15CA175D",
                table: "NhaCungCap");

            migrationBuilder.DropPrimaryKey(
                name: "PK__NguoiDun__C539D762FC376884",
                table: "NguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LichSuTr__1EBCBDED00E7051A",
                table: "LichSuTrangThaiDonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LichSuHe__C443222AA4C7E898",
                table: "LichSuHeThong");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LichLamV__0A7F064A0FED0DE0",
                table: "LichLamViec");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KyGuiThu__2B9FA9BEAFEA1E7D",
                table: "KyGuiThuCung");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KhuyenMa__2725CF159BBEBD14",
                table: "KhuyenMai");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KhoHang__3BDA9350B366DC9B",
                table: "KhoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__HoaDon__2725A6E0FD89A372",
                table: "HoaDon");

            migrationBuilder.DropPrimaryKey(
                name: "PK__GioHang__2725AE8533881736",
                table: "GioHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DonHang__27258661FDAAA8B1",
                table: "DonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DanhMuc__B37508872BE94D27",
                table: "DanhMuc");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DanhGia__AA9515BF7A434DCF",
                table: "DanhGia");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChiTietG__1E4FAF546172E42D",
                table: "ChiTietGioHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChiTietD__1E4E40F0CB3D44C1",
                table: "ChiTietDonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChiTietC__1E4E48C4744EFA0F",
                table: "ChiTietChamSoc");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BaoCaoDo__25A9188C72F9778F",
                table: "BaoCaoDoanhThu");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BaiDangN__BF5D50C56E2D950B",
                table: "BaiDangNhanNuoi");

            migrationBuilder.RenameIndex(
                name: "UQ__NguoiDun__55F68FC092605992",
                table: "NguoiDung",
                newName: "UQ__NguoiDun__55F68FC05F4EA5A2");

            migrationBuilder.RenameIndex(
                name: "UQ__KhuyenMa__152C7C5CE4736415",
                table: "KhuyenMai",
                newName: "UQ__KhuyenMa__152C7C5C95B7F5B4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayCapNhat",
                table: "LichSuTrangThaiDonHang",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "CaLam",
                table: "LichLamViec",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "LichLamViec",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "KyGuiThuCung",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiGiao",
                table: "DonHang",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "DonHang",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTenNhan",
                table: "DonHang",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaTTTT",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucThanhToan",
                table: "DonHang",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "DonHang",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__YeuThich__272330D453F09BBA",
                table: "YeuThich",
                column: "MaYT");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ThuCungN__A03CFC07E64A7684",
                table: "ThuCungNhanNuoi",
                column: "MaNhanNuoi");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ThanhToa__44843A5D0FA5E91A",
                table: "ThanhToanTrucTuyen",
                column: "MaTTTT");

            migrationBuilder.AddPrimaryKey(
                name: "PK__SanPham__2725081C3EEE9CD1",
                table: "SanPham",
                column: "MaSP");

            migrationBuilder.AddPrimaryKey(
                name: "PK__PhiGiaoH__3AE0AA65C312140C",
                table: "PhiGiaoHang",
                column: "MaPhi");

            migrationBuilder.AddPrimaryKey(
                name: "PK__NhaCungC__3A185DEB011488BD",
                table: "NhaCungCap",
                column: "MaNCC");

            migrationBuilder.AddPrimaryKey(
                name: "PK__NguoiDun__C539D7625505B874",
                table: "NguoiDung",
                column: "MaNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LichSuTr__1EBCBDEDFA0A6A31",
                table: "LichSuTrangThaiDonHang",
                column: "MaLichSuDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LichSuHe__C443222AB55B2896",
                table: "LichSuHeThong",
                column: "MaLichSu");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LichLamV__0A7F064AE10E18A7",
                table: "LichLamViec",
                column: "MaLichLam");

            migrationBuilder.AddPrimaryKey(
                name: "PK__KyGuiThu__2B9FA9BE7DC16F18",
                table: "KyGuiThuCung",
                column: "MaKyGui");

            migrationBuilder.AddPrimaryKey(
                name: "PK__KhuyenMa__2725CF15441787FD",
                table: "KhuyenMai",
                column: "MaKM");

            migrationBuilder.AddPrimaryKey(
                name: "PK__KhoHang__3BDA9350AA34B1D9",
                table: "KhoHang",
                column: "MaKho");

            migrationBuilder.AddPrimaryKey(
                name: "PK__HoaDon__2725A6E05503E113",
                table: "HoaDon",
                column: "MaHD");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GioHang__2725AE85CB90290A",
                table: "GioHang",
                column: "MaGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DonHang__27258661E6A07298",
                table: "DonHang",
                column: "MaDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DanhMuc__B3750887FD221FCC",
                table: "DanhMuc",
                column: "MaDanhMuc");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DanhGia__AA9515BF53F49DC4",
                table: "DanhGia",
                column: "MaDanhGia");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChiTietG__1E4FAF54BA8ECC03",
                table: "ChiTietGioHang",
                column: "MaCTGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChiTietD__1E4E40F05C8E5744",
                table: "ChiTietDonHang",
                column: "MaCTDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChiTietC__1E4E48C420DC318F",
                table: "ChiTietChamSoc",
                column: "MaCTCS");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BaoCaoDo__25A9188C59576223",
                table: "BaoCaoDoanhThu",
                column: "MaBaoCao");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BaiDangN__BF5D50C5D2EBC439",
                table: "BaiDangNhanNuoi",
                column: "MaBaiDang");

            migrationBuilder.CreateTable(
                name: "DeXuatNhanNuoi",
                columns: table => new
                {
                    MaDeXuat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKyGui = table.Column<int>(type: "int", nullable: false),
                    MaNhanVienChamSoc = table.Column<int>(type: "int", nullable: false),
                    NgayDeXuat = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    LyDo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Chờ duyệt")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DeXuatNh__22244765A11BC5AF", x => x.MaDeXuat);
                    table.ForeignKey(
                        name: "FK__DeXuatNha__MaKyG__17F790F9",
                        column: x => x.MaKyGui,
                        principalTable: "KyGuiThuCung",
                        principalColumn: "MaKyGui");
                    table.ForeignKey(
                        name: "FK__DeXuatNha__MaNha__18EBB532",
                        column: x => x.MaNhanVienChamSoc,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "GiaoHang",
                columns: table => new
                {
                    MaGiaoHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDH = table.Column<int>(type: "int", nullable: false),
                    MaNhanVienGiao = table.Column<int>(type: "int", nullable: true),
                    DonViGiao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayGiao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Đang giao"),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GiaoHang__81CCF4FD3D95E6AC", x => x.MaGiaoHang);
                    table.ForeignKey(
                        name: "FK__GiaoHang__MaDH__1DB06A4F",
                        column: x => x.MaDH,
                        principalTable: "DonHang",
                        principalColumn: "MaDH");
                    table.ForeignKey(
                        name: "FK__GiaoHang__MaNhan__1EA48E88",
                        column: x => x.MaNhanVienGiao,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "PhieuNhap",
                columns: table => new
                {
                    MaPN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNCC = table.Column<int>(type: "int", nullable: true),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuNha__2725E7F08E9D30DE", x => x.MaPN);
                    table.ForeignKey(
                        name: "FK__PhieuNhap__MaNCC__7B5B524B",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNCC");
                    table.ForeignKey(
                        name: "FK__PhieuNhap__MaNha__7C4F7684",
                        column: x => x.MaNhanVien,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "PhieuXuat",
                columns: table => new
                {
                    MaPX = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    MaKhachHang = table.Column<int>(type: "int", nullable: true),
                    NgayXuat = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    MaDH = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuXuat__2725E7CACE181046", x => x.MaPX);
                    table.ForeignKey(
                        name: "FK_PhieuXuat_DonHang",
                        column: x => x.MaDH,
                        principalTable: "DonHang",
                        principalColumn: "MaDH");
                    table.ForeignKey(
                        name: "FK_PhieuXuat_KhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_PhieuXuat_NhanVien",
                        column: x => x.MaNhanVien,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuNhap",
                columns: table => new
                {
                    MaCTPN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPN = table.Column<int>(type: "int", nullable: true),
                    MaSP = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietP__1E4E607516DCFB70", x => x.MaCTPN);
                    table.ForeignKey(
                        name: "FK__ChiTietPhi__MaPN__7F2BE32F",
                        column: x => x.MaPN,
                        principalTable: "PhieuNhap",
                        principalColumn: "MaPN");
                    table.ForeignKey(
                        name: "FK__ChiTietPhi__MaSP__00200768",
                        column: x => x.MaSP,
                        principalTable: "SanPham",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuXuat",
                columns: table => new
                {
                    MaCTPX = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPX = table.Column<int>(type: "int", nullable: false),
                    MaSP = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietPhieuXuat", x => x.MaCTPX);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuat_PhieuXuat",
                        column: x => x.MaPX,
                        principalTable: "PhieuXuat",
                        principalColumn: "MaPX");
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuXuat_SanPham",
                        column: x => x.MaSP,
                        principalTable: "SanPham",
                        principalColumn: "MaSP");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaTTTT",
                table: "DonHang",
                column: "MaTTTT");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhap_MaPN",
                table: "ChiTietPhieuNhap",
                column: "MaPN");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuNhap_MaSP",
                table: "ChiTietPhieuNhap",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuat_MaPX",
                table: "ChiTietPhieuXuat",
                column: "MaPX");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuXuat_MaSP",
                table: "ChiTietPhieuXuat",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_DeXuatNhanNuoi_MaKyGui",
                table: "DeXuatNhanNuoi",
                column: "MaKyGui");

            migrationBuilder.CreateIndex(
                name: "IX_DeXuatNhanNuoi_MaNhanVienChamSoc",
                table: "DeXuatNhanNuoi",
                column: "MaNhanVienChamSoc");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoHang_MaDH",
                table: "GiaoHang",
                column: "MaDH");

            migrationBuilder.CreateIndex(
                name: "IX_GiaoHang_MaNhanVienGiao",
                table: "GiaoHang",
                column: "MaNhanVienGiao");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhap_MaNCC",
                table: "PhieuNhap",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuNhap_MaNhanVien",
                table: "PhieuNhap",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuat_MaDH",
                table: "PhieuXuat",
                column: "MaDH");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuat_MaKhachHang",
                table: "PhieuXuat",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuat_MaNhanVien",
                table: "PhieuXuat",
                column: "MaNhanVien");

            migrationBuilder.AddForeignKey(
                name: "FK__BaiDangNh__MaNgu__1332DBDC",
                table: "BaiDangNhanNuoi",
                column: "MaNguoiTao",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__BaiDangNh__MaThu__123EB7A3",
                table: "BaiDangNhanNuoi",
                column: "MaThuCungNhanNuoi",
                principalTable: "ThuCungNhanNuoi",
                principalColumn: "MaNhanNuoi");

            migrationBuilder.AddForeignKey(
                name: "FK__BaoCaoDoa__Nguoi__282DF8C2",
                table: "BaoCaoDoanhThu",
                column: "NguoiLap",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietCh__MaKyG__08B54D69",
                table: "ChiTietChamSoc",
                column: "MaKyGui",
                principalTable: "KyGuiThuCung",
                principalColumn: "MaKyGui");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietDon__MaDH__6B24EA82",
                table: "ChiTietDonHang",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietDon__MaSP__6C190EBB",
                table: "ChiTietDonHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietGio__MaGH__60A75C0F",
                table: "ChiTietGioHang",
                column: "MaGH",
                principalTable: "GioHang",
                principalColumn: "MaGH");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietGio__MaSP__619B8048",
                table: "ChiTietGioHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__DanhGia__MaNguoi__2CF2ADDF",
                table: "DanhGia",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__DanhGia__MaSP__2DE6D218",
                table: "DanhGia",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_ThanhToanTrucTuyen",
                table: "DonHang",
                column: "MaTTTT",
                principalTable: "ThanhToanTrucTuyen",
                principalColumn: "MaTTTT");

            migrationBuilder.AddForeignKey(
                name: "FK__DonHang__MaKM__6754599E",
                table: "DonHang",
                column: "MaKM",
                principalTable: "KhuyenMai",
                principalColumn: "MaKM");

            migrationBuilder.AddForeignKey(
                name: "FK__DonHang__MaNguoi__66603565",
                table: "DonHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__DonHang__MaPhi__68487DD7",
                table: "DonHang",
                column: "MaPhi",
                principalTable: "PhiGiaoHang",
                principalColumn: "MaPhi");

            migrationBuilder.AddForeignKey(
                name: "FK__GioHang__MaNguoi__5CD6CB2B",
                table: "GioHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__HoaDon__MaDH__236943A5",
                table: "HoaDon",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__HoaDon__MaKeToan__22751F6C",
                table: "HoaDon",
                column: "MaKeToan",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__HoaDon__MaKyGui__245D67DE",
                table: "HoaDon",
                column: "MaKyGui",
                principalTable: "KyGuiThuCung",
                principalColumn: "MaKyGui");

            migrationBuilder.AddForeignKey(
                name: "FK__KhoHang__MaNCC__76969D2E",
                table: "KhoHang",
                column: "MaNCC",
                principalTable: "NhaCungCap",
                principalColumn: "MaNCC");

            migrationBuilder.AddForeignKey(
                name: "FK__KhoHang__MaSP__75A278F5",
                table: "KhoHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__KhoHang__NguoiTh__778AC167",
                table: "KhoHang",
                column: "NguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__KyGuiThuCu__MaKH__04E4BC85",
                table: "KyGuiThuCung",
                column: "MaKH",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichLamVi__MaNha__4A8310C6",
                table: "LichLamViec",
                column: "MaNhanVien",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuHeT__MaNgu__367C1819",
                table: "LichSuHeThong",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuTra__Nguoi__3B40CD36",
                table: "LichSuTrangThaiDonHang",
                column: "NguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuTran__MaDH__3A4CA8FD",
                table: "LichSuTrangThaiDonHang",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__ThanhToanT__MaDH__6FE99F9F",
                table: "ThanhToanTrucTuyen",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__ThuCungNh__MaKyG__0D7A0286",
                table: "ThuCungNhanNuoi",
                column: "MaKyGui",
                principalTable: "KyGuiThuCung",
                principalColumn: "MaKyGui");

            migrationBuilder.AddForeignKey(
                name: "FK__YeuThich__MaNguo__31B762FC",
                table: "YeuThich",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__YeuThich__MaSP__32AB8735",
                table: "YeuThich",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__BaiDangNh__MaNgu__1332DBDC",
                table: "BaiDangNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK__BaiDangNh__MaThu__123EB7A3",
                table: "BaiDangNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK__BaoCaoDoa__Nguoi__282DF8C2",
                table: "BaoCaoDoanhThu");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietCh__MaKyG__08B54D69",
                table: "ChiTietChamSoc");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietDon__MaDH__6B24EA82",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietDon__MaSP__6C190EBB",
                table: "ChiTietDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietGio__MaGH__60A75C0F",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ChiTietGio__MaSP__619B8048",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DanhGia__MaNguoi__2CF2ADDF",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK__DanhGia__MaSP__2DE6D218",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_ThanhToanTrucTuyen",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DonHang__MaKM__6754599E",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DonHang__MaNguoi__66603565",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__DonHang__MaPhi__68487DD7",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__GioHang__MaNguoi__5CD6CB2B",
                table: "GioHang");

            migrationBuilder.DropForeignKey(
                name: "FK__HoaDon__MaDH__236943A5",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK__HoaDon__MaKeToan__22751F6C",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK__HoaDon__MaKyGui__245D67DE",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK__KhoHang__MaNCC__76969D2E",
                table: "KhoHang");

            migrationBuilder.DropForeignKey(
                name: "FK__KhoHang__MaSP__75A278F5",
                table: "KhoHang");

            migrationBuilder.DropForeignKey(
                name: "FK__KhoHang__NguoiTh__778AC167",
                table: "KhoHang");

            migrationBuilder.DropForeignKey(
                name: "FK__KyGuiThuCu__MaKH__04E4BC85",
                table: "KyGuiThuCung");

            migrationBuilder.DropForeignKey(
                name: "FK__LichLamVi__MaNha__4A8310C6",
                table: "LichLamViec");

            migrationBuilder.DropForeignKey(
                name: "FK__LichSuHeT__MaNgu__367C1819",
                table: "LichSuHeThong");

            migrationBuilder.DropForeignKey(
                name: "FK__LichSuTra__Nguoi__3B40CD36",
                table: "LichSuTrangThaiDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__LichSuTran__MaDH__3A4CA8FD",
                table: "LichSuTrangThaiDonHang");

            migrationBuilder.DropForeignKey(
                name: "FK__ThanhToanT__MaDH__6FE99F9F",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropForeignKey(
                name: "FK__ThuCungNh__MaKyG__0D7A0286",
                table: "ThuCungNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK__YeuThich__MaNguo__31B762FC",
                table: "YeuThich");

            migrationBuilder.DropForeignKey(
                name: "FK__YeuThich__MaSP__32AB8735",
                table: "YeuThich");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuNhap");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuXuat");

            migrationBuilder.DropTable(
                name: "DeXuatNhanNuoi");

            migrationBuilder.DropTable(
                name: "GiaoHang");

            migrationBuilder.DropTable(
                name: "PhieuNhap");

            migrationBuilder.DropTable(
                name: "PhieuXuat");

            migrationBuilder.DropPrimaryKey(
                name: "PK__YeuThich__272330D453F09BBA",
                table: "YeuThich");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ThuCungN__A03CFC07E64A7684",
                table: "ThuCungNhanNuoi");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ThanhToa__44843A5D0FA5E91A",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SanPham__2725081C3EEE9CD1",
                table: "SanPham");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PhiGiaoH__3AE0AA65C312140C",
                table: "PhiGiaoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__NhaCungC__3A185DEB011488BD",
                table: "NhaCungCap");

            migrationBuilder.DropPrimaryKey(
                name: "PK__NguoiDun__C539D7625505B874",
                table: "NguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LichSuTr__1EBCBDEDFA0A6A31",
                table: "LichSuTrangThaiDonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LichSuHe__C443222AB55B2896",
                table: "LichSuHeThong");

            migrationBuilder.DropPrimaryKey(
                name: "PK__LichLamV__0A7F064AE10E18A7",
                table: "LichLamViec");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KyGuiThu__2B9FA9BE7DC16F18",
                table: "KyGuiThuCung");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KhuyenMa__2725CF15441787FD",
                table: "KhuyenMai");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KhoHang__3BDA9350AA34B1D9",
                table: "KhoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__HoaDon__2725A6E05503E113",
                table: "HoaDon");

            migrationBuilder.DropPrimaryKey(
                name: "PK__GioHang__2725AE85CB90290A",
                table: "GioHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DonHang__27258661E6A07298",
                table: "DonHang");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_MaTTTT",
                table: "DonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DanhMuc__B3750887FD221FCC",
                table: "DanhMuc");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DanhGia__AA9515BF53F49DC4",
                table: "DanhGia");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChiTietG__1E4FAF54BA8ECC03",
                table: "ChiTietGioHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChiTietD__1E4E40F05C8E5744",
                table: "ChiTietDonHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ChiTietC__1E4E48C420DC318F",
                table: "ChiTietChamSoc");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BaoCaoDo__25A9188C59576223",
                table: "BaoCaoDoanhThu");

            migrationBuilder.DropPrimaryKey(
                name: "PK__BaiDangN__BF5D50C5D2EBC439",
                table: "BaiDangNhanNuoi");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "LichLamViec");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "DiaChiGiao",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "HoTenNhan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "MaTTTT",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "PhuongThucThanhToan",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "DonHang");

            migrationBuilder.RenameIndex(
                name: "UQ__NguoiDun__55F68FC05F4EA5A2",
                table: "NguoiDung",
                newName: "UQ__NguoiDun__55F68FC092605992");

            migrationBuilder.RenameIndex(
                name: "UQ__KhuyenMa__152C7C5C95B7F5B4",
                table: "KhuyenMai",
                newName: "UQ__KhuyenMa__152C7C5CE4736415");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayCapNhat",
                table: "LichSuTrangThaiDonHang",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "CaLam",
                table: "LichLamViec",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__YeuThich__272330D4E7581D41",
                table: "YeuThich",
                column: "MaYT");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ThuCungN__A03CFC07E2869939",
                table: "ThuCungNhanNuoi",
                column: "MaNhanNuoi");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ThanhToa__44843A5D938BBDC7",
                table: "ThanhToanTrucTuyen",
                column: "MaTTTT");

            migrationBuilder.AddPrimaryKey(
                name: "PK__SanPham__2725081C2B6FE3D9",
                table: "SanPham",
                column: "MaSP");

            migrationBuilder.AddPrimaryKey(
                name: "PK__PhiGiaoH__3AE0AA6574C45B95",
                table: "PhiGiaoHang",
                column: "MaPhi");

            migrationBuilder.AddPrimaryKey(
                name: "PK__NhaCungC__3A185DEB15CA175D",
                table: "NhaCungCap",
                column: "MaNCC");

            migrationBuilder.AddPrimaryKey(
                name: "PK__NguoiDun__C539D762FC376884",
                table: "NguoiDung",
                column: "MaNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LichSuTr__1EBCBDED00E7051A",
                table: "LichSuTrangThaiDonHang",
                column: "MaLichSuDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LichSuHe__C443222AA4C7E898",
                table: "LichSuHeThong",
                column: "MaLichSu");

            migrationBuilder.AddPrimaryKey(
                name: "PK__LichLamV__0A7F064A0FED0DE0",
                table: "LichLamViec",
                column: "MaLichLam");

            migrationBuilder.AddPrimaryKey(
                name: "PK__KyGuiThu__2B9FA9BEAFEA1E7D",
                table: "KyGuiThuCung",
                column: "MaKyGui");

            migrationBuilder.AddPrimaryKey(
                name: "PK__KhuyenMa__2725CF159BBEBD14",
                table: "KhuyenMai",
                column: "MaKM");

            migrationBuilder.AddPrimaryKey(
                name: "PK__KhoHang__3BDA9350B366DC9B",
                table: "KhoHang",
                column: "MaKho");

            migrationBuilder.AddPrimaryKey(
                name: "PK__HoaDon__2725A6E0FD89A372",
                table: "HoaDon",
                column: "MaHD");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GioHang__2725AE8533881736",
                table: "GioHang",
                column: "MaGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DonHang__27258661FDAAA8B1",
                table: "DonHang",
                column: "MaDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DanhMuc__B37508872BE94D27",
                table: "DanhMuc",
                column: "MaDanhMuc");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DanhGia__AA9515BF7A434DCF",
                table: "DanhGia",
                column: "MaDanhGia");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChiTietG__1E4FAF546172E42D",
                table: "ChiTietGioHang",
                column: "MaCTGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChiTietD__1E4E40F0CB3D44C1",
                table: "ChiTietDonHang",
                column: "MaCTDH");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ChiTietC__1E4E48C4744EFA0F",
                table: "ChiTietChamSoc",
                column: "MaCTCS");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BaoCaoDo__25A9188C72F9778F",
                table: "BaoCaoDoanhThu",
                column: "MaBaoCao");

            migrationBuilder.AddPrimaryKey(
                name: "PK__BaiDangN__BF5D50C56E2D950B",
                table: "BaiDangNhanNuoi",
                column: "MaBaiDang");

            migrationBuilder.AddForeignKey(
                name: "FK__BaiDangNh__MaNgu__2BFE89A6",
                table: "BaiDangNhanNuoi",
                column: "MaNguoiTao",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__BaiDangNh__MaThu__2B0A656D",
                table: "BaiDangNhanNuoi",
                column: "MaThuCungNhanNuoi",
                principalTable: "ThuCungNhanNuoi",
                principalColumn: "MaNhanNuoi");

            migrationBuilder.AddForeignKey(
                name: "FK__BaoCaoDoa__Nguoi__245D67DE",
                table: "BaoCaoDoanhThu",
                column: "NguoiLap",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietCh__MaKyG__71D1E811",
                table: "ChiTietChamSoc",
                column: "MaKyGui",
                principalTable: "KyGuiThuCung",
                principalColumn: "MaKyGui");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietDon__MaDH__60A75C0F",
                table: "ChiTietDonHang",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietDon__MaSP__619B8048",
                table: "ChiTietDonHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietGio__MaGH__5812160E",
                table: "ChiTietGioHang",
                column: "MaGH",
                principalTable: "GioHang",
                principalColumn: "MaGH");

            migrationBuilder.AddForeignKey(
                name: "FK__ChiTietGio__MaSP__59063A47",
                table: "ChiTietGioHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__DanhGia__MaNguoi__0F624AF8",
                table: "DanhGia",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__DanhGia__MaSP__10566F31",
                table: "DanhGia",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__DonHang__MaKM__02FC7413",
                table: "DonHang",
                column: "MaKM",
                principalTable: "KhuyenMai",
                principalColumn: "MaKM");

            migrationBuilder.AddForeignKey(
                name: "FK__DonHang__MaNguoi__5DCAEF64",
                table: "DonHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__DonHang__MaPhi__06CD04F7",
                table: "DonHang",
                column: "MaPhi",
                principalTable: "PhiGiaoHang",
                principalColumn: "MaPhi");

            migrationBuilder.AddForeignKey(
                name: "FK__GioHang__MaNguoi__5441852A",
                table: "GioHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__HoaDon__MaDH__7B5B524B",
                table: "HoaDon",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__HoaDon__MaKeToan__7A672E12",
                table: "HoaDon",
                column: "MaKeToan",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__HoaDon__MaKyGui__7C4F7684",
                table: "HoaDon",
                column: "MaKyGui",
                principalTable: "KyGuiThuCung",
                principalColumn: "MaKyGui");

            migrationBuilder.AddForeignKey(
                name: "FK__KhoHang__MaNCC__68487DD7",
                table: "KhoHang",
                column: "MaNCC",
                principalTable: "NhaCungCap",
                principalColumn: "MaNCC");

            migrationBuilder.AddForeignKey(
                name: "FK__KhoHang__MaSP__6754599E",
                table: "KhoHang",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK__KhoHang__NguoiTh__693CA210",
                table: "KhoHang",
                column: "NguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__KyGuiThuCu__MaKH__6E01572D",
                table: "KyGuiThuCung",
                column: "MaKH",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichLamVi__MaNha__17F790F9",
                table: "LichLamViec",
                column: "MaNhanVien",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuHeT__MaNgu__1BC821DD",
                table: "LichSuHeThong",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuTra__Nguoi__208CD6FA",
                table: "LichSuTrangThaiDonHang",
                column: "NguoiThucHien",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__LichSuTran__MaDH__1F98B2C1",
                table: "LichSuTrangThaiDonHang",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__ThanhToanT__MaDH__0A9D95DB",
                table: "ThanhToanTrucTuyen",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");

            migrationBuilder.AddForeignKey(
                name: "FK__ThuCungNh__MaKyG__76969D2E",
                table: "ThuCungNhanNuoi",
                column: "MaKyGui",
                principalTable: "KyGuiThuCung",
                principalColumn: "MaKyGui");

            migrationBuilder.AddForeignKey(
                name: "FK__YeuThich__MaNguo__14270015",
                table: "YeuThich",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK__YeuThich__MaSP__151B244E",
                table: "YeuThich",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }
    }
}
