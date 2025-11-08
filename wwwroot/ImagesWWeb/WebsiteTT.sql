
CREATE DATABASE WebsiteQuanLyTrungTamAnhNgu;
GO
USE WebsiteQuanLyTrungTamAnhNgu;
GO

-- ===========================
-- 1️⃣ BẢNG VAI TRÒ & NGƯỜI DÙNG
-- ===========================
CREATE TABLE VaiTro (
    MaVaiTro INT IDENTITY PRIMARY KEY,
    TenVaiTro NVARCHAR(50) NOT NULL
);

CREATE TABLE NguoiDung (
    MaNguoiDung INT IDENTITY PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10),
    NgaySinh DATE,
    DiaChi NVARCHAR(200),
    SoDienThoai VARCHAR(15),
    Email VARCHAR(100) UNIQUE,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(100) NOT NULL,
    MaVaiTro INT FOREIGN KEY REFERENCES VaiTro(MaVaiTro),
    TrangThai NVARCHAR(20) DEFAULT N'Hoạt động',
    SoLanSaiMatKhau INT DEFAULT 0 NOT NULL,
    KhoaDenNgay DATETIME NULL,
    ResetToken NVARCHAR(255) NULL,
    ResetTokenExpiry DATETIME NULL
);

-- ===========================
-- 2️⃣ KHÓA HỌC
-- ===========================
CREATE TABLE KhoaHoc (
    MaKhoaHoc INT IDENTITY PRIMARY KEY,
    TenKhoaHoc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255),
    HocPhi DECIMAL(12,2),
    ThoiLuong NVARCHAR(50),
    CapDo NVARCHAR(50),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    ChuanDauRa NVARCHAR(MAX),
    TrangThai NVARCHAR(20) DEFAULT N'Đang mở'
);

-- ===========================
-- 3️⃣ GIÁO VIÊN
-- ===========================
CREATE TABLE GiaoVien (
    MaGiaoVien INT IDENTITY PRIMARY KEY,
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    TrinhDo NVARCHAR(100),
    KinhNghiem NVARCHAR(255),
    ChuyenMon NVARCHAR(100),
    LuongCoBan DECIMAL(12,2),
    TrangThai NVARCHAR(20) DEFAULT N'Đang giảng dạy'
);
ALTER TABLE GiaoVien
ADD 
    TrinhDo NVARCHAR(100) NULL,
    KinhNghiem NVARCHAR(255) NULL,
    LuongCoBan DECIMAL(12,2) NULL;


-- ===========================
-- 4️⃣ LỚP HỌC
-- ===========================
CREATE TABLE LopHoc (
    MaLop INT IDENTITY PRIMARY KEY,
    TenLop NVARCHAR(100),
    MaKhoaHoc INT FOREIGN KEY REFERENCES KhoaHoc(MaKhoaHoc),
    MaGiaoVien INT FOREIGN KEY REFERENCES GiaoVien(MaGiaoVien),
    SiSoToiDa INT DEFAULT 20,
    SiSoHienTai INT DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Đang học'
);

-- ===========================
-- 5️⃣ TEST ĐẦU VÀO
-- ===========================
CREATE TABLE TestDauVao (
    MaTest INT IDENTITY PRIMARY KEY,
    MaHocVien INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayTest DATE DEFAULT GETDATE(),
    DiemNghe DECIMAL(4,1),
    DiemDoc DECIMAL(4,1),
    DiemViet DECIMAL(4,1),
    DiemNguPhap DECIMAL(4,1),
    TongDiem DECIMAL(4,1),
    KhoaHocDeXuat INT NULL FOREIGN KEY REFERENCES KhoaHoc(MaKhoaHoc),
    LoTrinhHoc NVARCHAR(MAX),
    TrangThai NVARCHAR(30) DEFAULT N'Chờ xác nhận',
    HoTen NVARCHAR(100),
    Email NVARCHAR(100)
);

-- ===========================
-- 6️⃣ HỌC PHÍ
-- ===========================
CREATE TABLE HocPhi (
    MaHocPhi INT IDENTITY PRIMARY KEY,
    MaHocVien INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    MaLop INT FOREIGN KEY REFERENCES LopHoc(MaLop),
    SoTienPhaiDong DECIMAL(12,2),
    SoTienDaDong DECIMAL(12,2) DEFAULT 0,
    TrangThai NVARCHAR(20) DEFAULT N'Chưa thanh toán',
    NgayDongCuoi DATE
);

