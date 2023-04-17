INSERT INTO [dbo].[Positions] ([Name])
VALUES ('Manager'),
       ('Cassier'),
       ('Loader');

INSERT INTO [dbo].[Employees] ([Name], [PositionName])
VALUES ('John Doe', 'Manager'),
       ('Jane Smith', 'Cassier'),
       ('Bob Johnson', 'Loader');

INSERT INTO [dbo].[ProductTypes] ([Code], [Name])
VALUES ('A', 'Абрикоси'),
       ('B', 'Вовки'),
       ('C', 'Стегно');

INSERT INTO [dbo].[Products] ([CodeType], [Amount], [Price], [IsOnSale])
VALUES ('A', 10, 19.99, 0),
       ('B', 5, 29.99, 1),
       ('C', 20, 9.99, 0);
