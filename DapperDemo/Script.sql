CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[Price] [decimal](18, 4) NULL,
	[Perimeter] [float] NULL,
	[Area] [real] NULL,
	[IsSmoking] [bit] NULL,
	CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC))

GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Id], [Name], [Age], [Price], [Perimeter], [Area], [IsSmoking]) VALUES (1, N'Name1', 11, CAST(11.1000 AS Decimal(18, 4)), 11.2, 11.3, 1)
INSERT [dbo].[Customer] ([Id], [Name], [Age], [Price], [Perimeter], [Area], [IsSmoking]) VALUES (2, N'Name2', 12, CAST(12.1000 AS Decimal(18, 4)), 12.2, 12.3, 0)
INSERT [dbo].[Customer] ([Id], [Name], [Age], [Price], [Perimeter], [Area], [IsSmoking]) VALUES (3, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO

CREATE PROCEDURE GetAllCustomers @Name nvarchar(50)
AS
SELECT * FROM Customer WHERE Name = @Name

GO

CREATE PROCEDURE GetMultipleTables
AS
SELECT * FROM Customer
SELECT Id, Name FROM Customer