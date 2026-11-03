-- ==========================================================
-- TONYO'S PIZZA RESTAURANT MANAGEMENT SYSTEM DATABASE SCRIPT
-- Microsoft SQL Server (Transact-SQL)
-- ==========================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'TonyosPizzaDB')
BEGIN
    CREATE DATABASE [TonyosPizzaDB];
END
GO

USE [TonyosPizzaDB];
GO

-- 1. Users Table (Authentication & Role Management)
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Users] (
        [UserId] INT IDENTITY(1,1) PRIMARY KEY,
        [Username] NVARCHAR(50) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(256) NOT NULL,
        [FullName] NVARCHAR(100) NOT NULL,
        [Role] NVARCHAR(20) NOT NULL CHECK ([Role] IN ('Admin', 'Crew', 'Customer')),
        [Phone] NVARCHAR(20) NULL,
        [Address] NVARCHAR(255) NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Default User Accounts (admin / crew / customer)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'admin')
BEGIN
    INSERT INTO [dbo].[Users] ([Username], [PasswordHash], [FullName], [Role], [Phone], [Address], [CreatedAt])
    VALUES ('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Tonyo Admin', 'Admin', '09123456789', 'Tonyos Pizza HQ', GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'crew')
BEGIN
    INSERT INTO [dbo].[Users] ([Username], [PasswordHash], [FullName], [Role], [Phone], [Address], [CreatedAt])
    VALUES ('crew', '19c4a83c653042cdb94a36aa5599ecde24631d03019d55085a29d9a6058dc8e9', 'Chef Mario', 'Crew', '09187654321', 'Kitchen Counter 1', GETDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE [Username] = 'customer')
BEGIN
    INSERT INTO [dbo].[Users] ([Username], [PasswordHash], [FullName], [Role], [Phone], [Address], [CreatedAt])
    VALUES ('customer', 'b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6', 'Samantha Miller', 'Customer', '09223344556', '4525 Saints Alley, Plant City', GETDATE());
END
GO

