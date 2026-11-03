using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class CategoryRepository
    {
        public List<Category> GetAllCategories()
        {
            List<Category> list = new List<Category>();
            string sql = "SELECT * FROM Categories ORDER BY CategoryName ASC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToCategory(row));
            }
            return list;
        }

        public Category GetCategoryById(int categoryId)
        {
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

        public bool InsertCategory(Category category)
        {
            if (CategoryNameExists(category.CategoryName))
            {
                throw new InvalidOperationException("A category with this name already exists.");
            }

            string sql = @"INSERT INTO Categories (CategoryName, IconKey, Description, CreatedAt)
                           VALUES (@CategoryName, @IconKey, @Description, @CreatedAt);
                           SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = category.CategoryName },
                new SqlParameter("@IconKey", SqlDbType.NVarChar, 50) { Value = (object)category.IconKey ?? DBNull.Value },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)category.Description ?? DBNull.Value },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = category.CreatedAt }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                category.CategoryId = Convert.ToInt32(res);
                return true;
            }
            return false;
        }

        public bool UpdateCategory(Category category)
        {
            if (CategoryNameExists(category.CategoryName, category.CategoryId))
            {
                throw new InvalidOperationException("Another category with this name already exists.");
            }

            string sql = @"UPDATE Categories 
                           SET CategoryName = @CategoryName, IconKey = @IconKey, Description = @Description
                           WHERE CategoryId = @CategoryId";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = category.CategoryName },
                new SqlParameter("@IconKey", SqlDbType.NVarChar, 50) { Value = (object)category.IconKey ?? DBNull.Value },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)category.Description ?? DBNull.Value },
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = category.CategoryId }
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
            return new Category(
                Convert.ToInt32(row["CategoryId"]),
                Convert.ToString(row["CategoryName"]),
                row["IconKey"] != DBNull.Value ? Convert.ToString(row["IconKey"]) : "Pizza",
                row["Description"] != DBNull.Value ? Convert.ToString(row["Description"]) : string.Empty,
                Convert.ToDateTime(row["CreatedAt"])
            );
        }
    }
}
