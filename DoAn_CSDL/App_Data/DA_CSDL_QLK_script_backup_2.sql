USE [master]
GO
/****** Object:  Database [DA_CSDL_QLK]    Script Date: 12/1/2021 10:13:05 AM ******/
CREATE DATABASE [DA_CSDL_QLK]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DA_CSDL_QLK', FILENAME = N'D:\TaiLieuHocTap\20211\DoAnCSDL\DA_CSDL_QLK.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DA_CSDL_QLK_log', FILENAME = N'D:\TaiLieuHocTap\20211\DoAnCSDL\DA_CSDL_QLK_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DA_CSDL_QLK] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DA_CSDL_QLK].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DA_CSDL_QLK] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET ARITHABORT OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [DA_CSDL_QLK] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DA_CSDL_QLK] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DA_CSDL_QLK] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DA_CSDL_QLK] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DA_CSDL_QLK] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DA_CSDL_QLK] SET  MULTI_USER 
GO
ALTER DATABASE [DA_CSDL_QLK] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DA_CSDL_QLK] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DA_CSDL_QLK] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DA_CSDL_QLK] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DA_CSDL_QLK] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DA_CSDL_QLK] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [DA_CSDL_QLK] SET QUERY_STORE = OFF
GO
USE [DA_CSDL_QLK]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_IsOwnKho]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_IsOwnKho] (@userID int, @khoID int)
RETURNS bit
AS
BEGIN
	DECLARE @count int
	SELECT @count = COUNT(tbl_Kho.ID)
	FROM tbl_TaiKhoan 
		join tbl_HT_TK ON tbl_TaiKhoan.ID = tbl_HT_TK.TaiKhoanID
		join tbl_Kho ON tbl_HT_TK.HeTHongID = tbl_Kho.HeTHongID
	WHERE tbl_TaiKhoan.ID = @userID And tbl_Kho.ID = @khoID

	IF(@count > 0) RETURN 1 -- have permission

	RETURN 0 -- do not have permission
END
	
