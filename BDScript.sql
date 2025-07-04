USE [MessengerDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatParticipants]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatParticipants](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [nvarchar](450) NOT NULL,
	[id_chat] [int] NOT NULL,
 CONSTRAINT [PK_ChatParticipants] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Chats]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chats](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[isGroup] [bit] NOT NULL,
 CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[id_message] [int] NOT NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendRequests]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendRequests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_sender] [nvarchar](450) NOT NULL,
	[id_recipient] [nvarchar](450) NOT NULL,
	[date_send] [date] NOT NULL,
 CONSTRAINT [PK_FriendRequests] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Friends]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friends](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [nvarchar](450) NOT NULL,
	[id_friend] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[text] [nvarchar](500) NOT NULL,
	[id_user] [nvarchar](450) NOT NULL,
	[id_chat] [int] NOT NULL,
	[timeSend] [datetime2](7) NOT NULL,
	[isRead] [bit] NOT NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleClaims]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](max) NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[NormalizedName] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [nvarchar](450) NOT NULL,
	[last_login_date] [date] NOT NULL,
	[id_photo] [int] NULL,
	[UserName] [nvarchar](max) NULL,
	[NormalizedUserName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[NormalizedEmail] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserImages]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserImages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserImages] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[id] [nvarchar](450) NOT NULL,
	[surname] [nvarchar](50) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[lastname] [nvarchar](50) NULL,
	[birthday] [date] NOT NULL,
	[id_job] [int] NOT NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogins]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogins](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](max) NOT NULL,
	[ProviderKey] [nvarchar](max) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTasks]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[date_end] [datetime] NOT NULL,
	[description] [nvarchar](max) NULL,
	[id_user] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserTasks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTokens]    Script Date: 21.05.2025 0:41:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250504165749_InitialCreate', N'9.0.4')
GO
SET IDENTITY_INSERT [dbo].[ChatParticipants] ON 

INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1099, N'2656df65-9337-45a5-aff5-218f4388ab29', 1038)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1100, N'74ac8ee7-869a-4f44-927f-f52fc8def384', 1038)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1101, N'2656df65-9337-45a5-aff5-218f4388ab29', 1039)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1102, N'22be42dc-9289-4bfc-b52b-90a496e5bf51', 1039)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1103, N'2656df65-9337-45a5-aff5-218f4388ab29', 1040)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1104, N'befdae03-2309-4a5a-859a-a12ccebe2dcc', 1040)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1105, N'74ac8ee7-869a-4f44-927f-f52fc8def384', 1041)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1106, N'22be42dc-9289-4bfc-b52b-90a496e5bf51', 1041)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1108, N'2656df65-9337-45a5-aff5-218f4388ab29', 1041)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1109, N'2656df65-9337-45a5-aff5-218f4388ab29', 1042)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1110, N'6fe66d4e-928a-4ce8-b4ed-a85550f0f362', 1042)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1111, N'74ac8ee7-869a-4f44-927f-f52fc8def384', 1043)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1113, N'2656df65-9337-45a5-aff5-218f4388ab29', 1043)
INSERT [dbo].[ChatParticipants] ([id], [id_user], [id_chat]) VALUES (1114, N'22be42dc-9289-4bfc-b52b-90a496e5bf51', 1043)
SET IDENTITY_INSERT [dbo].[ChatParticipants] OFF
GO
SET IDENTITY_INSERT [dbo].[Chats] ON 

INSERT [dbo].[Chats] ([id], [name], [isGroup]) VALUES (1038, NULL, 0)
INSERT [dbo].[Chats] ([id], [name], [isGroup]) VALUES (1039, NULL, 0)
INSERT [dbo].[Chats] ([id], [name], [isGroup]) VALUES (1040, NULL, 0)
INSERT [dbo].[Chats] ([id], [name], [isGroup]) VALUES (1041, N'Управление', 1)
INSERT [dbo].[Chats] ([id], [name], [isGroup]) VALUES (1042, NULL, 0)
INSERT [dbo].[Chats] ([id], [name], [isGroup]) VALUES (1043, N'Согласование проектов', 1)
SET IDENTITY_INSERT [dbo].[Chats] OFF
GO
SET IDENTITY_INSERT [dbo].[Files] ON 

INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1048, N'86c59403-2de5-41cc-8518-bbfd6ab4f363fileSend', 1266)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1049, N'e68ff2fe-f39b-4b91-a8ed-0c988d9cf315fileSend', 1270)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1050, N'fb24105f-375c-4808-8757-6d2090e29335fileSend', 1276)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1051, N'fadce662-28cf-4eb7-8784-bff688fa65f4fileSend', 1278)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1052, N'863c0f42-28ca-48a3-b3d5-8c233cf997effileSend', 1279)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1053, N'74ca5e41-7941-4e48-8302-75864664abeefileSend', 1280)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1054, N'35b5b311-ee72-4708-9be5-24688a160eb6fileSend', 1285)
INSERT [dbo].[Files] ([id], [name], [id_message]) VALUES (1055, N'eafa6f96-171c-4d86-a95e-633146e0f633fileSend', 1287)
SET IDENTITY_INSERT [dbo].[Files] OFF
GO
SET IDENTITY_INSERT [dbo].[FriendRequests] ON 

INSERT [dbo].[FriendRequests] ([id], [id_sender], [id_recipient], [date_send]) VALUES (1024, N'2656df65-9337-45a5-aff5-218f4388ab29', N'6fe66d4e-928a-4ce8-b4ed-a85550f0f362', CAST(N'2025-05-21' AS Date))
INSERT [dbo].[FriendRequests] ([id], [id_sender], [id_recipient], [date_send]) VALUES (1025, N'befdae03-2309-4a5a-859a-a12ccebe2dcc', N'2656df65-9337-45a5-aff5-218f4388ab29', CAST(N'2025-05-21' AS Date))
SET IDENTITY_INSERT [dbo].[FriendRequests] OFF
GO
SET IDENTITY_INSERT [dbo].[Friends] ON 

INSERT [dbo].[Friends] ([id], [id_user], [id_friend]) VALUES (1032, N'74ac8ee7-869a-4f44-927f-f52fc8def384', N'2656df65-9337-45a5-aff5-218f4388ab29')
INSERT [dbo].[Friends] ([id], [id_user], [id_friend]) VALUES (1033, N'2656df65-9337-45a5-aff5-218f4388ab29', N'74ac8ee7-869a-4f44-927f-f52fc8def384')
INSERT [dbo].[Friends] ([id], [id_user], [id_friend]) VALUES (1034, N'22be42dc-9289-4bfc-b52b-90a496e5bf51', N'2656df65-9337-45a5-aff5-218f4388ab29')
INSERT [dbo].[Friends] ([id], [id_user], [id_friend]) VALUES (1035, N'2656df65-9337-45a5-aff5-218f4388ab29', N'22be42dc-9289-4bfc-b52b-90a496e5bf51')
SET IDENTITY_INSERT [dbo].[Friends] OFF
GO
SET IDENTITY_INSERT [dbo].[Jobs] ON 

