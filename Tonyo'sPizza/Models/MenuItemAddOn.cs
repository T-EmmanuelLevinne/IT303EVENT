using System;

namespace Tonyo_sPizza.Models
{
    public class MenuItemAddOn
    {
        public int AddOnId { get; set; }
        public int ItemId { get; set; }
        public int IngredientId { get; set; }
        public string AddOnName { get; set; }
        public decimal Price { get; set; } = 1.50m;
        public decimal PortionQty { get; set; } = 1.00m;
        public string CategoryName { get; set; } = "Toppings"; // e.g. "Cheese", "Veggie", "Meat", "Sauce"
        public string Unit { get; set; } = "g";
        public decimal CurrentStock { get; set; }

        public MenuItemAddOn() { }

        public MenuItemAddOn(int addOnId, int itemId, int ingredientId, string addOnName, decimal price, decimal portionQty, string categoryName, string unit = "g", decimal stock = 0)
        {
            AddOnId = addOnId;
            ItemId = itemId;
            IngredientId = ingredientId;
            AddOnName = addOnName;
            Price = price;
            PortionQty = portionQty;
            CategoryName = categoryName;
            Unit = unit;
            CurrentStock = stock;
        }

        public override string ToString()
        {
            return $"{AddOnName} (+${Price:N2})";
        }
    }
}
