using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPetShop.Migrations
{
    /// <inheritdoc />
    public partial class Add_YeuCauNhanNuoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_NguoiDung_NguoiDungMaNguoiDung",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_SanPham_SanPhamMaSp",
                table: "DanhGias");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DanhGia__AA9515BF53F49DC4",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_NguoiDungMaNguoiDung",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_SanPhamMaSp",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "SanPhamMaSp",
                table: "DanhGias");

            migrationBuilder.RenameTable(
                name: "DanhGias",
                newName: "DanhGia");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayDanhGia",
                table: "DanhGia",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<string>(
                name: "HinhAnh",
                table: "DanhGia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DanhGia",
                table: "DanhGia",
                column: "MaDanhGia");

            migrationBuilder.CreateTable(
                name: "YeuCauNhanNuois",
                columns: table => new
                {
                    MaYeuCau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiDang = table.Column<int>(type: "int", nullable: false),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    NgayYeuCau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaBaiDangNavigationMaBaiDang = table.Column<int>(type: "int", nullable: false),
                    MaKhachHangNavigationMaNguoiDung = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCauNhanNuois", x => x.MaYeuCau);
                    table.ForeignKey(
                        name: "FK_YeuCauNhanNuois_BaiDangNhanNuoi_MaBaiDangNavigationMaBaiDang",
                        column: x => x.MaBaiDangNavigationMaBaiDang,
                        principalTable: "BaiDangNhanNuoi",
                        principalColumn: "MaBaiDang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuCauNhanNuois_NguoiDung_MaKhachHangNavigationMaNguoiDung",
                        column: x => x.MaKhachHangNavigationMaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaNguoiDung",
                table: "DanhGia",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_MaSP",
                table: "DanhGia",
                column: "MaSP");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauNhanNuois_MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuois",
                column: "MaBaiDangNavigationMaBaiDang");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauNhanNuois_MaKhachHangNavigationMaNguoiDung",
                table: "YeuCauNhanNuois",
                column: "MaKhachHangNavigationMaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_NguoiDung",
                table: "DanhGia",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_SanPham",
                table: "DanhGia",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_NguoiDung",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_SanPham",
                table: "DanhGia");

            migrationBuilder.DropTable(
                name: "YeuCauNhanNuois");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DanhGia",
                table: "DanhGia");

            migrationBuilder.DropIndex(
                name: "IX_DanhGia_MaNguoiDung",
                table: "DanhGia");

            migrationBuilder.DropIndex(
                name: "IX_DanhGia_MaSP",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "HinhAnh",
                table: "DanhGia");

            migrationBuilder.RenameTable(
                name: "DanhGia",
                newName: "DanhGias");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayDanhGia",
                table: "DanhGias",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "DanhGias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SanPhamMaSp",
                table: "DanhGias",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__DanhGia__AA9515BF53F49DC4",
                table: "DanhGias",
                column: "MaDanhGia");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_NguoiDungMaNguoiDung",
                table: "DanhGias",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SanPhamMaSp",
                table: "DanhGias",
                column: "SanPhamMaSp");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_NguoiDung_NguoiDungMaNguoiDung",
                table: "DanhGias",
                column: "NguoiDungMaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_SanPham_SanPhamMaSp",
                table: "DanhGias",
                column: "SanPhamMaSp",
                principalTable: "SanPham",
                principalColumn: "MaSP");
        }
    }
}
