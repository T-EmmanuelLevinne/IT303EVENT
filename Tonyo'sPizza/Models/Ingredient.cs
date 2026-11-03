using System;

namespace Tonyo_sPizza.Models
{
    public class Ingredient
    {
        private int _ingredientId;
        private int? _categoryId;
        private string _categoryName = "General";
        private string _name;
        private decimal _currentStock;
        private string _unit;
        private decimal _minThreshold;
        private decimal _costPerUnit;
        private DateTime _createdAt;

        public int IngredientId
        {
            get => _ingredientId;
            set => _ingredientId = value;
        }

        public int? CategoryId
        {
            get => _categoryId;
            set => _categoryId = value;
        }

        public string CategoryName
        {
            get => _categoryName;
            set => _categoryName = string.IsNullOrWhiteSpace(value) ? "General" : value.Trim();
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ingredient name cannot be empty.");
                _name = value.Trim();
            }
        }

        public decimal CurrentStock
        {
            get => _currentStock;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Stock cannot be negative.");
                _currentStock = value;
            }
        }

        public string Unit
        {
            get => _unit;
            set => _unit = string.IsNullOrWhiteSpace(value) ? "g" : value.Trim();
        }

        public decimal MinThreshold
        {
            get => _minThreshold;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Minimum threshold cannot be negative.");
                _minThreshold = value;
            }
        }

        public decimal CostPerUnit
        {
            get => _costPerUnit;
            set => _costPerUnit = Math.Max(0, value);
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public bool IsLowStock => _currentStock <= _minThreshold;
        public bool IsOutOfStock => _currentStock <= 0;

        public string StockStatusDisplay
        {
            get
            {
                if (IsOutOfStock) return "Out of Stock";
                if (IsLowStock) return "Low Stock";
                return "In Stock";
            }
        }

        public Ingredient()
        {
            _categoryName = "General";
            _unit = "g";
            _minThreshold = 10;
            _createdAt = DateTime.Now;
        }

        public Ingredient(int ingredientId, string name, decimal currentStock, string unit, decimal minThreshold, decimal costPerUnit, DateTime createdAt, int? categoryId = null, string categoryName = "General")
        {
            _ingredientId = ingredientId;
            _categoryId = categoryId;
            _categoryName = string.IsNullOrWhiteSpace(categoryName) ? "General" : categoryName;
            Name = name;
            CurrentStock = currentStock;
            Unit = unit;
            MinThreshold = minThreshold;
            CostPerUnit = costPerUnit;
            _createdAt = createdAt;
        }

        public Ingredient Clone()
        {
            return new Ingredient(_ingredientId, _name, _currentStock, _unit, _minThreshold, _costPerUnit, _createdAt, _categoryId, _categoryName);
        }

        public override string ToString()
        {
            return $"{_name} ({_currentStock:N1} {_unit})";
        }
    }
}
