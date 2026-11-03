using System;

namespace Tonyo_sPizza.Models
{
    public class IngredientCategory
    {
        public int IngredientCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public IngredientCategory()
        {
            CreatedAt = DateTime.Now;
        }

        public IngredientCategory(int id, string name, string desc = null)
        {
            IngredientCategoryId = id;
            CategoryName = name;
            Description = desc;
            CreatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}
