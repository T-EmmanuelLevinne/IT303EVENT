using System;

namespace Tonyo_sPizza.Models
{
    public class PromoVoucher
    {
        public int PromoId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } = "Percentage"; // "Percentage" or "Fixed"
        public decimal DiscountValue { get; set; } = 10.00m;
        public decimal MinOrderAmount { get; set; } = 0.00m;
        public decimal? MaxDiscountAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ExpiresAt { get; set; }

        public string DisplayDiscount => DiscountType == "Percentage" ? $"{DiscountValue:G29}% OFF" : $"₱{DiscountValue:N2} OFF";

        public PromoVoucher() { }

        public PromoVoucher(int promoId, string code, string description, string discountType, decimal discountValue, decimal minOrderAmount, bool isActive, DateTime createdAt, decimal? maxDiscount = null, DateTime? expiresAt = null)
        {
            PromoId = promoId;
            Code = code;
            Description = description;
            DiscountType = discountType;
            DiscountValue = discountValue;
            MinOrderAmount = minOrderAmount;
            IsActive = isActive;
            CreatedAt = createdAt;
            MaxDiscountAmount = maxDiscount;
            ExpiresAt = expiresAt;
        }

        public PromoVoucher Clone()
        {
            return new PromoVoucher(PromoId, Code, Description, DiscountType, DiscountValue, MinOrderAmount, IsActive, CreatedAt, MaxDiscountAmount, ExpiresAt);
        }
    }
}
