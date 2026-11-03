using System;

namespace Tonyo_sPizza.Models
{
    public class MenuItemDoughSize
    {
        public int SizeId { get; set; }
        public int ItemId { get; set; }
        public string SizeName { get; set; } 
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int? IngredientId { get; set; }
        public string DoughIngredientName { get; set; }
        public decimal DoughPortionQty { get; set; } = 1.00m;
        public decimal CurrentStockAvailable { get; set; }
        public bool IsDefault { get; set; }

        public MenuItemDoughSize() { }

        public MenuItemDoughSize(int sizeId, int itemId, string sizeName, decimal price, decimal cost, int? ingredientId, decimal portionQty, bool isDefault = false)
        {
            SizeId = sizeId;
            ItemId = itemId;
            SizeName = sizeName;
            Price = price;
            Cost = cost;
            IngredientId = ingredientId;
            DoughPortionQty = portionQty;
            IsDefault = isDefault;
        }

        public override string ToString()
        {
            return $"{SizeName} - ${Price:N2}";
        }
    }
}