-- ===========================
-- 7️⃣ LƯƠNG GIÁO VIÊN
-- ===========================
CREATE TABLE LuongGiaoVien (
    MaLuong INT IDENTITY PRIMARY KEY,
    MaGiaoVien INT FOREIGN KEY REFERENCES GiaoVien(MaGiaoVien),
    Thang INT,
    Nam INT,
    SoBuoiDay INT,
    LuongMoiBuoi DECIMAL(10,2),
    TongLuong AS (SoBuoiDay * LuongMoiBuoi)
);

-- ===========================
-- 8️⃣ LỊCH HỌC & LỊCH THI
-- ===========================
CREATE TABLE LichHoc (
    MaLich INT IDENTITY PRIMARY KEY,
    MaLop INT FOREIGN KEY REFERENCES LopHoc(MaLop),
    NgayHoc DATE,
    GioBatDau TIME,
    GioKetThuc TIME,
    NoiDung NVARCHAR(255),
    PhongHoc NVARCHAR(100)
);

CREATE TABLE LichThi (
    MaThi INT IDENTITY PRIMARY KEY,
    MaLop INT FOREIGN KEY REFERENCES LopHoc(MaLop),
    NgayThi DATE,
    GioThi TIME,
    LoaiThi NVARCHAR(50),
    DiaDiem NVARCHAR(100)
);

-- ===========================
-- 9️⃣ TÀI LIỆU
-- ===========================
CREATE TABLE TaiLieu (
    MaTaiLieu INT IDENTITY PRIMARY KEY,
    TenTaiLieu NVARCHAR(100),
    MoTa NVARCHAR(255),
    DuongDan NVARCHAR(255),
    MaGiaoVien INT FOREIGN KEY REFERENCES GiaoVien(MaGiaoVien),
    MaLop INT FOREIGN KEY REFERENCES LopHoc(MaLop),
    NgayTaiLen DATE DEFAULT GETDATE()
);

-- ===========================
-- 🔟 ĐIỂM DANH
-- ===========================
CREATE TABLE DiemDanh (
    MaHocVien INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    MaLich INT FOREIGN KEY REFERENCES LichHoc(MaLich),
    TrangThai NVARCHAR(20) CHECK (TrangThai IN (N'Có mặt', N'Vắng', N'Muộn')),
    PRIMARY KEY (MaHocVien, MaLich)
);

-- ===========================
-- 11️⃣ LỊCH SỬ TRUY CẬP & BÁO CÁO
-- ===========================
CREATE TABLE LichSuTruyCap (
    MaLog INT IDENTITY PRIMARY KEY,
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    HanhDong NVARCHAR(255),
    ThoiGian DATETIME DEFAULT GETDATE(),
    DiaChiIP VARCHAR(50),
    MoTa NVARCHAR(255)
);

CREATE TABLE BaoCao (
    MaBaoCao INT IDENTITY PRIMARY KEY,
    LoaiBaoCao NVARCHAR(100),
    NoiDung NVARCHAR(MAX),
    NguoiLap INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayLap DATE DEFAULT GETDATE()
);

-- ===========================
-- 12️⃣ NHÂN VIÊN & LIÊN HỆ
-- ===========================
CREATE TABLE PhongDaoTao (
    MaPhongDaoTao INT IDENTITY PRIMARY KEY,
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    ChucVu NVARCHAR(100),
    GhiChu NVARCHAR(255),
    TrangThai NVARCHAR(20) DEFAULT N'Đang làm việc'
);

CREATE TABLE NhanVienLeTan (
    MaLeTan INT IDENTITY PRIMARY KEY,
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    CaLam NVARCHAR(50),
    KinhNghiem NVARCHAR(100),
    TrangThai NVARCHAR(20) DEFAULT N'Đang làm việc'
);



CREATE TABLE DangKyTuVan (
    Id INT IDENTITY PRIMARY KEY,
    HoTen NVARCHAR(100),
    Email NVARCHAR(100),
    SoDienThoai NVARCHAR(20),
    TrangThai NVARCHAR(50) DEFAULT N'Chưa liên hệ'
);

-- ===========================
-- 13️⃣ HỌC VIÊN PHỤ
-- ===========================
CREATE TABLE HocVien (
    MaHocVien INT IDENTITY PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgayDangKy DATETIME,
    Email NVARCHAR(50),
    TrangThai NVARCHAR(30),
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung)
);