-- 2. Categories Table (Menu Category CRUD - e.g. Signature Pizza, Classic Pizza, Sides, Beverages, Desserts)
IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Categories] (
        [CategoryId] INT IDENTITY(1,1) PRIMARY KEY,
        [CategoryName] NVARCHAR(50) NOT NULL UNIQUE,
        [IconKey] NVARCHAR(50) NULL,
        [Description] NVARCHAR(255) NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 3. IngredientCategories Table (Ingredient Category Management - e.g. Dough, Cheese, Veggie, Meat, Sauce)
IF OBJECT_ID(N'dbo.IngredientCategories', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[IngredientCategories] (
        [IngredientCategoryId] INT IDENTITY(1,1) PRIMARY KEY,
        [CategoryName] NVARCHAR(50) NOT NULL UNIQUE,
        [Description] NVARCHAR(255) NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Seed default Ingredient Categories
IF NOT EXISTS (SELECT 1 FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Dough')
    INSERT INTO [dbo].[IngredientCategories] ([CategoryName], [Description]) VALUES ('Dough', 'Pizza dough, crusts, and flour bases');
IF NOT EXISTS (SELECT 1 FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Cheese')
    INSERT INTO [dbo].[IngredientCategories] ([CategoryName], [Description]) VALUES ('Cheese', 'Mozzarella, cheddar, parmesan, and specialty cheeses');
IF NOT EXISTS (SELECT 1 FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Veggie')
    INSERT INTO [dbo].[IngredientCategories] ([CategoryName], [Description]) VALUES ('Veggie', 'Vegetables, herbs, and plant-based toppings');
IF NOT EXISTS (SELECT 1 FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Meat')
    INSERT INTO [dbo].[IngredientCategories] ([CategoryName], [Description]) VALUES ('Meat', 'Pepperoni, chicken, beef, bacon, and cured meats');
IF NOT EXISTS (SELECT 1 FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Sauce')
    INSERT INTO [dbo].[IngredientCategories] ([CategoryName], [Description]) VALUES ('Sauce', 'Tomato sauces, BBQ, garlic butter, and dips');
GO

-- 4. Ingredients Table (Admin Ingredient Stock Tracking & Management)
IF OBJECT_ID(N'dbo.Ingredients', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Ingredients] (
        [IngredientId] INT IDENTITY(1,1) PRIMARY KEY,
        [CategoryId] INT NULL FOREIGN KEY REFERENCES [dbo].[IngredientCategories]([IngredientCategoryId]) ON DELETE SET NULL,
        [CategoryName] NVARCHAR(50) NOT NULL DEFAULT 'General',
        [Name] NVARCHAR(100) NOT NULL UNIQUE,
        [CurrentStock] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Unit] NVARCHAR(20) NOT NULL, -- e.g., 'g', 'kg', 'pcs', 'ml', 'L', 'slices'
        [MinThreshold] DECIMAL(18,2) NOT NULL DEFAULT 10,
        [CostPerUnit] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'CategoryName' AND Object_ID = Object_ID(N'dbo.Ingredients'))
    BEGIN
        ALTER TABLE [dbo].[Ingredients] ADD [CategoryName] NVARCHAR(50) NOT NULL DEFAULT 'General';
    END
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'CategoryId' AND Object_ID = Object_ID(N'dbo.Ingredients'))
    BEGIN
        ALTER TABLE [dbo].[Ingredients] ADD [CategoryId] INT NULL FOREIGN KEY REFERENCES [dbo].[IngredientCategories]([IngredientCategoryId]) ON DELETE SET NULL;
    END
END
GO

-- 5. MenuItems Table (Food Menu Items CRUD)
IF OBJECT_ID(N'dbo.MenuItems', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[MenuItems] (
        [ItemId] INT IDENTITY(1,1) PRIMARY KEY,
        [CategoryId] INT NULL FOREIGN KEY REFERENCES [dbo].[Categories]([CategoryId]) ON DELETE SET NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Price] DECIMAL(18,2) NOT NULL,
        [OldPrice] DECIMAL(18,2) NULL,
        [ImageBase64] NVARCHAR(MAX) NULL,
        [IsAvailable] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 6. MenuItemDoughSizes Table (Up to 4 Dough Sizes / Types per Pizza)
IF OBJECT_ID(N'dbo.MenuItemDoughSizes', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[MenuItemDoughSizes] (
        [SizeId] INT IDENTITY(1,1) PRIMARY KEY,
        [ItemId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[MenuItems]([ItemId]) ON DELETE CASCADE,
        [SizeName] NVARCHAR(50) NOT NULL, -- e.g. 'Small 10"', 'Medium 12"', 'Large 14"', 'Extra Large 16"'
        [Price] DECIMAL(18,2) NOT NULL,
        [Cost] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [IngredientId] INT NULL FOREIGN KEY REFERENCES [dbo].[Ingredients]([IngredientId]) ON DELETE SET NULL,
        [DoughPortionQty] DECIMAL(18,2) NOT NULL DEFAULT 1.00,
        [IsDefault] BIT NOT NULL DEFAULT 0
    );
END
GO

-- 7. MenuItemIngredients Table (Recipe Ingredients Link)
IF OBJECT_ID(N'dbo.MenuItemIngredients', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[MenuItemIngredients] (
        [ItemId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[MenuItems]([ItemId]) ON DELETE CASCADE,
        [IngredientId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Ingredients]([IngredientId]) ON DELETE CASCADE,
        [QuantityRequired] DECIMAL(18,2) NOT NULL,
        [SizeName] NVARCHAR(50) NOT NULL DEFAULT '',
        PRIMARY KEY ([ItemId], [IngredientId], [SizeName])
    );
END
GO

-- 8. MenuItemAddOns Table (Available Customer Add-ons by Category)
IF OBJECT_ID(N'dbo.MenuItemAddOns', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[MenuItemAddOns] (
        [AddOnId] INT IDENTITY(1,1) PRIMARY KEY,
        [ItemId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[MenuItems]([ItemId]) ON DELETE CASCADE,
        [IngredientId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Ingredients]([IngredientId]) ON DELETE CASCADE,
        [AddOnName] NVARCHAR(100) NOT NULL,
        [Price] DECIMAL(18,2) NOT NULL DEFAULT 1.50,
        [PortionQty] DECIMAL(18,2) NOT NULL DEFAULT 1.00,
        [CategoryName] NVARCHAR(50) NOT NULL DEFAULT 'Topping'
    );
END
GO

-- 9. Orders Table (Customer & Crew Workflow)
IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Orders] (
        [OrderId] INT IDENTITY(1,1) PRIMARY KEY,
        [CustomerId] INT NULL FOREIGN KEY REFERENCES [dbo].[Users]([UserId]) ON DELETE SET NULL,
        [CustomerName] NVARCHAR(100) NOT NULL,
        [Phone] NVARCHAR(20) NULL,
        [Address] NVARCHAR(255) NULL,
        [TotalAmount] DECIMAL(18,2) NOT NULL,
        [Status] NVARCHAR(30) NOT NULL DEFAULT 'Pending' CHECK ([Status] IN ('Pending', 'Preparing', 'Ready to Serve', 'Completed', 'Cancelled')),
        [Notes] NVARCHAR(500) NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [IsArchived] BIT NOT NULL DEFAULT 0,
        [ArchivedAt] DATETIME NULL,
        [IsCustomerArchived] BIT NOT NULL DEFAULT 0,
        [CustomerArchivedAt] DATETIME NULL,
        [IsStockDeducted] BIT NOT NULL DEFAULT 0
    );
END
GO

-- 10. OrderItems Table (Order Line Items)
IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[OrderItems] (
        [OrderItemId] INT IDENTITY(1,1) PRIMARY KEY,
        [OrderId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Orders]([OrderId]) ON DELETE CASCADE,
        [ItemId] INT NULL FOREIGN KEY REFERENCES [dbo].[MenuItems]([ItemId]) ON DELETE SET NULL,
        [ItemName] NVARCHAR(150) NOT NULL,
        [UnitPrice] DECIMAL(18,2) NOT NULL,
        [Quantity] INT NOT NULL,
        [Subtotal] DECIMAL(18,2) NOT NULL,
        [SelectedDoughSize] NVARCHAR(50) NULL,
        [SelectedAddOns] NVARCHAR(500) NULL
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'SelectedDoughSize' AND Object_ID = Object_ID(N'dbo.OrderItems'))
    BEGIN
        ALTER TABLE [dbo].[OrderItems] ADD [SelectedDoughSize] NVARCHAR(50) NULL;
    END
    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'SelectedAddOns' AND Object_ID = Object_ID(N'dbo.OrderItems'))
    BEGIN
        ALTER TABLE [dbo].[OrderItems] ADD [SelectedAddOns] NVARCHAR(500) NULL;
    END
END
GO

-- Indices
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MenuItems_CategoryId')
    CREATE NONCLUSTERED INDEX [IX_MenuItems_CategoryId] ON [dbo].[MenuItems]([CategoryId]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orders_CustomerId')
    CREATE NONCLUSTERED INDEX [IX_Orders_CustomerId] ON [dbo].[Orders]([CustomerId]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Orders_Status')
    CREATE NONCLUSTERED INDEX [IX_Orders_Status] ON [dbo].[Orders]([Status]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_OrderItems_OrderId')
    CREATE NONCLUSTERED INDEX [IX_OrderItems_OrderId] ON [dbo].[OrderItems]([OrderId]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MenuItemDoughSizes_ItemId')
    CREATE NONCLUSTERED INDEX [IX_MenuItemDoughSizes_ItemId] ON [dbo].[MenuItemDoughSizes]([ItemId]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_MenuItemAddOns_ItemId')
    CREATE NONCLUSTERED INDEX [IX_MenuItemAddOns_ItemId] ON [dbo].[MenuItemAddOns]([ItemId]);
GO

-- ==========================================================
-- SEED DATA: Categories, Ingredients, MenuItems, Dough Sizes & Recipes
-- ==========================================================

-- Seed Food Menu Categories
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'Signature Pizza')
    INSERT INTO [dbo].[Categories] ([CategoryName], [IconKey], [Description]) VALUES ('Signature Pizza', 'Pizza', 'Hand-tossed artisan pizzas with gourmet toppings');
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'Classic Pizza')
    INSERT INTO [dbo].[Categories] ([CategoryName], [IconKey], [Description]) VALUES ('Classic Pizza', 'Pizza', 'Traditional crowd favorites and timeless recipes');
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'Sides & Appetizers')
    INSERT INTO [dbo].[Categories] ([CategoryName], [IconKey], [Description]) VALUES ('Sides & Appetizers', 'Chicken', 'Crispy wings, garlic bread, and finger foods');
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'Beverages')
    INSERT INTO [dbo].[Categories] ([CategoryName], [IconKey], [Description]) VALUES ('Beverages', 'Beverage', 'Chilled sodas, iced teas, and juices');
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [CategoryName] = 'Desserts')
    INSERT INTO [dbo].[Categories] ([CategoryName], [IconKey], [Description]) VALUES ('Desserts', 'Pastry', 'Sweet treats, lava cakes, and gelatos');
GO

-- Seed Ingredients with Categories
DECLARE @CatDough INT = (SELECT IngredientCategoryId FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Dough');
DECLARE @CatCheese INT = (SELECT IngredientCategoryId FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Cheese');
DECLARE @CatVeggie INT = (SELECT IngredientCategoryId FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Veggie');
DECLARE @CatMeat INT = (SELECT IngredientCategoryId FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Meat');
DECLARE @CatSauce INT = (SELECT IngredientCategoryId FROM [dbo].[IngredientCategories] WHERE [CategoryName] = 'Sauce');

-- Dough
IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Pizza Dough')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatDough, 'Dough', 'Pizza Dough', 50.00, 'pcs', 10.00, 1.20);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Thin Crust Dough')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatDough, 'Dough', 'Thin Crust Dough', 40.00, 'pcs', 10.00, 1.00);

-- Cheese
IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Mozzarella Cheese')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatCheese, 'Cheese', 'Mozzarella Cheese', 5000.00, 'g', 1000.00, 0.02);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Cheddar Cheese')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatCheese, 'Cheese', 'Cheddar Cheese', 2000.00, 'g', 400.00, 0.02);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Parmesan Cheese')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatCheese, 'Cheese', 'Parmesan Cheese', 1000.00, 'g', 200.00, 0.03);

-- Sauce
IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Pizza Sauce')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatSauce, 'Sauce', 'Pizza Sauce', 4000.00, 'ml', 800.00, 0.01);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Smoky BBQ Sauce')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatSauce, 'Sauce', 'Smoky BBQ Sauce', 3000.00, 'ml', 500.00, 0.02);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Garlic Butter Sauce')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatSauce, 'Sauce', 'Garlic Butter Sauce', 2000.00, 'ml', 300.00, 0.02);

-- Meat
IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Pepperoni Slices')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatMeat, 'Meat', 'Pepperoni Slices', 300.00, 'slices', 50.00, 0.10);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'BBQ Chicken')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatMeat, 'Meat', 'BBQ Chicken', 2500.00, 'g', 500.00, 0.03);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Chicken Wings')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatMeat, 'Meat', 'Chicken Wings', 80.00, 'pcs', 20.00, 0.75);

-- Veggie
IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Bell Peppers')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatVeggie, 'Veggie', 'Bell Peppers', 1500.00, 'g', 300.00, 0.01);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Mushrooms')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatVeggie, 'Veggie', 'Mushrooms', 1200.00, 'g', 200.00, 0.02);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Onions')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatVeggie, 'Veggie', 'Onions', 2000.00, 'g', 300.00, 0.01);

IF NOT EXISTS (SELECT 1 FROM [dbo].[Ingredients] WHERE [Name] = 'Pineapple Chunks')
    INSERT INTO [dbo].[Ingredients] ([CategoryId], [CategoryName], [Name], [CurrentStock], [Unit], [MinThreshold], [CostPerUnit]) 
    VALUES (@CatVeggie, 'Veggie', 'Pineapple Chunks', 1500.00, 'g', 200.00, 0.02);
GO

-- Update category names on existing ingredients if null
UPDATE [dbo].[Ingredients] SET [CategoryName] = 'Dough' WHERE [Name] LIKE '%Dough%' AND [CategoryName] = 'General';
UPDATE [dbo].[Ingredients] SET [CategoryName] = 'Cheese' WHERE [Name] LIKE '%Cheese%' AND [CategoryName] = 'General';
UPDATE [dbo].[Ingredients] SET [CategoryName] = 'Sauce' WHERE [Name] LIKE '%Sauce%' AND [CategoryName] = 'General';
UPDATE [dbo].[Ingredients] SET [CategoryName] = 'Meat' WHERE ([Name] LIKE '%Pepperoni%' OR [Name] LIKE '%Chicken%' OR [Name] LIKE '%Ham%' OR [Name] LIKE '%Bacon%' OR [Name] LIKE '%Wings%') AND [CategoryName] = 'General';
UPDATE [dbo].[Ingredients] SET [CategoryName] = 'Veggie' WHERE ([Name] LIKE '%Pepper%' OR [Name] LIKE '%Mushroom%' OR [Name] LIKE '%Onion%' OR [Name] LIKE '%Pineapple%') AND [CategoryName] = 'General';
GO

-- Seed MenuItems
DECLARE @CatSig INT = (SELECT CategoryId FROM [dbo].[Categories] WHERE [CategoryName] = 'Signature Pizza');
DECLARE @CatClassic INT = (SELECT CategoryId FROM [dbo].[Categories] WHERE [CategoryName] = 'Classic Pizza');
DECLARE @CatSides INT = (SELECT CategoryId FROM [dbo].[Categories] WHERE [CategoryName] = 'Sides & Appetizers');
DECLARE @CatBev INT = (SELECT CategoryId FROM [dbo].[Categories] WHERE [CategoryName] = 'Beverages');
DECLARE @CatDes INT = (SELECT CategoryId FROM [dbo].[Categories] WHERE [CategoryName] = 'Desserts');

IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItems] WHERE [Name] = 'Pepperoni Feast Pizza')
    INSERT INTO [dbo].[MenuItems] ([CategoryId], [Name], [Description], [Price], [OldPrice], [IsAvailable]) 
    VALUES (@CatSig, 'Pepperoni Feast Pizza', 'Loaded with double layers of premium sliced pepperoni, mozzarella, and savory herb tomato sauce.', 14.99, 18.99, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItems] WHERE [Name] = 'BBQ Chicken Supreme')
    INSERT INTO [dbo].[MenuItems] ([CategoryId], [Name], [Description], [Price], [OldPrice], [IsAvailable]) 
    VALUES (@CatSig, 'BBQ Chicken Supreme', 'Tender BBQ marinated grilled chicken, red onions, crisp bell peppers, mozzarella, and sweet smoky BBQ drizzle.', 16.50, 19.99, 1);

IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItems] WHERE [Name] = 'Margherita Classic')
    INSERT INTO [dbo].[MenuItems] ([CategoryId], [Name], [Description], [Price], [OldPrice], [IsAvailable]) 
    VALUES (@CatClassic, 'Margherita Classic', 'Fresh mozzarella pearls, vine-ripened Italian tomatoes, aromatic sweet basil, and extra virgin olive oil.', 12.99, 15.50, 1);
