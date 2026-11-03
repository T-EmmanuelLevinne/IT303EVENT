using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class MenuItemRepository
    {
        public List<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> list = new List<MenuItem>();
            string sql = @"SELECT m.*, c.CategoryName 
                           FROM MenuItems m 
                           LEFT JOIN Categories c ON m.CategoryId = c.CategoryId 
                           ORDER BY m.ItemId DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                var item = MapRowToMenuItem(row);
                item.RecipeIngredients = GetRecipeIngredients(item.ItemId);
                item.DoughSizes = GetMenuItemDoughSizes(item.ItemId);
                item.AvailableAddOns = GetMenuItemAddOns(item.ItemId);
                list.Add(item);
            }
            return list;
        }

        public List<MenuItem> GetMenuItemsByCategory(int categoryId)
        {
            List<MenuItem> list = new List<MenuItem>();
            string sql = @"SELECT m.*, c.CategoryName 
                           FROM MenuItems m 
                           LEFT JOIN Categories c ON m.CategoryId = c.CategoryId 
                           WHERE m.CategoryId = @CategoryId
                           ORDER BY m.ItemId DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId }
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                var item = MapRowToMenuItem(row);
                item.RecipeIngredients = GetRecipeIngredients(item.ItemId);
                item.DoughSizes = GetMenuItemDoughSizes(item.ItemId);
                item.AvailableAddOns = GetMenuItemAddOns(item.ItemId);
                list.Add(item);
            }
            return list;
        }

        public List<MenuItem> GetMenuItemsFiltered(string search = null, int? categoryId = null)
        {
            List<MenuItem> list = new List<MenuItem>();
            string sql = @"SELECT m.*, c.CategoryName 
                           FROM MenuItems m 
                           LEFT JOIN Categories c ON m.CategoryId = c.CategoryId 
                           WHERE 1=1 ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                sql += " AND m.CategoryId = @CategoryId ";
                parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId.Value });
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND (m.Name LIKE @Query OR m.Description LIKE @Query OR c.CategoryName LIKE @Query) ";
                parameters.Add(new SqlParameter("@Query", SqlDbType.NVarChar, 100) { Value = $"%{search.Trim()}%" });
            }
            sql += " ORDER BY m.ItemId DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            foreach (DataRow row in dt.Rows)
            {
                var item = MapRowToMenuItem(row);
                item.RecipeIngredients = GetRecipeIngredients(item.ItemId);
                item.DoughSizes = GetMenuItemDoughSizes(item.ItemId);
                item.AvailableAddOns = GetMenuItemAddOns(item.ItemId);
                list.Add(item);
            }
            return list;
        }

        public List<MenuItem> SearchMenuItems(string query)
        {
            List<MenuItem> list = new List<MenuItem>();
            string sql = @"SELECT m.*, c.CategoryName 
                           FROM MenuItems m 
                           LEFT JOIN Categories c ON m.CategoryId = c.CategoryId 
                           WHERE m.Name LIKE @Query OR m.Description LIKE @Query OR c.CategoryName LIKE @Query
                           ORDER BY m.ItemId DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@Query", SqlDbType.NVarChar, 100) { Value = "%" + query.Trim() + "%" }
            };
            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                var item = MapRowToMenuItem(row);
                item.RecipeIngredients = GetRecipeIngredients(item.ItemId);
                item.DoughSizes = GetMenuItemDoughSizes(item.ItemId);
                item.AvailableAddOns = GetMenuItemAddOns(item.ItemId);
                list.Add(item);
            }
            return list;
        }

        public MenuItem GetMenuItemById(int itemId)
        {
            string sql = @"SELECT m.*, c.CategoryName 
                           FROM MenuItems m 
                           LEFT JOIN Categories c ON m.CategoryId = c.CategoryId 
                           WHERE m.ItemId = @ItemId";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                var item = MapRowToMenuItem(dt.Rows[0]);
                item.RecipeIngredients = GetRecipeIngredients(item.ItemId);
                item.DoughSizes = GetMenuItemDoughSizes(item.ItemId);
                item.AvailableAddOns = GetMenuItemAddOns(item.ItemId);
                return item;
            }
            return null;
        }

        public bool InsertMenuItem(MenuItem item)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"INSERT INTO MenuItems (CategoryId, Name, Description, Price, OldPrice, ImageBase64, IsAvailable, CreatedAt)
                                       VALUES (@CategoryId, @Name, @Description, @Price, @OldPrice, @ImageBase64, @IsAvailable, @CreatedAt);
                                       SELECT SCOPE_IDENTITY();";

                        SqlParameter[] parameters = {
                            new SqlParameter("@CategoryId", SqlDbType.Int) { Value = (object)item.CategoryId ?? DBNull.Value },
                            new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = item.Name },
                            new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object)item.Description ?? DBNull.Value },
                            new SqlParameter("@Price", SqlDbType.Decimal) { Value = item.Price },
                            new SqlParameter("@OldPrice", SqlDbType.Decimal) { Value = (object)item.OldPrice ?? DBNull.Value },
                            new SqlParameter("@ImageBase64", SqlDbType.NVarChar, -1) { Value = (object)item.ImageBase64 ?? DBNull.Value },
                            new SqlParameter("@IsAvailable", SqlDbType.Bit) { Value = item.IsAvailable },
                            new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = item.CreatedAt }
                        };

                        object res = DatabaseHelper.ExecuteScalar(sql, parameters, trans);
                        if (res != null && res != DBNull.Value)
                        {
                            item.ItemId = Convert.ToInt32(res);
                            SaveRecipeIngredients(item.ItemId, item.RecipeIngredients, trans);
                            SaveMenuItemDoughSizes(item.ItemId, item.DoughSizes, trans);
                            SaveMenuItemAddOns(item.ItemId, item.AvailableAddOns, trans);
                            trans.Commit();
                            return true;
                        }
                        trans.Rollback();
                        return false;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool UpdateMenuItem(MenuItem item)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = @"UPDATE MenuItems 
                                       SET CategoryId = @CategoryId, Name = @Name, Description = @Description, 
                                           Price = @Price, OldPrice = @OldPrice, ImageBase64 = @ImageBase64, 
                                           IsAvailable = @IsAvailable
                                       WHERE ItemId = @ItemId";

                        SqlParameter[] parameters = {
                            new SqlParameter("@CategoryId", SqlDbType.Int) { Value = (object)item.CategoryId ?? DBNull.Value },
                            new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = item.Name },
                            new SqlParameter("@Description", SqlDbType.NVarChar, 500) { Value = (object)item.Description ?? DBNull.Value },
                            new SqlParameter("@Price", SqlDbType.Decimal) { Value = item.Price },
                            new SqlParameter("@OldPrice", SqlDbType.Decimal) { Value = (object)item.OldPrice ?? DBNull.Value },
                            new SqlParameter("@ImageBase64", SqlDbType.NVarChar, -1) { Value = (object)item.ImageBase64 ?? DBNull.Value },
                            new SqlParameter("@IsAvailable", SqlDbType.Bit) { Value = item.IsAvailable },
                            new SqlParameter("@ItemId", SqlDbType.Int) { Value = item.ItemId }
                        };

                        int rows = DatabaseHelper.ExecuteNonQuery(sql, parameters, trans);
                        if (rows > 0)
                        {
                            SaveRecipeIngredients(item.ItemId, item.RecipeIngredients, trans);
                            SaveMenuItemDoughSizes(item.ItemId, item.DoughSizes, trans);
                            SaveMenuItemAddOns(item.ItemId, item.AvailableAddOns, trans);
                            trans.Commit();
                            return true;
                        }
                        trans.Rollback();
                        return false;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool DeleteMenuItem(int itemId)
        {
            string sql = "DELETE FROM MenuItems WHERE ItemId = @ItemId";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public List<MenuItemIngredient> GetRecipeIngredients(int itemId)
        {
            List<MenuItemIngredient> list = new List<MenuItemIngredient>();
            string sql = @"SELECT mi.ItemId, mi.IngredientId, mi.QuantityRequired, i.Name AS IngredientName, i.Unit, ISNULL(mi.SizeName, '') AS SizeName 
                           FROM MenuItemIngredients mi 
                           JOIN Ingredients i ON mi.IngredientId = i.IngredientId 
                           WHERE mi.ItemId = @ItemId";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MenuItemIngredient(
                    Convert.ToInt32(row["ItemId"]),
                    Convert.ToInt32(row["IngredientId"]),
                    Convert.ToDecimal(row["QuantityRequired"]),
                    Convert.ToString(row["IngredientName"]),
                    Convert.ToString(row["Unit"]),
                    row.Table.Columns.Contains("SizeName") && row["SizeName"] != DBNull.Value ? Convert.ToString(row["SizeName"]) : ""
                ));
            }
            return list;
        }

        private void SaveRecipeIngredients(int itemId, List<MenuItemIngredient> ingredients, SqlTransaction trans)
        {
            string delSql = "DELETE FROM MenuItemIngredients WHERE ItemId = @ItemId";
            SqlParameter[] delParams = { new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId } };
            DatabaseHelper.ExecuteNonQuery(delSql, delParams, trans);

            if (ingredients != null && ingredients.Count > 0)
            {
                foreach (var ing in ingredients)
                {
                    string insSql = @"INSERT INTO MenuItemIngredients (ItemId, IngredientId, QuantityRequired, SizeName) 
                                      VALUES (@ItemId, @IngredientId, @QuantityRequired, @SizeName)";
                    SqlParameter[] insParams = {
                        new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId },
                        new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ing.IngredientId },
                        new SqlParameter("@QuantityRequired", SqlDbType.Decimal) { Value = ing.QuantityRequired },
                        new SqlParameter("@SizeName", SqlDbType.NVarChar, 50) { Value = (object)ing.SizeName ?? "" }
                    };
                    DatabaseHelper.ExecuteNonQuery(insSql, insParams, trans);
                }
            }
        }

        public List<MenuItemDoughSize> GetMenuItemDoughSizes(int itemId)
        {
            List<MenuItemDoughSize> list = new List<MenuItemDoughSize>();
            string sql = @"SELECT ds.*, i.Name AS DoughIngredientName, i.CurrentStock AS CurrentStockAvailable
                           FROM MenuItemDoughSizes ds
                           LEFT JOIN Ingredients i ON ds.IngredientId = i.IngredientId
                           WHERE ds.ItemId = @ItemId
                           ORDER BY ds.Price ASC";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MenuItemDoughSize
                {
                    SizeId = Convert.ToInt32(row["SizeId"]),
                    ItemId = Convert.ToInt32(row["ItemId"]),
                    SizeName = Convert.ToString(row["SizeName"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    Cost = Convert.ToDecimal(row["Cost"]),
                    IngredientId = row["IngredientId"] != DBNull.Value ? (int?)Convert.ToInt32(row["IngredientId"]) : null,
                    DoughIngredientName = row["DoughIngredientName"] != DBNull.Value ? Convert.ToString(row["DoughIngredientName"]) : null,
                    DoughPortionQty = Convert.ToDecimal(row["DoughPortionQty"]),
                    CurrentStockAvailable = row["CurrentStockAvailable"] != DBNull.Value ? Convert.ToDecimal(row["CurrentStockAvailable"]) : 0,
                    IsDefault = Convert.ToBoolean(row["IsDefault"])
                });
            }
            return list;
        }

        private void SaveMenuItemDoughSizes(int itemId, List<MenuItemDoughSize> sizes, SqlTransaction trans)
        {
            string delSql = "DELETE FROM MenuItemDoughSizes WHERE ItemId = @ItemId";
            SqlParameter[] delParams = { new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId } };
            DatabaseHelper.ExecuteNonQuery(delSql, delParams, trans);

            if (sizes != null && sizes.Count > 0)
            {
                foreach (var size in sizes)
                {
                    string insSql = @"INSERT INTO MenuItemDoughSizes (ItemId, SizeName, Price, Cost, IngredientId, DoughPortionQty, IsDefault)
                                      VALUES (@ItemId, @SizeName, @Price, @Cost, @IngredientId, @DoughPortionQty, @IsDefault)";
                    SqlParameter[] insParams = {
                        new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId },
                        new SqlParameter("@SizeName", SqlDbType.NVarChar, 50) { Value = size.SizeName },
                        new SqlParameter("@Price", SqlDbType.Decimal) { Value = size.Price },
                        new SqlParameter("@Cost", SqlDbType.Decimal) { Value = size.Cost },
                        new SqlParameter("@IngredientId", SqlDbType.Int) { Value = (object)size.IngredientId ?? DBNull.Value },
                        new SqlParameter("@DoughPortionQty", SqlDbType.Decimal) { Value = size.DoughPortionQty },
                        new SqlParameter("@IsDefault", SqlDbType.Bit) { Value = size.IsDefault }
                    };
                    DatabaseHelper.ExecuteNonQuery(insSql, insParams, trans);
                }
            }
        }

        public List<MenuItemAddOn> GetMenuItemAddOns(int itemId)
        {
            List<MenuItemAddOn> list = new List<MenuItemAddOn>();
            string sql = @"SELECT ao.*, i.Unit, i.CurrentStock 
                           FROM MenuItemAddOns ao
                           JOIN Ingredients i ON ao.IngredientId = i.IngredientId
                           WHERE ao.ItemId = @ItemId
                           ORDER BY ao.CategoryName ASC, ao.AddOnName ASC";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MenuItemAddOn(
                    Convert.ToInt32(row["AddOnId"]),
                    Convert.ToInt32(row["ItemId"]),
                    Convert.ToInt32(row["IngredientId"]),
                    Convert.ToString(row["AddOnName"]),
                    Convert.ToDecimal(row["Price"]),
                    Convert.ToDecimal(row["PortionQty"]),
                    Convert.ToString(row["CategoryName"]),
                    Convert.ToString(row["Unit"]),
                    Convert.ToDecimal(row["CurrentStock"])
                ));
            }
            return list;
        }

        private void SaveMenuItemAddOns(int itemId, List<MenuItemAddOn> addOns, SqlTransaction trans)
        {
            string delSql = "DELETE FROM MenuItemAddOns WHERE ItemId = @ItemId";
            SqlParameter[] delParams = { new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId } };
            DatabaseHelper.ExecuteNonQuery(delSql, delParams, trans);

            if (addOns != null && addOns.Count > 0)
            {
                foreach (var ao in addOns)
                {
                    string insSql = @"INSERT INTO MenuItemAddOns (ItemId, IngredientId, AddOnName, Price, PortionQty, CategoryName)
                                      VALUES (@ItemId, @IngredientId, @AddOnName, @Price, @PortionQty, @CategoryName)";
                    SqlParameter[] insParams = {
                        new SqlParameter("@ItemId", SqlDbType.Int) { Value = itemId },
                        new SqlParameter("@IngredientId", SqlDbType.Int) { Value = ao.IngredientId },
                        new SqlParameter("@AddOnName", SqlDbType.NVarChar, 100) { Value = ao.AddOnName },
                        new SqlParameter("@Price", SqlDbType.Decimal) { Value = ao.Price },
                        new SqlParameter("@PortionQty", SqlDbType.Decimal) { Value = ao.PortionQty },
                        new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = ao.CategoryName ?? "Toppings" }
                    };
                    DatabaseHelper.ExecuteNonQuery(insSql, insParams, trans);
                }
            }
        }

        public MenuItem FindDuplicateMenuItem(int excludeItemId, string name, int? categoryId, IEnumerable<int> recipeIngredientIds, IEnumerable<int> addOnIngredientIds)
        {
            var allItems = GetAllMenuItems();
            var targetRecipeSet = new HashSet<int>(recipeIngredientIds ?? Enumerable.Empty<int>());
            var targetAddOnSet = new HashSet<int>(addOnIngredientIds ?? Enumerable.Empty<int>());

            foreach (var item in allItems)
            {
                if (item.ItemId == excludeItemId) continue;

                bool sameName = string.Equals(item.Name?.Trim(), name?.Trim(), StringComparison.OrdinalIgnoreCase);
                bool sameCategory = (item.CategoryId == categoryId || (!item.CategoryId.HasValue && !categoryId.HasValue));

                if (sameName && sameCategory)
                {
                    var itemRecipeSet = new HashSet<int>((item.RecipeIngredients ?? new List<MenuItemIngredient>()).Select(r => r.IngredientId));
                    var itemAddOnSet = new HashSet<int>((item.AvailableAddOns ?? new List<MenuItemAddOn>()).Select(a => a.IngredientId));

                    if (targetRecipeSet.SetEquals(itemRecipeSet) && targetAddOnSet.SetEquals(itemAddOnSet))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        private MenuItem MapRowToMenuItem(DataRow row)
        {
            return new MenuItem(
                Convert.ToInt32(row["ItemId"]),
                row["CategoryId"] != DBNull.Value ? (int?)Convert.ToInt32(row["CategoryId"]) : null,
                Convert.ToString(row["Name"]),
                row["Description"] != DBNull.Value ? Convert.ToString(row["Description"]) : string.Empty,
                Convert.ToDecimal(row["Price"]),
                row["OldPrice"] != DBNull.Value ? (decimal?)Convert.ToDecimal(row["OldPrice"]) : null,
                row["ImageBase64"] != DBNull.Value ? Convert.ToString(row["ImageBase64"]) : null,
                Convert.ToBoolean(row["IsAvailable"]),
                Convert.ToDateTime(row["CreatedAt"]),
                row.Table.Columns.Contains("CategoryName") && row["CategoryName"] != DBNull.Value ? Convert.ToString(row["CategoryName"]) : "General"
            );
        }
    }
}
