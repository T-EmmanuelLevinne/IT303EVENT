using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Services;

namespace Tonyo_sPizza.Data
{
    public static class DatabaseHelper
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    try
                    {
                        var connSetting = ConfigurationManager.ConnectionStrings["TonyosPizzaDB"];
                        if (connSetting != null && !string.IsNullOrEmpty(connSetting.ConnectionString))
                        {
                            _connectionString = connSetting.ConnectionString;
                        }
                    }
                    catch
                    {
                        // Fallback
                    }

                    if (string.IsNullOrEmpty(_connectionString))
                    {
                        // Default fallback connection string for Local SQL Server / SQLEXPRESS / LocalDB
                        _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=TonyosPizzaDB;Integrated Security=True;TrustServerCertificate=True;";
                    }
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT 1", conn))
                    {
                        cmd.ExecuteScalar();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, $"ExecuteQuery: {query}");
                throw;
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var conn = GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            if (parameters != null && parameters.Length > 0)
                            {
                                cmd.Parameters.AddRange(parameters);
                            }
                            return cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, $"ExecuteNonQuery: {query}");
                throw;
            }
        }

        public static object ExecuteScalar(string query, SqlParameter[] parameters = null, SqlTransaction transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    using (var cmd = new SqlCommand(query, transaction.Connection, transaction))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
                else
                {
                    using (var conn = GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new SqlCommand(query, conn))
                        {
                            if (parameters != null && parameters.Length > 0)
                            {
                                cmd.Parameters.AddRange(parameters);
                            }
                            return cmd.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, $"ExecuteScalar: {query}");
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null, CommandBehavior behavior = CommandBehavior.CloseConnection)
        {
            EnsureSchemaUpgrades();
            SqlConnection conn = GetConnection();
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteReader(behavior);
                }
            }
            catch
            {
                conn.Dispose();
                throw;
            }
        }

        private static bool _schemaChecked = false;

        public static void ResetSchemaCheck()
        {
            _schemaChecked = false;
        }

        public static void EnsureSchemaUpgrades()
        {
            if (_schemaChecked) return;
            try
            {
                string[] statements = new string[]
                {
                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsArchived' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [IsArchived] BIT NOT NULL CONSTRAINT DF_Orders_IsArchived DEFAULT 0;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'ArchivedAt' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [ArchivedAt] DATETIME NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsCustomerArchived' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [IsCustomerArchived] BIT NOT NULL CONSTRAINT DF_Orders_IsCustomerArchived DEFAULT 0;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'CustomerArchivedAt' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [CustomerArchivedAt] DATETIME NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsCustomerDeleted' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [IsCustomerDeleted] BIT NOT NULL CONSTRAINT DF_Orders_IsCustomerDeleted DEFAULT 0;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'CustomerDeletedAt' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [CustomerDeletedAt] DATETIME NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsStockDeducted' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [IsStockDeducted] BIT NOT NULL CONSTRAINT DF_Orders_IsStockDeducted DEFAULT 0;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'SizeName' AND Object_ID = Object_ID(N'dbo.MenuItemIngredients'))
                      BEGIN
                          DECLARE @pkName NVARCHAR(128);
                          SELECT @pkName = name FROM sys.key_constraints WHERE type = 'PK' AND parent_object_id = OBJECT_ID(N'dbo.MenuItemIngredients');
                          IF @pkName IS NOT NULL
                          BEGIN
                              EXEC('ALTER TABLE [dbo].[MenuItemIngredients] DROP CONSTRAINT [' + @pkName + ']');
                          END
                          ALTER TABLE [dbo].[MenuItemIngredients] ADD [SizeName] NVARCHAR(50) NOT NULL CONSTRAINT DF_MenuItemIngredients_SizeName DEFAULT '';
                          ALTER TABLE [dbo].[MenuItemIngredients] ADD CONSTRAINT PK_MenuItemIngredients PRIMARY KEY ([ItemId], [IngredientId], [SizeName]);
                      END",

                    @"IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
                      BEGIN
                          CREATE TABLE [dbo].[AuditLogs] (
                              [LogId] INT IDENTITY(1,1) PRIMARY KEY,
                              [UserId] INT NULL,
                              [Username] NVARCHAR(50) NOT NULL,
                              [Action] NVARCHAR(50) NOT NULL,
                              [TableName] NVARCHAR(50) NULL,
                              [RecordId] NVARCHAR(50) NULL,
                              [Details] NVARCHAR(500) NOT NULL,
                              [Timestamp] DATETIME NOT NULL DEFAULT GETDATE()
                          );
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'TableName' AND Object_ID = Object_ID(N'dbo.AuditLogs'))
                      BEGIN
                          ALTER TABLE [dbo].[AuditLogs] ADD [TableName] NVARCHAR(50) NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RecordId' AND Object_ID = Object_ID(N'dbo.AuditLogs'))
                      BEGIN
                          ALTER TABLE [dbo].[AuditLogs] ADD [RecordId] NVARCHAR(50) NULL;
                      END",

                    // Security & Soft-Delete upgrades on Users
                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'Salt' AND Object_ID = Object_ID(N'dbo.Users'))
                      BEGIN
                          ALTER TABLE [dbo].[Users] ADD [Salt] NVARCHAR(64) NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsActive' AND Object_ID = Object_ID(N'dbo.Users'))
                      BEGIN
                          ALTER TABLE [dbo].[Users] ADD [IsActive] BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'FailedLoginAttempts' AND Object_ID = Object_ID(N'dbo.Users'))
                      BEGIN
                          ALTER TABLE [dbo].[Users] ADD [FailedLoginAttempts] INT NOT NULL CONSTRAINT DF_Users_FailedAttempts DEFAULT 0;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsLocked' AND Object_ID = Object_ID(N'dbo.Users'))
                      BEGIN
                          ALTER TABLE [dbo].[Users] ADD [IsLocked] BIT NOT NULL CONSTRAINT DF_Users_IsLocked DEFAULT 0;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'LockoutUntil' AND Object_ID = Object_ID(N'dbo.Users'))
                      BEGIN
                          ALTER TABLE [dbo].[Users] ADD [LockoutUntil] DATETIME NULL;
                      END",

                    // Soft-Delete IsActive flags on master tables
                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsActive' AND Object_ID = Object_ID(N'dbo.Categories'))
                      BEGIN
                          ALTER TABLE [dbo].[Categories] ADD [IsActive] BIT NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT 1;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsActive' AND Object_ID = Object_ID(N'dbo.Ingredients'))
                      BEGIN
                          ALTER TABLE [dbo].[Ingredients] ADD [IsActive] BIT NOT NULL CONSTRAINT DF_Ingredients_IsActive DEFAULT 1;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsActive' AND Object_ID = Object_ID(N'dbo.MenuItems'))
                      BEGIN
                          ALTER TABLE [dbo].[MenuItems] ADD [IsActive] BIT NOT NULL CONSTRAINT DF_MenuItems_IsActive DEFAULT 1;
                      END",

                    // Optimistic Concurrency RowVersion columns across master and transaction tables
                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RowVersion' AND Object_ID = Object_ID(N'dbo.Users'))
                      BEGIN
                          ALTER TABLE [dbo].[Users] ADD [RowVersion] ROWVERSION NOT NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RowVersion' AND Object_ID = Object_ID(N'dbo.Categories'))
                      BEGIN
                          ALTER TABLE [dbo].[Categories] ADD [RowVersion] ROWVERSION NOT NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RowVersion' AND Object_ID = Object_ID(N'dbo.Ingredients'))
                      BEGIN
                          ALTER TABLE [dbo].[Ingredients] ADD [RowVersion] ROWVERSION NOT NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RowVersion' AND Object_ID = Object_ID(N'dbo.MenuItems'))
                      BEGIN
                          ALTER TABLE [dbo].[MenuItems] ADD [RowVersion] ROWVERSION NOT NULL;
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RowVersion' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          ALTER TABLE [dbo].[Orders] ADD [RowVersion] ROWVERSION NOT NULL;
                      END",

                    // Second Header/Detail Transaction Module: StockTransactions & StockTransactionItems
                    @"IF OBJECT_ID(N'dbo.StockTransactions', N'U') IS NULL
                      BEGIN
                          CREATE TABLE [dbo].[StockTransactions] (
                              [StockTxId] INT IDENTITY(1,1) PRIMARY KEY,
                              [TransactionType] NVARCHAR(30) NOT NULL DEFAULT 'Restock' CHECK ([TransactionType] IN ('Restock', 'Adjustment', 'Void')),
                              [UserId] INT NULL FOREIGN KEY REFERENCES [dbo].[Users]([UserId]) ON DELETE SET NULL,
                              [Username] NVARCHAR(50) NOT NULL DEFAULT 'Admin',
                              [Notes] NVARCHAR(500) NULL,
                              [Status] NVARCHAR(20) NOT NULL DEFAULT 'Posted' CHECK ([Status] IN ('Posted', 'Voided')),
                              [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
                              [RowVersion] ROWVERSION NOT NULL
                          );
                      END",

                    @"IF OBJECT_ID(N'dbo.StockTransactionItems', N'U') IS NULL
                      BEGIN
                          CREATE TABLE [dbo].[StockTransactionItems] (
                              [DetailId] INT IDENTITY(1,1) PRIMARY KEY,
                              [StockTxId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[StockTransactions]([StockTxId]) ON DELETE CASCADE,
                              [IngredientId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Ingredients]([IngredientId]),
                              [IngredientName] NVARCHAR(100) NOT NULL,
                              [QuantityChange] DECIMAL(18,2) NOT NULL,
                              [Unit] NVARCHAR(20) NOT NULL,
                              [StockBefore] DECIMAL(18,2) NOT NULL,
                              [StockAfter] DECIMAL(18,2) NOT NULL
                          );
                      END",

                    @"IF OBJECT_ID(N'dbo.PromoVouchers', N'U') IS NULL
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
                              [ExpiresAt] DATETIME NULL,
                              [RowVersion] ROWVERSION NOT NULL
                          );

                          INSERT INTO [dbo].[PromoVouchers] ([Code], [Description], [DiscountType], [DiscountValue], [MinOrderAmount], [IsActive])
                          VALUES 
                          ('TONYO25', '25% discount voucher on wood-fired orders', 'Percentage', 25.00, 0.00, 1),
                          ('PIZZA50', '₱50 off on artisan family orders', 'Fixed', 50.00, 300.00, 1),
                          ('FREEPIZZA', 'Special promo treat voucher', 'Fixed', 100.00, 500.00, 1);
                      END",

                    @"IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'RowVersion' AND Object_ID = Object_ID(N'dbo.PromoVouchers'))
                      BEGIN
                          ALTER TABLE [dbo].[PromoVouchers] ADD [RowVersion] ROWVERSION NOT NULL;
                      END",

                    @"IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IsArchived' AND Object_ID = Object_ID(N'dbo.Orders'))
                      BEGIN
                          EXEC('UPDATE [dbo].[Orders] SET [IsArchived] = 1, [ArchivedAt] = ISNULL([ArchivedAt], GETDATE()) WHERE [Status] = ''Completed'' AND ([IsArchived] = 0 OR [IsArchived] IS NULL)');
                          EXEC('DELETE FROM [dbo].[Orders] WHERE [IsArchived] = 1 AND [ArchivedAt] < DATEADD(day, -30, GETDATE())');
                      END"
                };

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    foreach (var stmt in statements)
                    {
                        try
                        {
                            using (var cmd = new SqlCommand(stmt, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception innerEx)
                        {
                            ErrorLogger.Log(innerEx, "EnsureSchemaUpgrades statement");
                        }
                    }
                }
                _schemaChecked = true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "EnsureSchemaUpgrades connection");
            }
        }
    }
}
