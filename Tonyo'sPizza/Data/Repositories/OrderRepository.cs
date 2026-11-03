using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class OrderRepository
    {
        public bool CreateOrder(Order order)
        {
            if (order == null || order.Items == null || order.Items.Count == 0)
            {
                throw new ArgumentException("Order must have at least one item.");
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert Order
                        string orderSql = @"INSERT INTO Orders (CustomerId, CustomerName, Phone, Address, TotalAmount, Status, Notes, CreatedAt)
                                            VALUES (@CustomerId, @CustomerName, @Phone, @Address, @TotalAmount, @Status, @Notes, @CreatedAt);
                                            SELECT SCOPE_IDENTITY();";

                        SqlParameter[] orderParams = {
                            new SqlParameter("@CustomerId", SqlDbType.Int) { Value = (object)order.CustomerId ?? DBNull.Value },
                            new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = order.CustomerName },
                            new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)order.Phone ?? DBNull.Value },
                            new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)order.Address ?? DBNull.Value },
                            new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = order.TotalAmount },
                            new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = order.Status },
                            new SqlParameter("@Notes", SqlDbType.NVarChar, 500) { Value = (object)order.Notes ?? DBNull.Value },
                            new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = order.CreatedAt }
                        };

                        object orderIdObj = DatabaseHelper.ExecuteScalar(orderSql, orderParams, trans);
                        order.OrderId = Convert.ToInt32(orderIdObj);

                        // 2. Insert Order Items & Deduct Ingredient Stock if recipe linked
                        foreach (var item in order.Items)
                        {
                            string itemSql = @"INSERT INTO OrderItems (OrderId, ItemId, ItemName, UnitPrice, Quantity, Subtotal, SelectedDoughSize, SelectedAddOns)
                                               VALUES (@OrderId, @ItemId, @ItemName, @UnitPrice, @Quantity, @Subtotal, @SelectedDoughSize, @SelectedAddOns);
                                               SELECT SCOPE_IDENTITY();";

                            SqlParameter[] itemParams = {
                                new SqlParameter("@OrderId", SqlDbType.Int) { Value = order.OrderId },
                                new SqlParameter("@ItemId", SqlDbType.Int) { Value = (object)item.ItemId ?? DBNull.Value },
                                new SqlParameter("@ItemName", SqlDbType.NVarChar, 150) { Value = item.ItemName },
                                new SqlParameter("@UnitPrice", SqlDbType.Decimal) { Value = item.UnitPrice },
                                new SqlParameter("@Quantity", SqlDbType.Int) { Value = item.Quantity },
                                new SqlParameter("@Subtotal", SqlDbType.Decimal) { Value = item.Subtotal },
                                new SqlParameter("@SelectedDoughSize", SqlDbType.NVarChar, 50) { Value = (object)item.SelectedDoughSize ?? DBNull.Value },
                                new SqlParameter("@SelectedAddOns", SqlDbType.NVarChar, 500) { Value = (object)item.SelectedAddOns ?? DBNull.Value }
                            };

                            object orderItemIdObj = DatabaseHelper.ExecuteScalar(itemSql, itemParams, trans);
                            item.OrderItemId = Convert.ToInt32(orderItemIdObj);
                        }

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Order> GetOrdersByCustomerId(int customerId, string filter = "MyOrders")
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            List<Order> list = new List<Order>();
            string sql;
            if (string.Equals(filter, "Archive", StringComparison.OrdinalIgnoreCase))
            {
                sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId AND (IsCustomerDeleted = 0 OR IsCustomerDeleted IS NULL) AND IsCustomerArchived = 1 ORDER BY OrderId DESC";
            }
            else if (string.Equals(filter, "Active", StringComparison.OrdinalIgnoreCase))
            {
                sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId AND (IsCustomerDeleted = 0 OR IsCustomerDeleted IS NULL) AND (IsCustomerArchived = 0 OR IsCustomerArchived IS NULL) AND Status IN ('Pending', 'Preparing', 'Ready to Serve') ORDER BY OrderId DESC";
            }
            else if (string.Equals(filter, "Past", StringComparison.OrdinalIgnoreCase))
            {
                sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId AND (IsCustomerDeleted = 0 OR IsCustomerDeleted IS NULL) AND (IsCustomerArchived = 0 OR IsCustomerArchived IS NULL) AND Status IN ('Completed', 'Cancelled') ORDER BY OrderId DESC";
            }
            else // "MyOrders" or "All"
            {
                sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId AND (IsCustomerDeleted = 0 OR IsCustomerDeleted IS NULL) AND (IsCustomerArchived = 0 OR IsCustomerArchived IS NULL) ORDER BY OrderId DESC";
            }
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                var order = MapRowToOrder(row);
                order.Items = GetOrderItems(order.OrderId);
                list.Add(order);
            }
            return list;
        }

        public List<Order> GetOrdersByCustomerId(int customerId, bool isArchived)
        {
            return GetOrdersByCustomerId(customerId, isArchived ? "Archive" : "MyOrders");
        }

        public bool ArchiveOrderForCustomer(int orderId, int customerId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Orders SET IsCustomerArchived = 1, CustomerArchivedAt = GETDATE() WHERE OrderId = @OrderId AND CustomerId = @CustomerId";
            SqlParameter[] parameters = {
                new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId },
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UnarchiveOrderForCustomer(int orderId, int customerId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Orders SET IsCustomerArchived = 0, CustomerArchivedAt = NULL WHERE OrderId = @OrderId AND CustomerId = @CustomerId";
            SqlParameter[] parameters = {
                new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId },
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool ArchiveOrdersForCustomer(List<int> orderIds, int customerId)
        {
            if (orderIds == null || orderIds.Count == 0) return false;
            DatabaseHelper.EnsureSchemaUpgrades();
            string idList = string.Join(",", orderIds);
            string sql = $"UPDATE Orders SET IsCustomerArchived = 1, CustomerArchivedAt = GETDATE() WHERE OrderId IN ({idList}) AND CustomerId = @CustomerId";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool UnarchiveOrdersForCustomer(List<int> orderIds, int customerId)
        {
            if (orderIds == null || orderIds.Count == 0) return false;
            DatabaseHelper.EnsureSchemaUpgrades();
            string idList = string.Join(",", orderIds);
            string sql = $"UPDATE Orders SET IsCustomerArchived = 0, CustomerArchivedAt = NULL WHERE OrderId IN ({idList}) AND CustomerId = @CustomerId";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteOrderForCustomer(int orderId, int customerId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            // Soft-deletes completed and cancelled orders from customer view while keeping
            // database records and items intact for accurate Admin Sales Reports.
            string sql = @"UPDATE Orders 
                           SET IsCustomerDeleted = 1, CustomerDeletedAt = GETDATE() 
                           WHERE OrderId = @OrderId AND CustomerId = @CustomerId AND Status IN ('Completed', 'Cancelled')";
            SqlParameter[] parameters = {
                new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId },
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public int DeleteOrdersForCustomer(List<int> orderIds, int customerId)
        {
            if (orderIds == null || orderIds.Count == 0) return 0;
            DatabaseHelper.EnsureSchemaUpgrades();
            string idList = string.Join(",", orderIds);
            string sql = $@"UPDATE Orders 
                           SET IsCustomerDeleted = 1, CustomerDeletedAt = GETDATE() 
                           WHERE OrderId IN ({idList}) AND CustomerId = @CustomerId AND Status IN ('Completed', 'Cancelled')";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerId", SqlDbType.Int) { Value = customerId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters);
        }

        public int DeleteCancelledOrdersForCustomer(List<int> orderIds, int customerId)
        {
            return DeleteOrdersForCustomer(orderIds, customerId);
        }

        public List<Order> GetAllOrdersForCrew(string statusFilter = null)
        {
            List<Order> list = new List<Order>();
            DatabaseHelper.EnsureSchemaUpgrades();

            string sql = "SELECT * FROM Orders WHERE ";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (statusFilter == "Archived")
            {
                sql += " IsArchived = 1 AND (ArchivedAt IS NULL OR ArchivedAt >= DATEADD(day, -30, GETDATE())) ";
            }
            else if (statusFilter == "Completed")
            {
                sql += " Status = 'Completed' AND (ArchivedAt IS NULL OR ArchivedAt >= DATEADD(day, -30, GETDATE())) ";
            }
            else
            {
                sql += " (IsArchived = 0 OR IsArchived IS NULL) ";
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    sql += " AND Status = @Status ";
                    parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = statusFilter });
                }
            }

            sql += " ORDER BY OrderId DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            foreach (DataRow row in dt.Rows)
            {
                var order = MapRowToOrder(row);
                order.Items = GetOrderItems(order.OrderId);
                list.Add(order);
            }
            return list;
        }

        public bool ArchiveOrder(int orderId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Orders SET IsArchived = 1, ArchivedAt = GETDATE() WHERE OrderId = @OrderId";
            SqlParameter[] parameters = { new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId } };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool ArchiveOrders(List<int> orderIds)
        {
            if (orderIds == null || orderIds.Count == 0) return false;
            DatabaseHelper.EnsureSchemaUpgrades();
            string idList = string.Join(",", orderIds);
            string sql = $"UPDATE Orders SET IsArchived = 1, ArchivedAt = GETDATE() WHERE OrderId IN ({idList})";
            return DatabaseHelper.ExecuteNonQuery(sql) > 0;
        }

        public bool CancelOrder(int orderId)
        {
            return UpdateOrderStatus(orderId, "Cancelled");
        }

        public bool CancelOrders(List<int> orderIds)
        {
            if (orderIds == null || orderIds.Count == 0) return false;
            bool anySuccess = false;
            foreach (var id in orderIds)
            {
                if (CancelOrder(id)) anySuccess = true;
            }
            return anySuccess;
        }

        public void PruneExpiredArchives()
        {
            try
            {
                string sql = "DELETE FROM Orders WHERE IsArchived = 1 AND ArchivedAt < DATEADD(day, -30, GETDATE())";
                DatabaseHelper.ExecuteNonQuery(sql);
            }
            catch
            {
                // Ignore
            }
        }

        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            DatabaseHelper.EnsureSchemaUpgrades();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Check current status and stock deduction flag
                        string checkSql = "SELECT Status, IsStockDeducted FROM Orders WHERE OrderId = @OrderId";
                        bool isDeducted = false;
                        string currentStatus = string.Empty;
                        using (var cmd = new SqlCommand(checkSql, conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                            using (var r = cmd.ExecuteReader())
                            {
                                if (r.Read())
                                {
                                    currentStatus = r["Status"] != DBNull.Value ? Convert.ToString(r["Status"]) : string.Empty;
                                    isDeducted = r["IsStockDeducted"] != DBNull.Value && Convert.ToBoolean(r["IsStockDeducted"]);
                                }
                            }
                        }

                        bool statusRequiresDeduction = string.Equals(newStatus, "Ready to Serve", StringComparison.OrdinalIgnoreCase) ||
                                                       string.Equals(newStatus, "Completed", StringComparison.OrdinalIgnoreCase);

                        // 1. If transitioning to "Ready to Serve" (or Completed directly) and not yet deducted: deduct stock!
                        if (statusRequiresDeduction && !isDeducted)
                        {
                            DeductOrderStock(orderId, conn, trans);
                            isDeducted = true;
                        }
                        // 2. If cancelling an order whose stock was previously deducted: restore stock!
                        else if (string.Equals(newStatus, "Cancelled", StringComparison.OrdinalIgnoreCase) && isDeducted)
                        {
                            RestoreOrderStock(orderId, conn, trans);
                            isDeducted = false;
                        }

                        // 3. Update Order Status and IsStockDeducted flag
                        string sql;
                        if (string.Equals(newStatus, "Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            sql = "UPDATE Orders SET Status = @Status, IsStockDeducted = @IsStockDeducted, IsArchived = 1, ArchivedAt = GETDATE() WHERE OrderId = @OrderId";
                        }
                        else
                        {
                            sql = "UPDATE Orders SET Status = @Status, IsStockDeducted = @IsStockDeducted WHERE OrderId = @OrderId";
                        }

                        using (var cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = newStatus });
                            cmd.Parameters.Add(new SqlParameter("@IsStockDeducted", SqlDbType.Bit) { Value = isDeducted });
                            cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();

                        // Notify in-memory stock service to immediately refresh
                        try
                        {
                            Tonyo_sPizza.Services.StockService.Instance.ReloadFromDatabase();
                        }
                        catch { }

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        private void DeductOrderStock(int orderId, SqlConnection conn, SqlTransaction trans)
        {
            // A. Deduct recipe ingredients
            string recipeDeduct = @"UPDATE i 
                                   SET CurrentStock = CASE WHEN i.CurrentStock >= (mi.QuantityRequired * oi.Quantity) 
                                                           THEN i.CurrentStock - (mi.QuantityRequired * oi.Quantity) 
                                                           ELSE 0 END 
                                   FROM Ingredients i 
                                   INNER JOIN MenuItemIngredients mi ON i.IngredientId = mi.IngredientId 
                                   INNER JOIN OrderItems oi ON mi.ItemId = oi.ItemId 
                                       AND (mi.SizeName = ISNULL(oi.SelectedDoughSize, '') 
                                            OR (mi.SizeName = '' AND NOT EXISTS (SELECT 1 FROM MenuItemIngredients mi2 WHERE mi2.ItemId = oi.ItemId AND mi2.SizeName = ISNULL(oi.SelectedDoughSize, ''))))
                                   WHERE oi.OrderId = @OrderId";
            using (var cmd = new SqlCommand(recipeDeduct, conn, trans))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                cmd.ExecuteNonQuery();
            }

            // B. Deduct dough ingredients (matched by ItemId and SelectedDoughSize)
            string doughDeduct = @"UPDATE i 
                                  SET CurrentStock = CASE WHEN i.CurrentStock >= (ds.DoughPortionQty * oi.Quantity) 
                                                          THEN i.CurrentStock - (ds.DoughPortionQty * oi.Quantity) 
                                                          ELSE 0 END 
                                  FROM Ingredients i 
                                  INNER JOIN MenuItemDoughSizes ds ON i.IngredientId = ds.IngredientId 
                                  INNER JOIN OrderItems oi ON ds.ItemId = oi.ItemId AND ds.SizeName = oi.SelectedDoughSize 
                                  WHERE oi.OrderId = @OrderId AND ds.IngredientId IS NOT NULL";
            using (var cmd = new SqlCommand(doughDeduct, conn, trans))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                cmd.ExecuteNonQuery();
            }

            // C. Deduct add-on ingredients
            string addOnDeduct = @"UPDATE i 
                                  SET CurrentStock = CASE WHEN i.CurrentStock >= (ao.PortionQty * oi.Quantity) 
                                                          THEN i.CurrentStock - (ao.PortionQty * oi.Quantity) 
                                                          ELSE 0 END 
                                  FROM Ingredients i 
                                  INNER JOIN MenuItemAddOns ao ON i.IngredientId = ao.IngredientId 
                                  INNER JOIN OrderItems oi ON ao.ItemId = oi.ItemId 
                                  WHERE oi.OrderId = @OrderId 
                                    AND oi.SelectedAddOns IS NOT NULL 
                                    AND CHARINDEX(ao.AddOnName, oi.SelectedAddOns) > 0";
            using (var cmd = new SqlCommand(addOnDeduct, conn, trans))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                cmd.ExecuteNonQuery();
            }
        }

        private void RestoreOrderStock(int orderId, SqlConnection conn, SqlTransaction trans)
        {
            // A. Restore recipe ingredients
            string recipeRestore = @"UPDATE i 
                                    SET CurrentStock = i.CurrentStock + (mi.QuantityRequired * oi.Quantity) 
                                    FROM Ingredients i 
                                    INNER JOIN MenuItemIngredients mi ON i.IngredientId = mi.IngredientId 
                                    INNER JOIN OrderItems oi ON mi.ItemId = oi.ItemId 
                                        AND (mi.SizeName = ISNULL(oi.SelectedDoughSize, '') 
                                             OR (mi.SizeName = '' AND NOT EXISTS (SELECT 1 FROM MenuItemIngredients mi2 WHERE mi2.ItemId = oi.ItemId AND mi2.SizeName = ISNULL(oi.SelectedDoughSize, ''))))
                                    WHERE oi.OrderId = @OrderId";
            using (var cmd = new SqlCommand(recipeRestore, conn, trans))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                cmd.ExecuteNonQuery();
            }

            // B. Restore dough ingredients
            string doughRestore = @"UPDATE i 
                                   SET CurrentStock = i.CurrentStock + (ds.DoughPortionQty * oi.Quantity) 
                                   FROM Ingredients i 
                                   INNER JOIN MenuItemDoughSizes ds ON i.IngredientId = ds.IngredientId 
                                   INNER JOIN OrderItems oi ON ds.ItemId = oi.ItemId AND ds.SizeName = oi.SelectedDoughSize 
                                   WHERE oi.OrderId = @OrderId AND ds.IngredientId IS NOT NULL";
            using (var cmd = new SqlCommand(doughRestore, conn, trans))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                cmd.ExecuteNonQuery();
            }

            // C. Restore add-ons
            string addOnRestore = @"UPDATE i 
                                   SET CurrentStock = i.CurrentStock + (ao.PortionQty * oi.Quantity) 
                                   FROM Ingredients i 
                                   INNER JOIN MenuItemAddOns ao ON i.IngredientId = ao.IngredientId 
                                   INNER JOIN OrderItems oi ON ao.ItemId = oi.ItemId 
                                   WHERE oi.OrderId = @OrderId 
                                     AND oi.SelectedAddOns IS NOT NULL 
                                     AND CHARINDEX(ao.AddOnName, oi.SelectedAddOns) > 0";
            using (var cmd = new SqlCommand(addOnRestore, conn, trans))
            {
                cmd.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId });
                cmd.ExecuteNonQuery();
            }
        }

        public List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> list = new List<OrderItem>();
            string sql = @"SELECT oi.*, m.ImageBase64 
                           FROM OrderItems oi 
                           LEFT JOIN MenuItems m ON oi.ItemId = m.ItemId 
                           WHERE oi.OrderId = @OrderId";

            SqlParameter[] parameters = {
                new SqlParameter("@OrderId", SqlDbType.Int) { Value = orderId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                string doughSize = row.Table.Columns.Contains("SelectedDoughSize") && row["SelectedDoughSize"] != DBNull.Value ? Convert.ToString(row["SelectedDoughSize"]) : null;
                string addOns = row.Table.Columns.Contains("SelectedAddOns") && row["SelectedAddOns"] != DBNull.Value ? Convert.ToString(row["SelectedAddOns"]) : null;

                list.Add(new OrderItem(
                    Convert.ToInt32(row["OrderItemId"]),
                    Convert.ToInt32(row["OrderId"]),
                    row["ItemId"] != DBNull.Value ? (int?)Convert.ToInt32(row["ItemId"]) : null,
                    Convert.ToString(row["ItemName"]),
                    Convert.ToDecimal(row["UnitPrice"]),
                    Convert.ToInt32(row["Quantity"]),
                    Convert.ToDecimal(row["Subtotal"]),
                    row["ImageBase64"] != DBNull.Value ? Convert.ToString(row["ImageBase64"]) : null,
                    doughSize,
                    addOns
                ));
            }
            return list;
        }

        private Order MapRowToOrder(DataRow row)
        {
            var o = new Order(
                Convert.ToInt32(row["OrderId"]),
                row["CustomerId"] != DBNull.Value ? (int?)Convert.ToInt32(row["CustomerId"]) : null,
                Convert.ToString(row["CustomerName"]),
                row["Phone"] != DBNull.Value ? Convert.ToString(row["Phone"]) : string.Empty,
                row["Address"] != DBNull.Value ? Convert.ToString(row["Address"]) : string.Empty,
                Convert.ToDecimal(row["TotalAmount"]),
                Convert.ToString(row["Status"]),
                row["Notes"] != DBNull.Value ? Convert.ToString(row["Notes"]) : string.Empty,
                Convert.ToDateTime(row["CreatedAt"])
            );

            if (row.Table.Columns.Contains("IsArchived") && row["IsArchived"] != DBNull.Value)
            {
                o.IsArchived = Convert.ToBoolean(row["IsArchived"]);
            }
            if (row.Table.Columns.Contains("ArchivedAt") && row["ArchivedAt"] != DBNull.Value)
            {
                o.ArchivedAt = Convert.ToDateTime(row["ArchivedAt"]);
            }
            if (row.Table.Columns.Contains("IsCustomerArchived") && row["IsCustomerArchived"] != DBNull.Value)
            {
                o.IsCustomerArchived = Convert.ToBoolean(row["IsCustomerArchived"]);
            }
            if (row.Table.Columns.Contains("CustomerArchivedAt") && row["CustomerArchivedAt"] != DBNull.Value)
            {
                o.CustomerArchivedAt = Convert.ToDateTime(row["CustomerArchivedAt"]);
            }
            if (row.Table.Columns.Contains("IsCustomerDeleted") && row["IsCustomerDeleted"] != DBNull.Value)
            {
                o.IsCustomerDeleted = Convert.ToBoolean(row["IsCustomerDeleted"]);
            }
            if (row.Table.Columns.Contains("CustomerDeletedAt") && row["CustomerDeletedAt"] != DBNull.Value)
            {
                o.CustomerDeletedAt = Convert.ToDateTime(row["CustomerDeletedAt"]);
            }
            if (row.Table.Columns.Contains("IsStockDeducted") && row["IsStockDeducted"] != DBNull.Value)
            {
                o.IsStockDeducted = Convert.ToBoolean(row["IsStockDeducted"]);
            }

            return o;
        }

        public List<Order> GetOrdersForSalesReport(DateTime? fromDate = null, DateTime? toDate = null, string statusFilter = null, string search = null)
        {
            List<Order> list = new List<Order>();
            DatabaseHelper.EnsureSchemaUpgrades();

            string sql = "SELECT * FROM Orders WHERE 1=1 ";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (fromDate.HasValue)
            {
                sql += " AND CreatedAt >= @FromDate ";
                parameters.Add(new SqlParameter("@FromDate", SqlDbType.DateTime) { Value = fromDate.Value.Date });
            }

            if (toDate.HasValue)
            {
                sql += " AND CreatedAt <= @ToDate ";
                parameters.Add(new SqlParameter("@ToDate", SqlDbType.DateTime) { Value = toDate.Value.Date.AddDays(1).AddTicks(-1) });
            }

            if (!string.IsNullOrEmpty(statusFilter) && 
                !string.Equals(statusFilter, "All Orders", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(statusFilter, "Show All Transactions", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(statusFilter, "All Transactions", StringComparison.OrdinalIgnoreCase) &&
                !statusFilter.StartsWith("Show All", StringComparison.OrdinalIgnoreCase) &&
                !statusFilter.StartsWith("All", StringComparison.OrdinalIgnoreCase))
            {
                sql += " AND Status = @Status ";
                parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 30) { Value = statusFilter });
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND (CustomerName LIKE @Search OR Phone LIKE @Search OR Address LIKE @Search) ";
                parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100) { Value = $"%{search.Trim()}%" });
            }

            sql += " ORDER BY OrderId DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            foreach (DataRow row in dt.Rows)
            {
                var order = MapRowToOrder(row);
                order.Items = GetOrderItems(order.OrderId);
                list.Add(order);
            }
            return list;
        }
    }
}