GO

-- Seed Dough Sizes for Pizzas (Up to 4 Sizes: Small, Medium, Large, Extra Large)
DECLARE @ItemPep INT = (SELECT ItemId FROM [dbo].[MenuItems] WHERE [Name] = 'Pepperoni Feast Pizza');
DECLARE @ItemBBQ INT = (SELECT ItemId FROM [dbo].[MenuItems] WHERE [Name] = 'BBQ Chicken Supreme');
DECLARE @ItemMar INT = (SELECT ItemId FROM [dbo].[MenuItems] WHERE [Name] = 'Margherita Classic');
DECLARE @IngDough INT = (SELECT IngredientId FROM [dbo].[Ingredients] WHERE [Name] = 'Pizza Dough');

IF @ItemPep IS NOT NULL AND @IngDough IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemPep AND [SizeName] = 'Small 10"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemPep, 'Small 10"', 11.99, 1.20, @IngDough, 0.75, 0);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemPep AND [SizeName] = 'Medium 12"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemPep, 'Medium 12"', 14.99, 1.50, @IngDough, 1.00, 1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemPep AND [SizeName] = 'Large 14"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemPep, 'Large 14"', 18.99, 2.00, @IngDough, 1.50, 0);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemPep AND [SizeName] = 'Extra Large 16"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemPep, 'Extra Large 16"', 22.99, 2.50, @IngDough, 2.00, 0);
END