GO
/****** Object:  UserDefinedFunction [dbo].[pr_Login]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION  [dbo].[pr_Login](@UserName VARCHAR(300), @Password VARCHAR(300)) RETURNS  INT
AS
BEGIN



		IF  ( (SELECT PhanQuyenID 
				FROM dbo.tbl_TaiKhoan 
				WHERE dbo.tbl_TaiKhoan.UserName=@UserName) IS NULL)
				RETURN -3 ELSE
		BEGIN
       
		IF ( (SELECT PhanQuyenID
				FROM dbo.tbl_TaiKhoan 
				WHERE dbo.tbl_TaiKhoan.Pass=@Password AND dbo.tbl_TaiKhoan.UserName=@UserName) IS NULL) 
				RETURN -1
				
				ELSE
		BEGIN
		    RETURN (SELECT TOP(1) ID 
				FROM dbo.tbl_TaiKhoan 
				WHERE dbo.tbl_TaiKhoan.UserName=@UserName AND dbo.tbl_TaiKhoan.Pass=@Password
				ORDER BY ID
			) ;
			
		END;
		END;
	
	 
	 RETURN -1;
END;
 
 
GO
/****** Object:  Table [dbo].[tbl_TaiKhoan]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TaiKhoan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](300) NOT NULL,
	[Pass] [varchar](300) NOT NULL,
	[HoTen] [nvarchar](300) NOT NULL,
	[NgaySinh] [date] NOT NULL,
	[DiaChi] [nvarchar](500) NULL,
	[SDT] [varchar](12) NOT NULL,
	[ChucVu] [nvarchar](300) NULL,
	[PhanQuyenID] [int] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[Token] [nvarchar](500) NULL,
	[Img] [ntext] NULL,
 CONSTRAINT [PK__tbl_TaiK__3214EC27A7FE9582] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_TaiKhoan]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_TaiKhoan]
AS
SELECT        UserName, HoTen, NgaySinh, DiaChi, SDT, ChucVu, PhanQuyenID, NgayTao, IsDelete, NgayCapNhat, Img, Token, ID, Pass
FROM            dbo.tbl_TaiKhoan
GO
/****** Object:  Table [dbo].[tbl_HeThong]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_HeThong](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[DiaChi] [nvarchar](500) NOT NULL,
	[SDT] [varchar](12) NOT NULL,
	[STK] [varchar](30) NOT NULL,
	[NganHang] [nvarchar](300) NOT NULL,
	[TaiKhoanID] [int] NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_HeThong]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_HeThong]
AS
SELECT        dbo.tbl_HeThong.ID, dbo.tbl_HeThong.Ten, dbo.tbl_HeThong.DiaChi, dbo.tbl_HeThong.SDT, dbo.tbl_HeThong.STK, dbo.tbl_HeThong.NganHang, dbo.tbl_HeThong.NgayTao, dbo.tbl_HeThong.NgayCapNhat, 
                         dbo.tbl_HeThong.IsDelete, dbo.tbl_HeThong.TaiKhoanID, dbo.tbl_TaiKhoan.UserName, dbo.tbl_TaiKhoan.HoTen, dbo.tbl_TaiKhoan.Img
FROM            dbo.tbl_HeThong LEFT OUTER JOIN
                         dbo.tbl_TaiKhoan ON dbo.tbl_HeThong.TaiKhoanID = dbo.tbl_TaiKhoan.ID
GO
/****** Object:  Table [dbo].[tbl_LinkMenu]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LinkMenu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PhanQuyen] [int] NOT NULL,
	[MenuID] [int] NOT NULL,
 CONSTRAINT [PK_tbl_LinkMenu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Menu]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Menu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[reID] [int] NULL,
	[Content] [nvarchar](500) NULL,
	[Role] [nchar](10) NULL,
	[Order] [int] NOT NULL,
	[Active] [bit] NULL,
	[Link] [nvarchar](500) NOT NULL,
	[Icon] [nvarchar](500) NULL,
 CONSTRAINT [PK_tbl_Menu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_Menu]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_Menu]
AS
SELECT        dbo.tbl_Menu.ID, dbo.tbl_Menu.Icon, dbo.tbl_Menu.Title, dbo.tbl_Menu.reID, dbo.tbl_Menu.[Content], dbo.tbl_Menu.Role, dbo.tbl_Menu.[Order], dbo.tbl_Menu.Active, dbo.tbl_Menu.Link, dbo.tbl_LinkMenu.PhanQuyen
FROM            dbo.tbl_Menu INNER JOIN
                         dbo.tbl_LinkMenu ON dbo.tbl_Menu.ID = dbo.tbl_LinkMenu.MenuID
GO
/****** Object:  Table [dbo].[tbl_PhanQuyen]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PhanQuyen](
	[ID] [int] NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[GhiChu] [nvarchar](max) NULL,
	[NgayTao] [datetime] NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NULL,
 CONSTRAINT [PK__tbl_Phan__3214EC270C758789] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_PhanQuyen]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_PhanQuyen]
AS
SELECT        ID, Ten, GhiChu, NgayTao, NgayCapNhat, IsDelete
FROM            dbo.tbl_PhanQuyen
GO
/****** Object:  Table [dbo].[tbl_Kho]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Kho](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[DiaChi] [nvarchar](500) NOT NULL,
	[HeThongID] [int] NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_Kho]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_Kho]
AS
SELECT        dbo.tbl_HeThong.Ten AS TenHeThong, dbo.tbl_HeThong.DiaChi AS DiaChiHeThong, dbo.tbl_Kho.ID, dbo.tbl_Kho.Ten, dbo.tbl_Kho.DiaChi, dbo.tbl_Kho.HeThongID, dbo.tbl_Kho.NgayTao, dbo.tbl_Kho.NgayCapNhat, 
                         dbo.tbl_Kho.IsDelete
FROM            dbo.tbl_Kho INNER JOIN
                         dbo.tbl_HeThong ON dbo.tbl_Kho.HeThongID = dbo.tbl_HeThong.ID
GO
/****** Object:  Table [dbo].[tbl_ViTri]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ViTri](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HangID] [int] NOT NULL,
	[CotID] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[KhoID] [int] NULL,
	[NgayTao] [datetime] NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_ViTri]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_ViTri]
AS
SELECT        dbo.tbl_ViTri.*, dbo.tbl_Kho.Ten, dbo.tbl_Kho.DiaChi
FROM            dbo.tbl_ViTri INNER JOIN
                         dbo.tbl_Kho ON dbo.tbl_ViTri.KhoID = dbo.tbl_Kho.ID
GO
/****** Object:  Table [dbo].[tbl_NhaCungCap]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NhaCungCap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[DiaChi] [nvarchar](500) NOT NULL,
	[SDT] [varchar](12) NOT NULL,
	[STK] [varchar](30) NOT NULL,
	[NganHang] [nvarchar](300) NOT NULL,
	[TaiKhoanID] [int] NULL,
	[NgayTao] [datetime] NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK__tbl_NhaC__3214EC27B83E5C3F] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_NhaCungCap]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_NhaCungCap]
AS
SELECT        dbo.tbl_NhaCungCap.*, dbo.tbl_TaiKhoan.UserName, dbo.tbl_TaiKhoan.HoTen
FROM            dbo.tbl_NhaCungCap INNER JOIN
                         dbo.tbl_TaiKhoan ON dbo.tbl_NhaCungCap.TaiKhoanID = dbo.tbl_TaiKhoan.ID
GO
/****** Object:  Table [dbo].[tbl_HopDong]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_HopDong](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NgayLap] [datetime] NOT NULL,
	[NgayHT] [datetime] NULL,
	[TrangThai] [int] NOT NULL,
	[NguoiLap] [int] NULL,
	[NhaCungCapID] [int] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_HopDong]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_HopDong]
AS
SELECT        dbo.tbl_HopDong.ID, dbo.tbl_HopDong.NgayLap, dbo.tbl_HopDong.NgayHT, dbo.tbl_HopDong.TrangThai, dbo.tbl_HopDong.NguoiLap, dbo.tbl_HopDong.NhaCungCapID, dbo.tbl_HopDong.IsDelete, dbo.tbl_TaiKhoan.UserName, 
                         dbo.tbl_TaiKhoan.HoTen, dbo.tbl_NhaCungCap.Ten, dbo.tbl_NhaCungCap.DiaChi, dbo.tbl_NhaCungCap.SDT
FROM            dbo.tbl_HopDong INNER JOIN
                         dbo.tbl_TaiKhoan ON dbo.tbl_HopDong.NguoiLap = dbo.tbl_TaiKhoan.ID INNER JOIN
                         dbo.tbl_NhaCungCap ON dbo.tbl_HopDong.NhaCungCapID = dbo.tbl_NhaCungCap.ID
GO
/****** Object:  Table [dbo].[tbl_ChiTietHD]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ChiTietHD](
	[HopDongID] [int] NOT NULL,
	[MatHangID] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[HopDongID] ASC,
	[MatHangID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ChiTietPN]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ChiTietPN](
	[PhieuNhapID] [int] NOT NULL,
	[MatHangID] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[ViTriID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MatHangID] ASC,
	[PhieuNhapID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ChiTietPX]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ChiTietPX](
	[PhieuXuatID] [int] NOT NULL,
	[MatHangID] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PhieuXuatID] ASC,
	[MatHangID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CuaHang]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CuaHang](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[DiaChi] [nvarchar](500) NOT NULL,
	[SDT] [varchar](12) NOT NULL,
	[STK] [varchar](30) NOT NULL,
	[NganHang] [nvarchar](300) NOT NULL,
	[TaiKhoanID] [int] NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_HT_TK]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_HT_TK](
	[TaiKhoanID] [int] NOT NULL,
	[HeTHongID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TaiKhoanID] ASC,
	[HeTHongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_LoaiMatHang]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LoaiMatHang](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[GhiChu] [nvarchar](max) NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK__tbl_Loai__3214EC27B9BC6C07] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_MatHang]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_MatHang](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ten] [nvarchar](300) NOT NULL,
	[MoTa] [nvarchar](max) NOT NULL,
	[Gia] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[NhaCungCapID] [int] NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[IsDelete] [bit] NOT NULL,
 CONSTRAINT [PK__tbl_MatH__3214EC27D29522B5] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Message]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Message](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaTK] [varchar](max) NULL,
	[Title] [nvarchar](500) NULL,
	[Content] [nvarchar](max) NULL,
	[Created] [datetime] NULL,
	[IsDelete] [bit] NULL,
	[IsSeen] [bit] NULL,
 CONSTRAINT [PK_tbl_Message] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_MH_LMH]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_MH_LMH](
	[LoaiMatHangID] [int] NOT NULL,
	[MatHangID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LoaiMatHangID] ASC,
	[MatHangID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Notification]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Notification](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](300) NOT NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[ThoiGian] [datetime] NOT NULL,
	[TaiKhoanID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PhieuNhap]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PhieuNhap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NguoiLap] [int] NULL,
	[NgayLap] [datetime] NOT NULL,
	[NguoiGiao] [nvarchar](300) NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[HopDongID] [int] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PhieuXuat]    Script Date: 12/1/2021 10:13:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PhieuXuat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NgayLap] [datetime] NOT NULL,
	[NguoiDuyet] [int] NULL,
	[NgayDuyet] [datetime] NULL,
	[NgayHT] [datetime] NULL,
	[TrangThai] [int] NOT NULL,
	[CuaHangID] [int] NULL,
	[IsDelete] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_HeThong] ON 

INSERT [dbo].[tbl_HeThong] ([ID], [Ten], [DiaChi], [SDT], [STK], [NganHang], [TaiKhoanID], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (1, N'Hệ thống 1', N'Hà Nội', N'0978236732', N'91241791423', N'Agribank', 8, CAST(N'2021-09-17T17:17:59.563' AS DateTime), CAST(N'2021-09-17T17:17:59.563' AS DateTime), 1)
INSERT [dbo].[tbl_HeThong] ([ID], [Ten], [DiaChi], [SDT], [STK], [NganHang], [TaiKhoanID], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (4, N'Hệ thống giám sát tự động', N'Hà Nội', N'0978236732', N'91241791423', N'Agribank', 8, CAST(N'2021-09-17T17:26:42.463' AS DateTime), CAST(N'2021-11-24T12:47:10.647' AS DateTime), 0)
INSERT [dbo].[tbl_HeThong] ([ID], [Ten], [DiaChi], [SDT], [STK], [NganHang], [TaiKhoanID], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (5, N'Hệ thống 3', N'Hà Nội', N'0895734633', N'7214241165', N'Tehcgash', 9, CAST(N'2021-09-18T21:02:24.493' AS DateTime), CAST(N'2021-09-18T21:02:24.493' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[tbl_HeThong] OFF
GO
INSERT [dbo].[tbl_HT_TK] ([TaiKhoanID], [HeTHongID]) VALUES (8, 4)
GO
SET IDENTITY_INSERT [dbo].[tbl_Kho] ON 

INSERT [dbo].[tbl_Kho] ([ID], [Ten], [DiaChi], [HeThongID], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (1, N'Kho hàng A', N'Local', 4, CAST(N'2021-11-25T16:51:20.880' AS DateTime), CAST(N'2021-11-25T16:51:20.880' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[tbl_Kho] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_LinkMenu] ON 

INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (1, 2, 1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (2, 2, 2)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (3, 2, 3)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (4, 2, 5)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (5, 2, 6)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (6, 2, 7)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (7, 2, 10)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (8, 2, 11)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (9, 2, 13)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (10, 2, 15)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (11, 2, 16)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (12, 2, 18)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (13, 2, 19)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (14, 2, 21)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (15, 2, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (16, 2, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (17, 1, 1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (18, 1, 2)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (19, 1, 3)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (20, 1, 4)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (21, 1, 5)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (22, 1, 6)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (23, 1, 7)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (24, 1, 9)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (26, 1, 10)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (27, 1, 30)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (28, 1, 11)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (29, 1, 13)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (30, 1, 15)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (31, 1, 16)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (32, 1, 17)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (33, 1, 18)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (34, 1, 19)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (35, 1, 20)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (36, 1, 21)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (37, 1, 22)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (38, 1, 23)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (39, 1, 25)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (40, 4, 1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (41, 4, 6)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (42, 4, 7)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (43, 4, 9)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (44, 4, 10)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (45, 4, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (46, 4, 13)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (47, 4, 18)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (48, 4, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (49, 3, 1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (50, 3, 2)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (51, 3, 5)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (52, 3, 15)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (53, 3, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (54, 3, 26)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (55, 3, 3)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (56, 4, 27)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (57, 4, 21)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (58, 3, 21)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (59, 0, 1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (60, 0, 2)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (61, 0, 3)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (62, 0, 4)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (63, 0, 5)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (64, 0, 6)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (65, 0, 7)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (66, 0, 9)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (67, 0, 10)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (68, 0, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (69, 0, 11)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (70, 0, 13)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (71, 0, 15)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (72, 0, 16)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (73, 0, 17)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (74, 0, 18)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (75, 0, 19)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (76, 0, 20)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (77, 0, 31)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (78, 0, 22)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (79, 0, 23)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (80, 0, 25)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (81, 0, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (82, 0, -1)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (83, 0, 32)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (84, 0, 33)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (85, 0, 34)
INSERT [dbo].[tbl_LinkMenu] ([ID], [PhanQuyen], [MenuID]) VALUES (86, 0, 35)
SET IDENTITY_INSERT [dbo].[tbl_LinkMenu] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_Menu] ON 

INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (1, N'Trang chủ', -1, N'', N'1111111111', -1, 1, N'/', N'<i class="fas fa-home"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (2, N'Hợp đồng mua hàng', -1, N'', N'1111111111', 0, 1, N'javascript: void(0)', N'<i class="fas fa-file-contract"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (3, N'Danh sách hợp đồng', 2, N'', N'1111111111', 0, 1, N'/HopDong/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (4, N'Thêm hợp đồng', 2, N'', N'1111111111', 1, 1, N'/HopDong/Create', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (5, N'Danh sách phiếu nhập', 2, N'', N'1111111111', 2, 1, N'/PhieuNhap/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (6, N'Yêu cầu xuất hàng', -1, N'', N'1111111111', 2, 1, N'javascript: void(0)', N'<i class="fas fa-file-contract"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (7, N'Danh sách yêu cầu', 6, N'', N'1111111111', 0, 1, N'/PhieuXuat/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (9, N'Thêm yêu cầu', 6, N'', N'1111111111', 1, 1, N'/PhieuXuat/Create', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (10, N'Hàng trong kho', -1, N'', N'1111111111', 4, 1, N'javascript: void(0)', N'<i class="fas fa-dolly"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (11, N'Danh sách hàng', 10, N'', N'1111111111', 0, 1, N'/SanPham/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (13, N'Danh sách mặt hàng', 10, N'', N'1111111111', 1, 1, N'/LoaiSanPham/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (15, N'Nhà cung cấp', -1, N'', N'1111111111', 1, 1, N'javascript: void(0)', N'<i class="fas fa-search"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (16, N'Danh sách nhà cung cấp', 15, N'', N'1111111111', 0, 1, N'/NCC/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (17, N'Thêm nhà cung cấp', 15, N'', N'1111111111', 1, 1, N'/NCC/Create', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (18, N'Cửa hàng', -1, N'', N'1111111111', 3, 1, N'javascript: void(0)', N'<i class="fas fa-store"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (19, N'Danh sách cửa hàng', 18, N'', N'1111111111', 0, 1, N'/CuaHang/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (20, N'Thêm cửa hàng', 18, N'', N'1111111111', 1, 1, N'/CuaHang/Create', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (21, N'Thông tin hệ thống', -1, N'', N'1111111111', 8, 1, N'/Home/SystemDetail', N'<i class="fas fa-tools"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (22, N'Tài khoản', -1, N'', N'1111111111', 6, 1, N'javascript: void(0)', N'<i class="fas fa-users"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (23, N'Danh sách tài khoản', 22, N'', N'1111111111', 0, 1, N'/TaiKhoan/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (25, N'Thêm tài khoản', 22, N'', N'1111111111', 1, 1, N'/TaiKhoan/Create', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (26, N'Thông tin nhà cung cấp', 15, N'', N'1111111111', -1, 1, N'/NCC/Detail', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (27, N'Thông tin cửa hàng', 18, N'', N'1111111111', -1, 1, N'/CuaHang/Detail', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (28, N'Đặt lại trạng thái hệ thống', -1, N'', N'1111111111', 9, 1, N'/Home/SystemReset', N'<i class="fas fa-radiation-alt"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (30, N'Thống kê sản phẩm', -1, N'', N'1111111111', 7, 1, N'/LoaiSanPham/Report', N'<i class="fas fa-signal"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (31, N'Hệ thống', -1, N'', N'1111111111', 8, 1, N'javascript: void(0)', N'<i class="fas fa-tools"></i>')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (32, N'Danh sách hệ thống', 31, N'', N'1111111111', 0, 1, N'/HeThong/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (33, N'Thêm hệ thống', 31, N'', N'1111111111', 1, 1, N'/HeThong/Create', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (34, N'Danh sách kho hàng', 31, N'', N'1111111111', 2, 1, N'/Kho/Index', N'')
INSERT [dbo].[tbl_Menu] ([ID], [Title], [reID], [Content], [Role], [Order], [Active], [Link], [Icon]) VALUES (35, N'Thêm mới kho hàng', 31, N'', N'1111111111', 3, 1, N'/Kho/Create', N'')
SET IDENTITY_INSERT [dbo].[tbl_Menu] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_Message] ON 

INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (31, N'USER3', N'Hợp đồng HD1 đã được tạo!', N'Hợp đồng HD1 đã được tạo vào 08:34 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD1">Tại đây</a>!', CAST(N'2021-08-15T08:34:31.230' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (32, N'USER3', N'Hợp đồng HD1 đã được cập nhật danh sách sản phẩm!', N'Hợp đồng HD1 đã cập nhật vào 08:34 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD1">Tại đây</a>!', CAST(N'2021-08-15T08:34:49.033' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (33, N'USER3', N'Hợp đồng HD1 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD1 đã được thêm phiếu nhập có mã  vào 08:35 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD1">Tại đây</a>!', CAST(N'2021-08-15T08:35:09.910' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (34, N'USER3', N'Hợp đồng HD2 đã được tạo!', N'Hợp đồng HD2 đã được tạo vào 08:35 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD2">Tại đây</a>!', CAST(N'2021-08-15T08:35:45.547' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (35, N'USER3', N'Hợp đồng HD2 đã được cập nhật danh sách sản phẩm!', N'Hợp đồng HD2 đã cập nhật vào 08:36 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD2">Tại đây</a>!', CAST(N'2021-08-15T08:36:02.740' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (36, N'USER3', N'Hợp đồng HD2 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD2 đã được thêm phiếu nhập có mã  vào 08:36 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD2">Tại đây</a>!', CAST(N'2021-08-15T08:36:21.293' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (37, N'USER3', N'Hợp đồng HD2 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD2 đã được thêm phiếu nhập có mã  vào 08:37 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD2">Tại đây</a>!', CAST(N'2021-08-15T08:37:02.427' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (38, N'USER3', N'Hợp đồng HD2 đã hoàn thành!', N'Hợp đồng HD2 đã cập nhật trạng thái hoàn thành vào 08:37 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD2">Tại đây</a>!', CAST(N'2021-08-15T08:37:02.833' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (39, N'USER1', N'Hợp đồng HD2 đã hoàn thành!', N'Hợp đồng HD2 đã cập nhật trạng thái hoàn thành vào 08:37 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD2">Tại đây</a>!', CAST(N'2021-08-15T08:37:02.837' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (40, N'USER5', N'Hợp đồng HD3 đã được tạo!', N'Hợp đồng HD3 đã được tạo vào 10:28 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD3">Tại đây</a>!', CAST(N'2021-08-15T10:28:13.730' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (41, N'USER5', N'Hợp đồng HD3 đã được cập nhật danh sách sản phẩm!', N'Hợp đồng HD3 đã cập nhật vào 10:28 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD3">Tại đây</a>!', CAST(N'2021-08-15T10:28:29.350' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (42, N'USER5', N'Hợp đồng HD3 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD3 đã được thêm phiếu nhập có mã  vào 10:28 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD3">Tại đây</a>!', CAST(N'2021-08-15T10:28:53.563' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (43, N'USER6', N'Yêu cầu xuất hàng PX2 đã được tạo!', N'Yêu cầu xuất hàng PX2 đã được tạo bởi cửa hàng CH1 vào 11:00 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T11:01:00.000' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (44, N'USER1', N'Yêu cầu xuất hàng PX2 đã được tạo!', N'Yêu cầu xuất hàng PX2 đã được tạo bởi cửa hàng CH1 vào 11:00 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T11:01:00.000' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (45, N'USER6', N'Yêu cầu xuất hàng PX2 đã được phê duyệt!', N'Yêu cầu xuất hàng PX2 đã được phê duyệt bởi Lưu Văn Lợi(loiql) vào 11:02 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T11:02:44.663' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (46, N'USER1', N'Yêu cầu xuất hàng PX2 đã được phê duyệt!', N'Yêu cầu xuất hàng PX2 đã được phê duyệt bởi Lưu Văn Lợi(loiql) vào 11:02 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T11:02:44.677' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (47, N'USER1', N'Yêu cầu xuất hàng PX2 đã hoàn thành!', N'Yêu cầu xuất hàng PX2 đã được xác nhận hoàn thành vào 11:03 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T11:03:22.737' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (48, N'USER10', N'Hợp đồng HD4 đã được tạo!', N'Hợp đồng HD4 đã được tạo vào 17:06 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:06:52.493' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (49, N'USER10', N'Hợp đồng HD4 đã được cập nhật danh sách sản phẩm!', N'Hợp đồng HD4 đã cập nhật vào 17:07 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:07:13.680' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (50, N'USER10', N'Hợp đồng HD4 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD4 đã được thêm phiếu nhập có mã  vào 17:07 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:07:51.967' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (51, N'USER11', N'Yêu cầu xuất hàng PX3 đã được tạo!', N'Yêu cầu xuất hàng PX3 đã được tạo bởi cửa hàng CH2 vào 17:11 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:11:55.907' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (52, N'USER1', N'Yêu cầu xuất hàng PX3 đã được tạo!', N'Yêu cầu xuất hàng PX3 đã được tạo bởi cửa hàng CH2 vào 17:11 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:11:55.947' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (53, N'USER7', N'Yêu cầu xuất hàng PX3 đã được tạo!', N'Yêu cầu xuất hàng PX3 đã được tạo bởi cửa hàng CH2 vào 17:11 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:11:55.950' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (54, N'USER8', N'Yêu cầu xuất hàng PX3 đã được tạo!', N'Yêu cầu xuất hàng PX3 đã được tạo bởi cửa hàng CH2 vào 17:11 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:11:55.957' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (55, N'USER9', N'Yêu cầu xuất hàng PX3 đã được tạo!', N'Yêu cầu xuất hàng PX3 đã được tạo bởi cửa hàng CH2 vào 17:11 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:11:55.967' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (56, N'USER10', N'Hợp đồng HD4 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD4 đã được thêm phiếu nhập có mã  vào 17:13 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:13:39.967' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (57, N'USER10', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:13 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:13:40.497' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (58, N'USER1', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:13 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:13:40.503' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (59, N'USER7', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:13 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:13:40.503' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (60, N'USER8', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:13 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:13:40.503' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (61, N'USER9', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:13 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:13:40.507' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (62, N'USER10', N'Hợp đồng HD4 đã được tạo thêm phiếu nhập!', N'Hợp đồng HD4 đã được thêm phiếu nhập có mã  vào 17:15 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:15:05.547' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (63, N'USER10', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:15 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:15:05.703' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (64, N'USER1', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:15 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:15:05.710' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (65, N'USER7', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:15 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:15:05.713' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (66, N'USER8', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:15 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:15:05.717' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (67, N'USER9', N'Hợp đồng HD4 đã hoàn thành!', N'Hợp đồng HD4 đã cập nhật trạng thái hoàn thành vào 17:15 15/08/2021.
 Xem chi tiết <a href="/HopDong/Detail/HD4">Tại đây</a>!', CAST(N'2021-08-15T17:15:05.720' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (68, N'USER11', N'Yêu cầu xuất hàng PX3 đã được phê duyệt!', N'Yêu cầu xuất hàng PX3 đã được phê duyệt bởi Lưu Văn Lợi (loiql) vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:16.107' AS DateTime), 0, 1)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (69, N'USER1', N'Yêu cầu xuất hàng PX3 đã được phê duyệt!', N'Yêu cầu xuất hàng PX3 đã được phê duyệt bởi Lưu Văn Lợi (loiql) vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:16.120' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (70, N'USER7', N'Yêu cầu xuất hàng PX3 đã được phê duyệt!', N'Yêu cầu xuất hàng PX3 đã được phê duyệt bởi Lưu Văn Lợi (loiql) vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:16.130' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (71, N'USER8', N'Yêu cầu xuất hàng PX3 đã được phê duyệt!', N'Yêu cầu xuất hàng PX3 đã được phê duyệt bởi Lưu Văn Lợi (loiql) vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:16.137' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (72, N'USER9', N'Yêu cầu xuất hàng PX3 đã được phê duyệt!', N'Yêu cầu xuất hàng PX3 đã được phê duyệt bởi Lưu Văn Lợi (loiql) vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:16.140' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (73, N'USER1', N'Yêu cầu xuất hàng PX3 đã hoàn thành!', N'Yêu cầu xuất hàng PX3 đã được xác nhận hoàn thành vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:44.623' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (74, N'USER7', N'Yêu cầu xuất hàng PX3 đã hoàn thành!', N'Yêu cầu xuất hàng PX3 đã được xác nhận hoàn thành vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:44.630' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (75, N'USER8', N'Yêu cầu xuất hàng PX3 đã hoàn thành!', N'Yêu cầu xuất hàng PX3 đã được xác nhận hoàn thành vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:44.637' AS DateTime), 0, 0)
INSERT [dbo].[tbl_Message] ([ID], [MaTK], [Title], [Content], [Created], [IsDelete], [IsSeen]) VALUES (76, N'USER9', N'Yêu cầu xuất hàng PX3 đã hoàn thành!', N'Yêu cầu xuất hàng PX3 đã được xác nhận hoàn thành vào 17:15 15/08/2021.
Xem chi tiết <a href="/PhieuXuat/QuanLy">Tại đây</a>!', CAST(N'2021-08-15T17:15:44.647' AS DateTime), 0, 0)
SET IDENTITY_INSERT [dbo].[tbl_Message] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_NhaCungCap] ON 

INSERT [dbo].[tbl_NhaCungCap] ([ID], [Ten], [DiaChi], [SDT], [STK], [NganHang], [TaiKhoanID], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (1, N'Lợi - Nhà cung cấp', N'Local Brand', N'0964062210', N'Agribank', N'033200004347', 10, CAST(N'2021-10-04T14:25:57.920' AS DateTime), CAST(N'2021-10-04T14:55:43.210' AS DateTime), 0)
SET IDENTITY_INSERT [dbo].[tbl_NhaCungCap] OFF
GO
INSERT [dbo].[tbl_PhanQuyen] ([ID], [Ten], [GhiChu], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (0, N'Admin', N'I can do anything', NULL, NULL, NULL)
INSERT [dbo].[tbl_PhanQuyen] ([ID], [Ten], [GhiChu], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (1, N'Quản lý', N'Duyệt Phiếu, tạo phiếu , ký hợp đồng ...', NULL, NULL, NULL)
INSERT [dbo].[tbl_PhanQuyen] ([ID], [Ten], [GhiChu], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (2, N'Nhân viên', N'tạo phiếu nhập,phiếu xuất...', NULL, NULL, NULL)
INSERT [dbo].[tbl_PhanQuyen] ([ID], [Ten], [GhiChu], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (3, N'Đại diện cửa hàng', N'Quản lý thông tin cửa hàng', NULL, NULL, NULL)
INSERT [dbo].[tbl_PhanQuyen] ([ID], [Ten], [GhiChu], [NgayTao], [NgayCapNhat], [IsDelete]) VALUES (4, N'Đại diện nhà cung cấp', N'Quản lý thông tin nhà cung cấp', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[tbl_TaiKhoan] ON 

INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (1, N'skyblack', N'a788f6d55914857d4b97c1de99cb896b', N'Đức Thiện ParaDog', CAST(N'2000-10-20' AS Date), N'Ninh Bình', N'0932200445', N'Giám Đốc', 0, CAST(N'2021-09-17T15:40:20.460' AS DateTime), 0, CAST(N'2021-11-20T02:39:49.880' AS DateTime), NULL, N'https://localhost:44387/Data/Upload/Images/User/cat_1210bfe4c-2f4d-4003-a060-3fb264576672.jpg')
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (6, N'hung', N'a788f6d55914857d4b97c1de99cb896b', N'Mạnh Hùng', CAST(N'2000-10-10' AS Date), N'Trần Duy Hưng', N'0912387453', N'Nhân viên', 2, CAST(N'2021-09-17T15:40:20.460' AS DateTime), 0, NULL, NULL, NULL)
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (7, N'user01', N'a788f6d55914857d4b97c1de99cb896b', N'Nguyễn Đoàn Nam', CAST(N'2000-10-10' AS Date), N'Trần Duy Hưng', N'0923274624', N'bốc cứt', 2, CAST(N'2021-09-17T16:31:19.963' AS DateTime), 0, CAST(N'2021-09-17T16:39:32.583' AS DateTime), NULL, NULL)
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (8, N'quanly_1', N'a788f6d55914857d4b97c1de99cb896b', N'Lưu Văn Lợi', CAST(N'2000-10-10' AS Date), N'Hà Nội', N'0921623313', N'Tổng Giám Đốc', 1, CAST(N'2021-09-17T17:17:03.047' AS DateTime), 0, CAST(N'2021-11-20T00:20:34.393' AS DateTime), NULL, NULL)
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (9, N'admin2', N'a788f6d55914857d4b97c1de99cb896b', N'Nguyễn Văn Minh', CAST(N'1999-04-01' AS Date), N'Hà Nội', N'02748412414', N'Quản Lý', 1, CAST(N'2021-09-18T21:01:28.883' AS DateTime), 0, NULL, NULL, NULL)
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (10, N'luuvanloi', N'a788f6d55914857d4b97c1de99cb896b', N'Lưu Văn Lợi', CAST(N'2000-06-05' AS Date), N'Local', N'0964062210', N'Đại diện nhà cung cấp', 4, CAST(N'2021-10-04T14:24:07.420' AS DateTime), 0, NULL, N'fa988b4a-6922-4164-aab5-96b79a2ad3c4', NULL)
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (11, N'admin', N'a788f6d55914857d4b97c1de99cb896b', N'Super Administrator', CAST(N'2000-01-01' AS Date), N'Local', N'0964062210', N'Admin', 0, CAST(N'2021-01-01T00:00:00.000' AS DateTime), 0, CAST(N'2021-11-20T01:57:10.980' AS DateTime), N'e1fd9674-c18b-4334-be04-8255d9ad3587', N'https://localhost:44387/Data/Upload/Images/User/z2700682178383_3e06eccfea37147c9876b546e599b65fbaac7d6b-e04b-43ef-bd2e-627f2a408300.jpg')
INSERT [dbo].[tbl_TaiKhoan] ([ID], [UserName], [Pass], [HoTen], [NgaySinh], [DiaChi], [SDT], [ChucVu], [PhanQuyenID], [NgayTao], [IsDelete], [NgayCapNhat], [Token], [Img]) VALUES (12, N'loi_nhanvien', N'a788f6d55914857d4b97c1de99cb896b', N'Lưu Văn Lợi Nhân Viên', CAST(N'2000-06-05' AS Date), N'Local', N'0964062210', NULL, 2, CAST(N'2021-11-20T09:44:48.760' AS DateTime), 0, CAST(N'2021-11-20T09:44:48.760' AS DateTime), N'9bb9a997-a995-4880-84d6-987379cb704f', N'https://localhost:44387/Data/Upload/Images/User/cat_1825f6798-5206-4f8b-8a49-c3a8da88ca2d.jpg')
SET IDENTITY_INSERT [dbo].[tbl_TaiKhoan] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tbl_HeTh__C451FA8362AB6AE3]    Script Date: 12/1/2021 10:13:06 AM ******/
ALTER TABLE [dbo].[tbl_HeThong] ADD UNIQUE NONCLUSTERED 
(
	[Ten] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tbl_Loai__C451FA8323EA7E8B]    Script Date: 12/1/2021 10:13:06 AM ******/
