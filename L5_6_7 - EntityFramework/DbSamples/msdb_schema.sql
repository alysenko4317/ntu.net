-- Створення таблиці клієнтів
CREATE TABLE t_customer (
    CustomerId INT PRIMARY KEY,
    CustomerName NVARCHAR(100)
);
GO

-- Створення таблиці замовлень
CREATE TABLE t_order (
    OrderId INT PRIMARY KEY,
    CustomerId INT,
    CreateDate DATETIME,
    CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerId) REFERENCES t_customer(CustomerId)
);
GO

-- Створення таблиці продуктів
CREATE TABLE t_product (
    ProductId INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    ProductPrice DECIMAL(10, 2)
);
GO

-- Створення таблиці, що зв'язує замовлення та продукти
CREATE TABLE t_order_product (
    OrderId INT,
    ProductId INT,
    Count INT,
    PRIMARY KEY (OrderId, ProductId),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY (OrderId) REFERENCES t_order(OrderId),
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY (ProductId) REFERENCES t_product(ProductId)
);
GO