IF @ItemBBQ IS NOT NULL AND @IngDough IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemBBQ AND [SizeName] = 'Small 10"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemBBQ, 'Small 10"', 12.99, 1.30, @IngDough, 0.75, 0);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemBBQ AND [SizeName] = 'Medium 12"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemBBQ, 'Medium 12"', 16.50, 1.60, @IngDough, 1.00, 1);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemBBQ AND [SizeName] = 'Large 14"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemBBQ, 'Large 14"', 20.50, 2.10, @IngDough, 1.50, 0);

    IF NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemDoughSizes] WHERE [ItemId] = @ItemBBQ AND [SizeName] = 'Extra Large 16"')
        INSERT INTO [dbo].[MenuItemDoughSizes] ([ItemId], [SizeName], [Price], [Cost], [IngredientId], [DoughPortionQty], [IsDefault])
        VALUES (@ItemBBQ, 'Extra Large 16"', 24.50, 2.60, @IngDough, 2.00, 0);
END
GO

-- Seed Available Add-ons for Pizzas
DECLARE @ItemPep INT = (SELECT ItemId FROM [dbo].[MenuItems] WHERE [Name] = 'Pepperoni Feast Pizza');
DECLARE @IngMozz INT = (SELECT IngredientId FROM [dbo].[Ingredients] WHERE [Name] = 'Mozzarella Cheese');
DECLARE @IngCheddar INT = (SELECT IngredientId FROM [dbo].[Ingredients] WHERE [Name] = 'Cheddar Cheese');
DECLARE @IngPep INT = (SELECT IngredientId FROM [dbo].[Ingredients] WHERE [Name] = 'Pepperoni Slices');
DECLARE @IngMushroom INT = (SELECT IngredientId FROM [dbo].[Ingredients] WHERE [Name] = 'Mushrooms');
DECLARE @IngBBQSauce INT = (SELECT IngredientId FROM [dbo].[Ingredients] WHERE [Name] = 'Smoky BBQ Sauce');