-- ===========================
-- 14️⃣ PHÂN CÔNG GIẢNG DẠY
-- ===========================
CREATE TABLE PhanCongGiangDay (
    MaPhanCong INT IDENTITY(1,1) PRIMARY KEY,
    MaGiaoVien INT FOREIGN KEY REFERENCES GiaoVien(MaGiaoVien),
    MaLop INT FOREIGN KEY REFERENCES LopHoc(MaLop),
    NgayPhanCong DATE DEFAULT GETDATE(),
    GhiChu NVARCHAR(255)
);
CREATE TABLE DkHocVienLopHoc (
    MaDk INT IDENTITY(1,1) PRIMARY KEY,
    MaHocVien INT NOT NULL,
    MaLop INT NOT NULL,
    TrangThaiHoc NVARCHAR(50) NULL,
    NgayDangKy DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_DkHocVienLopHoc_HocVien FOREIGN KEY (MaHocVien)
        REFERENCES NguoiDung(MaNguoiDung),

    CONSTRAINT FK_DkHocVienLopHoc_LopHoc FOREIGN KEY (MaLop)
        REFERENCES LopHoc(MaLop)
);
ALTER TABLE DK_HocVien_LopHoc
ADD TrangThaiHoc NVARCHAR(50) NULL DEFAULT N'Chưa bắt đầu';



-- ===========================
-- 15️⃣ DỮ LIỆU MẪU
-- ===========================
INSERT INTO KhoaHoc (TenKhoaHoc, MoTa, HocPhi, ThoiLuong, CapDo)
VALUES 
(N'Cambridge English', N'Khóa luyện thi Cambridge online', 2500000, N'8 tuần', N'Cơ bản'),
(N'TOEIC Preparation', N'Luyện thi TOEIC trực tuyến', 3000000, N'10 tuần', N'Trung bình'),
(N'IELTS Intensive', N'Khóa luyện thi IELTS chuyên sâu', 3500000, N'12 tuần', N'Nâng cao');

INSERT INTO VaiTro (TenVaiTro) VALUES
(N'Admin'), (N'Kế toán'), (N'Lễ tân'), (N'Học viên'), (N'Giáo viên'), (N'Phòng đào tạo');

INSERT INTO NguoiDung (HoTen, GioiTinh, NgaySinh, DiaChi, SoDienThoai, Email, TenDangNhap, MatKhau, MaVaiTro)
VALUES 
(N'Trần Thị Diệu', N'Nữ', '1990-05-10', N'TDM, Bình Dương', '0909123456', 'admin@enlight.edu.vn', 'admin', '123', 1),
(N'Phạm Thị Hân', N'Nữ', '1993-03-25', N'TDM, Bình Dương', '0911222333', 'ketoan@enlight.edu.vn', 'ketoan', '123', 2),
(N'Nguyễn Thị Hồng Hân', N'Nữ', '1996-09-21', N'TDM, Bình Dương', '0912555777', 'letan@enlight.edu.vn', 'letan', '123', 3),
(N'Phạm Trung Tín', N'Nam', '1988-11-02', N'TDM, Bình Dương', '0909555111', 'giaovien@enlight.edu.vn', 'giaovien', '123', 5),
(N'Trần Hoài Tín', N'Nam', '1991-06-16', N'TDM, Bình Dương', '0912888999', 'phongdt@enlight.edu.vn', 'phongdt', '123', 6);


INSERT INTO GiaoVien (MaNguoiDung, TrinhDo, KinhNghiem, ChuyenMon, LuongCoBan)
SELECT MaNguoiDung, N'Thạc sĩ', N'5 năm giảng dạy IELTS', N'Ngôn ngữ Anh', 15000000 FROM NguoiDung WHERE TenDangNhap = 'giaovien';

INSERT INTO NhanVienLeTan (MaNguoiDung, CaLam, KinhNghiem)
SELECT MaNguoiDung, N'Sáng - Chiều', N'2 năm làm việc tại trung tâm ngoại ngữ' FROM NguoiDung WHERE TenDangNhap = 'letan';

INSERT INTO PhongDaoTao (MaNguoiDung, ChucVu, GhiChu)
SELECT MaNguoiDung, N'Trưởng phòng đào tạo', N'Phụ trách duyệt test và xếp lớp' FROM NguoiDung WHERE TenDangNhap = 'phongdt';
