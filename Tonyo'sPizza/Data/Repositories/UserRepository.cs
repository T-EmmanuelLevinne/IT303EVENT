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
            DatabaseHelper.EnsureSchemaUpgrades();
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

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "SELECT * FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username.Trim() }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRowToUser(dt.Rows[0]);
            }
            return null;
        }

        public bool UsernameExists(string username, int? excludeUserId = null)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            var parameters = new List<SqlParameter> {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username.Trim() }
            };
            if (excludeUserId.HasValue)
            {
                sql += " AND UserId <> @ExcludeUserId";
                parameters.Add(new SqlParameter("@ExcludeUserId", SqlDbType.Int) { Value = excludeUserId.Value });
            }

            object result = DatabaseHelper.ExecuteScalar(sql, parameters.ToArray());
            return Convert.ToInt32(result) > 0;
        }

        public bool Register(User user)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            if (UsernameExists(user.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            string sql = @"INSERT INTO Users (Username, PasswordHash, Salt, FullName, Role, Phone, Address, IsActive, FailedLoginAttempts, IsLocked, CreatedAt)
                           VALUES (@Username, @PasswordHash, @Salt, @FullName, @Role, @Phone, @Address, @IsActive, 0, 0, @CreatedAt);
                           SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = user.Username },
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 256) { Value = user.PasswordHash },
                new SqlParameter("@Salt", SqlDbType.NVarChar, 64) { Value = (object)user.Salt ?? DBNull.Value },
                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = user.FullName ?? string.Empty },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = user.Role },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)user.Phone ?? DBNull.Value },
                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)user.Address ?? DBNull.Value },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = user.IsActive },
                new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = user.CreatedAt }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                user.UserId = Convert.ToInt32(res);
                var fresh = GetUserById(user.UserId);
                if (fresh != null) user.RowVersion = fresh.RowVersion;
                return true;
            }
            return false;
        }

        public User GetUserById(int userId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
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
            DatabaseHelper.EnsureSchemaUpgrades();
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
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = @"UPDATE Users 
                           SET FullName = @FullName, Role = @Role, Phone = @Phone, Address = @Address, IsActive = @IsActive
                           WHERE UserId = @UserId";

            var parameters = new List<SqlParameter> {
                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = user.FullName ?? string.Empty },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = user.Role },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)user.Phone ?? DBNull.Value },
                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)user.Address ?? DBNull.Value },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = user.IsActive },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = user.UserId }
            };

            if (user.RowVersion != null && user.RowVersion.Length == 8)
            {
                sql += " AND RowVersion = @RowVersion";
                parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = user.RowVersion });
            }

            int affected = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray());
            if (affected == 0 && user.RowVersion != null)
            {
                if (GetUserById(user.UserId) != null)
                {
                    throw new DBConcurrencyException("This user account was modified by another administrator. Please refresh and try again.");
                }
            }
            return affected > 0;
        }

        public bool UpdatePasswordHashAndSalt(int userId, string passwordHash, string salt)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = @"UPDATE Users 
                           SET PasswordHash = @PasswordHash, Salt = @Salt, FailedLoginAttempts = 0, IsLocked = 0, LockoutUntil = NULL 
                           WHERE UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@PasswordHash", SqlDbType.NVarChar, 256) { Value = passwordHash },
                new SqlParameter("@Salt", SqlDbType.NVarChar, 64) { Value = (object)salt ?? DBNull.Value },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public void RecordFailedLoginAttempt(int userId, int newCount, bool lockAccount, DateTime? lockoutUntil)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = @"UPDATE Users 
                           SET FailedLoginAttempts = @Attempts, IsLocked = @IsLocked, LockoutUntil = @LockoutUntil 
                           WHERE UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@Attempts", SqlDbType.Int) { Value = newCount },
                new SqlParameter("@IsLocked", SqlDbType.Bit) { Value = lockAccount },
                new SqlParameter("@LockoutUntil", SqlDbType.DateTime) { Value = (object)lockoutUntil ?? DBNull.Value },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };
            DatabaseHelper.ExecuteNonQuery(sql, parameters);
        }

        public void ResetLoginLockout(int userId)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Users SET FailedLoginAttempts = 0, IsLocked = 0, LockoutUntil = NULL WHERE UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };
            DatabaseHelper.ExecuteNonQuery(sql, parameters);
        }

        public bool SetUserActiveStatus(int userId, bool isActive)
        {
            DatabaseHelper.EnsureSchemaUpgrades();
            string sql = "UPDATE Users SET IsActive = @IsActive WHERE UserId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool HasUserTransactions(int userId)
        {
            string sql = "SELECT COUNT(*) FROM Orders WHERE CustomerId = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };
            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(res) > 0;
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
                                           SET Role = @Role, FullName = @FullName, Phone = @Phone, Address = @Address, IsActive = @IsActive 
                                           WHERE UserId = @UserId";

                            var parameters = new List<SqlParameter> {
                                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = u.Role },
                                new SqlParameter("@FullName", SqlDbType.NVarChar, 100) { Value = u.FullName ?? string.Empty },
                                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)u.Phone ?? DBNull.Value },
                                new SqlParameter("@Address", SqlDbType.NVarChar, 255) { Value = (object)u.Address ?? DBNull.Value },
                                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = u.IsActive },
                                new SqlParameter("@UserId", SqlDbType.Int) { Value = u.UserId }
                            };

                            if (u.RowVersion != null && u.RowVersion.Length == 8)
                            {
                                sql += " AND RowVersion = @RowVersion";
                                parameters.Add(new SqlParameter("@RowVersion", SqlDbType.Timestamp) { Value = u.RowVersion });
                            }

                            int rows = DatabaseHelper.ExecuteNonQuery(sql, parameters.ToArray(), trans);
                            if (rows == 0 && u.RowVersion != null)
                            {
                                throw new DBConcurrencyException($"User '{u.Username}' was updated by someone else. Please cancel or refresh before saving.");
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

        private User MapRowToUser(DataRow row)
        {
            var user = new User(
                Convert.ToInt32(row["UserId"]),
                Convert.ToString(row["Username"]),
                Convert.ToString(row["PasswordHash"]),
                Convert.ToString(row["FullName"]),
                Convert.ToString(row["Role"]),
                row["Phone"] != DBNull.Value ? Convert.ToString(row["Phone"]) : string.Empty,
                row["Address"] != DBNull.Value ? Convert.ToString(row["Address"]) : string.Empty,
                Convert.ToDateTime(row["CreatedAt"])
            );

            if (row.Table.Columns.Contains("Salt") && row["Salt"] != DBNull.Value)
                user.Salt = Convert.ToString(row["Salt"]);
            if (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value)
                user.IsActive = Convert.ToBoolean(row["IsActive"]);
            if (row.Table.Columns.Contains("FailedLoginAttempts") && row["FailedLoginAttempts"] != DBNull.Value)
                user.FailedLoginAttempts = Convert.ToInt32(row["FailedLoginAttempts"]);
            if (row.Table.Columns.Contains("IsLocked") && row["IsLocked"] != DBNull.Value)
                user.IsLocked = Convert.ToBoolean(row["IsLocked"]);
            if (row.Table.Columns.Contains("LockoutUntil") && row["LockoutUntil"] != DBNull.Value)
                user.LockoutUntil = Convert.ToDateTime(row["LockoutUntil"]);
            if (row.Table.Columns.Contains("RowVersion") && row["RowVersion"] != DBNull.Value)
                user.RowVersion = (byte[])row["RowVersion"];

            return user;
        }
    }
}
