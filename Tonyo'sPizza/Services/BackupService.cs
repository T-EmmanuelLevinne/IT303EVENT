using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Tonyo_sPizza.Data;

namespace Tonyo_sPizza.Services
{
    public class BackupFileEntry
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public long SizeBytes { get; set; }
        public DateTime CreatedAt { get; set; }

        public string FormattedSize => SizeBytes >= 1048576
            ? $"{SizeBytes / 1048576.0:N2} MB"
            : $"{Math.Max(1, SizeBytes / 1024.0):N0} KB";
    }

    public static class BackupService
    {
        public static string GetDefaultBackupDirectory()
        {
            try
            {
                string publicDir = Environment.GetEnvironmentVariable("PUBLIC");
                string baseDir = !string.IsNullOrWhiteSpace(publicDir)
                    ? Path.Combine(publicDir, "TonyosPizzaBackups")
                    : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TonyosPizza", "Backups");

                if (!Directory.Exists(baseDir))
                {
                    Directory.CreateDirectory(baseDir);
                }
                return baseDir;
            }
            catch
            {
                return Path.GetTempPath();
            }
        }

        public static string GetCurrentDatabaseName()
        {
            var builder = new SqlConnectionStringBuilder(DatabaseHelper.ConnectionString);
            return string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "TonyosPizzaDB" : builder.InitialCatalog;
        }

        public static List<BackupFileEntry> GetAvailableBackups(string directory = null)
        {
            var results = new List<BackupFileEntry>();
            try
            {
                string dir = string.IsNullOrWhiteSpace(directory) ? GetDefaultBackupDirectory() : directory;
                if (!Directory.Exists(dir)) return results;

                foreach (string file in Directory.GetFiles(dir, "*.bak").OrderByDescending(f => File.GetLastWriteTime(f)))
                {
                    var fi = new FileInfo(file);
                    results.Add(new BackupFileEntry
                    {
                        FileName = fi.Name,
                        FullPath = fi.FullName,
                        SizeBytes = fi.Length,
                        CreatedAt = fi.LastWriteTime
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "BackupService.GetAvailableBackups");
            }
            return results;
        }

        public static bool BackupDatabase(string targetBakPath, out string message)
        {
            message = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");

                if (string.IsNullOrWhiteSpace(targetBakPath))
                {
                    message = "Please specify a valid backup file path.";
                    return false;
                }

                string dir = Path.GetDirectoryName(targetBakPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string dbName = GetCurrentDatabaseName();
                string safeDbBracket = $"[{dbName.Replace("]", "]]")}]";

                string sql = $@"BACKUP DATABASE {safeDbBracket} 
                                TO DISK = @DiskPath 
                                WITH FORMAT, INIT, NAME = @BackupName;";

                using (var conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.Add(new SqlParameter("@DiskPath", SqlDbType.NVarChar, 500) { Value = targetBakPath });
                        cmd.Parameters.Add(new SqlParameter("@BackupName", SqlDbType.NVarChar, 200) { Value = $"{dbName} Full Backup {DateTime.Now:yyyy-MM-dd HH:mm}" });
                        cmd.ExecuteNonQuery();
                    }
                }

                AuditService.Instance.Log("Backup Database", $"Created full database backup at '{Path.GetFileName(targetBakPath)}'.", dbName);
                message = $"Database backup saved successfully to:\n{targetBakPath}";
                return true;
            }
            catch (UnauthorizedAccessException uex)
            {
                message = uex.Message;
                return false;
            }
            catch (SqlException sqlEx)
            {
                ErrorLogger.Log(sqlEx, "BackupService.BackupDatabase");
                message = $"SQL Server could not write the backup file. Try saving inside '{GetDefaultBackupDirectory()}' so SQL Server has write permission.\n\nDetails: {sqlEx.Message}";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "BackupService.BackupDatabase");
                message = $"Backup failed: {ex.Message}";
                return false;
            }
        }

        public static bool RestoreDatabase(string sourceBakPath, out string message)
        {
            message = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");

                if (string.IsNullOrWhiteSpace(sourceBakPath) || !File.Exists(sourceBakPath))
                {
                    message = "The selected backup (.bak) file does not exist.";
                    return false;
                }

                string dbName = GetCurrentDatabaseName();
                string safeDbBracket = $"[{dbName.Replace("]", "]]")}]";

                var masterBuilder = new SqlConnectionStringBuilder(DatabaseHelper.ConnectionString)
                {
                    InitialCatalog = "master",
                    Pooling = false
                };

                SqlConnection.ClearAllPools();

                using (var conn = new SqlConnection(masterBuilder.ConnectionString))
                {
                    conn.Open();

                    bool dbExists;
                    using (var checkCmd = new SqlCommand("SELECT DB_ID(@DbName)", conn))
                    {
                        checkCmd.Parameters.Add(new SqlParameter("@DbName", SqlDbType.NVarChar, 128) { Value = dbName });
                        dbExists = checkCmd.ExecuteScalar() != DBNull.Value && checkCmd.ExecuteScalar() != null;
                    }

                    if (dbExists)
                    {
                        using (var singleCmd = new SqlCommand($"ALTER DATABASE {safeDbBracket} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", conn))
                        {
                            singleCmd.CommandTimeout = 60;
                            singleCmd.ExecuteNonQuery();
                        }
                    }

                    try
                    {
                        using (var restoreCmd = new SqlCommand($"RESTORE DATABASE {safeDbBracket} FROM DISK = @DiskPath WITH REPLACE;", conn))
                        {
                            restoreCmd.CommandTimeout = 300;
                            restoreCmd.Parameters.Add(new SqlParameter("@DiskPath", SqlDbType.NVarChar, 500) { Value = sourceBakPath });
                            restoreCmd.ExecuteNonQuery();
                        }
                    }
                    finally
                    {
                        if (dbExists)
                        {
                            try
                            {
                                using (var multiCmd = new SqlCommand($"ALTER DATABASE {safeDbBracket} SET MULTI_USER;", conn))
                                {
                                    multiCmd.CommandTimeout = 60;
                                    multiCmd.ExecuteNonQuery();
                                }
                            }
                            catch { }
                        }
                    }
                }

                SqlConnection.ClearAllPools();
                DatabaseHelper.ResetSchemaCheck();
                DatabaseHelper.EnsureSchemaUpgrades();

                // Reload in-memory caches cleanly
                StockService.Instance.ReloadFromDatabase();
                UserService.Instance.ReloadFromDatabase();

                AuditService.Instance.Log("Restore Database", $"Database '{dbName}' restored from backup '{Path.GetFileName(sourceBakPath)}'.", dbName);
                message = $"Database '{dbName}' has been restored successfully from:\n{sourceBakPath}";
                return true;
            }
            catch (UnauthorizedAccessException uex)
            {
                message = uex.Message;
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "BackupService.RestoreDatabase");
                message = $"Restore failed: {ex.Message}";
                return false;
            }
        }
    }
}
