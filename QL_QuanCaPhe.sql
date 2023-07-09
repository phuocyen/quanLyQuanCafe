create database QL_QuanCaPhe
go
use QL_QuanCaPhe
go

-----------Bảng loại nhân viên----------------
create table LoaiNV
(
MaLoaiNV	varchar(10),
TenLoaiNV	nvarchar(50),
constraint	pk_LoaiNV_MaLoaiNV primary key(MaLoaiNV)
)

-----------Insert Loại Nhân Viên--------------
insert into LoaiNV
values
--Mã loại NV	  Tên loại
('LNV01',		N'Full time'),
('LNV02',		N'Part time')

select * from LoaiNV

-----------Bảng nhân viên----------------
create table NhanVien
(
MaNV			varchar(10),
HoTenNV			nvarchar(max)	not null,
ChucVu			nvarchar(30),
GioiTinh		nvarchar(10)	not null,
DiaChi			nvarchar(max)	not null,
SoDienThoai		varchar(10)		not null,
QueQuan			nvarchar(50)	not null,
SoCCCD			varchar(30)		not null,
NgaySinh		date			not null,
MaLoaiNV		varchar(10),
NgayVaoLam		date			not null,
LinkHinh		nvarchar(max) default N'None', -- Mặc định link hình = None
constraint pk_NV_MaNV		primary key(MaNV),
constraint Uni_NV_SDT		unique (SoDienThoai), -- Tính duy nhất của số điện thoại
constraint Uni_NV_SoCCCD	unique (SoCCCD), -- Tính duy nhất của số căn cước công dân
constraint Ck_NV_GioiTinh	check  (GioiTinh = N'Nam' or GioiTinh = N'Nữ'),	-- Giới tính chỉ có thể = Nam hoặc nữ
constraint Ck_NV_Tuoi		check  (year(getdate()) - year(NgaySinh) >=18), -- Tuổi nhân viên >= 18 mới được nhận làm
constraint fk_NV_MaLoaiNV	foreign key(MaLoaiNV) references LoaiNV(MaLoaiNV)
)

------------Insert Nhân Viên--------------
set dateformat dmy
go
insert into NhanVien(MaNV,HoTenNV,ChucVu,GioiTinh,DiaChi,SoDienThoai,QueQuan,SoCCCD,NgaySinh,MaLoaiNV,NgayVaoLam)
values
--Mã NV		  Họ Tên			Chức vụ	   Giới tính						Địa chỉ									 Số điện thoại		Quê quán		    Số CCCD		  Ngày sinh	   Mã loại NV   Ngày vào làm	 
('NV01',  N'Trần Đức Huy',	   N'Quản lý',	 N'Nam',   N'9D Lê Thái, Q.11, P.Minh Vương, TP.Hồ Chí Minh',			 '0992582581',	N'TP.Hồ Chí Minh',	'092502950505',	 '12/01/1995',	'LNV01',	'22/12/2017'),
('NV02',  N'Tố Minh Ngọc',	   N'Pha chế',	 N'Nữ',    N'24F Lũy Bán Bích, Q.12, P.Minh Hòa, TP.Hồ Chí Minh',		 '0872582582',	N'Hà Nội',			'003202950511',	 '17/12/1996',	'LNV01',	'22/12/2018'),
('NV03',  N'Nguyễn Thái Học',  N'Lễ tân',	 N'Nữ',    N'12A Lý Thái Tổ, Q4, P.Bình Thạnh, TP.Hồ Chí Minh',			 '0112582583',	N'TP.Hồ Chí Minh',	'099872950501',	 '22/04/2000',	'LNV02',	'22/12/2015'),
('NV04',  N'Đỗ Tuấn An',	   N'Đầu bếp',	 N'Nam',   N'11 Quang Trung, Q.Gò Vấp, P.14, TP.Hồ Chí Minh',			 '0772582523',	N'Đà Nẵng',			'012502950507',	 '14/08/1990',	'LNV01',	'22/12/2019'),
('NV05',  N'Đặng Thái Duy',	   N'Phục vụ',	 N'Nam',   N'142/11X Bình Triệu, Q.10, P.Cộng Hòa, TP.Hồ Chí Minh',		 '0432582022',	N'Huế',				'023534950511',	 '17/09/1995',	'LNV02',	'22/12/2019')

