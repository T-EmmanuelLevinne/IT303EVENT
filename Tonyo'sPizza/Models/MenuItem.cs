using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Tonyo_sPizza.Models
{
    public class MenuItem
    {
        private int _itemId;
        private int? _categoryId;
        private string _categoryName;
        private string _name;
        private string _description;
        private decimal _price;
        private decimal? _oldPrice;
        private string _imageBase64;
        private bool _isAvailable;
        private DateTime _createdAt;

        public int ItemId
        {
            get => _itemId;
            set => _itemId = value;
        }

        public int? CategoryId
        {
            get => _categoryId;
            set => _categoryId = value;
        }

        public string CategoryName
        {
            get => _categoryName ?? "General";
            set => _categoryName = value;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Item name cannot be empty.");
                _name = value.Trim();
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value?.Trim() ?? string.Empty;
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public decimal? OldPrice
        {
            get => _oldPrice;
            set => _oldPrice = value;
        }

        public string ImageBase64
        {
            get => _imageBase64;
            set => _imageBase64 = value;
        }

        public bool IsAvailable
        {
            get => _isAvailable;
            set => _isAvailable = value;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        private List<MenuItemIngredient> _recipeIngredients;
        private List<MenuItemDoughSize> _doughSizes;
        private List<MenuItemAddOn> _availableAddOns;

        public List<MenuItemDoughSize> DoughSizes
        {
            get
            {
                if (_doughSizes == null)
                    _doughSizes = new List<MenuItemDoughSize>();
                return _doughSizes;
            }
            set => _doughSizes = value;
        }

        public List<MenuItemAddOn> AvailableAddOns
        {
            get
            {
                if (_availableAddOns == null)
                    _availableAddOns = new List<MenuItemAddOn>();
                return _availableAddOns;
            }
            set => _availableAddOns = value;
        }

        public List<MenuItemIngredient> RecipeIngredients
        {
            get
            {
                if (_recipeIngredients == null)
                    _recipeIngredients = new List<MenuItemIngredient>();
                return _recipeIngredients;
            }
            set => _recipeIngredients = value;
        }

        public bool HasDiscount => _oldPrice.HasValue && _oldPrice.Value > _price;

        public MenuItem()
        {
            _isAvailable = true;
            _createdAt = DateTime.Now;
            _recipeIngredients = new List<MenuItemIngredient>();
            _doughSizes = new List<MenuItemDoughSize>();
            _availableAddOns = new List<MenuItemAddOn>();
        }

        public MenuItem(int itemId, int? categoryId, string name, string description, decimal price, decimal? oldPrice, string imageBase64, bool isAvailable, DateTime createdAt, string categoryName = "")
        {
            _itemId = itemId;
            _categoryId = categoryId;
            Name = name;
            Description = description;
            Price = price;
            OldPrice = oldPrice;
            ImageBase64 = imageBase64;
            IsAvailable = isAvailable;
            CreatedAt = createdAt;
            CategoryName = categoryName;
            _recipeIngredients = new List<MenuItemIngredient>();
        }

        public Image GetImage()
        {
            if (string.IsNullOrEmpty(_imageBase64)) return null;
            try
            {
                byte[] bytes = Convert.FromBase64String(_imageBase64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        public void SetImage(Image image)
        {
            if (image == null)
            {
                _imageBase64 = null;
                return;
            }
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Save as PNG to preserve transparency
                    using (Bitmap bmp = new Bitmap(image))
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    _imageBase64 = Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {
                _imageBase64 = null;
            }
        }
    }
}
