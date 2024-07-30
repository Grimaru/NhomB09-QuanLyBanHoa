use master
if exists (select * from sysdatabases where name='doan')
drop database doan
go 
create database doan
go
use doan
CREATE TABLE [dbo].[AdminUser] (
    [TenTK]     NVARCHAR (50) NOT NULL,
    [HoTen]     NVARCHAR (MAX) NULL,
    [MatKhau]   NCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([TenTK] ASC)
);

CREATE TABLE [dbo].[User] (
    [IDUser]    INT            IDENTITY (1, 1) NOT NULL,
	[TKND]   NVARCHAR (50) NULL,
    [TenND]  NVARCHAR (MAX) NULL,
    [SDTND] NVARCHAR (15)  NULL,
    [EmailND] NVARCHAR (MAX) NULL,
	[MatKhauND]   NCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([IDUser] ASC)
);

CREATE TABLE [dbo].[Voucher] (
    [IDVoucher] INT IDENTITY (1,1) NOT NULL,
	[CodeVoucher] CHAR(8) NULL,
	[PhantramVoucher] DECIMAL (18, 2) NULL,
	[HansudungVoucher] DATETIME NULL,
	[Tinhtrang] BIT DEFAULT 1 NULL
	PRIMARY KEY CLUSTERED ([IDVoucher] ASC)
);

CREATE TABLE [dbo].[Product] (
    [IDSP]     INT             IDENTITY (1, 1) NOT NULL,
    [TenSP]       NVARCHAR (MAX)  NULL,
    [MotaSP] NVARCHAR (MAX)  NULL,
    [GiaSP]         DECIMAL (18, 2) NULL,
    [HinhanhSP]      NVARCHAR (MAX)  NULL,
	[IDVoucher] INT        NULL,
    PRIMARY KEY CLUSTERED ([IDSP] ASC),
	FOREIGN KEY ([IDVoucher]) REFERENCES [dbo].[Voucher] ([IDVoucher])
);

CREATE TABLE [dbo].[OrderProduct] (
    [IDOP]             INT            IDENTITY (1, 1) NOT NULL,
    [NgayDH]         DATE           NULL,
    [IDUser]         INT            NULL,
	[TenND]  NVARCHAR (MAX) NULL,
    [Diachigiaohang] NVARCHAR (MAX) NULL,
	[IDVoucher] INT        NULL,
	[CodeVoucher] CHAR(8) NULL,
    PRIMARY KEY CLUSTERED ([IDOP] ASC),
    FOREIGN KEY ([IDUser]) REFERENCES [dbo].[User] ([IDUser]),
	FOREIGN KEY ([IDVoucher]) REFERENCES [dbo].[Voucher] ([IDVoucher])
);

CREATE TABLE [dbo].[DetailProduct] (
    [ID]        INT        IDENTITY (1, 1) NOT NULL,
    [IDSP] INT        NULL,
    [IDOP]   INT        NULL,
    [Soluong]  INT        NULL,
    [TongTien] FLOAT (53) NULL,
	[IDVoucher] INT        NULL,
	[CodeVoucher] CHAR(8) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([IDSP]) REFERENCES [dbo].[Product] ([IDSP]),
    FOREIGN KEY ([IDOP]) REFERENCES [dbo].[OrderProduct] ([IDOP]),
	FOREIGN KEY ([IDVoucher]) REFERENCES [dbo].[Voucher] ([IDVoucher])
);