select * from NhanVien

select * from NhanVien where GioiTinh = N'Nữ'

select LinkHinh from NhanVien where MaNV = 'NV02'

select count(*) from NhanVien where SoCCCD = '097534950222'

------------Thủ tục/hàm nhân viên----------------

--            2001200561_Mai Nguyễn Phước Yến làm Thủ tục (1,2,3)

--1. Thủ tục Xuất nhân viên
create proc XuatNV
as
select MaNV, HoTenNV, ChucVu, GioiTinh, DiaChi, SoDienThoai, QueQuan, SoCCCD, NgaySinh, TenLoaiNV, NgayVaoLam 
from NhanVien nv, LoaiNV lnv
where nv.MaLoaiNV = lnv.MaLoaiNV

exec XuatNV


--2. Thủ tục Xuất nhân viên theo giới tính
create proc XuatNV_GT @GT nvarchar(10)
as
select MaNV, HoTenNV, ChucVu, GioiTinh, DiaChi, SoDienThoai, QueQuan, SoCCCD, NgaySinh, TenLoaiNV, NgayVaoLam 
from NhanVien nv, LoaiNV lnv
where nv.MaLoaiNV = lnv.MaLoaiNV and GioiTinh = @GT

exec XuatNV_GT N'Nữ'


--3. Thủ tục Xuất nhân viên theo Số CCCD
create proc XuatNV_SoCCCD @SoCCCD varchar(30)
as
select MaNV, HoTenNV, ChucVu, GioiTinh, DiaChi, SoDienThoai, QueQuan, SoCCCD, NgaySinh, TenLoaiNV, NgayVaoLam 
from NhanVien nv, LoaiNV lnv
where nv.MaLoaiNV = lnv.MaLoaiNV 
and SoCCCD like '%'+@SoCCCD+'%'

exec XuatNV_SoCCCD '11'

--          2001202039_Đoàn Công Đạt làm Thủ tục (4,5,6)

--4. Thủ tục Xuất nhân viên theo họ tên
create proc XuatNV_HoTen @hoten nvarchar(30)
as
select MaNV, HoTenNV, ChucVu, GioiTinh, DiaChi, SoDienThoai, QueQuan, SoCCCD, NgaySinh, TenLoaiNV, NgayVaoLam 
from NhanVien nv, LoaiNV lnv
where nv.MaLoaiNV = lnv.MaLoaiNV 
and HoTenNV like N'%'+@hoten+'%'

exec XuatNV_HoTen N'a'


--5. Thủ tục Xuất nhân viên theo loại nhân viên
create proc XuatNV_LoaiNV @LoaiNV varchar(10)
as
select MaNV, HoTenNV, ChucVu, GioiTinh, DiaChi, SoDienThoai, QueQuan, SoCCCD, NgaySinh, TenLoaiNV, NgayVaoLam 
from NhanVien nv, LoaiNV lnv
where nv.MaLoaiNV = lnv.MaLoaiNV 
and lnv.MaLoaiNV = @LoaiNV

exec XuatNV_LoaiNV 'LNV02'


--6. Thủ tục Xuất nhân viên theo số điện thoại
create proc XuatNV_SDT @SDT varchar(20)
as
select MaNV, HoTenNV, ChucVu, GioiTinh, DiaChi, SoDienThoai, QueQuan, SoCCCD, NgaySinh, TenLoaiNV, NgayVaoLam 
from NhanVien nv, LoaiNV lnv
where nv.MaLoaiNV = lnv.MaLoaiNV 
and SoDienThoai like '%'+ @SDT+'%'

exec XuatNV_SDT '20'

select * from NhanVien

------------Bảng khách hàng----------------
create table KhachHang
(
SoDienThoai		varchar(10),
HoTen			nvarchar(max)	not null, -- Họ tên không được để trống
GioiTinh		nvarchar(10),
DiaChi			nvarchar(max),
constraint		Ck_KH_GT check  (GioiTinh = N'Nam' or GioiTinh = N'Nữ'), -- Giới tính chỉ có thể = Nam hoặc nữ
constraint pk_KH_SDT primary key(SoDienThoai)
)