IF @ItemPep IS NOT NULL
BEGIN
    IF @IngMozz IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemAddOns] WHERE [ItemId] = @ItemPep AND [IngredientId] = @IngMozz)
        INSERT INTO [dbo].[MenuItemAddOns] ([ItemId], [IngredientId], [AddOnName], [Price], [PortionQty], [CategoryName])
        VALUES (@ItemPep, @IngMozz, 'Extra Mozzarella Cheese', 1.75, 50.00, 'Cheese');

    IF @IngCheddar IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemAddOns] WHERE [ItemId] = @ItemPep AND [IngredientId] = @IngCheddar)
        INSERT INTO [dbo].[MenuItemAddOns] ([ItemId], [IngredientId], [AddOnName], [Price], [PortionQty], [CategoryName])
        VALUES (@ItemPep, @IngCheddar, 'Aged White Cheddar', 1.50, 40.00, 'Cheese');

    IF @IngPep IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemAddOns] WHERE [ItemId] = @ItemPep AND [IngredientId] = @IngPep)
        INSERT INTO [dbo].[MenuItemAddOns] ([ItemId], [IngredientId], [AddOnName], [Price], [PortionQty], [CategoryName])
        VALUES (@ItemPep, @IngPep, 'Extra Pepperoni (10 slices)', 2.00, 10.00, 'Meat');

    IF @IngMushroom IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemAddOns] WHERE [ItemId] = @ItemPep AND [IngredientId] = @IngMushroom)
        INSERT INTO [dbo].[MenuItemAddOns] ([ItemId], [IngredientId], [AddOnName], [Price], [PortionQty], [CategoryName])
        VALUES (@ItemPep, @IngMushroom, 'Sliced Fresh Mushrooms', 1.25, 40.00, 'Veggie');

    IF @IngBBQSauce IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[MenuItemAddOns] WHERE [ItemId] = @ItemPep AND [IngredientId] = @IngBBQSauce)
        INSERT INTO [dbo].[MenuItemAddOns] ([ItemId], [IngredientId], [AddOnName], [Price], [PortionQty], [CategoryName])
        VALUES (@ItemPep, @IngBBQSauce, 'Smoky BBQ Drizzle', 1.00, 30.00, 'Sauce');
