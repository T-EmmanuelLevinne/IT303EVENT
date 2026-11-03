using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class UserRepository
    {
        public User Authenticate(string username, string passwordHash)
        {
            string sql = "SELECT * FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username.Trim() },
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 256) { Value = passwordHash }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRowToUser(dt.Rows[0]);
            }
            return null;
        }

        public bool UsernameExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username.Trim() }
            };

            object result = DatabaseHelper.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }

        public bool Register(User user)
        {
            if (UsernameExists(user.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            string sql = @"INSERT INTO Users (Username, PasswordHash, FullName, Role, Phone, Address, CreatedAt)
                           VALUES (@Username, @PasswordHash, @FullName, @Role, @Phone, @Address, @CreatedAt);
                           SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = user.Username },
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 256) { Value = user.PasswordHash },
                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = user.FullName ?? string.Empty },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = user.Role },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)user.Phone ?? DBNull.Value },
                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)user.Address ?? DBNull.Value },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = user.CreatedAt }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                user.UserId = Convert.ToInt32(res);
                return true;
            }
            return false;
        }

        public User GetUserById(int userId)
        {
            string sql = "SELECT * FROM Users WHERE UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRowToUser(dt.Rows[0]);
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();
            string sql = "SELECT * FROM Users ORDER BY UserId DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToUser(row));
            }
            return list;
        }

        public bool UpdateUser(User user)
        {
            string sql = @"UPDATE Users 
                           SET FullName = @FullName, Role = @Role, Phone = @Phone, Address = @Address
                           WHERE UserId = @UserId";

            SqlParameter[] parameters = {
                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = user.FullName ?? string.Empty },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = user.Role },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)user.Phone ?? DBNull.Value },
                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)user.Address ?? DBNull.Value },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = user.UserId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteUser(int userId)
        {
            string sql = "DELETE FROM Users WHERE UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool BatchUpdateUserRoles(List<User> users)
        {
            if (users == null || users.Count == 0) return true;

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var u in users)
                        {
                            string sql = @"UPDATE Users 
                                           SET Role = @Role, FullName = @FullName, Phone = @Phone, Address = @Address 
                                           WHERE UserId = @UserId";

                            SqlParameter[] parameters = {
                                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = u.Role },
                                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = u.FullName ?? string.Empty },
                                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)u.Phone ?? DBNull.Value },
                                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)u.Address ?? DBNull.Value },
                                new SqlParameter("@UserId", SqlDbType.Int) { Value = u.UserId }
                            };

                            DatabaseHelper.ExecuteNonQuery(sql, parameters, trans);
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

        private User MapRowToUser(DataRow row)
        {
            return new User(
                Convert.ToInt32(row["UserId"]),
                Convert.ToString(row["Username"]),
                Convert.ToString(row["PasswordHash"]),
                Convert.ToString(row["FullName"]),
                Convert.ToString(row["Role"]),
                row["Phone"] != DBNull.Value ? Convert.ToString(row["Phone"]) : string.Empty,
                row["Address"] != DBNull.Value ? Convert.ToString(row["Address"]) : string.Empty,
                Convert.ToDateTime(row["CreatedAt"])
            );
        }
    }
}