------------Insert Khách Hàng--------------
insert into KhachHang
values
--Số điện thoại			Họ tên		  Giới tính						Địa chỉ
('0589345803',	  N'Đỗ Tuấn An',		N'Nam',		N'12A Chu Tấn,Q.12,P.Minh Kỳ,TP.Hồ Chí Minh'),
('0249345803',	  N'Đào Minh Công',		N'Nam',		null),
('0189345211',	  N'Huyền Minh Ngọc',	N'Nữ',		null),
('0389345844',	  N'Cao Tuấn Tài',		N'Nam',		null),
('0789345867',	  N'Đỗ Huệ Mẫn',		N'Nữ',		null),
('0489345833',	  N'Tố Anh Ngọc',		N'Nữ',		N'167 Tân Ký,Q.03,P.Châu Đức,TP.Hồ Chí Minh'),
('0201345801',	  N'Trần Thái Vũ',		N'Nam',		N'123/52 Tân Định,Q.08,P.Thiên Châu,TP.Hồ Chí Minh'),
('0689345899',	  N'Thiên Nhật Minh',	N'Nam',		null),
('0889345855',	  N'Nguyễn Thái Học',	N'Nam',		null),
('0984045803',	  N'Trần Ánh Linh',		N'Nữ',		N'16B Lũy Bán Bích,Q.12,P.Minh Hòa,TP.Hồ Chí Minh')

select * from KhachHang

update KhachHang set SoDienThoai = '', HoTen = N'', GioiTinh = N'', DiaChi = N'' where SoDienThoai = ''

select HoTen from KhachHang where SoDienThoai = ''

select count(*) from KhachHang where SoDienThoai = ''

select HoTen from KhachHang where SoDienThoai = ''

select GioiTinh from KhachHang where SoDienThoai = ''

select DiaChi from KhachHang where SoDienThoai = ''

------------Bảng tài khoản----------------
create table TaiKhoan
(
TenTK		varchar(100),
MatKhau		varchar(max) not null, -- Mật khẩu không được để trống
Quyen		nvarchar(50) not null, -- Quyền không được để trống
MANV		varchar(10),
constraint	pk_TK_TenTK	 primary key(TenTK),
constraint	fk_TK_MANV	 foreign key(MANV) references NhanVien(MANV)
)
------------Bảng Tài Khoản--------------
insert into TaiKhoan
values
--Tài khoản		Mật khẩu		 Quyền		   Mã NV
('NV03',		 '123',			N'User',	  'NV03'),
('NV01',		 '123',			N'Manage',	  'NV01'),
('Admin',		 '123',			N'Admin',	   null)

select * from TaiKhoan

update TaiKhoan set MatKhau = '' where TenTK = '' 

--------------Hàm/ Thủ tục Tài khoản-----------------

--        2001202157-Phan Dư Hoài Minh làm Thủ tục (7,8)

--7. Thủ tục Kiểm tra tài khoản có tồn tại không
create proc KT_TenTaiKhoan @tentk varchar(10)
as
begin
	select count(*) from TaiKhoan where TenTK = @tentk
end

exec KT_TenTaiKhoan 'Admin'


--8. Thủ tục Kiểm tra mật khẩu tài khoản có đúng không
create proc KT_MatKhau @tentk varchar(10), @MK varchar(max)
as
begin
	select count(*) from TaiKhoan where TenTK = @tentk and MatKhau = @MK
end

exec KT_MatKhau 'Admin', '123'

select Quyen from TaiKhoan where TenTK = 'Admin'

------------Bảng loại sản phẩm----------------
create table LoaiSP
(
MaLoaiSP	varchar(10),
TenLoai		nvarchar(100),
constraint	Uni_LoaiSP_TenLoai unique(TenLoai), -- Tên loại là duy nhất
constraint	pk_LoaiSP_MaLoaiSP primary key(MaLoaiSP)
)