END
GO

-- 11. AuditLogs Table (System Action & Security Audit Trail)
IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AuditLogs] (
        [LogId] INT IDENTITY(1,1) PRIMARY KEY,
        [UserId] INT NULL,
        [Username] NVARCHAR(50) NOT NULL,
        [Action] NVARCHAR(50) NOT NULL,
        [Details] NVARCHAR(500) NOT NULL,
        [Timestamp] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- 12. PromoVouchers Table (Discounts and Marketing Vouchers)
IF OBJECT_ID(N'dbo.PromoVouchers', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PromoVouchers] (
        [PromoId] INT IDENTITY(1,1) PRIMARY KEY,
        [Code] NVARCHAR(50) NOT NULL UNIQUE,
        [Description] NVARCHAR(255) NULL,
        [DiscountType] NVARCHAR(20) NOT NULL DEFAULT 'Percentage',
        [DiscountValue] DECIMAL(18,2) NOT NULL DEFAULT 10.00,
        [MinOrderAmount] DECIMAL(18,2) NOT NULL DEFAULT 0.00,
        [MaxDiscountAmount] DECIMAL(18,2) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ExpiresAt] DATETIME NULL
    );

    INSERT INTO [dbo].[PromoVouchers] ([Code], [Description], [DiscountType], [DiscountValue], [MinOrderAmount], [IsActive])
    VALUES 
    ('TONYO25', '25% discount voucher on wood-fired orders', 'Percentage', 25.00, 0.00, 1),
    ('PIZZA50', '₱50 off on artisan family orders', 'Fixed', 50.00, 300.00, 1),
    ('FREEPIZZA', 'Special promo treat voucher', 'Fixed', 100.00, 500.00, 1);
END
GO

PRINT 'Database TonyosPizzaDB updated successfully with all tables, constraints, audit logs, and seed data.';
GO