INSERT [dbo].[Jobs] ([id], [name]) VALUES (1005, N'Отдел кадров')
INSERT [dbo].[Jobs] ([id], [name]) VALUES (1006, N'Бухгалтер')
INSERT [dbo].[Jobs] ([id], [name]) VALUES (1007, N'Зам. Директора')
INSERT [dbo].[Jobs] ([id], [name]) VALUES (1008, N'Начальник КСУ')
INSERT [dbo].[Jobs] ([id], [name]) VALUES (1009, N'Секретарь')
INSERT [dbo].[Jobs] ([id], [name]) VALUES (1010, N'Программист')
SET IDENTITY_INSERT [dbo].[Jobs] OFF
GO
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1261, N'Привет!!!', N'2656df65-9337-45a5-aff5-218f4388ab29', 1038, CAST(N'2025-05-21T00:07:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1262, N'Привет!', N'74ac8ee7-869a-4f44-927f-f52fc8def384', 1038, CAST(N'2025-05-21T00:07:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1263, N'Ты сейчас в офисе?', N'74ac8ee7-869a-4f44-927f-f52fc8def384', 1038, CAST(N'2025-05-21T00:07:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1264, N'Да, что нужно?', N'2656df65-9337-45a5-aff5-218f4388ab29', 1038, CAST(N'2025-05-21T00:08:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1265, N'Можешь отправить вчерашний документ?', N'74ac8ee7-869a-4f44-927f-f52fc8def384', 1038, CAST(N'2025-05-21T00:08:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1266, N'<a href="/userFile/86c59403-2de5-41cc-8518-bbfd6ab4f363Документ.docx" class="link-warning link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">Документ.docx</a><p>Без проблем, вот.</p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1038, CAST(N'2025-05-21T00:08:35.6723868' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1267, N'Привет Иван, можешь подойти в мой кабинет?', N'2656df65-9337-45a5-aff5-218f4388ab29', 1039, CAST(N'2025-05-21T00:10:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1268, N'Привет, хорошо, сейчас буду', N'22be42dc-9289-4bfc-b52b-90a496e5bf51', 1039, CAST(N'2025-05-21T00:10:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1269, N'Отправь фото со вчерашней встречи, пожалуйста', N'2656df65-9337-45a5-aff5-218f4388ab29', 1039, CAST(N'2025-05-21T00:10:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1270, N'<img src="/userFile/e68ff2fe-f39b-4b91-a8ed-0c988d9cf315Photo2.jpg" class="p-1" width="100%"/><p>Конечно, вот</p>', N'22be42dc-9289-4bfc-b52b-90a496e5bf51', 1039, CAST(N'2025-05-21T00:11:03.6621500' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1271, N'Здравствуйте, отошлите мне пожалуйста отчёт по закупкам', N'befdae03-2309-4a5a-859a-a12ccebe2dcc', 1040, CAST(N'2025-05-21T00:12:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1272, N'Добрый день, этот чат предназначен для сотрудников управления', N'2656df65-9337-45a5-aff5-218f4388ab29', 1041, CAST(N'2025-05-21T00:20:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1273, N'Здравствуйте, да, довольно удобно ', N'befdae03-2309-4a5a-859a-a12ccebe2dcc', 1041, CAST(N'2025-05-21T00:20:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1274, N'Добрый вечер!', N'2656df65-9337-45a5-aff5-218f4388ab29', 1042, CAST(N'2025-05-21T00:22:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1275, N'Здравствуй, жду отчёт по прибывшей технике', N'6fe66d4e-928a-4ce8-b4ed-a85550f0f362', 1042, CAST(N'2025-05-21T00:22:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1276, N'<a href="/userFile/fb24105f-375c-4808-8757-6d2090e29335Отчёт.docx" class="link-warning link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">Отчёт.docx</a><p>Да, вот он</p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1042, CAST(N'2025-05-21T00:22:47.6774991' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1277, N'И вчерашний документ обсуждаемый на собрании', N'6fe66d4e-928a-4ce8-b4ed-a85550f0f362', 1042, CAST(N'2025-05-21T00:23:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1278, N'<a href="/userFile/fadce662-28cf-4eb7-8784-bff688fa65f4Документ.docx" class="link-warning link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">Документ.docx</a><p>Вот</p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1042, CAST(N'2025-05-21T00:23:41.0613108' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1279, N'<a href="/userFile/863c0f42-28ca-48a3-b3d5-8c233cf997efДокумент.docx" class="link-warning link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">Документ.docx</a><p>Всем необходимо ознакомится со следующим документом!!!</p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1041, CAST(N'2025-05-21T00:25:51.0552747' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1280, N'<a href="/userFile/74ca5e41-7941-4e48-8302-75864664abeeЗакупки.docx" class="link-warning link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover">Закупки.docx</a><p></p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1040, CAST(N'2025-05-21T00:26:51.9643330' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1281, N'Просмотрите пометки', N'2656df65-9337-45a5-aff5-218f4388ab29', 1040, CAST(N'2025-05-21T00:27:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1282, N'Да вижу, внесу необходимые изменения', N'befdae03-2309-4a5a-859a-a12ccebe2dcc', 1040, CAST(N'2025-05-21T00:33:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1283, N'Спасибо', N'2656df65-9337-45a5-aff5-218f4388ab29', 1040, CAST(N'2025-05-21T00:33:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1284, N'Добрый день, этот чат для согласования документов!', N'2656df65-9337-45a5-aff5-218f4388ab29', 1043, CAST(N'2025-05-21T00:35:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1285, N'<video src="/userFile/35b5b311-ee72-4708-9be5-24688a160eb6VideoCat.mp4" width="100%" controls class="p-1" preload="metadata"></video><p>В наш офис забежал кот, теперь не знаю, что делать</p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1039, CAST(N'2025-05-21T00:36:59.2966256' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1286, N'Ничего себе!', N'22be42dc-9289-4bfc-b52b-90a496e5bf51', 1039, CAST(N'2025-05-21T00:38:00.0000000' AS DateTime2), 0)
INSERT [dbo].[Messages] ([id], [text], [id_user], [id_chat], [timeSend], [isRead]) VALUES (1287, N'<div style="overflow: hidden;"><audio src="/userFile/eafa6f96-171c-4d86-a95e-633146e0f633SoundCat.mp3" class="mt-1" controls preload="metadata"></audio></div><p>Мурчит вот сидит</p>', N'2656df65-9337-45a5-aff5-218f4388ab29', 1039, CAST(N'2025-05-21T00:39:00.7256161' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Messages] OFF
GO
INSERT [dbo].[User] ([id], [last_login_date], [id_photo], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'22be42dc-9289-4bfc-b52b-90a496e5bf51', CAST(N'2025-05-21' AS Date), 1009, N'Petrov123@mail.ru', N'PETROV123@MAIL.RU', N'Petrov123@mail.ru', N'PETROV123@MAIL.RU', 0, N'AQAAAAIAAYagAAAAEPcag0L771YFVw3I+5iei6X9OHp5xH+NzVNkMJxLl7QxwJrfhbrmmd82xP53pEdGxA==', N'TSYC7CI2C3NUN5U7YV5JWZ7QEGIW3D33', N'41d6a11f-eb18-46df-88f8-6a5eb1aef2bc', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[User] ([id], [last_login_date], [id_photo], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2656df65-9337-45a5-aff5-218f4388ab29', CAST(N'2025-05-20' AS Date), NULL, N'Kozin123@mail.ru', N'KOZIN123@MAIL.RU', N'Kozin123@mail.ru', N'KOZIN123@MAIL.RU', 0, N'AQAAAAIAAYagAAAAEFcFclU1xqfoi/k4VLe09wnCzkdSTmM2YrUjpgIw4t0edy1w6Y2hdd9ne/OklCftYg==', N'FVVE47CHVLEICA55WOSONB6PUDRZ3YXD', N'd9ff43ff-8d1d-4303-9eff-9b52bae7480f', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[User] ([id], [last_login_date], [id_photo], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'6fe66d4e-928a-4ce8-b4ed-a85550f0f362', CAST(N'2025-05-21' AS Date), NULL, N'Guseev123@mail.ru', N'GUSEEV123@MAIL.RU', N'Guseev123@mail.ru', N'GUSEEV123@MAIL.RU', 0, N'AQAAAAIAAYagAAAAEL/Io2oThXQPXLfRPV+l3bb0gU6CN9blhyO3yKI40bpChBBxzI8UijBdGp55WHNfUQ==', N'B7CBIBHSUKMJWARHB4NGLIE3E4IOVV6G', N'bcafeaaf-10f2-4649-ace6-a5b63673e89d', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[User] ([id], [last_login_date], [id_photo], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'74ac8ee7-869a-4f44-927f-f52fc8def384', CAST(N'2025-05-20' AS Date), 1008, N'Varganova123@mail.ru', N'VARGANOVA123@MAIL.RU', N'Varganova123@mail.ru', N'VARGANOVA123@MAIL.RU', 0, N'AQAAAAIAAYagAAAAEPM4k0xKVc0G722rtLvvZltPy2TQEC0+zwS+fng80hnhzIsRp7CCIX6J80Cp2jk2yA==', N'7ORQNFWNT2Z5YBBRC6LUTJP6IHV4LTJB', N'2b5d03de-5a75-4487-9566-8b5873d0ecc9', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[User] ([id], [last_login_date], [id_photo], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'befdae03-2309-4a5a-859a-a12ccebe2dcc', CAST(N'2025-05-21' AS Date), 1010, N'Gleb123@mail.ru', N'GLEB123@MAIL.RU', N'Gleb123@mail.ru', N'GLEB123@MAIL.RU', 0, N'AQAAAAIAAYagAAAAEJ5oUrJr7ldvR3rcEqk0vZgRpu9rBp+COLhZ5b2tiRdmQJIlj72KuPq8v7GRu0DAxg==', N'U4XBHBTKMPB3HHTCWNI2DKX2WXEF746I', N'363b432b-2a9a-4f5c-bfad-37b6eb61bc99', NULL, 0, 0, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[UserImages] ON 

INSERT [dbo].[UserImages] ([id], [name]) VALUES (1004, N'Сталкер.jpg')
INSERT [dbo].[UserImages] ([id], [name]) VALUES (1005, N'full_vzlCQnjZ.jpg')
INSERT [dbo].[UserImages] ([id], [name]) VALUES (1006, N'User.png')
INSERT [dbo].[UserImages] ([id], [name]) VALUES (1007, N'avatar2.jpg')
INSERT [dbo].[UserImages] ([id], [name]) VALUES (1008, N'avatar5.jpg')
INSERT [dbo].[UserImages] ([id], [name]) VALUES (1009, N'avatar4.png')
INSERT [dbo].[UserImages] ([id], [name]) VALUES (1010, N'avatar1.jpg')
SET IDENTITY_INSERT [dbo].[UserImages] OFF
GO
INSERT [dbo].[UserInfo] ([id], [surname], [name], [lastname], [birthday], [id_job]) VALUES (N'22be42dc-9289-4bfc-b52b-90a496e5bf51', N'Петров', N'Иван', N'Александрович', CAST(N'1980-06-12' AS Date), 1005)
INSERT [dbo].[UserInfo] ([id], [surname], [name], [lastname], [birthday], [id_job]) VALUES (N'2656df65-9337-45a5-aff5-218f4388ab29', N'Козин', N'Владислав', N'Денисович', CAST(N'2005-01-03' AS Date), 1010)
INSERT [dbo].[UserInfo] ([id], [surname], [name], [lastname], [birthday], [id_job]) VALUES (N'6fe66d4e-928a-4ce8-b4ed-a85550f0f362', N'Гусеев', N'Данила', N'Адреевич', CAST(N'1987-12-03' AS Date), 1007)
INSERT [dbo].[UserInfo] ([id], [surname], [name], [lastname], [birthday], [id_job]) VALUES (N'74ac8ee7-869a-4f44-927f-f52fc8def384', N'Варганова', N'Анна', N'Витальевна', CAST(N'2005-03-25' AS Date), 1009)
INSERT [dbo].[UserInfo] ([id], [surname], [name], [lastname], [birthday], [id_job]) VALUES (N'befdae03-2309-4a5a-859a-a12ccebe2dcc', N'Фистахов', N'Глеб', N'Олегович', CAST(N'1976-05-19' AS Date), 1008)
GO
ALTER TABLE [dbo].[ChatParticipants]  WITH CHECK ADD  CONSTRAINT [FK_ChatParticipants_Chats] FOREIGN KEY([id_chat])
REFERENCES [dbo].[Chats] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChatParticipants] CHECK CONSTRAINT [FK_ChatParticipants_Chats]
GO
ALTER TABLE [dbo].[ChatParticipants]  WITH CHECK ADD  CONSTRAINT [FK_ChatParticipants_Users] FOREIGN KEY([id_user])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChatParticipants] CHECK CONSTRAINT [FK_ChatParticipants_Users]
GO
ALTER TABLE [dbo].[Files]  WITH CHECK ADD  CONSTRAINT [FK_Files_Messages] FOREIGN KEY([id_message])
REFERENCES [dbo].[Messages] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Files] CHECK CONSTRAINT [FK_Files_Messages]
GO
ALTER TABLE [dbo].[FriendRequests]  WITH CHECK ADD  CONSTRAINT [FK_FriendRequests_Users] FOREIGN KEY([id_recipient])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[FriendRequests] CHECK CONSTRAINT [FK_FriendRequests_Users]
GO
ALTER TABLE [dbo].[FriendRequests]  WITH CHECK ADD  CONSTRAINT [FK_FriendRequests_Users1] FOREIGN KEY([id_sender])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[FriendRequests] CHECK CONSTRAINT [FK_FriendRequests_Users1]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users] FOREIGN KEY([id_friend])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users]
GO
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users1] FOREIGN KEY([id_user])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users1]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Chats] FOREIGN KEY([id_chat])
REFERENCES [dbo].[Chats] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Chats]
GO
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users] FOREIGN KEY([id_user])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserImages] FOREIGN KEY([id_photo])
REFERENCES [dbo].[UserImages] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_Users_UserImages]
GO
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_Jobs] FOREIGN KEY([id_job])
REFERENCES [dbo].[Jobs] ([id])
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_Jobs]
GO
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_Users] FOREIGN KEY([id])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_Users]
GO
ALTER TABLE [dbo].[UserTasks]  WITH CHECK ADD  CONSTRAINT [FK_Tasks_Users] FOREIGN KEY([id_user])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserTasks] CHECK CONSTRAINT [FK_Tasks_Users]
GO