------------Insert Loại Sản Phẩm--------------
insert into LoaiSP
values
--Mã loại SP		Tên loại
('LSP01',			N'Đồ ăn'),
('LSP02',			N'Đồ uống')

select * from LoaiSP

------------Bảng sản phẩm----------------
create table SanPham
(
MaSP		varchar(10),
TenSP		nvarchar(200),
MaLoaiSP	varchar(10),
Gia			int,
DonViTinh	nvarchar(30),
LinkHinh	nvarchar(max) default N'None', -- Mặc định link hình là None
constraint	Uni_SP_TenSP   unique(TenSP), -- Tên sản phẩm là duy nhất
constraint	pk_SP_MaSP	   primary key(MaSP),
constraint	fk_SP_MaLoaiSP foreign key(MaLoaiSP) references LoaiSP(MaLoaiSP)
)

------------Bảng sản phẩm--------------
insert into SanPham(MaSP,TenSP,MaLoaiSP,Gia,DonViTinh)
values
--Mã SP			Tên SP						Loại SP			Giá			Đơn vị
('SP01',	N'Bánh trứng muối',				'LSP01',		55000,		N'Dĩa'),
('SP02',	N'Cà phê trứng',				'LSP02',		45000,		N'Ly'),
('SP03',	N'Cà phê đen',					'LSP02',		30000,		N'Ly'),
('SP04',	N'Cà phê sữa matcha',			'LSP02',		68000,		N'Ly'),
('SP05',	N'Cà phê sữa',					'LSP02',		35000,		N'Ly'),
('SP06',	N'Cà phê cacao trứng sữa',		'LSP02',		62000,		N'Ly'),
('SP07',	N'Sinh tố dưa hấu',				'LSP02',		25000,		N'Ly'),
('SP08',	N'Bánh kem matcha',				'LSP01',		85000,		N'Cái'),
('SP09',	N'Bánh thịt gà sốt cay',		'LSP01',		90000,		N'Dĩa'),
('SP10',	N'Bánh kem nhân dâu',			'LSP01',		92000,		N'Dĩa'),
('SP11',	N'Sinh tố dâu',					'LSP02',		25000,		N'Ly'),
('SP12',	N'Bánh nhân phô mai chảy',		'LSP01',		75000,		N'Dĩa'),
('SP13',	N'Cà phê rượu',					'LSP02',		125000,		N'Ly'),
('SP14',	N'Khoai tây chiên',				'LSP01',		45000,		N'Hộp'),
('SP15',	N'Khoai tây chiên phô mai',		'LSP01',		50000,		N'Hộp')

select * from SanPham

------------Thủ tục/ Hàm sản phẩm----------------

--           2001202174_Phan Nguyễn làm Thủ tục (9, 10, 11, 14, 15)

--9. Thủ tục Xuất sản phẩm
create proc XuatSP
as
select MaSP, TenSP, TenLoai, Gia, DonViTinh
from SanPham sp, LoaiSP lsp 
where sp.MaLoaiSP = lsp.MaLoaiSP

exec XuatSP


--10. Thủ tục Xuất sản phẩm theo tên sản phẩm
create proc XuatSP_TenSP @TenSP nvarchar(200)
as
select MaSP, TenSP, TenLoai, Gia, DonViTinh
from SanPham sp, LoaiSP lsp 
where sp.MaLoaiSP = lsp.MaLoaiSP
and TenSP like N'%'+@TenSP+'%'

exec XuatSP_TenSP N'dâu'

--11. Thủ tục Xuất sản phẩm theo loại sản phẩm
create proc XuatSP_LoaiSP @LoaiSP varchar(10)
as
select MaSP, TenSP, TenLoai, Gia, DonViTinh
from SanPham sp, LoaiSP lsp 
where sp.MaLoaiSP = lsp.MaLoaiSP
and lsp.MaLoaiSP = @LoaiSP

exec XuatSP_LoaiSP 'LSP02'

--         2001202252_Mai Ngọc Thiện làm Thủ tục (12, 13)

--12. Thủ tục Xuất sản phẩm theo giá tăng dần
create proc XuatSP_GiaTang
as
select MaSP, TenSP, TenLoai, Gia, DonViTinh
from SanPham sp, LoaiSP lsp 
where sp.MaLoaiSP = lsp.MaLoaiSP
order by Gia asc, TenSP asc

