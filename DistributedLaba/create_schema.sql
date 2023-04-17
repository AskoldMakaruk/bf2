CREATE TABLE [dbo].[Positions]
(
    [Name] NVARCHAR(32) NOT NULL,
    CONSTRAINT [PK_Positions] PRIMARY KEY ([Name])
);

CREATE TABLE [dbo].[Employees]
(
    [Id]           INT           NOT NULL IDENTITY,
    [Name]         NVARCHAR(MAX) NULL,
    [PositionName] NVARCHAR(32)  NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_Positions_PositionName] FOREIGN KEY ([PositionName]) REFERENCES [dbo].[Positions] ([Name])
);

CREATE TABLE [dbo].[ProductTypes]
(
    [Code] NVARCHAR(64)  NOT NULL,
    [Name] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_ProductTypes] PRIMARY KEY ([Code])
);

CREATE TABLE [dbo].[Products]
(
    [Id]       INT            NOT NULL IDENTITY,
    [CodeType] NVARCHAR(64)   NULL,
    [Amount]   INT            NOT NULL,
    [Price]    DECIMAL(19, 2) NOT NULL,
    [IsOnSale] BIT            NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_ProductTypes_CodeType] FOREIGN KEY ([CodeType]) REFERENCES [dbo].[ProductTypes] ([Code])
);

EXEC sp_addlinkedserver @server='Bigboy\SQL', @srvproduct='', @provider='SQLNCLI', @datasrc='Bigboy\SQL', @catalog='ShopFilial'
EXEC sp_addlinkedserver @server='BRANCH2', @srvproduct='', @provider='SQLNCLI', @datasrc='ROOT18C8C\MSSQLSERVER02', @catalog='master'