ALTER TABLE [dbo].[tbl_LoaiMatHang] ADD  CONSTRAINT [UQ__tbl_Loai__C451FA8323EA7E8B] UNIQUE NONCLUSTERED 
(
	[Ten] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tbl_Phan__C451FA83F27946C3]    Script Date: 12/1/2021 10:13:06 AM ******/
ALTER TABLE [dbo].[tbl_PhanQuyen] ADD  CONSTRAINT [UQ__tbl_Phan__C451FA83F27946C3] UNIQUE NONCLUSTERED 
(
	[Ten] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_ChiTietHD] ADD  DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[tbl_ChiTietPN] ADD  DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[tbl_ChiTietPX] ADD  DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[tbl_CuaHang] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbl_HeThong] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[tbl_HeThong] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbl_HopDong] ADD  DEFAULT ((0)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[tbl_Kho] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[tbl_Kho] ADD  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbl_MatHang] ADD  CONSTRAINT [DF__tbl_MatHang__Gia__4222D4EF]  DEFAULT ((0)) FOR [Gia]
GO
ALTER TABLE [dbo].[tbl_MatHang] ADD  CONSTRAINT [DF__tbl_MatHa__SoLuo__4316F928]  DEFAULT ((0)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[tbl_Menu] ADD  CONSTRAINT [DF_tbl_Menu_Order]  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[tbl_Notification] ADD  DEFAULT ((0)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[tbl_PhieuNhap] ADD  DEFAULT ((0)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[tbl_PhieuXuat] ADD  DEFAULT ((0)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[tbl_TaiKhoan] ADD  CONSTRAINT [DF__tbl_TaiKh__NgayT__2B0A656D]  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[tbl_TaiKhoan] ADD  CONSTRAINT [DF_tbl_TaiKhoan_IsDelete]  DEFAULT ((0)) FOR [IsDelete]
GO
ALTER TABLE [dbo].[tbl_ViTri] ADD  DEFAULT ((0)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[tbl_ChiTietHD]  WITH CHECK ADD FOREIGN KEY([HopDongID])
REFERENCES [dbo].[tbl_HopDong] ([ID])
GO
ALTER TABLE [dbo].[tbl_ChiTietHD]  WITH CHECK ADD  CONSTRAINT [FK__tbl_ChiTi__MatHa__5165187F] FOREIGN KEY([MatHangID])
REFERENCES [dbo].[tbl_MatHang] ([ID])
GO
ALTER TABLE [dbo].[tbl_ChiTietHD] CHECK CONSTRAINT [FK__tbl_ChiTi__MatHa__5165187F]
GO
ALTER TABLE [dbo].[tbl_ChiTietPN]  WITH CHECK ADD  CONSTRAINT [FK__tbl_ChiTi__MatHa__5AEE82B9] FOREIGN KEY([MatHangID])
REFERENCES [dbo].[tbl_MatHang] ([ID])
GO
ALTER TABLE [dbo].[tbl_ChiTietPN] CHECK CONSTRAINT [FK__tbl_ChiTi__MatHa__5AEE82B9]
GO
ALTER TABLE [dbo].[tbl_ChiTietPN]  WITH CHECK ADD FOREIGN KEY([PhieuNhapID])
REFERENCES [dbo].[tbl_PhieuNhap] ([ID])
GO
ALTER TABLE [dbo].[tbl_ChiTietPN]  WITH CHECK ADD FOREIGN KEY([ViTriID])
REFERENCES [dbo].[tbl_ViTri] ([ID])
GO
ALTER TABLE [dbo].[tbl_ChiTietPX]  WITH CHECK ADD  CONSTRAINT [FK__tbl_ChiTi__MatHa__68487DD7] FOREIGN KEY([MatHangID])
REFERENCES [dbo].[tbl_MatHang] ([ID])
GO
ALTER TABLE [dbo].[tbl_ChiTietPX] CHECK CONSTRAINT [FK__tbl_ChiTi__MatHa__68487DD7]
GO
ALTER TABLE [dbo].[tbl_ChiTietPX]  WITH CHECK ADD FOREIGN KEY([PhieuXuatID])
REFERENCES [dbo].[tbl_PhieuXuat] ([ID])
GO
ALTER TABLE [dbo].[tbl_CuaHang]  WITH CHECK ADD  CONSTRAINT [FK__tbl_CuaHa__TaiKh__5EBF139D] FOREIGN KEY([TaiKhoanID])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_CuaHang] CHECK CONSTRAINT [FK__tbl_CuaHa__TaiKh__5EBF139D]
GO
ALTER TABLE [dbo].[tbl_HeThong]  WITH CHECK ADD  CONSTRAINT [FK__tbl_HeTho__TaiKh__2B3F6F97] FOREIGN KEY([TaiKhoanID])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_HeThong] CHECK CONSTRAINT [FK__tbl_HeTho__TaiKh__2B3F6F97]
GO
ALTER TABLE [dbo].[tbl_HopDong]  WITH CHECK ADD  CONSTRAINT [FK__tbl_HopDo__Nguoi__4BAC3F29] FOREIGN KEY([NguoiLap])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_HopDong] CHECK CONSTRAINT [FK__tbl_HopDo__Nguoi__4BAC3F29]
GO
ALTER TABLE [dbo].[tbl_HopDong]  WITH CHECK ADD  CONSTRAINT [FK__tbl_HopDo__NhaCu__4CA06362] FOREIGN KEY([NhaCungCapID])
REFERENCES [dbo].[tbl_NhaCungCap] ([ID])
GO
ALTER TABLE [dbo].[tbl_HopDong] CHECK CONSTRAINT [FK__tbl_HopDo__NhaCu__4CA06362]
GO
ALTER TABLE [dbo].[tbl_HT_TK]  WITH CHECK ADD FOREIGN KEY([HeTHongID])
REFERENCES [dbo].[tbl_HeThong] ([ID])
GO
ALTER TABLE [dbo].[tbl_HT_TK]  WITH CHECK ADD  CONSTRAINT [FK__tbl_HT_TK__TaiKh__2E1BDC42] FOREIGN KEY([TaiKhoanID])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_HT_TK] CHECK CONSTRAINT [FK__tbl_HT_TK__TaiKh__2E1BDC42]
GO
ALTER TABLE [dbo].[tbl_Kho]  WITH CHECK ADD FOREIGN KEY([HeThongID])
REFERENCES [dbo].[tbl_HeThong] ([ID])
GO
ALTER TABLE [dbo].[tbl_MatHang]  WITH CHECK ADD  CONSTRAINT [FK__tbl_MatHa__NhaCu__440B1D61] FOREIGN KEY([NhaCungCapID])
REFERENCES [dbo].[tbl_NhaCungCap] ([ID])
GO
ALTER TABLE [dbo].[tbl_MatHang] CHECK CONSTRAINT [FK__tbl_MatHa__NhaCu__440B1D61]
GO
ALTER TABLE [dbo].[tbl_MH_LMH]  WITH CHECK ADD  CONSTRAINT [FK__tbl_MH_LM__LoaiM__47DBAE45] FOREIGN KEY([LoaiMatHangID])
REFERENCES [dbo].[tbl_LoaiMatHang] ([ID])
GO
ALTER TABLE [dbo].[tbl_MH_LMH] CHECK CONSTRAINT [FK__tbl_MH_LM__LoaiM__47DBAE45]
GO
ALTER TABLE [dbo].[tbl_MH_LMH]  WITH CHECK ADD  CONSTRAINT [FK__tbl_MH_LM__MatHa__46E78A0C] FOREIGN KEY([MatHangID])
REFERENCES [dbo].[tbl_MatHang] ([ID])
GO
ALTER TABLE [dbo].[tbl_MH_LMH] CHECK CONSTRAINT [FK__tbl_MH_LM__MatHa__46E78A0C]
GO
ALTER TABLE [dbo].[tbl_NhaCungCap]  WITH CHECK ADD  CONSTRAINT [FK__tbl_NhaCu__TaiKh__3C69FB99] FOREIGN KEY([TaiKhoanID])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_NhaCungCap] CHECK CONSTRAINT [FK__tbl_NhaCu__TaiKh__3C69FB99]
GO
ALTER TABLE [dbo].[tbl_Notification]  WITH CHECK ADD  CONSTRAINT [FK__tbl_Notif__TaiKh__32E0915F] FOREIGN KEY([TaiKhoanID])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_Notification] CHECK CONSTRAINT [FK__tbl_Notif__TaiKh__32E0915F]
GO
ALTER TABLE [dbo].[tbl_PhieuNhap]  WITH CHECK ADD FOREIGN KEY([HopDongID])
REFERENCES [dbo].[tbl_HopDong] ([ID])
GO
ALTER TABLE [dbo].[tbl_PhieuNhap]  WITH CHECK ADD  CONSTRAINT [FK__tbl_Phieu__Nguoi__5441852A] FOREIGN KEY([NguoiLap])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_PhieuNhap] CHECK CONSTRAINT [FK__tbl_Phieu__Nguoi__5441852A]
GO
ALTER TABLE [dbo].[tbl_PhieuXuat]  WITH CHECK ADD FOREIGN KEY([CuaHangID])
REFERENCES [dbo].[tbl_CuaHang] ([ID])
GO
ALTER TABLE [dbo].[tbl_PhieuXuat]  WITH CHECK ADD  CONSTRAINT [FK__tbl_Phieu__Nguoi__619B8048] FOREIGN KEY([NguoiDuyet])
REFERENCES [dbo].[tbl_TaiKhoan] ([ID])
GO
ALTER TABLE [dbo].[tbl_PhieuXuat] CHECK CONSTRAINT [FK__tbl_Phieu__Nguoi__619B8048]
GO
ALTER TABLE [dbo].[tbl_TaiKhoan]  WITH CHECK ADD  CONSTRAINT [FK__tbl_TaiKh__PhanQ__276EDEB3] FOREIGN KEY([PhanQuyenID])
REFERENCES [dbo].[tbl_PhanQuyen] ([ID])
GO
ALTER TABLE [dbo].[tbl_TaiKhoan] CHECK CONSTRAINT [FK__tbl_TaiKh__PhanQ__276EDEB3]
GO
ALTER TABLE [dbo].[tbl_ViTri]  WITH CHECK ADD FOREIGN KEY([KhoID])
REFERENCES [dbo].[tbl_Kho] ([ID])
GO
/****** Object:  StoredProcedure [dbo].[pr_Kho_Create]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_Kho_Create] @Result INT  OUTPUT,@CurUserID INT ,@TenKho NVARCHAR(300),@DiaChi NVARCHAR(500),
			@HeThongID INT 

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1)
		BEGIN
				SET @Result=-1;
				IF(@PhanQuyenID=1)
					BEGIN
						IF((SELECT ID 
								FROM dbo.tbl_HeThong
								WHERE dbo.tbl_HeThong.TaiKhoanID=@CurUserID AND dbo.tbl_HeThong.IsDelete=0 AND dbo.tbl_HeThong.ID=@HeThongID  	)IS NOT NULL)	
								BEGIN
										IF ((	SELECT  dbo.tbl_Kho.ID
													FROM dbo.tbl_Kho 
													WHERE  dbo.tbl_Kho.Ten=@TenKho AND dbo.tbl_Kho.IsDelete=0 AND @HeThongID=dbo.tbl_Kho.HeTHongID) IS NULL)
											BEGIN
											
												
														BEGIN TRY
																
																INSERT INTO dbo.tbl_Kho
																(
																    Ten,
																    DiaChi,
																    HeTHongID
																)
																VALUES
																(   @TenKho, -- Ten - nvarchar(300)
																    @DiaChi, -- DiaChi - nvarchar(500)
																    @HeThongID -- HeTHongID - int
																    )
																SET @Result=1;
																END TRY

															BEGIN CATCH
															SET @Result =-1;
															PRINT ERROR_MESSAGE();
								
															END CATCH
												END
										ELSE

												BEGIN 
												SET @Result=-2;
												END
								END
						
				
					END

				IF (@PhanQuyenID=0)
				BEGIN
								IF ((	SELECT  dbo.tbl_Kho.ID
													FROM dbo.tbl_Kho 
													WHERE  dbo.tbl_Kho.Ten=@TenKho AND dbo.tbl_Kho.IsDelete=0 AND @HeThongID=dbo.tbl_Kho.HeTHongID) IS NULL)
											BEGIN
											
												
														BEGIN TRY
																
																INSERT INTO dbo.tbl_Kho
																(
																    Ten,
																    DiaChi,
																    HeTHongID
																)
																VALUES
																(   @TenKho, -- Ten - nvarchar(300)
																    @DiaChi, -- DiaChi - nvarchar(500)
																    @HeThongID -- HeTHongID - int
																    )
																SET @Result=1;
																END TRY

															BEGIN CATCH
															SET @Result =-1;
															
								
															END CATCH
												END
										ELSE

												BEGIN 
												SET @Result=-2;
												END
									
				END
			
		END

		ELSE
		
		BEGIN
					SET @Result=-4;
		END
	
	
		

END
GO
/****** Object:  StoredProcedure [dbo].[pr_LoaiMatHang_Create]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_LoaiMatHang_Create] @Result INT  OUTPUT, @CurUserID INT, @TenMatHang NVARCHAR(300), @GhiChu NVARCHAR(MAX)

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 Or @PhanQuyenID = 1))
	BEGIN
		SET @Result=-1;
		IF (
				(SELECT dbo.tbl_LoaiMatHang.ID
					FROM dbo.tbl_LoaiMatHang
					WHERE dbo.tbl_LoaiMatHang.Ten = @TenMatHang AND dbo.tbl_LoaiMatHang.IsDelete = 0) IS NULL
			)
			BEGIN						
				BEGIN TRY
					INSERT INTO dbo.tbl_LoaiMatHang
					(
						Ten,
						GhiChu,
						NgayTao,
						IsDelete
					)
					VALUES
					(   
						@TenMatHang,
						@GhiChu,
						GETDATE(),
						0
					)
																    
					SET @Result=1;
				END TRY

				BEGIN CATCH
					SET @Result =-1;
					PRINT ERROR_MESSAGE();	
				END CATCH 
			END -- End of If
		ELSE
			BEGIN 
				SET @Result=-2; --Existed
			END -- End of Else
		END -- End of If PhanQuyen
	ELSE
	BEGIN
		SET @Result=-4;
	END -- End of Else
END -- End of Proc
GO
/****** Object:  StoredProcedure [dbo].[pr_LoaiMatHang_Delete]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_LoaiMatHang_Delete] @Result INT  OUTPUT,@CurUserID INT ,@ID INT

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 OR @PhanQuyenID = 1))
		BEGIN
			SET @Result=-1;

			IF(
				(SELECT ID 
					FROM dbo.tbl_LoaiMatHang
					WHERE dbo.tbl_LoaiMatHang.ID=@ID AND dbo.tbl_LoaiMatHang.IsDelete=0) IS NOT NULL
				)
				BEGIN
					BEGIN TRY
						UPDATE dbo.tbl_LoaiMatHang
						SET IsDelete = 1
						WHERE ID=@ID
									
						SET @Result = 1;	
					END TRY

					BEGIN CATCH
						SET @Result =-1;		
					END CATCH
				END
			ELSE
				BEGIN
					SET @Result = -3;
				END
		END -- End of if Phan quyen
		ELSE
			BEGIN
				SET @Result=-4;
			END
END





GO
/****** Object:  StoredProcedure [dbo].[pr_LoaiMatHang_GetByID]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_LoaiMatHang_GetByID] (@ID INT )
	AS
	BEGIN
	    SELECT * FROM dbo.tbl_LoaiMatHang
		WHERE dbo.tbl_LoaiMatHang.ID=ID 
	END
    
	
GO
/****** Object:  StoredProcedure [dbo].[pr_LoaiMatHang_SearchPaging]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[pr_LoaiMatHang_SearchPaging] @SearchValue NVARCHAR(300),@SearchType INT,@CurPage INT ,@PageSize INT ,@OrderBy INT,@IsDes BIT 
AS
BEGIN
	DECLARE @a INT ;
DECLARE @Result TABLE 
				(	ID INT PRIMARY KEY,
					Ten NVARCHAR(300) NOT NULL,
					GhiChu NVARCHAR(MAX),
					NgayTao DateTime NOT NULL,
					NgayCapNhat DateTime,
					IsDelete BIT
				);
DECLARE @ResultFinal TABLE 
				(
					ID INT PRIMARY KEY,
					Ten NVARCHAR(300) NOT NULL,
					GhiChu NVARCHAR(MAX),
					NgayTao DateTime NOT NULL,
					NgayCapNhat DateTime,
					IsDelete BIT
				);

		IF(@SearchType=0) 
			BEGIN
			    INSERT INTO @Result
			    
				SELECT *
							
				FROM dbo.tbl_LoaiMatHang
				WHERE dbo.tbl_LoaiMathang.IsDelete = 0
				
			END

			ELSE IF (@SearchType = 1)
					BEGIN
								INSERT INTO @Result
								
								SELECT 	*
									
								FROM dbo.tbl_LoaiMatHang
								WHERE dbo.tbl_LoaiMatHang.ID=CONVERT(INT,@SearchValue) AND dbo.tbl_LoaiMatHang.IsDelete = 0

					END

			ELSE IF (@SearchType=3)
								BEGIN
									INSERT INTO @Result
								
									SELECT *
									FROM dbo.tbl_LoaiMatHang
									WHERE LOWER(dbo.tbl_LoaiMatHang.Ten) LIKE '%' + LOWER(@SearchValue) + '%' AND dbo.tbl_LoaiMatHang.IsDelete=0
								END
								

		---------------------------------------------------------
        
		
		IF ( @IsDes=1 ) 
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT  * 
					FROM @Result
					ORDER BY [@Result].NgayTao DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

	
		END
		ELSE
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayTao ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

		END
		

END
GO
/****** Object:  StoredProcedure [dbo].[pr_LoatMatHang_Edit]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_LoatMatHang_Edit] @Result INT  OUTPUT, @CurUserID INT, @ID INT, @TenLoaiMatHang NVARCHAR(300), @GhiChu NVARCHAR(MAX), @SDT VARCHAR(12), @STK VARCHAR(30), @NganHang NVARCHAR(300)

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 Or @PhanQuyenID = 1))
	BEGIN
		SET @Result=-1;
		IF (
				(SELECT  dbo.tbl_LoaiMatHang.ID
					FROM dbo.tbl_LoaiMatHang 
					WHERE  dbo.tbl_LoaiMatHang.ID = @ID AND dbo.tbl_LoaiMatHang.IsDelete=0) IS NULL
			)
			BEGIN						
				BEGIN TRY
					UPDATE dbo.tbl_LoaiMatHang
					SET
						Ten = @TenLoaiMatHang,
						GhiChu = @GhiChu,
						NgayCapNhat = GetDate()
					WHERE ID = @ID
															
					SET @Result=1;
				END TRY

				BEGIN CATCH
					SET @Result =-1;
					PRINT ERROR_MESSAGE();	
				END CATCH 
			END -- End of If NCC
		ELSE
			BEGIN 
				SET @Result=-3; --Existed
			END -- End of Else
	END -- End of If PhanQuyen
	ELSE
	BEGIN
		SET @Result=-4;
	END -- End of Else
END -- End of Proc
GO
/****** Object:  StoredProcedure [dbo].[pr_MatHang_Create]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_MatHang_Create] @Result INT  OUTPUT, @CurUserID INT, @Ten NVARCHAR(300), @MoTa NVARCHAR(MAX), @Gia INT, @NhaCungCapID INT

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 Or @PhanQuyenID = 4))
	BEGIN
		SET @Result=-1;
		IF(@PhanQuyenID = 4)
			BEGIN
							--	(SELECT dbo.tbl_NhaCungCap.ID
				--		FROM dbo.tbl_NhaCungCap
				--		WHERE  (dbo.tbl_NhaCungCap.Ten=@TenNCC AND dbo.tbl_NhaCungCap.IsDelete=0)) IS NULL
													
				--AND 
				--	(SELECT dbo.tbl_NhaCungCap.ID
				--			FROM dbo.tbl_NhaCungCap
				--			WHERE dbo.tbl_NhaCungCap.TaiKhoanID = @TaiKhoanID And dbo.tbl_NhaCungCap.IsDelete=0) IS  NULL
				--AND 
				--	@TaiKhoanID=@CurUserID
			IF (
					(SELECT dbo.tbl_NhaCungCap.ID
						FROM dbo.tbl_NhaCungCap
						WHERE dbo.tbl_NhaCungCap.ID = @NhaCungCapID AND dbo.tbl_NhaCungCap.TaiKhoanID = @CurUserID) IS NOT NULL
					AND
					(SELECT dbo.tbl_MatHang.ID
						FROM dbo.tbl_MatHang
						WHERE dbo.tbl_MatHang.Ten = @Ten AND dbo.tbl_MatHang.NhaCungCapID = @NhaCungCapID) IS NULL
				)
				BEGIN
					BEGIN TRY
						INSERT INTO dbo.tbl_MatHang
						(
							Ten,
							MoTa,
							Gia,
							NhaCungCapID,
							NgayTao,
							ISDelete
						)
						VALUES
						(   
							@Ten,
							@MoTa,
							@Gia,
							@NhaCungCapID,
							GETDATE(),
							0
						)								    
															
						SET @Result=1;
					END TRY

					BEGIN CATCH
						SET @Result =-1;
						PRINT ERROR_MESSAGE();	
					END CATCH
				END -- End of If NCC
			ELSE
				BEGIN 
					SET @Result=-2; -- Existed
				END
			END -- End of If PhanQuyen

		IF (@PhanQuyenID=0)
		BEGIN
			IF (
					(SELECT dbo.tbl_MatHang.ID
						FROM dbo.tbl_MatHang
						WHERE dbo.tbl_MatHang.Ten = @Ten AND dbo.tbl_MatHang.NhaCungCapID = @NhaCungCapID) IS NULL
				)
				BEGIN
					BEGIN TRY
						INSERT INTO dbo.tbl_MatHang
						(
							Ten,
							MoTa,
							Gia,
							NhaCungCapID,
							NgayTao,
							ISDelete
						)
						VALUES
						(   
							@Ten,
							@MoTa,
							@Gia,
							@NhaCungCapID,
							GETDATE(),
							0
						)								    
															
						SET @Result=1;
					END TRY

					BEGIN CATCH
						SET @Result =-1;
						PRINT ERROR_MESSAGE();	
					END CATCH
				END -- End of If
			ELSE
				BEGIN 
					SET @Result=-2; --Existed
				END -- End of Else
			END -- End of If PhanQuyen
		END -- End of If
	ELSE
	BEGIN
		SET @Result=-4;
	END -- End of Else
END -- End of Proc
GO
/****** Object:  StoredProcedure [dbo].[pr_MatHang_Delete]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_MatHang_Delete] @Result INT  OUTPUT,@CurUserID INT ,@ID INT

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 OR @PhanQuyenID = 4))
		BEGIN
			SET @Result=-1;

			IF(
				(SELECT dbo.tbl_MatHang.ID
					FROM dbo.tbl_MatHang
					WHERE dbo.tbl_MatHang.ID = @ID AND dbo.tbl_MatHang.IsDelete = 0) IS NOT NULL
				)
			BEGIN
				IF(@PhanQuyenID=0)
					BEGIN
						BEGIN TRY
							UPDATE dbo.tbl_MatHang
							SET IsDelete = 1
							WHERE ID=@ID
									
							SET @Result = 1;	
						END TRY

						BEGIN CATCH
							SET @Result =-1;		
						END CATCH
					END
				IF (@PhanQuyenID = 4)
					BEGIN
					
						IF(	
								(SELECT dbo.tbl_MatHang.ID
									FROM dbo.tbl_NhaCungCap
										JOIN dbo.tbl_MatHang ON dbo.tbl_NhaCungCap.ID = dbo.tbl_MatHang.NhaCungCapID
									WHERE dbo.tbl_MatHang.ID = @ID AND dbo.tbl_NhaCungCap.TaiKhoanID = @CurUserID AND dbo.tbl_MatHang.IsDelete = 0) IS NOT NULl
							)
							BEGIN
								BEGIN TRY
									UPDATE dbo.tbl_MatHang
									SET IsDelete = 1
									WHERE ID=@ID
									
									SET @Result = 1;	
								END TRY

								BEGIN CATCH
									SET @Result =-1;
								END CATCH
							END
						ELSE
							BEGIN 
								SET @Result = -4;
							END
					END -- End of If Phan quyen
				END -- End of If NCC
			ELSE
				BEGIN
					SET @Result = -3
				END
		END -- End of if Phan quyen
		ELSE
			BEGIN
				SET @Result=-4;
			END
END





GO
/****** Object:  StoredProcedure [dbo].[pr_MatHang_GetByID]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_MatHang_GetByID] (@ID INT )
	AS
	BEGIN
	    SELECT * FROM dbo.tbl_MatHang
		WHERE dbo.tbl_MatHang.ID=ID 
	END
    
	
GO
/****** Object:  StoredProcedure [dbo].[pr_MatHang_SearchPaging]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[pr_MatHang_SearchPaging] @SearchValue NVARCHAR(300),@SearchType INT,@CurPage INT ,@PageSize INT ,@OrderBy INT,@IsDes BIT 
AS
BEGIN
	DECLARE @a INT ;
DECLARE @Result TABLE 
				(	
					ID INT PRIMARY KEY,
					Ten NVARCHAR(300) NOT NULL,
					MoTa NVARCHAR(MAX) NOT NULL,
					Gia INT NOT NULL,
					SoLuong INT NOT NULL,
					NhaCungCapID INT NOT NULL,
					NgayTao DateTime NOT NULL,
					NgayCapNhat DateTime,
					IsDelete BIT NOT NULL
				);
DECLARE @ResultFinal TABLE 
				(
					ID INT PRIMARY KEY,
					Ten NVARCHAR(300) NOT NULL,
					MoTa NVARCHAR(MAX) NOT NULL,
					Gia INT NOT NULL,
					SoLuong INT NOT NULL,
					NhaCungCapID INT NOT NULL,
					NgayTao DateTime NOT NULL,
					NgayCapNhat DateTime,
					IsDelete BIT NOT NULL

				);
		IF(@SearchType=0) 
			BEGIN
			    INSERT INTO @Result
			    
				SELECT *
							
				FROM dbo.tbl_MatHang
				WHERE dbo.tbl_MatHang.IsDelete=0
				
			END

			ELSE IF (@SearchType=1 )
					BEGIN
								INSERT INTO @Result
								
								SELECT 	*
									
								FROM dbo.tbl_MatHang
								WHERE dbo.tbl_MatHang.ID=CONVERT(INT,@SearchValue) AND dbo.tbl_MatHang.IsDelete=0

					END

			ELSE IF (@SearchType=3)
								BEGIN
									INSERT INTO @Result
								
									SELECT *
									FROM dbo.tbl_MatHang
									WHERE LOWER(dbo.tbl_MatHang.Ten) LIKE '%' + LOWER(@SearchValue) + '%' AND dbo.tbl_MatHang.IsDelete=0
								END
			ELSE IF (@SearchType=4)
									BEGIN
										INSERT INTO @Result
										
										SELECT *
										FROM dbo.tbl_MatHang
										WHERE dbo.tbl_MatHang.NhaCungCapID=CONVERT(INT,@SearchValue) AND dbo.tbl_MatHang.IsDelete=0
									END

								

		---------------------------------------------------------
        
		
		IF ( @IsDes=1 ) 
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT  * 
					FROM @Result
					ORDER BY [@Result].NgayTao DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

	
		END
		ELSE
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayTao ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

		END
		

END
GO
/****** Object:  StoredProcedure [dbo].[pr_NhaCungCap_Create]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_NhaCungCap_Create] @Result INT  OUTPUT ,@CurUserID INT, @TenNCC NVARCHAR(300), @DiaChi NVARCHAR(500), @SDT VARCHAR(12), @STK VARCHAR(30), @NganHang NVARCHAR(300), @TaiKhoanID INT

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 Or @PhanQuyenID = 4))
	BEGIN
		SET @Result=-1;
		IF(@PhanQuyenID = 4)
			BEGIN
			IF (
					(SELECT dbo.tbl_NhaCungCap.ID
						FROM dbo.tbl_NhaCungCap
						WHERE  (dbo.tbl_NhaCungCap.Ten=@TenNCC AND dbo.tbl_NhaCungCap.IsDelete=0)) IS NULL
													
				AND 
					(SELECT dbo.tbl_NhaCungCap.ID
							FROM dbo.tbl_NhaCungCap
							WHERE dbo.tbl_NhaCungCap.TaiKhoanID = @TaiKhoanID And dbo.tbl_NhaCungCap.IsDelete=0) IS  NULL
				AND 
					@TaiKhoanID=@CurUserID
				)
				BEGIN
					BEGIN TRY
						INSERT INTO dbo.tbl_NhaCungCap
						(
							Ten,
							DiaChi,
							SDT,
							STK,
							NganHang,
							TaiKhoanID,
							NgayTao,
							IsDelete
						)
						VALUES
						(   
							@TenNCC, -- Ten - nvarchar(300)
							@DiaChi, -- DiaChi - nvarchar(500)
							@SDT,  -- SDT - varchar(12)
							@STK,  -- STK - varchar(30)
							@NganHang, -- NganHang - nvarchar(300)
							@TaiKhoanID, -- TaiKhoanID - int
							GETDATE(),
							0
						)								    
															
						SET @Result=1;
					END TRY

					BEGIN CATCH
						SET @Result =-1;
						PRINT ERROR_MESSAGE();	
					END CATCH
				END -- End of If NCC
			ELSE
				BEGIN 
					SET @Result=-2; -- Existed
				END
			END -- End of If PhanQuyen

		IF (@PhanQuyenID=0)
		BEGIN
			IF (
					(SELECT  dbo.tbl_NhaCungCap.ID
						FROM dbo.tbl_NhaCungCap 
						WHERE  dbo.tbl_NhaCungCap.Ten=@TenNCC AND dbo.tbl_NhaCungCap.IsDelete=0) IS NULL								
				AND 
					(SELECT dbo.tbl_NhaCungCap.ID
						FROM dbo.tbl_NhaCungCap
						WHERE dbo.tbl_NhaCungCap.TaiKhoanID = @TaiKhoanID And dbo.tbl_NhaCungCap.IsDelete=0) IS  NULL
				)
				BEGIN						
					BEGIN TRY
						INSERT INTO dbo.tbl_NhaCungCap
						(
							Ten,
							DiaChi,
							SDT,
							STK,
							NganHang,
							TaiKhoanID,
							NgayTao,
							IsDelete
						)
						VALUES
						(   
							@TenNCC, -- Ten - nvarchar(300)
							@DiaChi, -- DiaChi - nvarchar(500)
							@SDT,  -- SDT - varchar(12)
							@STK,  -- STK - varchar(30)
							@NganHang, -- NganHang - nvarchar(300)
							@TaiKhoanID, -- TaiKhoanID - int
							GETDATE(),
							0
						)
																    
						SET @Result=1;
					END TRY

					BEGIN CATCH
						SET @Result =-1;
						PRINT ERROR_MESSAGE();	
					END CATCH 
				END -- End of If NCC
			ELSE
				BEGIN 
					SET @Result=-2; --Existed
				END -- End of Else
			END -- End of If PhanQuyen
		END -- End of If
	ELSE
	BEGIN
		SET @Result=-4;
	END -- End of Else
END -- End of Proc
GO
/****** Object:  StoredProcedure [dbo].[pr_NhaCungCap_Edit]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_NhaCungCap_Edit] @Result INT  OUTPUT, @CurUserID INT, @ID INT, @Ten NVARCHAR(300), @MoTa NVARCHAR(MAX), @Gia INT

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
	SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
	FROM dbo.tbl_TaiKhoan
	WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

	IF (@PhanQuyenID IS NOT NULL AND (@PhanQuyenID = 0 Or @PhanQuyenID = 4))
	BEGIN
		SET @Result=-1;
		IF(@PhanQuyenID = 4)
			BEGIN
			IF (
					(SELECT dbo.tbl_MatHang.ID
						FROM dbo.tbl_NhaCungCap
							JOIN dbo.tbl_MatHang ON dbo.tbl_NhaCungCap.ID = dbo.tbl_MatHang.NhaCungCapID
						WHERE  dbo.tbl_MatHang.ID = @ID AND dbo.tbl_NhaCungCap.TaiKhoanID = @CurUserID) IS NOT NULL
				)
				BEGIN
					BEGIN TRY
						UPDATE dbo.tbl_MatHang
						SET
							Ten = @Ten,
							MoTa = @MoTa,
							Gia = @Gia,
							NgayCapNhat = GETDATE()
						WHERE ID = @ID
															
						SET @Result=1;
					END TRY

					BEGIN CATCH
						SET @Result =-1;
						PRINT ERROR_MESSAGE();	
					END CATCH
				END -- End of If NCC
			ELSE
				BEGIN 
					SET @Result=-3; -- Empty
				END
			END -- End of If PhanQuyen

		IF (@PhanQuyenID=0)
		BEGIN
			IF (
					(SELECT dbo.tbl_MatHang.ID
						FROM dbo.tbl_MatHang
						WHERE  dbo.tbl_MatHang.ID = @ID) IS NOT NULL
				)
				BEGIN
					BEGIN TRY
						UPDATE dbo.tbl_MatHang
						SET
							Ten = @Ten,
							MoTa = @MoTa,
							Gia = @Gia,
							NgayCapNhat = GETDATE()
						WHERE ID = @ID
															
						SET @Result=1;
					END TRY

					BEGIN CATCH
						SET @Result =-1;
						PRINT ERROR_MESSAGE();	
					END CATCH
				END -- End of If NCC
			ELSE
				BEGIN 
					SET @Result=-3; --Empty
				END -- End of Else
			END -- End of If PhanQuyen
		END -- End of If
	ELSE
	BEGIN
		SET @Result=-4;
	END -- End of Else
END -- End of Proc
GO
/****** Object:  StoredProcedure [dbo].[pr_NhaCungCap_SearchPaging]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[pr_NhaCungCap_SearchPaging] @SearchValue NVARCHAR(300),@SearchType INT,@CurPage INT ,@PageSize INT ,@OrderBy INT,@IsDes BIT 
AS
BEGIN
	DECLARE @a INT ;
DECLARE @Result TABLE 
				(	
					ID INT PRIMARY KEY  ,
					Ten NVARCHAR(300) NOT NULL ,
					DiaChi NVARCHAR(500) NOT NULL,
					SDT VARCHAR(12) NOT NULL,
					STK VARCHAR(30) NOT NULL,
					NganHang NVARCHAR(300) NOT NULL,
					TaiKhoanID INT,
					NgayTao DATETIME NOT NULL,
					NgayCapNhat DATETIME,
					IsDelete BIT
				);
DECLARE @ResultFinal TABLE 
				(
					ID INT PRIMARY KEY  ,
					Ten NVARCHAR(300) NOT NULL ,
					DiaChi NVARCHAR(500) NOT NULL,
					SDT VARCHAR(12) NOT NULL,
					STK VARCHAR(30) NOT NULL,
					NganHang NVARCHAR(300) NOT NULL,
					TaiKhoanID INT,
					NgayTao DATETIME NOT NULL,
					NgayCapNhat DATETIME,
					IsDelete BIT

				);
		IF(@SearchType=0) 
			BEGIN
			    INSERT INTO @Result
			    
				SELECT *
							
				FROM dbo.tbl_NhaCungCap
				WHERE dbo.tbl_NhaCungCap.IsDelete=0
				
			END

			ELSE IF (@SearchType=1 )
					BEGIN
								INSERT INTO @Result
								
								SELECT 	*
									
								FROM dbo.tbl_NhaCungCap
								WHERE dbo.tbl_NhaCungCap.ID=CONVERT(INT,@SearchValue) AND dbo.tbl_NhaCungCap.IsDelete=0

					END

			ELSE IF (@SearchType=3)
								BEGIN
									INSERT INTO @Result
								
									SELECT *
									FROM dbo.tbl_NhaCungCap
									WHERE LOWER(dbo.tbl_NhaCungCap.Ten) LIKE '%' + LOWER(@SearchValue) + '%' AND dbo.tbl_NhaCungCap.IsDelete=0
								END
			ELSE IF (@SearchType=4)
									BEGIN
										INSERT INTO @Result
										
										SELECT *
										FROM dbo.tbl_NhaCungCap
										WHERE dbo.tbl_NhaCungCap.TaiKhoanID=CONVERT(INT,@SearchValue) AND dbo.tbl_NhaCungCap.IsDelete=0
									END

								

		---------------------------------------------------------
        
		
		IF ( @IsDes=1 ) 
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT  * 
					FROM @Result
					ORDER BY [@Result].NgayTao DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

	
		END
		ELSE
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayTao ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

		END
		

END
GO
/****** Object:  StoredProcedure [dbo].[pr_System_Create]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_System_Create] @Result INT  OUTPUT,@CurUserID INT ,@TenHT NVARCHAR(300),@DiaChi NVARCHAR(500),
			@SDT VARCHAR(12),@STK VARCHAR(30),@NganHang NVARCHAR(300),@TaiKhoanID INT 

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1)
		BEGIN
				SET @Result=-1;
				IF ((	SELECT  ID
						FROM dbo.tbl_TaiKhoan 
						WHERE @TaiKhoanID=dbo.tbl_TaiKhoan.ID AND PhanQuyenID=1 AND IsDelete=0) IS NOT NULL)
						BEGIN
									IF((SELECT  ID 
												FROM dbo.tbl_HeThong 
												WHERE dbo.tbl_HeThong.TaiKhoanID=@TaiKhoanID AND dbo.tbl_HeThong.IsDelete=0) IS NULL)
												BEGIN
														BEGIN TRY
															INSERT INTO dbo.tbl_HeThong
															(
																Ten,
																DiaChi,
																SDT,
																STK,
																NganHang,
																TaiKhoanID
															)
															VALUES
															(   @TenHT, -- Ten - nvarchar(300)
															   @DiaChi, -- DiaChi - nvarchar(500)
																@SDT,  -- SDT - varchar(12)
															   @STK,  -- STK - varchar(30)
																@NganHang, -- NganHang - nvarchar(300)
																@TaiKhoanID -- TaiKhoanID - int
																)
																SET @Result=1;
																END TRY

															BEGIN CATCH
															SET @Result =-1;
															PRINT ERROR_MESSAGE();
								
															END CATCH
												END
												ELSE
												BEGIN 
												SET @Result=-2;
												END
						END
						ELSE SET @Result=-3
				
			END
			ELSE

			BEGIN
						SET @Result=-4;
			END
	
	
END

GO
/****** Object:  StoredProcedure [dbo].[pr_System_Delete]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_System_Delete] @Result INT  OUTPUT,@CurUserID INT ,@SystemID int

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1 )
		BEGIN
				SET @Result=-1;
				IF((SELECT  ID 
						FROM dbo.tbl_HeThong 
						WHERE ID=@SystemID AND IsDelete=0) IS NOT NULL )
						BEGIN
									BEGIN TRY
									
									--DELETE FROM dbo.tbl_TaiKhoan
									--WHERE ID=@UserID;
									UPDATE dbo.tbl_HeThong
									SET IsDelete=1
									WHERE ID=@SystemID
									
									SET @Result=1;	
									
									END TRY

									BEGIN CATCH
									SET @Result =-1;
								
									END CATCH
						END
						ELSE
						BEGIN 
						SET @Result=-3;
						END
			END
			ELSE

			BEGIN
						SET @Result=-4;
			END
	
	
END



GO
/****** Object:  StoredProcedure [dbo].[pr_System_Edit]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_System_Edit] @Result INT  OUTPUT,@CurUserID INT,@ID INT, @TenHT NVARCHAR(300),@DiaChi NVARCHAR(500),
@SDT VARCHAR(12),@STK VARCHAR(30),@NganHang NVARCHAR(300),@TaiKhoanID INT, @NgayCapNhat VARCHAR(10)

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		SET @Result=-1;
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1 )
		BEGIN
				
				IF(@PhanQuyenID=1) 
					BEGIN 
						IF((SELECT  ID 
						FROM dbo.tbl_HeThong 
						WHERE @CurUserID=dbo.tbl_HeThong.TaiKhoanID AND @ID=dbo.tbl_HeThong.ID) IS NOT NULL)
						BEGIN
									BEGIN TRY
								
										UPDATE dbo.tbl_HeThong
										SET
											Ten=@TenHT,
											DiaChi=@DiaChi,
											SDT=@SDT,
											STK=@STK,
											NganHang=@NganHang,
											TaiKhoanID=@TaiKhoanID,
											NgayCapNhat=GETDATE()
											WHERE dbo.tbl_HeThong.ID=@ID

									SET @Result=1;	
				
									END TRY

									BEGIN CATCH
									SET @Result =-1;
								
									END CATCH
						END
						ELSE
						BEGIN 
						SET @Result=-3;
						END

					END
				IF(@PhanQuyenID=0)
					BEGIN
						IF((SELECT  ID 
						FROM dbo.tbl_HeThong 
						WHERE  @ID=dbo.tbl_HeThong.ID) IS NOT NULL)
						BEGIN
									BEGIN TRY
										UPDATE dbo.tbl_HeThong
										SET
											Ten=@TenHT,
											DiaChi=@DiaChi,
											SDT=@SDT,
											STK=@STK,
											NganHang=@NganHang,
											TaiKhoanID=@TaiKhoanID,
											NgayCapNhat=GETDATE()
										WHERE dbo.tbl_HeThong.ID=@ID
									SET @Result=1;	
				
									END TRY

									BEGIN CATCH
									SET @Result =-1;
								
									END CATCH
						END
						ELSE
						BEGIN 
						SET @Result=-3;
						END
					END

			
			END
			ELSE

			BEGIN
						SET @Result=-4;
			END
	
	
END

GO
/****** Object:  StoredProcedure [dbo].[pr_System_GetByID]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_System_GetByID] (@ID INT )
	AS
	BEGIN
	    SELECT * FROM dbo.tbl_HeThong
		WHERE ID=@ID 
	END
    
	

    
	
GO
/****** Object:  StoredProcedure [dbo].[pr_System_GetByOwner]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_System_GetByOwner] (@UserID INT)
	AS
	BEGIN
		DECLARE @HeThongID INT;
		SELECT @HeThongID=dbo.tbl_HeThong.ID
		FROM dbo.tbl_HeThong
		WHERE TaiKhoanID=@UserID AND dbo.tbl_HeThong.IsDelete=0 

	    SELECT * FROM dbo.tbl_Kho
		WHERE dbo.tbl_Kho.HeTHongID=@HeThongID;
	END
    
	

    
	
GO
/****** Object:  StoredProcedure [dbo].[pr_System_SearchPaging]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[pr_System_SearchPaging] @SearchValue NVARCHAR(300),@SearchType INT,@CurPage INT ,@PageSize INT ,@OrderBy INT,@IsDes BIT 
AS
BEGIN
	DECLARE @a INT ;
DECLARE @Result TABLE 
				(	ID INT PRIMARY KEY ,
					Ten NVARCHAR(300) NOT NULL UNIQUE,
					DiaChi NVARCHAR(500) NOT NULL ,
					SDT VARCHAR(12) NOT NULL,
					STK VARCHAR(30) NOT NULL ,
					NganHang NVARCHAR(300) NOT NULL,
					TaiKhoanID INT,
					NgayTao DATETIME NOT NULL,
					NgayCapNhat DATETIME,
					IsDelete BIT
				);
DECLARE @ResultFinal TABLE 
				(
					ID INT PRIMARY KEY ,
					Ten NVARCHAR(300) NOT NULL UNIQUE,
					DiaChi NVARCHAR(500) NOT NULL ,
					SDT VARCHAR(12) NOT NULL,
					STK VARCHAR(30) NOT NULL ,
					NganHang NVARCHAR(300) NOT NULL,
					TaiKhoanID INT,
					NgayTao DATETIME NOT NULL,
					NgayCapNhat DATETIME,
					IsDelete BIT 


				);
		IF(@SearchType=0) 
			BEGIN
			    INSERT INTO @Result
			    
				SELECT 
						dbo.tbl_HeThong.ID,
						dbo.tbl_HeThong.Ten,
						dbo.tbl_HeThong.DiaChi,
						dbo.tbl_HeThong.SDT,
						dbo.tbl_HeThong.STK,
						dbo.tbl_HeThong.NganHang,
						dbo.tbl_HeThong.TaiKhoanID,
						dbo.tbl_HeThong.NgayTao,
						dbo.tbl_HeThong.NgayCapNhat,
						dbo.tbl_HeThong.IsDelete
				FROM dbo.tbl_HeThong
				WHERE dbo.tbl_HeThong.IsDelete=0
				
			END

			ELSE IF (@SearchType=1 )
					BEGIN
								INSERT INTO @Result
								
								SELECT 	*
									
								FROM dbo.tbl_HeThong
								WHERE dbo.tbl_HeThong.ID=CONVERT(INT,@SearchValue) AND dbo.tbl_HeThong.IsDelete=0

					END

			ELSE IF (@SearchType=3)
								BEGIN
									INSERT INTO @Result
								
									SELECT *
									FROM dbo.tbl_HeThong
									WHERE LOWER(dbo.tbl_HeThong.Ten) LIKE '%' + LOWER(@SearchValue) + '%' AND dbo.tbl_HeThong.IsDelete=0
								END
			ELSE IF (@SearchType=4)
									BEGIN
										INSERT INTO @Result
										
										SELECT *
										FROM dbo.tbl_HeThong
										WHERE dbo.tbl_HeThong.TaiKhoanID=CONVERT(INT,@SearchValue) AND dbo.tbl_HeThong.IsDelete=0
									END

								

		---------------------------------------------------------
        
		
		IF ( @IsDes=1 ) 
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT  * 
					FROM @Result
					ORDER BY [@Result].NgayTao DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

	
		END
		ELSE
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayTao ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].Ten ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

		END
		

END
GO
/****** Object:  StoredProcedure [dbo].[pr_User_Create]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_User_Create] @Result INT  OUTPUT,@CurUserID INT ,@UserName VARCHAR(300), @Password VARCHAR(300),@HoTen NVARCHAR(300),
@NgaySinh VARCHAR(10) ,@SDT VARCHAR(12),@PhanQuyen INT ,@DiaChi NVARCHAR(500),@ChucVu NVARCHAR(300)

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1)
		BEGIN
				SET @Result=-1;
				IF((SELECT  ID 
						FROM dbo.tbl_TaiKhoan 
						WHERE @UserName=UserName) IS NULL)
						BEGIN
								BEGIN TRY
									INSERT INTO dbo.tbl_TaiKhoan
									(
										UserName,
										Pass,
										HoTen,
										NgaySinh,
										DiaChi,
										SDT,
										ChucVu,
										PhanQuyenID
									)
									VALUES
									(   @UserName,        -- UserName - varchar(300)
										@Password,        -- Pass - varchar(300)
										@HoTen,       -- HoTen - nvarchar(300)
										@NgaySinh, -- NgaySinh - date
										@DiaChi,      -- DiaChi - nvarchar(500)
										@SDT,        -- SDT - varchar(12)
										@ChucVu,      -- ChucVu - nvarchar(300)
										@PhanQuyen     -- PhanQuyenID - int
										)
										SET @Result=1;
										END TRY

									BEGIN CATCH
									SET @Result =-1;
								
									END CATCH
						END
						ELSE
						BEGIN 
						SET @Result=-2;
						END
			END
			ELSE

			BEGIN
						SET @Result=-4;
			END
	
	
END

GO
/****** Object:  StoredProcedure [dbo].[pr_User_Delete]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_User_Delete] @Result INT  OUTPUT,@CurUserID INT ,@UserID int

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1 )
		BEGIN
				SET @Result=-1;
				IF((SELECT  ID 
						FROM dbo.tbl_TaiKhoan 
						WHERE ID=@UserID AND IsDelete=0) IS NOT NULL )
						BEGIN
									BEGIN TRY
									
									--DELETE FROM dbo.tbl_TaiKhoan
									--WHERE ID=@UserID;
									UPDATE dbo.tbl_TaiKhoan
									SET IsDelete=1
									WHERE ID=@UserID
									
									SET @Result=1;	
									
									END TRY

									BEGIN CATCH
									SET @Result =-1;
								
									END CATCH
						END
						ELSE
						BEGIN 
						SET @Result=-3;
						END
			END
			ELSE

			BEGIN
						SET @Result=-4;
			END
	
	
END

GO
/****** Object:  StoredProcedure [dbo].[pr_User_Edit]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_User_Edit] @Result INT  OUTPUT,@CurUserID INT ,@UserID int, @Password VARCHAR(300),@HoTen NVARCHAR(300),
@NgaySinh VARCHAR(10) ,@SDT VARCHAR(12),@PhanQuyen INT ,@DiaChi NVARCHAR(500),@ChucVu NVARCHAR(300)

AS
BEGIN
	DECLARE @PhanQuyenID INT ;
		
		SELECT TOP(1) @PhanQuyenID=dbo.tbl_TaiKhoan.PhanQuyenID 
		FROM dbo.tbl_TaiKhoan
		WHERE @CurUserID=dbo.tbl_TaiKhoan.ID

		IF (@PhanQuyenID IS NOT NULL AND @PhanQuyenID<=1 )
		BEGIN
				SET @Result=-1;
				IF((SELECT  ID 
						FROM dbo.tbl_TaiKhoan 
						WHERE ID=@UserID) IS NOT NULL)
						BEGIN
									BEGIN TRY
									IF (@Password IS NOT NULL AND @Password<>'')
										UPDATE dbo.tbl_TaiKhoan 
										SET Pass=@Password 
										WHERE ID=@UserID;

									UPDATE dbo.tbl_TaiKhoan
									SET HoTen = @HoTen,
										NgaySinh = @NgaySinh,
										SDT = @SDT,
										DiaChi = @DiaChi,
										ChucVu = @ChucVu,
										PhanQuyenID = @PhanQuyen,
										NgayCapNhat= GETDATE()
									WHERE ID=@UserID ;

									SET @Result=1;	
									
									END TRY

									BEGIN CATCH
									SET @Result =-1;
								
									END CATCH
						END
						ELSE
						BEGIN 
						SET @Result=-3;
						END
			END
			ELSE

			BEGIN
						SET @Result=-4;
			END
	
	
END

GO
/****** Object:  StoredProcedure [dbo].[pr_User_GetByID]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_User_GetByID] (@ID INT )
	AS
	BEGIN
	    SELECT * FROM dbo.tbl_TaiKhoan
		WHERE ID=@ID
	END
    
	

    
	
GO
/****** Object:  StoredProcedure [dbo].[pr_User_GetByUserName]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[pr_User_GetByUserName] (@UserName VARCHAR(300) )
	AS
	BEGIN
	    SELECT * FROM dbo.tbl_TaiKhoan
		WHERE UserName=@UserName;
	END
    
	

    
	
GO
/****** Object:  StoredProcedure [dbo].[pr_User_SearchPaging]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[pr_User_SearchPaging] @SearchValue NVARCHAR(300),@SearchType INT,@CurPage INT ,@PageSize INT ,@OrderBy INT,@IsDes BIT 
AS
BEGIN
	DECLARE @a INT ;
DECLARE @Result TABLE 
				(ID INT PRIMARY KEY,
				UserName VARCHAR(300) NOT NULL,
				HoTen NVARCHAR(300) NOT NULL,
				NgaySinh DATE NOT NULL,
				DiaChi NVARCHAR(500) ,
				SDT VARCHAR(12) NOT NULL,
				ChucVu NVARCHAR(300),
				PhanQuyenID INT NOT NULL,
				NgayTao DATETIME NOT NULL,
				NgayCapNhat DATETIME
				);
DECLARE @ResultFinal TABLE 
				(ID INT PRIMARY KEY,
				UserName VARCHAR(300) NOT NULL,
				HoTen NVARCHAR(300) NOT NULL,
				NgaySinh DATE NOT NULL,
				DiaChi NVARCHAR(500) ,
				SDT VARCHAR(12) NOT NULL,
				ChucVu NVARCHAR(300),
				PhanQuyenID INT NOT NULL,
				NgayTao DATETIME NOT NULL,
				NgayCapNhat DATETIME


				);
		IF(@SearchType=0) 
			BEGIN
			    INSERT INTO @Result
			    (
			        ID,
			        UserName,
			        HoTen,
			        NgaySinh,
			        DiaChi,
			        SDT,
			        ChucVu,
			        PhanQuyenID,
					NgayTao,
					NgayCapNhat
			    )
				SELECT dbo.tbl_TaiKhoan.ID,
						dbo.tbl_TaiKhoan.UserName,
						dbo.tbl_TaiKhoan.HoTen,
						dbo.tbl_TaiKhoan.NgaySinh,
						dbo.tbl_TaiKhoan.DiaChi,
						dbo.tbl_TaiKhoan.SDT,
						dbo.tbl_TaiKhoan.ChucVu,
						dbo.tbl_TaiKhoan.PhanQuyenID,
						dbo.tbl_TaiKhoan.NgayTao,
						dbo.tbl_TaiKhoan.NgayCapNhat
				FROM dbo.tbl_TaiKhoan
				WHERE dbo.tbl_TaiKhoan.IsDelete=0
				
			END

			ELSE IF (@SearchType=1 OR @SearchType=4)
					BEGIN
								INSERT INTO @Result
								(
									ID,
									UserName,
									HoTen,
									NgaySinh,
									DiaChi,
									SDT,
									ChucVu,
									PhanQuyenID,
									NgayTao,
									NgayCapNhat
								)
								SELECT dbo.tbl_TaiKhoan.ID,
										dbo.tbl_TaiKhoan.UserName,
										dbo.tbl_TaiKhoan.HoTen,
										dbo.tbl_TaiKhoan.NgaySinh,
										dbo.tbl_TaiKhoan.DiaChi,
										dbo.tbl_TaiKhoan.SDT,
										dbo.tbl_TaiKhoan.ChucVu,
										dbo.tbl_TaiKhoan.PhanQuyenID,
										dbo.tbl_TaiKhoan.NgayTao,
										dbo.tbl_TaiKhoan.NgayCapNhat
								FROM dbo.tbl_TaiKhoan
								WHERE dbo.tbl_TaiKhoan.ID=CONVERT(INT,@SearchValue) AND dbo.tbl_TaiKhoan.IsDelete=0

					END

			ELSE IF (@SearchType=2)
								BEGIN
									INSERT INTO @Result
									(
										ID,
										UserName,
										HoTen,
										NgaySinh,
										DiaChi,
										SDT,
										ChucVu,
										PhanQuyenID,
										NgayTao,
										NgayCapNhat
									)
									SELECT dbo.tbl_TaiKhoan.ID,
											dbo.tbl_TaiKhoan.UserName,
											dbo.tbl_TaiKhoan.HoTen,
											dbo.tbl_TaiKhoan.NgaySinh,
											dbo.tbl_TaiKhoan.DiaChi,
											dbo.tbl_TaiKhoan.SDT,
											dbo.tbl_TaiKhoan.ChucVu,
											dbo.tbl_TaiKhoan.PhanQuyenID,
											dbo.tbl_TaiKhoan.NgayTao,
											dbo.tbl_TaiKhoan.NgayCapNhat
									FROM dbo.tbl_TaiKhoan
									WHERE LOWER(dbo.tbl_TaiKhoan.UserName) LIKE '%' + LOWER(@SearchValue) + '%' AND dbo.tbl_TaiKhoan.IsDelete=0
								END
			ELSE IF (@SearchType=3)
									BEGIN
										INSERT INTO @Result
										(
											ID,
											UserName,
											HoTen,
											NgaySinh,
											DiaChi,
											SDT,
											ChucVu,
											PhanQuyenID,
											NgayTao,
											NgayCapNhat
										)
										SELECT dbo.tbl_TaiKhoan.ID,
												dbo.tbl_TaiKhoan.UserName,
												dbo.tbl_TaiKhoan.HoTen,
												dbo.tbl_TaiKhoan.NgaySinh,
												dbo.tbl_TaiKhoan.DiaChi,
												dbo.tbl_TaiKhoan.SDT,
												dbo.tbl_TaiKhoan.ChucVu,
												dbo.tbl_TaiKhoan.PhanQuyenID,
												dbo.tbl_TaiKhoan.NgayTao,
												dbo.tbl_TaiKhoan.NgayCapNhat
										FROM dbo.tbl_TaiKhoan
										WHERE LOWER(dbo.tbl_TaiKhoan.HoTen) LIKE '%' + LOWER(@SearchValue) + '%' AND dbo.tbl_TaiKhoan.IsDelete=0
									END

								

		---------------------------------------------------------
        
		
		IF ( @IsDes=1 ) 
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT  * 
					FROM @Result
					ORDER BY [@Result].NgayTao DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].UserName DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat DESC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

	
		END
		ELSE
		BEGIN
				IF(@OrderBy=0)
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayTao ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=1) 
					--INSERT INTO @ResultFinal
				
					SELECT * 
					FROM @Result
					ORDER BY [@Result].ID ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=2) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].UserName ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

				IF(@OrderBy=3) 
					--INSERT INTO @ResultFinal
					
					SELECT * 
					FROM @Result
					ORDER BY [@Result].NgayCapNhat ASC
					OFFSET ((@CurPage-1)*@PageSize) ROWS  FETCH NEXT @PageSize ROWS ONLY 

		END
		

END
GO
/****** Object:  StoredProcedure [dbo].[pr_User_SerchPaging]    Script Date: 12/1/2021 10:13:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[pr_User_SerchPaging] @SearchValue NVARCHAR(300),@SearchType INT,@CurPage INT ,@PageSize INT ,@OrderBy INT,@IsDes BIT 
AS
BEGIN

DECLARE @Result TABLE 
				(ID INT PRIMARY KEY,
				UserName VARCHAR(300) NOT NULL,
				HoTen NVARCHAR(300) NOT NULL,
				NgaySinh DATE NOT NULL,
				DiaChi NVARCHAR(500) ,
				SDT VARCHAR(12) NOT NULL,
				ChucVu NVARCHAR(300),
				PhanQuyenID INT NOT NULL
				);
	
	INSERT INTO @Result
	(
	    ID,
	    UserName,
	    HoTen,
	    NgaySinh,
	    DiaChi,
	    SDT,
	    ChucVu,
	    PhanQuyenID
	) 
		SELECT	dbo.tbl_TaiKhoan.ID, 
				dbo.tbl_TaiKhoan.UserName,
				dbo.tbl_TaiKhoan.HoTen,
				dbo.tbl_TaiKhoan.NgaySinh,
				dbo.tbl_TaiKhoan.DiaChi,
				dbo.tbl_TaiKhoan.SDT,
				dbo.tbl_TaiKhoan.ChucVu,
				dbo.tbl_TaiKhoan.PhanQuyenID
		FROM dbo.tbl_TaiKhoan
		
		SELECT* 
		FROM @Result
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_HeThong"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 272
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "tbl_TaiKhoan"
            Begin Extent = 
               Top = 81
               Left = 352
               Bottom = 211
               Right = 522
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_HeThong'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_HeThong'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_HopDong"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 264
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_TaiKhoan"
            Begin Extent = 
               Top = 1
               Left = 280
               Bottom = 215
               Right = 459
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "tbl_NhaCungCap"
            Begin Extent = 
               Top = 67
               Left = 506
               Bottom = 313
               Right = 710
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_HopDong'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_HopDong'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_Kho"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 247
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_HeThong"
            Begin Extent = 
               Top = 27
               Left = 377
               Bottom = 202
               Right = 547
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1500
         Table = 1740
         Output = 1725
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_Kho'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_Kho'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_Menu"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 280
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_LinkMenu"
            Begin Extent = 
               Top = 24
               Left = 345
               Bottom = 137
               Right = 515
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_Menu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_Menu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_NhaCungCap"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 247
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_TaiKhoan"
            Begin Extent = 
               Top = 5
               Left = 331
               Bottom = 316
               Right = 538
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_NhaCungCap'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_NhaCungCap'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_PhanQuyen"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 198
               Right = 245
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 4785
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_PhanQuyen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_PhanQuyen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_TaiKhoan"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 318
               Right = 429
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2865
         Table = 1170
         Output = 1755
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_TaiKhoan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_TaiKhoan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "tbl_ViTri"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 258
               Right = 206
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tbl_Kho"
            Begin Extent = 
               Top = 8
               Left = 342
               Bottom = 138
               Right = 512
            End
            DisplayFlags = 280
            TopColumn = 3
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_ViTri'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_ViTri'
GO
USE [master]
GO
ALTER DATABASE [DA_CSDL_QLK] SET  READ_WRITE 
GO
