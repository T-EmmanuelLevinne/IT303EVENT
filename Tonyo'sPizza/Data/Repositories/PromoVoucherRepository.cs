using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Data.Repositories
{
    public class PromoVoucherRepository
    {
        public List<PromoVoucher> GetAllVouchers(string search = null)
        {
            List<PromoVoucher> list = new List<PromoVoucher>();
            DatabaseHelper.EnsureSchemaUpgrades();

            string sql = "SELECT * FROM PromoVouchers WHERE 1=1 ";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND (Code LIKE @Search OR Description LIKE @Search) ";
                parameters.Add(new SqlParameter("@Search", SqlDbType.NVarChar, 100) { Value = $"%{search.Trim()}%" });
            }

            sql += " ORDER BY PromoId DESC";

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToPromoVoucher(row));
            }
            return list;
        }

        public PromoVoucher GetVoucherByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            DatabaseHelper.EnsureSchemaUpgrades();

            string sql = "SELECT TOP 1 * FROM PromoVouchers WHERE UPPER(Code) = @Code AND IsActive = 1";
            SqlParameter[] parameters = {
                new SqlParameter("@Code", SqlDbType.NVarChar, 50) { Value = code.Trim().ToUpper() }
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return MapRowToPromoVoucher(dt.Rows[0]);
            }
            return null;
        }

        public bool InsertVoucher(PromoVoucher voucher)
        {
            if (voucher == null) return false;
            DatabaseHelper.EnsureSchemaUpgrades();

            string sql = @"
                INSERT INTO PromoVouchers (Code, Description, DiscountType, DiscountValue, MinOrderAmount, MaxDiscountAmount, IsActive, CreatedAt)
                VALUES (@Code, @Description, @DiscountType, @DiscountValue, @MinOrderAmount, @MaxDiscountAmount, @IsActive, GETDATE());
                SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Code", SqlDbType.NVarChar, 50) { Value = voucher.Code.Trim().ToUpper() },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)voucher.Description ?? DBNull.Value },
                new SqlParameter("@DiscountType", SqlDbType.NVarChar, 20) { Value = voucher.DiscountType },
                new SqlParameter("@DiscountValue", SqlDbType.Decimal) { Value = voucher.DiscountValue },
                new SqlParameter("@MinOrderAmount", SqlDbType.Decimal) { Value = voucher.MinOrderAmount },
                new SqlParameter("@MaxDiscountAmount", SqlDbType.Decimal) { Value = (object)voucher.MaxDiscountAmount ?? DBNull.Value },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = voucher.IsActive }
            };

            object res = DatabaseHelper.ExecuteScalar(sql, parameters);
            if (res != null && res != DBNull.Value)
            {
                voucher.PromoId = Convert.ToInt32(res);
                return true;
            }
            return false;
        }

        public bool UpdateVoucher(PromoVoucher voucher)
        {
            if (voucher == null) return false;
            DatabaseHelper.EnsureSchemaUpgrades();

            string sql = @"
                UPDATE PromoVouchers 
                SET Code = @Code,
                    Description = @Description,
                    DiscountType = @DiscountType,
                    DiscountValue = @DiscountValue,
                    MinOrderAmount = @MinOrderAmount,
                    MaxDiscountAmount = @MaxDiscountAmount,
                    IsActive = @IsActive
                WHERE PromoId = @PromoId";

            SqlParameter[] parameters = {
                new SqlParameter("@PromoId", SqlDbType.Int) { Value = voucher.PromoId },
                new SqlParameter("@Code", SqlDbType.NVarChar, 50) { Value = voucher.Code.Trim().ToUpper() },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Value = (object)voucher.Description ?? DBNull.Value },
                new SqlParameter("@DiscountType", SqlDbType.NVarChar, 20) { Value = voucher.DiscountType },
                new SqlParameter("@DiscountValue", SqlDbType.Decimal) { Value = voucher.DiscountValue },
                new SqlParameter("@MinOrderAmount", SqlDbType.Decimal) { Value = voucher.MinOrderAmount },
                new SqlParameter("@MaxDiscountAmount", SqlDbType.Decimal) { Value = (object)voucher.MaxDiscountAmount ?? DBNull.Value },
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = voucher.IsActive }
            };

            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool ToggleActive(int promoId, bool isActive)
        {
            string sql = "UPDATE PromoVouchers SET IsActive = @IsActive WHERE PromoId = @PromoId";
            SqlParameter[] parameters = {
                new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive },
                new SqlParameter("@PromoId", SqlDbType.Int) { Value = promoId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool DeleteVoucher(int promoId)
        {
            string sql = "DELETE FROM PromoVouchers WHERE PromoId = @PromoId";
            SqlParameter[] parameters = {
                new SqlParameter("@PromoId", SqlDbType.Int) { Value = promoId }
            };
            return DatabaseHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        private PromoVoucher MapRowToPromoVoucher(DataRow row)
        {
            return new PromoVoucher(
                Convert.ToInt32(row["PromoId"]),
                Convert.ToString(row["Code"]),
                row["Description"] != DBNull.Value ? Convert.ToString(row["Description"]) : string.Empty,
                Convert.ToString(row["DiscountType"]),
                Convert.ToDecimal(row["DiscountValue"]),
                row.Table.Columns.Contains("MinOrderAmount") && row["MinOrderAmount"] != DBNull.Value ? Convert.ToDecimal(row["MinOrderAmount"]) : 0m,
                Convert.ToBoolean(row["IsActive"]),
                Convert.ToDateTime(row["CreatedAt"]),
                row.Table.Columns.Contains("MaxDiscountAmount") && row["MaxDiscountAmount"] != DBNull.Value ? (decimal?)Convert.ToDecimal(row["MaxDiscountAmount"]) : null,
                row.Table.Columns.Contains("ExpiresAt") && row["ExpiresAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["ExpiresAt"]) : null
            );
        }
    }
}
