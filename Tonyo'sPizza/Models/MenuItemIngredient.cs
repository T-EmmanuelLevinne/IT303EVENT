using System;

namespace Tonyo_sPizza.Models
{
    public class MenuItemIngredient
    {
        private int _itemId;
        private int _ingredientId;
        private string _ingredientName;
        private string _unit;
        private decimal _quantityRequired;
        private string _sizeName = "";

        public int ItemId
        {
            get => _itemId;
            set => _itemId = value;
        }

        public int IngredientId
        {
            get => _ingredientId;
            set => _ingredientId = value;
        }

        public string IngredientName
        {
            get => _ingredientName;
            set => _ingredientName = value;
        }

        public string Unit
        {
            get => _unit;
            set => _unit = value;
        }

        public string SizeName
        {
            get => _sizeName ?? "";
            set => _sizeName = value ?? "";
        }

        public decimal QuantityRequired
        {
            get => _quantityRequired;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantity required must be greater than zero.");
                _quantityRequired = value;
            }
        }

        public MenuItemIngredient() { }

        public MenuItemIngredient(int itemId, int ingredientId, decimal quantityRequired, string ingredientName = "", string unit = "", string sizeName = "")
        {
            _itemId = itemId;
            _ingredientId = ingredientId;
            QuantityRequired = quantityRequired;
            _ingredientName = ingredientName;
            _unit = unit;
            _sizeName = sizeName ?? "";
        }
    }
}
