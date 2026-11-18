using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class CategoryRepository
    {
        public List<Category> GetAllCategories(bool includeArchived = false)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            List<Category> list = new List<Category>();
            string sql = includeArchived
                ? "SELECT * FROM Categories ORDER BY IsActive DESC, CategoryName ASC"
                : "SELECT * FROM Categories WHERE ISNULL(IsActive, 1) = 1 ORDER BY CategoryName ASC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToCategory(row));
            }
            return list;
        }

        public Category GetCategoryById(int categoryId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "SELECT * FROM Categories WHERE CategoryId = @CategoryId";
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRowToCategory(dt.Rows[0]);
            }
            return null;
        }

        public bool CategoryNameExists(string categoryName, int? excludeId = null)
        {
            string sql = "SELECT COUNT(*) FROM Categories WHERE CategoryName = @CategoryName";
            if (excludeId.HasValue)
            {
                sql += " AND CategoryId <> @ExcludeId";
            }

            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = categoryName.Trim() }
            };
            if (excludeId.HasValue)
            {
                parameters.Add(new SqlParameter("@ExcludeId", SqlDbType.Int) { Value = excludeId.Value });
            }

            object res = DatabaseHelper.ExecuteScalar(sql, parameters.ToArray());
            return Convert.ToInt32(res) > 0;
        }

        public bool HasCategoryMenuItems(int categoryId)
        {
            string sql = "SELECT COUNT(*) FROM MenuItems WHERE CategoryId = @CategoryId";
            object res = DatabaseHelper.ExecuteScalar(sql, new[] { new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId } });
            return Convert.ToInt32(res) > 0;
        }

        public bool InsertCategory(Category category)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            if (CategoryNameExists(category.CategoryName))
            {
                throw new InvalidOperationException("A category with this name already exists.");
            }

            string sql = @"INSERT INTO Categories (CategoryName, IconKey, Description, IsActive, CreatedAt)
                           VALUES (@CategoryName, @IconKey, @Description, @IsActive, @CreatedAt);
                           SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = category.CategoryName },
                new SqlParameter("@IconKey", SqlDbType.NVarChar, 50) { Value = (object)category.IconKey ?? DBNull.Value },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)category.Description ?? DBNull.Value },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = category.IsActive },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = category.CreatedAt }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                category.CategoryId = Convert.ToInt32(res);
                var fresh = GetCategoryById(category.CategoryId);
                if (fresh != null) category.RowVersion = fresh.RowVersion;
                return true;
            }
            return false;
        }

        public bool UpdateCategory(Category category)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            if (CategoryNameExists(category.CategoryName, category.CategoryId))
            {
                throw new InvalidOperationException("Another category with this name already exists.");
            }

            string sql = @"UPDATE Categories 
                           SET CategoryName = @CategoryName, IconKey = @IconKey, Description = @Description, IsActive = @IsActive
                           WHERE CategoryId = @CategoryId";

            var parameters = new List<SqlParameter> {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = category.CategoryName },
                new SqlParameter("@IconKey", SqlDbType.NVarChar, 50) { Value = (object)category.IconKey ?? DBNull.Value },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)category.Description ?? DBNull.Value },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = category.IsActive },
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = category.CategoryId }
            };

            if (category.RowVersion != null && category.RowVersion.Length == 8)
            {
                sql += " AND RowVersion = @RowVersion";
                parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = category.RowVersion });
            }

            int rows = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
            if (rows == 0 && category.RowVersion != null)
            {
                if (GetCategoryById(category.CategoryId) != null)
                {
                    throw new DBConcurrencyException("This category was modified by another administrator. Please refresh before saving.");
                }
            }
            return rows > 0;
        }

        public bool SetCategoryActiveStatus(int categoryId, bool isActive)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Categories SET IsActive = @IsActive WHERE CategoryId = @CategoryId";
            SqlParameter[] parameters = {
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteCategory(int categoryId)
        {
            string sql = "DELETE FROM Categories WHERE CategoryId = @CategoryId";
            SqlParameter[] parameters = {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        private Category MapRowToCategory(DataRow row)
        {
            var cat = new Category(
                Convert.ToInt32(row["CategoryId"]),
                Convert.ToString(row["CategoryName"]),
                row["IconKey"] != DBNull.Value ? Convert.ToString(row["IconKey"]) : "Pizza",
                row["Description"] != DBNull.Value ? Convert.ToString(row["Description"]) : string.Empty,
                Convert.ToDateTime(row["CreatedAt"])
            );
            if (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value)
                cat.IsActive = Convert.ToBoolean(row["IsActive"]);
            if (row.Table.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                cat.RowVersion = (byte[])row["RowVersion"];
            return cat;
        }
    }
}
