using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class IngredientRepository
    {
        public List<Ingredient> GetAllIngredients(bool includeArchived = false)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            List<Ingredient> list = new List<Ingredient>();
            string sql = includeArchived
                ? "SELECT * FROM Ingredients ORDER BY IsActive DESC, Name ASC"
                : "SELECT * FROM Ingredients WHERE ISNULL(IsActive, 1) = 1 ORDER BY Name ASC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToIngredient(row));
            }
            return list;
        }

        public Ingredient GetIngredientById(int ingredientId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "SELECT * FROM Ingredients WHERE IngredientId = @IngredientId";
            SqlParameter[] parameters = {
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRowToIngredient(dt.Rows[0]);
            }
            return null;
        }

        public bool IngredientNameExists(string name, int? excludeId = null)
        {
            string sql = "SELECT COUNT(*) FROM Ingredients WHERE Name = @Name";
            if (excludeId.HasValue)
            {
                sql += " AND IngredientId <> @ExcludeId";
            }

            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = name.Trim() }
            };
            if (excludeId.HasValue)
            {
                parameters.Add(new SqlParameter("@ExcludeId", SqlDbType.Int) { Value = excludeId.Value });
            }

            object res = DatabaseHelper.ExecuteScalar(sql, parameters.ToArray());
            return Convert.ToInt32(res) > 0;
        }

        public bool InsertIngredient(Ingredient ingredient)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            if (IngredientNameExists(ingredient.Name))
            {
                throw new InvalidOperationException("An ingredient with this name already exists.");
            }

            string sql = @"INSERT INTO Ingredients (CategoryId, CategoryName, Name, CurrentStock, Unit, MinThreshold, CostPerUnit, IsActive, CreatedAt)
                           VALUES (@CategoryId, @CategoryName, @Name, @CurrentStock, @Unit, @MinThreshold, @CostPerUnit, @IsActive, @CreatedAt);
                           SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = (object)ingredient.CategoryId ?? DBNull.Value },
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = ingredient.CategoryName ?? "General" },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = ingredient.Name },
                new SqlParameter("@CurrentStock", SqlDbType.Decimal) { Value = ingredient.CurrentStock },
                new SqlParameter("@Unit", SqlDbType.NVarChar, 20) { Value = ingredient.Unit },
                new SqlParameter("@MinThreshold", SqlDbType.Decimal) { Value = ingredient.MinThreshold },
                new SqlParameter("@CostPerUnit", SqlDbType.Decimal) { Value = ingredient.CostPerUnit },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = ingredient.IsActive },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = ingredient.CreatedAt }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                ingredient.IngredientId = Convert.ToInt32(res);
                var fresh = GetIngredientById(ingredient.IngredientId);
                if (fresh != null) ingredient.RowVersion = fresh.RowVersion;
                return true;
            }
            return false;
        }

        public bool UpdateIngredient(Ingredient ingredient)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            if (IngredientNameExists(ingredient.Name, ingredient.IngredientId))
            {
                throw new InvalidOperationException("Another ingredient with this name already exists.");
            }

            string sql = @"UPDATE Ingredients 
                           SET CategoryId = @CategoryId, CategoryName = @CategoryName, Name = @Name, 
                               CurrentStock = @CurrentStock, Unit = @Unit, 
                               MinThreshold = @MinThreshold, CostPerUnit = @CostPerUnit, IsActive = @IsActive
                           WHERE IngredientId = @IngredientId";

            var parameters = new List<SqlParameter> {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = (object)ingredient.CategoryId ?? DBNull.Value },
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = ingredient.CategoryName ?? "General" },
                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = ingredient.Name },
                new SqlParameter("@CurrentStock", SqlDbType.Decimal) { Value = ingredient.CurrentStock },
                new SqlParameter("@Unit", SqlDbType.NVarChar, 20) { Value = ingredient.Unit },
                new SqlParameter("@MinThreshold", SqlDbType.Decimal) { Value = ingredient.MinThreshold },
                new SqlParameter("@CostPerUnit", SqlDbType.Decimal) { Value = ingredient.CostPerUnit },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = ingredient.IsActive },
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredient.IngredientId }
            };

            if (ingredient.RowVersion != null && ingredient.RowVersion.Length == 8)
            {
                sql += " AND RowVersion = @RowVersion";
                parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = ingredient.RowVersion });
            }

            int rows = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
            if (rows == 0 && ingredient.RowVersion != null)
            {
                if (GetIngredientById(ingredient.IngredientId) != null)
                {
                    throw new DBConcurrencyException($"'{ingredient.Name}' was updated by another user. Please refresh the inventory list before saving.");
                }
            }
            return rows > 0;
        }

        public bool SetIngredientActiveStatus(int ingredientId, bool isActive)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Ingredients SET IsActive = @IsActive WHERE IngredientId = @IngredientId";
            SqlParameter[] parameters = {
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool Restock(int ingredientId, decimal quantityToAdd)
        {
            if (quantityToAdd <= 0)
            {
                throw new ArgumentException("Restock quantity must be greater than zero.");
            }

            string sql = @"UPDATE Ingredients 
                           SET CurrentStock = CurrentStock + @QuantityToAdd 
                           WHERE IngredientId = @IngredientId";

            SqlParameter[] parameters = {
                new SqlParameter("@QuantityToAdd", SqlDbType.Decimal) { Value = quantityToAdd },
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool IsIngredientInUse(int ingredientId, out List<string> usedInMenuNames)
        {
            usedInMenuNames = new List<string>();
            string sql = @"
                SELECT DISTINCT m.Name 
                FROM MenuItems m
                LEFT JOIN MenuItemIngredients mi ON m.ItemId = mi.ItemId
                LEFT JOIN MenuItemDoughSizes md ON m.ItemId = md.ItemId
                LEFT JOIN MenuItemAddOns ma ON m.ItemId = ma.ItemId
                WHERE mi.IngredientId = @IngredientId 
                   OR md.IngredientId = @IngredientId 
                   OR ma.IngredientId = @IngredientId";

            SqlParameter[] parameters = {
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                string name = row["Name"]?.ToString();
                if (!string.IsNullOrWhiteSpace(name) && !usedInMenuNames.Contains(name))
                {
                    usedInMenuNames.Add(name);
                }
            }
            return usedInMenuNames.Count > 0;
        }

        public bool HasStockTransactions(int ingredientId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "SELECT COUNT(*) FROM StockTransactionItems WHERE IngredientId = @IngredientId";
            SqlParameter[] parameters = {
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
            };
            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(res) > 0;
        }

        public bool DeleteIngredient(int ingredientId)
        {
            string sql = "DELETE FROM Ingredients WHERE IngredientId = @IngredientId";
            SqlParameter[] parameters = {
                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ingredientId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool BatchSaveIngredients(List<Ingredient> ingredients)
        {
            if (ingredients == null) return true;
            DatabaseHelper.EnsureSchemaUpgrades();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var ing in ingredients)
                        {
                            string sql = @"UPDATE Ingredients 
                                           SET CategoryId = @CategoryId, CategoryName = @CategoryName, Name = @Name, 
                                               CurrentStock = @CurrentStock, Unit = @Unit, 
                                               MinThreshold = @MinThreshold, CostPerUnit = @CostPerUnit, IsActive = @IsActive
                                           WHERE IngredientId = @IngredientId";

                            var parameters = new List<SqlParameter> {
                                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = (object)ing.CategoryId ?? DBNull.Value },
                                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = ing.CategoryName ?? "General" },
                                new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = ing.Name },
                                new SqlParameter("@CurrentStock", SqlDbType.Decimal) { Value = ing.CurrentStock },
                                new SqlParameter("@Unit", SqlDbType.NVarChar, 20) { Value = ing.Unit },
                                new SqlParameter("@MinThreshold", SqlDbType.Decimal) { Value = ing.MinThreshold },
                                new SqlParameter("@CostPerUnit", SqlDbType.Decimal) { Value = ing.CostPerUnit },
                                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = ing.IsActive },
                                new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ing.IngredientId }
                            };

                            if (ing.RowVersion != null && ing.RowVersion.Length == 8)
                            {
                                sql += " AND RowVersion = @RowVersion";
                                parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = ing.RowVersion });
                            }

                            int rows = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray(), trans);
                            if (rows == 0 && ing.RowVersion != null)
                            {
                                throw new DBConcurrencyException($"Stock item '{ing.Name}' was modified concurrently. Please refresh inventory and retry.");
                            }
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

        private Ingredient MapRowToIngredient(DataRow row)
        {
            int? catId = row.Table.Columns.Contains("CategoryId") && row["CategoryId"] != DBNull.Value ? (int?)Convert.ToInt32(row["CategoryId"]) : null;
            string catName = row.Table.Columns.Contains("CategoryName") && row["CategoryName"] != DBNull.Value ? Convert.ToString(row["CategoryName"]) : "General";

            var ing = new Ingredient(
                Convert.ToInt32(row["IngredientId"]),
                Convert.ToString(row["Name"]),
                Convert.ToDecimal(row["CurrentStock"]),
                Convert.ToString(row["Unit"]),
                Convert.ToDecimal(row["MinThreshold"]),
                Convert.ToDecimal(row["CostPerUnit"]),
                Convert.ToDateTime(row["CreatedAt"]),
                catId,
                catName
            );

            if (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value)
                ing.IsActive = Convert.ToBoolean(row["IsActive"]);
            if (row.Table.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                ing.RowVersion = (byte[])row["RowVersion"];

            return ing;
        }
    }
}
