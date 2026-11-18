using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    /// <summary>
    /// Manages atomic Header/Detail inventory transactions (Restock & Adjustment Ledger)
    /// using SqlTransaction so stock quantities and ledger records never go out of sync.
    /// </summary>
    public class StockTransactionRepository
    {
        public bool CreateRestockTransaction(int ingredientId, decimal addQuantity, int? userId, string username, string notes, out int createdTxId)
        {
            createdTxId = 0;
            if (addQuantity <= 0) throw new ArgumentException("Quantity to restock must be greater than zero.");

            DatabaseHelper.EnsureSchemaUpgrades();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Read current stock with UPDLOCK inside transaction to prevent lost updates
                        string readSql = "SELECT Name, CurrentStock, Unit FROM Ingredients WITH (UPDLOCK) WHERE IngredientId = @IngredientId";
                        string ingName = null, unit = null;
                        decimal stockBefore = 0m;

                        using (var cmd = new SqlCommand(readSql, conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId });
                            using (var r = cmd.ExecuteReader())
                            {
                                if (r.Read())
                                {
                                    ingName = Convert.ToString(r["Name"]);
                                    stockBefore = Convert.ToDecimal(r["CurrentStock"]);
                                    unit = Convert.ToString(r["Unit"]);
                                }
                            }
                        }

                        if (ingName == null)
                        {
                            throw new InvalidOperationException("Selected ingredient was not found in inventory.");
                        }

                        decimal stockAfter = stockBefore + addQuantity;

                        // 2. Insert StockTransaction Header
                        string hdrSql = @"INSERT INTO StockTransactions (TransactionType, UserId, Username, Notes, Status, CreatedAt)
                                          VALUES ('Restock', @UserId, @Username, @Notes, 'Posted', GETDATE());
                                          SELECT SCOPE_IDENTITY();";
                        SqlParameter[] hdrParams = {
                            new SqlParameter("@UserId", SqlDbType.Int) { Value = (object)userId ?? DBNull.Value },
                            new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username ?? "Admin" },
                            new SqlParameter("@Notes", SqlDbType.NVarChar, 500) { Value = string.IsNullOrWhiteSpace(notes) ? $"Restocked {ingName}" : notes }
                        };
                        createdTxId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(hdrSql, hdrParams, trans));

                        // 3. Insert StockTransactionItems Detail
                        string dtlSql = @"INSERT INTO StockTransactionItems (StockTxId, IngredientId, IngredientName, QuantityChange, Unit, StockBefore, StockAfter)
                                          VALUES (@StockTxId, @IngredientId, @IngredientName, @QuantityChange, @Unit, @StockBefore, @StockAfter);";
                        SqlParameter[] dtlParams = {
                            new SqlParameter("@StockTxId", SqlDbType.Int) { Value = createdTxId },
                            new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId },
                            new SqlParameter("@IngredientName", SqlDbType.NVarChar, 100) { Value = ingName },
                            new SqlParameter("@QuantityChange", SqlDbType.Decimal) { Value = addQuantity },
                            new SqlParameter("@Unit", SqlDbType.NVarChar, 20) { Value = unit },
                            new SqlParameter("@StockBefore", SqlDbType.Decimal) { Value = stockBefore },
                            new SqlParameter("@StockAfter", SqlDbType.Decimal) { Value = stockAfter }
                        };
                        DatabaseHelper.ExecuteNonQuery(dtlSql, dtlParams, trans);

                        // 4. Update Ingredients Stock
                        string updSql = "UPDATE Ingredients SET CurrentStock = @StockAfter WHERE IngredientId = @IngredientId";
                        SqlParameter[] updParams = {
                            new SqlParameter("@StockAfter", SqlDbType.Decimal) { Value = stockAfter },
                            new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
                        };
                        DatabaseHelper.ExecuteNonQuery(updSql, updParams, trans);

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

        public bool RecordBatchAdjustmentTransaction(List<StockTransactionItem> changes, int? userId, string username, string notes, out int createdTxId)
        {
            createdTxId = 0;
            if (changes == null || changes.Count == 0) return true;

            DatabaseHelper.EnsureSchemaUpgrades();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string hdrSql = @"INSERT INTO StockTransactions (TransactionType, UserId, Username, Notes, Status, CreatedAt)
                                          VALUES ('Adjustment', @UserId, @Username, @Notes, 'Posted', GETDATE());
                                          SELECT SCOPE_IDENTITY();";
                        SqlParameter[] hdrParams = {
                            new SqlParameter("@UserId", SqlDbType.Int) { Value = (object)userId ?? DBNull.Value },
                            new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username ?? "Admin" },
                            new SqlParameter("@Notes", SqlDbType.NVarChar, 500) { Value = notes ?? "Manual inventory adjustment" }
                        };
                        createdTxId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(hdrSql, hdrParams, trans));

                        foreach (var item in changes)
                        {
                            string dtlSql = @"INSERT INTO StockTransactionItems (StockTxId, IngredientId, IngredientName, QuantityChange, Unit, StockBefore, StockAfter)
                                              VALUES (@StockTxId, @IngredientId, @IngredientName, @QuantityChange, @Unit, @StockBefore, @StockAfter);";
                            SqlParameter[] dtlParams = {
                                new SqlParameter("@StockTxId", SqlDbType.Int) { Value = createdTxId },
                                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = item.IngredientId },
                                new SqlParameter("@IngredientName", SqlDbType.NVarChar, 100) { Value = item.IngredientName },
                                new SqlParameter("@QuantityChange", SqlDbType.Decimal) { Value = item.QuantityChange },
                                new SqlParameter("@Unit", SqlDbType.NVarChar, 20) { Value = item.Unit },
                                new SqlParameter("@StockBefore", SqlDbType.Decimal) { Value = item.StockBefore },
                                new SqlParameter("@StockAfter", SqlDbType.Decimal) { Value = item.StockAfter }
                            };
                            DatabaseHelper.ExecuteNonQuery(dtlSql, dtlParams, trans);
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

        public bool VoidStockTransaction(int stockTxId, byte[] expectedRowVersion = null)
        {
            DatabaseHelper.EnsureSchemaUpgrades();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string hdrCheck = "SELECT Status, RowVersion FROM StockTransactions WITH (UPDLOCK) WHERE StockTxId = @StockTxId";
                        string currentStatus = null;
                        byte[] dbRowVer = null;

                        using (var cmd = new SqlCommand(hdrCheck, conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("@StockTxId", SqlDbType.Int) { Value = stockTxId });
                            using (var r = cmd.ExecuteReader())
                            {
                                if (r.Read())
                                {
                                    currentStatus = Convert.ToString(r["Status"]);
                                    if (r["RowVersion"] != DBNull.Value) dbRowVer = (byte[])r["RowVersion"];
                                }
                            }
                        }

                        if (currentStatus == null)
                            throw new InvalidOperationException("Stock transaction record not found.");

                        if (string.Equals(currentStatus, "Voided", StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException("This stock transaction has already been voided.");

                        if (expectedRowVersion != null && dbRowVer != null && !System.Linq.Enumerable.SequenceEqual(expectedRowVersion, dbRowVer))
                            throw new DBConcurrencyException("This stock transaction was updated by another user. Please refresh and try again.");

                        // Load detail lines
                        var details = new List<StockTransactionItem>();
                        string dtlSql = "SELECT * FROM StockTransactionItems WHERE StockTxId = @StockTxId";
                        using (var cmd = new SqlCommand(dtlSql, conn, trans))
                        {
                            cmd.Parameters.Add(new SqlParameter("@StockTxId", SqlDbType.Int) { Value = stockTxId });
                            using (var r = cmd.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    details.Add(new StockTransactionItem
                                    {
                                        DetailId = Convert.ToInt32(r["DetailId"]),
                                        StockTxId = stockTxId,
                                        IngredientId = Convert.ToInt32(r["IngredientId"]),
                                        IngredientName = Convert.ToString(r["IngredientName"]),
                                        QuantityChange = Convert.ToDecimal(r["QuantityChange"]),
                                        Unit = Convert.ToString(r["Unit"])
                                    });
                                }
                            }
                        }

                        // Reverse stock change atomically (guard against negative stock)
                        foreach (var item in details)
                        {
                            string revSql = @"UPDATE Ingredients 
                                              SET CurrentStock = CASE WHEN (CurrentStock - @QtyChange) < 0 THEN 0 ELSE (CurrentStock - @QtyChange) END
                                              WHERE IngredientId = @IngredientId";
                            SqlParameter[] revParams = {
                                new SqlParameter("@QtyChange", SqlDbType.Decimal) { Value = item.QuantityChange },
                                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = item.IngredientId }
                            };
                            DatabaseHelper.ExecuteNonQuery(revSql, revParams, trans);
                        }

                        // Mark header Voided
                        string updHdr = "UPDATE StockTransactions SET Status = 'Voided' WHERE StockTxId = @StockTxId";
                        DatabaseHelper.ExecuteNonQuery(updHdr, new[] { new SqlParameter("@StockTxId", SqlDbType.Int) { Value = stockTxId } }, trans);

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

        public List<StockTransaction> GetTransactions(DateTime? fromDate = null, DateTime? toDate = null, string statusFilter = "All", string search = null)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            var list = new List<StockTransaction>();
            string sql = "SELECT * FROM StockTransactions WHERE 1=1 ";
            var parameters = new List<SqlParameter>();

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
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                sql += " AND Status = @Status ";
                parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, 20) { Value = statusFilter });
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += @" AND (Username LIKE @Search OR Notes LIKE @Search OR TransactionType LIKE @Search
                          OR EXISTS (SELECT 1 FROM StockTransactionItems d WHERE d.StockTxId = StockTransactions.StockTxId AND d.IngredientName LIKE @Search)) ";
                parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100) { Value = $"%{search.Trim()}%" });
            }

            sql += " ORDER BY StockTxId DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            foreach (DataRow row in dt.Rows)
            {
                var tx = new StockTransaction
                {
                    StockTxId = Convert.ToInt32(row["StockTxId"]),
                    TransactionType = Convert.ToString(row["TransactionType"]),
                    UserId = row["UserId"] != DBNull.Value ? (int?)Convert.ToInt32(row["UserId"]) : null,
                    Username = Convert.ToString(row["Username"]),
                    Notes = row["Notes"] != DBNull.Value ? Convert.ToString(row["Notes"]) : string.Empty,
                    Status = Convert.ToString(row["Status"]),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    RowVersion = row.Table.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value ? (byte[])row["RowVersion"] : null
                };
                tx.Items = GetTransactionItems(tx.StockTxId);
                list.Add(tx);
            }
            return list;
        }

        public List<StockTransactionItem> GetTransactionItems(int stockTxId)
        {
            var items = new List<StockTransactionItem>();
            string sql = "SELECT * FROM StockTransactionItems WHERE StockTxId = @StockTxId ORDER BY DetailId ASC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, new[] { new SqlParameter("@StockTxId", SqlDbType.Int) { Value = stockTxId } });
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new StockTransactionItem
                {
                    DetailId = Convert.ToInt32(row["DetailId"]),
                    StockTxId = Convert.ToInt32(row["StockTxId"]),
                    IngredientId = Convert.ToInt32(row["IngredientId"]),
                    IngredientName = Convert.ToString(row["IngredientName"]),
                    QuantityChange = Convert.ToDecimal(row["QuantityChange"]),
                    Unit = Convert.ToString(row["Unit"]),
                    StockBefore = Convert.ToDecimal(row["StockBefore"]),
                    StockAfter = Convert.ToDecimal(row["StockAfter"])
                });
            }
            return items;
        }
    }
}
