SET STATISTICS TIME ON;


SELECT [products].[CodeType],
    SUM([products].[amount]) AS [Amount]
FROM (SELECT *
      FROM [Bigboy\SQL].[ShopFilial].[dbo].[Products]
      UNION ALL
      SELECT *
      FROM [192.168.3.10,1433].[ShopMain].[dbo].[Products]) AS [products]
GROUP BY [products].[CodeType];

SELECT *
FROM OPENROWSET(
        'SQLNCLI',
        'Server=Bigboy\SQL;Database=ShopFilial;UID=admin;PWD=123',
        'Select * from [dbo].[Employees] where PositionName = ''Manager''')
UNION ALL
SELECT *
FROM OPENROWSET(
        'SQLNCLI',
        'Server=192.168.3.10,1433;Database=ShopMain;UID=admin2;PWD=123',
        'select * from [dbo].[Employees] where PositionName = ''Manager''')


SELECT *
FROM OPENDATASOURCE(
        'SQLNCLI',
        'Data Source=Bigboy\SQL;User ID=admin;Password=123').[ShopFilial].[dbo].[Employees]
WHERE PositionName = 'Manager'
UNION ALL
SELECT *
FROM OPENDATASOURCE(
        'SQLNCLI',
        'Data Source=192.168.3.10,1433;User ID=admin2;Password=123').[ShopMain].[dbo].[Employees]
WHERE PositionName = 'Manager';

SELECT *
FROM OPENQUERY(
        [Bigboy\SQL],
        'SELECT * FROM [ShopFilial].[dbo].[Employees] where PositionName = ''Manager''')
UNION ALL
SELECT *
FROM OPENQUERY(
        [192.168.3.10,1433],
        'SELECT * FROM [ShopMain].[dbo].[Employees] where PositionName = ''Manager''')

SELECT [name],
    [is_data_access_enabled]
FROM [sys].[servers];

EXEC [sp_helpserver];

EXEC
            [sp_serveroption]
            @Server = 'Bigboy\SQL',
            @Optname = 'DATA ACCESS',
            @Optvalue = 'TRUE';

SELECT *
FROM [Bigboy\SQL].[ShopFilial].[dbo].[Employees]
WHERE [PositionName] =
      'Manager'
UNION ALL
SELECT *
FROM [192.168.3.10,1433].[ShopMain].[dbo].[Employees]
WHERE [PositionName] =
      'Manager';

EXEC
            [sp_addlinkedserver]
            @Server='Bigboy\SQL',
            @Srvproduct='',
            @Provider='SQLNCLI',
            @Datasrc='Bigboy\SQL',
            @Catalog='ShopFilial'

SELECT *
FROM [sys].[sysservers];