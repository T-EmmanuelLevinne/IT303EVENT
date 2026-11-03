using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class IngredientCategoryRepository
    {
        public List<IngredientCategory> GetAllCategories()
        {
            var list = new List<IngredientCategory>();
            string sql = "SELECT IngredientCategoryId, CategoryName, Description, CreatedAt FROM IngredientCategories ORDER BY CategoryName ASC";

            using (var reader = DatabaseHelper.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    list.Add(new IngredientCategory
                    {
                        IngredientCategoryId = Convert.ToInt32(reader["IngredientCategoryId"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    });
                }
            }
            return list;
        }

        public bool InsertCategory(IngredientCategory category)
        {
            string sql = @"INSERT INTO IngredientCategories (CategoryName, Description, CreatedAt)
                           VALUES (@CategoryName, @Description, @CreatedAt);
                           SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = category.CategoryName },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)category.Description ?? DBNull.Value },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = category.CreatedAt }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                category.IngredientCategoryId = Convert.ToInt32(res);
                return true;
            }
            return false;
        }

        public bool UpdateCategory(IngredientCategory category)
        {
            string sql = @"UPDATE IngredientCategories 
                           SET CategoryName = @CategoryName, Description = @Description 
                           WHERE IngredientCategoryId = @IngredientCategoryId";

            SqlParameter[] parameters = {
                new SqlParameter("@CategoryName", SqlDbType.NVarChar, 50) { Value = category.CategoryName },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)category.Description ?? DBNull.Value },
                new SqlParameter("@IngredientCategoryId", SqlDbType.Int) { Value = category.IngredientCategoryId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteCategory(int categoryId)
        {
            string sql = "DELETE FROM IngredientCategories WHERE IngredientCategoryId = @IngredientCategoryId";
            SqlParameter[] parameters = {
                new SqlParameter("@IngredientCategoryId", SqlDbType.Int) { Value = categoryId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