exec XuatSP_GiaTang

--13. Thủ tục Xuất sản phẩm theo giá giảm dần
create proc XuatSP_GiaGiam
as
select MaSP, TenSP, TenLoai, Gia, DonViTinh
from SanPham sp, LoaiSP lsp 
where sp.MaLoaiSP = lsp.MaLoaiSP
order by Gia desc, TenSP asc

exec XuatSP_GiaGiam

select LinkHinh from SanPham where MaSP = 'SP02'

------------Bảng hóa đơn----------------
create table HoaDon
(
MaHD			varchar(10),
SDTKH			varchar(10),
TenTK_HD		varchar(100),
ThoiGian		datetime,
ThanhToan		int default 0, -- Mặc định thanh toán là 0
constraint pk_HD_MaHD	primary key(MaHD),
constraint fk_HD_SDTKH	foreign key(SDTKH) references KhachHang(SoDienThoai),
constraint fk_HD_TenTK_HD	foreign key(TenTK_HD) references TaiKhoan(TenTK),
)

set dateformat dmy update HoaDon set SDTKH = '', TenTK_HD = '', ThoiGian = '' where MaHD = ''

insert into HoaDon(MaHD) values('')

select count(*) from HoaDon where ThoiGian is not null and MaHD = 'HD06'

select count(*) from HoaDon where MaHD = 'HD06'

------------Insert Bảng hóa đơn--------------
set dateformat dmy 
go
insert into HoaDon(MaHD, SDTKH, TenTK_HD, ThoiGian)
values
--Mã HD		Số điện thoại khách		 Tên tài khoản			Thời Gian			
('HD01',		null,					'NV01',			'10/12/2022 10:35:09'),
('HD02',		'0589345803',			'NV03',			'10/12/2022 14:25:10'),
('HD03',		'0189345211',			'NV01',			'11/12/2022 10:35:09'),
('HD04',		null,					'NV03',			'12/11/2022 11:45:15'),
('HD05',		'0201345801',			'NV03',			'13/12/2022 12:55:21')

select * from HoaDon

--14. Thủ tục tính tiền thanh toán cho hóa đơn---------------
create proc CapNhatThanhToan @MaHD varchar(10)
as
update HoaDon
set ThanhToan = 
(select sum(TongTien_CTHD) 
from HoaDon hd, ChiTietHD cthd 
where hd.MaHD = cthd.MaCTHD 
and hd.MaHD = @MaHD)
from HoaDon where MaHD = @MaHD

exec CapNhatThanhToan 'HD05'

--15. Thủ tục in hóa đơn---------------
create proc InHoaDon @MaHD varchar(10)
as
select TenSP,Gia,DonViTinh,SoLuong,TongTien_CTHD 
from ChiTietHD cthd, SanPham sp
where cthd.MaSP_CTHD = sp.MaSP
and MaCTHD = @MaHD

exec InHoaDon 'HD02'

select * from HoaDon

select sum(SoLuong) from ChiTietHD where MaCTHD = 'HD01'

select ThanhToan from HoaDon where MaHD =''

select HoTen FROM KhachHang where SoDienThoai = ''

------------Bảng chi tiết hóa đơn-------------
create table ChiTietHD
(
MaCTHD		varchar(10),
MaSP_CTHD		varchar(10),
SoLuong			int default 0, -- Mặc định số lượng là 0
TongTien_CTHD	int default 0, -- Mặc định tổng tiền là 0
constraint pk_CTHD_MaHD_MaSP primary key(MaCTHD, MaSP_CTHD),
constraint fk_CTHD_MaSP_MaCTHD foreign key(MaCTHD) references HoaDon(MaHD),
constraint fk_CTHD_MaSP foreign key(MaSP_CTHD) references SanPham(MaSP)
)

