using System;
using System.Collections.Generic;

namespace Tonyo_sPizza.Models
{
    public class StockTransaction
    {
        public int StockTxId { get; set; }
        public string TransactionType { get; set; } = "Restock"; // "Restock", "Adjustment", "Void"
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } = "Posted"; // "Posted", "Voided"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public byte[] RowVersion { get; set; }

        public List<StockTransactionItem> Items { get; set; } = new List<StockTransactionItem>();
    }

    public class StockTransactionItem
    {
        public int DetailId { get; set; }
        public int StockTxId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public decimal QuantityChange { get; set; }
        public string Unit { get; set; }
        public decimal StockBefore { get; set; }
        public decimal StockAfter { get; set; }
    }
}
