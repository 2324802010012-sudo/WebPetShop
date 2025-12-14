using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPetShop.Migrations
{
    /// <inheritdoc />
    public partial class AddNgayThanhToanToHoaDon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK__ThanhToanT__MaDH__6FE99F9F",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ThanhToa__44843A5D0FA5E91A",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropIndex(
                name: "IX_ThanhToanTrucTuyen_MaDH",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_MaTTTT",
                table: "DonHang");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "ThanhToanTrucTuyen",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThuc",
                table: "ThanhToanTrucTuyen",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayThanhToan",
                table: "ThanhToanTrucTuyen",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<string>(
                name: "MaGiaoDich",
                table: "ThanhToanTrucTuyen",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HinhThucThanhToan",
                table: "KyGuiThuCung",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LoaiThuCung",
                table: "KyGuiThuCung",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayThanhToan",
                table: "KyGuiThuCung",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTien",
                table: "KyGuiThuCung",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiDon",
                table: "KyGuiThuCung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiThanhToan",
                table: "KyGuiThuCung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaHTTT",
                table: "DonHang",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "ChiTietPhieuXuat",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThanhToanTrucTuyen",
                table: "ThanhToanTrucTuyen",
                column: "MaTTTT");

            migrationBuilder.CreateTable(
                name: "HinhThucThanhToanThucTe",
                columns: table => new
                {
                    MaHTTT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHinhThuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucThanhToanThucTe", x => x.MaHTTT);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToanTrucTuyen_MaDH",
                table: "ThanhToanTrucTuyen",
                column: "MaDH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaHTTT",
                table: "DonHang",
                column: "MaHTTT");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_HTThanhToanThucTe",
                table: "DonHang",
                column: "MaHTTT",
                principalTable: "HinhThucThanhToanThucTe",
                principalColumn: "MaHTTT");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_KhuyenMai_MaKM",
                table: "DonHang",
                column: "MaKM",
                principalTable: "KhuyenMai",
                principalColumn: "MaKM");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_NguoiDung_MaNguoiDung",
                table: "DonHang",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_PhiGiaoHang_MaPhi",
                table: "DonHang",
                column: "MaPhi",
                principalTable: "PhiGiaoHang",
                principalColumn: "MaPhi");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHang_ThanhToanTrucTuyen",
                table: "ThanhToanTrucTuyen",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_HTThanhToanThucTe",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_KhuyenMai_MaKM",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_NguoiDung_MaNguoiDung",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_PhiGiaoHang_MaPhi",
                table: "DonHang");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHang_ThanhToanTrucTuyen",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropTable(
                name: "HinhThucThanhToanThucTe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThanhToanTrucTuyen",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropIndex(
                name: "IX_ThanhToanTrucTuyen_MaDH",
                table: "ThanhToanTrucTuyen");

            migrationBuilder.DropIndex(
                name: "IX_DonHang_MaHTTT",
                table: "DonHang");

            migrationBuilder.DropColumn(
                name: "HinhThucThanhToan",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "LoaiThuCung",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "NgayThanhToan",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "TrangThaiDon",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "TrangThaiThanhToan",
                table: "KyGuiThuCung");

            migrationBuilder.DropColumn(
                name: "MaHTTT",
                table: "DonHang");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "ThanhToanTrucTuyen",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThuc",
                table: "ThanhToanTrucTuyen",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayThanhToan",
                table: "ThanhToanTrucTuyen",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaGiaoDich",
                table: "ThanhToanTrucTuyen",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "ChiTietPhieuXuat",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK__ThanhToa__44843A5D0FA5E91A",
                table: "ThanhToanTrucTuyen",
                column: "MaTTTT");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToanTrucTuyen_MaDH",
                table: "ThanhToanTrucTuyen",
                column: "MaDH");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_MaTTTT",
                table: "DonHang",
                column: "MaTTTT");

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
                name: "FK__ThanhToanT__MaDH__6FE99F9F",
                table: "ThanhToanTrucTuyen",
                column: "MaDH",
                principalTable: "DonHang",
                principalColumn: "MaDH");
        }
    }
}
