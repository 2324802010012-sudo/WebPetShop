using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPetShop.Migrations
{
    /// <inheritdoc />
    public partial class AddSanPhamToKhuyenMai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauNhanNuoi_BaiDangNhanNuoi_MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauNhanNuoi_NguoiDung_MaKhachHangNavigationMaNguoiDung",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropIndex(
                name: "IX_YeuCauNhanNuoi_MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropPrimaryKey(
                name: "PK__KhuyenMa__2725CF15441787FD",
                table: "KhuyenMai");

            migrationBuilder.DropColumn(
                name: "MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropColumn(
                name: "MaKhachHang",
                table: "YeuCauNhanNuoi");

            migrationBuilder.RenameColumn(
                name: "MaKhachHangNavigationMaNguoiDung",
                table: "YeuCauNhanNuoi",
                newName: "MaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_YeuCauNhanNuoi_MaKhachHangNavigationMaNguoiDung",
                table: "YeuCauNhanNuoi",
                newName: "IX_YeuCauNhanNuoi_MaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "MaKM",
                table: "KhuyenMai",
                newName: "MaKm");

            migrationBuilder.RenameIndex(
                name: "UQ__KhuyenMa__152C7C5C95B7F5B4",
                table: "KhuyenMai",
                newName: "IX_KhuyenMai_MaCode");

            migrationBuilder.AddColumn<int>(
                name: "MaSP",
                table: "KhuyenMai",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuyenMai",
                table: "KhuyenMai",
                column: "MaKm");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauNhanNuoi_MaBaiDang",
                table: "YeuCauNhanNuoi",
                column: "MaBaiDang");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMai_MaSP",
                table: "KhuyenMai",
                column: "MaSP");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMai_SanPham_MaSP",
                table: "KhuyenMai",
                column: "MaSP",
                principalTable: "SanPham",
                principalColumn: "MaSP",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauNhanNuoi_BaiDangNhanNuoi_MaBaiDang",
                table: "YeuCauNhanNuoi",
                column: "MaBaiDang",
                principalTable: "BaiDangNhanNuoi",
                principalColumn: "MaBaiDang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauNhanNuoi_NguoiDung_MaNguoiDung",
                table: "YeuCauNhanNuoi",
                column: "MaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMai_SanPham_MaSP",
                table: "KhuyenMai");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauNhanNuoi_BaiDangNhanNuoi_MaBaiDang",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropForeignKey(
                name: "FK_YeuCauNhanNuoi_NguoiDung_MaNguoiDung",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropIndex(
                name: "IX_YeuCauNhanNuoi_MaBaiDang",
                table: "YeuCauNhanNuoi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuyenMai",
                table: "KhuyenMai");

            migrationBuilder.DropIndex(
                name: "IX_KhuyenMai_MaSP",
                table: "KhuyenMai");

            migrationBuilder.DropColumn(
                name: "MaSP",
                table: "KhuyenMai");

            migrationBuilder.RenameColumn(
                name: "MaNguoiDung",
                table: "YeuCauNhanNuoi",
                newName: "MaKhachHangNavigationMaNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_YeuCauNhanNuoi_MaNguoiDung",
                table: "YeuCauNhanNuoi",
                newName: "IX_YeuCauNhanNuoi_MaKhachHangNavigationMaNguoiDung");

            migrationBuilder.RenameColumn(
                name: "MaKm",
                table: "KhuyenMai",
                newName: "MaKM");

            migrationBuilder.RenameIndex(
                name: "IX_KhuyenMai_MaCode",
                table: "KhuyenMai",
                newName: "UQ__KhuyenMa__152C7C5C95B7F5B4");

            migrationBuilder.AddColumn<int>(
                name: "MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuoi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaKhachHang",
                table: "YeuCauNhanNuoi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK__KhuyenMa__2725CF15441787FD",
                table: "KhuyenMai",
                column: "MaKM");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCauNhanNuoi_MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuoi",
                column: "MaBaiDangNavigationMaBaiDang");

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauNhanNuoi_BaiDangNhanNuoi_MaBaiDangNavigationMaBaiDang",
                table: "YeuCauNhanNuoi",
                column: "MaBaiDangNavigationMaBaiDang",
                principalTable: "BaiDangNhanNuoi",
                principalColumn: "MaBaiDang",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_YeuCauNhanNuoi_NguoiDung_MaKhachHangNavigationMaNguoiDung",
                table: "YeuCauNhanNuoi",
                column: "MaKhachHangNavigationMaNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "MaNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
