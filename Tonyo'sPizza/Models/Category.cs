using System;

namespace Tonyo_sPizza.Models
{
    public class Category
    {
        private int _categoryId;
        private string _categoryName;
        private string _iconKey;
        private string _description;
        private DateTime _createdAt;

        public int CategoryId
        {
            get => _categoryId;
            set => _categoryId = value;
        }

        public string CategoryName
        {
            get => _categoryName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Category name cannot be empty.");
                _categoryName = value.Trim();
            }
        }

        public string IconKey
        {
            get => _iconKey;
            set => _iconKey = string.IsNullOrWhiteSpace(value) ? "Pizza" : value.Trim();
        }

        public string Description
        {
            get => _description;
            set => _description = value?.Trim() ?? string.Empty;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public Category()
        {
            _createdAt = DateTime.Now;
            _iconKey = "Pizza";
        }

        public Category(int categoryId, string categoryName, string iconKey, string description, DateTime createdAt)
        {
            _categoryId = categoryId;
            CategoryName = categoryName;
            IconKey = iconKey;
            Description = description;
            _createdAt = createdAt;
        }

        public override string ToString()
        {
            return _categoryName ?? string.Empty;
        }
    }
}
