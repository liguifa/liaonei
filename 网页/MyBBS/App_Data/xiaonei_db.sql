
/****** Object:  Database [XiaoNei_DB]    Script Date: 2014/4/22 21:40:50 ******/
CREATE DATABASE [XiaoNei_DB] ON  PRIMARY 
( NAME = N'TEST', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\TEST.mdf' , SIZE = 7168KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TEST_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\TEST_log.ldf' , SIZE = 26816KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [XiaoNei_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [XiaoNei_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [XiaoNei_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [XiaoNei_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [XiaoNei_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [XiaoNei_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [XiaoNei_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [XiaoNei_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [XiaoNei_DB] SET  MULTI_USER 
GO
ALTER DATABASE [XiaoNei_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [XiaoNei_DB] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'XiaoNei_DB', N'ON'
GO
USE [XiaoNei_DB]
GO
/****** Object:  Table [dbo].[Publish]    Script Date: 2014/4/22 21:40:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Publish](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Goods] [bigint] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](250) NOT NULL,
	[Time] [varchar](50) NOT NULL,
	[Reply] [bigint] NOT NULL,
	[Status] [int] NOT NULL,
	[beRepliedUsername] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Publish] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_College]    Script Date: 2014/4/22 21:40:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_College](
	[ID] [bigint] NOT NULL,
	[College] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Grade]    Script Date: 2014/4/22 21:40:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Grade](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Grade] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_Person]    Script Date: 2014/4/22 21:40:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Person](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](max) NOT NULL,
	[college] [nvarchar](50) NOT NULL,
	[grade] [nvarchar](50) NOT NULL,
	[email] [nvarchar](50) NULL,
	[vip] [int] NULL,
	[qq] [nchar](20) NULL,
	[phone] [nchar](20) NULL,
	[sex] [nvarchar](10) NULL,
	[PubNum] [int] NULL,
 CONSTRAINT [PK_T_Person] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Table_3]    Script Date: 2014/4/22 21:40:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table_3](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[SRC] [nvarchar](250) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Introduction] [nvarchar](50) NOT NULL,
	[Details] [nvarchar](max) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[QQ] [nvarchar](50) NULL,
	[Time] [nvarchar](50) NOT NULL,
	[Price] [nvarchar](50) NOT NULL,
	[VIP] [int] NOT NULL,
	[Classification] [int] NOT NULL,
	[Donate] [int] NOT NULL,
 CONSTRAINT [PK_Table_3] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[T_College] ([ID], [College]) VALUES (1, N'信息学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (2, N'机械学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (3, N'外国语学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (4, N'纺织学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (5, N'生物学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (6, N'食品学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (7, N'艺术学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (8, N'生化学院')
INSERT [dbo].[T_College] ([ID], [College]) VALUES (9, N'管理学院')
SET IDENTITY_INSERT [dbo].[T_Grade] ON 

INSERT [dbo].[T_Grade] ([ID], [Grade]) VALUES (1, N'大一')
INSERT [dbo].[T_Grade] ([ID], [Grade]) VALUES (2, N'大二')
INSERT [dbo].[T_Grade] ([ID], [Grade]) VALUES (3, N'大三')
INSERT [dbo].[T_Grade] ([ID], [Grade]) VALUES (4, N'大四')
SET IDENTITY_INSERT [dbo].[T_Grade] OFF
SET IDENTITY_INSERT [dbo].[T_Person] ON 

INSERT [dbo].[T_Person] ([ID], [username], [password], [college], [grade], [email], [vip], [qq], [phone], [sex], [PubNum]) VALUES (40043, N'admin', N'B9D11B3BE25F5A1A7DC8CA04CD310B28', N'1', N'1', N'45', -1, N'1281054354          ', N'13204052042         ', N'', 2)
INSERT [dbo].[T_Person] ([ID], [username], [password], [college], [grade], [email], [vip], [qq], [phone], [sex], [PubNum]) VALUES (40047, N'root', N'2DFA2F725CECC23403BBEC1E401ABDDB', N'hyaha', N'1', N'1048229044', -1, N'10482290442QQ.COM   ', N'18840848462         ', N'', 0)
INSERT [dbo].[T_Person] ([ID], [username], [password], [college], [grade], [email], [vip], [qq], [phone], [sex], [PubNum]) VALUES (50048, N'xiaonei', N'CDF7B1014A293709C4599D257E14F458', N'1', N'2', N'liuhao2050qq@gmail.com', 1, N'12815054354         ', N'13204052042         ', N'男', 14)
INSERT [dbo].[T_Person] ([ID], [username], [password], [college], [grade], [email], [vip], [qq], [phone], [sex], [PubNum]) VALUES (50049, N'vanilla', N'C9524B386791FAFF03C2D7A8C59876AB', N'1', N'1', N'vanilla@qq.com', 0, N'vanilla             ', N'vanilla             ', N'男', 1)
INSERT [dbo].[T_Person] ([ID], [username], [password], [college], [grade], [email], [vip], [qq], [phone], [sex], [PubNum]) VALUES (50050, N'sss', N'2E027463D5B44E3419FC421354862B18', N'1', N'1', N'@qq.com', 0, N'                    ', N'                    ', N'男', 0)
SET IDENTITY_INSERT [dbo].[T_Person] OFF
SET IDENTITY_INSERT [dbo].[Table_3] ON 

INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10171, N'uploadfile/201404221936530556621.jpg', N'蜂蜜面包', N'delicious', N'你值得拥有', N'admin', N'13204052042         ', N'1281054354          ', N'2014年04月22日', N'22', 1, 6, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10172, N'uploadfile/201404221946007627781.jpg', N'阿狸', N'这是一只阿狸', N'刚买的，不想要了', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'5', 0, 4, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10173, N'uploadfile/201404221950178950138.jpg', N'折叠式蓝牙耳机9成新', N'折叠式蓝牙耳机9成新，', N'1. 使用时长：1.5年
2. 新旧程度描述：9新
3. 存在的问题：无
', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'80', 0, 2, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10177, N'uploadfile/201404221955415551333.jpg', N'超酷无线鼠标', N'很炫的鼠标', N'我手太小，用着不爽', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'10', 1, 2, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10178, N'uploadfile/201404222000569515934.jpg', N'九成新OPPO手机', N'宝贝不多说', N'这个卖家太懒了，宝贝描述里面一个字都不肯写。^_^！', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'500', 1, 2, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10179, N'uploadfile/201404222002297722419.jpg', N'闹钟', N'伴随我大学四年的闹表', N'真心舍不得(⊙o⊙)…', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'5', 1, 3, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10180, N'uploadfile/201404222003305080433.jpg', N'asp.net', N'大二的时候买的', N'大二的时候买的，现在用不着了，想捐给真正有用的', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'0', 1, 1, 1)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10181, N'uploadfile/201404222010536743900.jpg', N'Carrera GT', N'二手保时捷', N'要买兰博基尼，这台车子要转让，无事故记录。', N'vanilla', N'vanilla             ', N'vanilla             ', N'2014年04月22日', N'1500000', 0, 6, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10182, N'uploadfile/201404222132270904258.jpg', N'娃娃', N'捐赠', N'捐赠', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'0', 1, 5, 1)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10183, N'uploadfile/201404222133003580354.jpg', N'闹钟', N'捐赠', N'捐赠', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'0', 1, 5, 1)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10184, N'uploadfile/201404222134509041254.jpg', N'书', N'asp', N'书', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'20', 1, 1, 0)
INSERT [dbo].[Table_3] ([id], [SRC], [Title], [Introduction], [Details], [Username], [Phone], [QQ], [Time], [Price], [VIP], [Classification], [Donate]) VALUES (10186, N'uploadfile/201404222136035884732.jpg', N'阿狸', N'阿狸', N'测试', N'xiaonei', N'13204052042         ', N'12815054354         ', N'2014年04月22日', N'2', 1, 5, 0)
SET IDENTITY_INSERT [dbo].[Table_3] OFF
USE [master]
GO
ALTER DATABASE [XiaoNei_DB] SET  READ_WRITE 
GO
