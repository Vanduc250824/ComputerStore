USE ComputerStore;
GO

-- Bảng Users
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    CreatedAt DATETIME DEFAULT GETDATE(),
    Role NVARCHAR(50) DEFAULT 'Customer' CHECK (Role IN ('Admin', 'Customer'))
);

-- Bảng Categories
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

-- Bảng Products
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID) ON DELETE CASCADE,
    Price DECIMAL(10,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    ImageUrl NVARCHAR(255),
    Description NVARCHAR(1000)
);

-- Bảng Promotions (Chương trình khuyến mãi)
CREATE TABLE Promotions (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    DiscountPercent DECIMAL(5,2) CHECK (DiscountPercent BETWEEN 0 AND 100),
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Description NVARCHAR(1000)
);

-- Bảng liên kết sản phẩm với khuyến mãi
CREATE TABLE ProductPromotions (
    ProductPromotionID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID) ON DELETE CASCADE,
    PromotionID INT FOREIGN KEY REFERENCES Promotions(PromotionID) ON DELETE CASCADE
);

-- Bảng Orders
CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID) ON DELETE CASCADE,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL DEFAULT 0,
    Status NVARCHAR(50) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Shipped', 'Delivered', 'Cancelled')),
    PromotionID INT FOREIGN KEY REFERENCES Promotions(PromotionID) ON DELETE SET NULL -- Khuyến mãi áp dụng cho đơn hàng
);

-- Bảng OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID) ON DELETE CASCADE,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID) ON DELETE CASCADE,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);
CREATE TRIGGER trg_SetRoleBasedOnUsername
ON Users
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET Users.Role = 
        CASE 
            WHEN i.Username LIKE '%admin%' THEN 'Admin'
            ELSE 'Customer'
        END
    FROM Users
    INNER JOIN inserted i ON Users.UserID = i.UserID;
END;
Select * from Products
ALTER TABLE Products DROP COLUMN ImageData;
ALTER TABLE Products ADD ImageUrl NVARCHAR(255);
SELECT ProductID, ImageUrl FROM Products
UPDATE Products 
SET ImageUrl = '/Content/Images/Products/Check.png' 
WHERE Name = 'checkAdmin';
SELECT * FROM Products WHERE Name = 'checkAdmin';
