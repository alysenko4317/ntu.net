-- Заповнення таблиці клієнтів
INSERT INTO t_customer (CustomerId, CustomerName)
VALUES
(1, 'John Doe'),
(2, 'Jane Smith'),
(3, 'Alice Johnson');
GO

-- Заповнення таблиці продуктів
INSERT INTO t_product (ProductId, ProductName, ProductPrice)
VALUES
(1, 'Laptop', 1500.00),
(2, 'Smartphone', 800.00),
(3, 'Tablet', 600.00);
GO

-- Заповнення таблиці замовлень
INSERT INTO t_order (OrderId, CustomerId, CreateDate)
VALUES
(1, 1, '2024-09-20'),
(2, 2, '2024-09-21'),
(3, 3, '2024-09-22');
GO

-- Заповнення таблиці, що зв'язує замовлення та продукти
INSERT INTO t_order_product (OrderId, ProductId, Count)
VALUES
(1, 1, 1),  -- John Doe купив 1 Laptop
(1, 2, 2),  -- John Doe купив 2 Smartphones
(2, 2, 1),  -- Jane Smith купила 1 Smartphone
(2, 3, 1),  -- Jane Smith купила 1 Tablet
(3, 1, 2),  -- Alice Johnson купила 2 Laptops
(3, 3, 1);  -- Alice Johnson купила 1 Tablet
GO
