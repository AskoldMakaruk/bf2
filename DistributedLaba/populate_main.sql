INSERT INTO [dbo].[Positions] ([Name])
VALUES ('Manager'),
       ('CEO'),
       ('HR');

INSERT INTO [dbo].[Employees] ([Name], [PositionName])
VALUES ('John Manager', 'Manager'),
       ('Jane CEO', 'CEO'),
       ('Bob Hrson', 'HR');

INSERT INTO [dbo].[ProductTypes] ([Code], [Name])
VALUES ('N', 'nothing'),
       ('K', 'kiwi'),
       ('C', 'Стегно');

INSERT INTO [dbo].[Products] ([CodeType], [Amount], [Price], [IsOnSale])
VALUES ('N', 10, 9.99, 0),
       ('K', 5, 3.99, 1),
       ('C', 20, 9.99, 0);