------------Insert Bảng chi tiết hóa đơn--------------
insert into ChiTietHD(MaCTHD, MaSP_CTHD, SoLuong)
values
--Mã CTHD			Mã SP	  Số lượng
('HD01',		'SP01',		2),
('HD01',		'SP03',		3),
('HD01',		'SP06',		1),
('HD02',		'SP11',		1),
('HD02',		'SP12',		1),
('HD03',		'SP15',		2),
('HD03',		'SP09',		2),
('HD03',		'SP07',		1),
('HD04',		'SP03',		5),
('HD05',		'SP02',		3)

select * from ChiTietHD

select * from ChiTietHD where MaCTHD = 'HD02'

----------------------Truy vấn---------------------

select * from LoaiSP

select * from LoaiNV

select * from NhanVien

select * from KhachHang

select * from TaiKhoan

select * from SanPham

select * from HoaDon

select * from ChiTietHD

--------------------Trigger cập nhật tổng tiền cho chi tiết hóa đơn-----------------
create trigger CapNhatTongTien_CTHD on ChiTietHD
for insert, update
as
update ChiTietHD
set TongTien_CTHD = SoLuong * Gia
from ChiTietHD cthd, SanPham sp
where cthd.MaSP_CTHD = sp.MaSP
and MaCTHD = (select MaCTHD from inserted)

insert ChiTietHD(MaCTHD, MaSP, SoLuong) values('HD01','SP07',3)

update ChiTietHD set SoLuong = 3 where MaCTHD = 'HD05' and MaSP_CTHD = 'SP02'

select * from ChiTietHD




-------------Công việc trong nhóm-----------

--1.	2001202174, Phan Nguyễn
--	Tạo bảng loại sản phẩm, sản phẩm và nhập liệu cho 2 bảng đó
--	Tạo thủ tục :
--		Xuất sản phẩm 
--		Xuất sản phẩm theo tên sản phẩm
--		Xuất sản phẩm theo loại sản phẩm
--		Tính tiền thanh toán cho hóa đơn
--		In hóa đơn
--	Tạo trigger :
--		Cập nhật tổng tiền cho chi tiết hóa đơn
--	Cài đặt ứng dụng :
--		Thiết kế form order, xuất hóa đơn, thông tin nhân viên và xây dựng các chức năng đi kèm


--2.	2001200561, Mai Nguyễn Phước Yến
--	Tạo bảng tài khoản và nhập liệu cho bảng đó
--	Tạo thủ tục :
--		Xuất nhân viên 
--		Xuất nhân viên theo giới tính
--		Xuất nhân viên theo số căn cước công dân
--	Cài đặt ứng dụng :
--		Thiết kế form quản lý khách hàng và xây dựng các chức năng đi kèm
--		Thiết kế form đăng xuất và xây dựng các chức năng đi kèm


--3.	2001202157, Phan Dư Hoài Minh
--	Tạo bảng nhân viên, loại nhân viên và nhập liệu cho 2 bảng đó
--	Tạo thủ tục :
--		Kiểm tra tài khoản có tồn tại không
--		Kiểm tra mất khẩu tài khoản có đúng không
--	Cài đặt ứng dụng :
--		Thiết kế form quản lý tài khoản, và xây dựng các chức năng đi kèm
--		Thiết kế form đổi mật khẩu, đăng nhập và xây dựng các chức năng đi kèm


--4.	2001202039, Đoàn Công Đạt
--	Tạo bảng khách hàng và nhập liệu cho bảng
--	Tạo thủ tục :
--		Xuất nhân viên theo họ tên
--		Xuất nhân viên theo loại nhân viên
--		Xuất nhân viên theo số điện thoại
--	Cài đặt ứng dụng :
--		Thiết kế form quản lý nhân viên, sản phẩm và xây dựng các chức năng đi kèm

--5.	2001202252, Mai Ngọc Thiện
--	Tạo bảng hóa đơn, chi tiết hóa đơn và nhập liệu cho 2 bảng đó
--	Tạo thủ tục :
--		Xuất sản phẩm theo giá tăng dần
--		Xuất sản phẩm theo giá giảm dần
--	Cài đặt ứng dụng :
--		Thiết kế form quản lý hóa đơn và xây dựng các chức năng đi kèm
--		Thiết kế form chi tiết hóa đơn, thêm khách hàng và xây dựng các chức năng đi kèm

