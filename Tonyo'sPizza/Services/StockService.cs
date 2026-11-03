using System;
using System.Collections.Generic;
using System.Linq;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Services
{
    public class StockService
    {
        private static StockService _instance;
        private readonly IngredientRepository _ingredientRepository;
        private List<Ingredient> _originalStock;
        private List<Ingredient> _workingStock;

        public static StockService Instance => _instance ?? (_instance = new StockService());

        public List<Ingredient> WorkingStock => _workingStock;
        public bool HasPendingChanges => CheckPendingChanges();

        public event Action StockDataChanged;

        public StockService()
        {
            _ingredientRepository = new IngredientRepository();
            ReloadFromDatabase();
        }

        public void ReloadFromDatabase()
        {
            _originalStock = _ingredientRepository.GetAllIngredients();
            _workingStock = _originalStock.Select(i => i.Clone()).ToList();
            StockDataChanged?.Invoke();
        }

        public void Restock(int ingredientId, decimal addQty)
        {
            var item = _workingStock.FirstOrDefault(i => i.IngredientId == ingredientId);
            if (item != null)
            {
                item.CurrentStock += addQty;
                StockDataChanged?.Invoke();
            }
        }

        public void UpdateWorkingItem(Ingredient modified)
        {
            var item = _workingStock.FirstOrDefault(i => i.IngredientId == modified.IngredientId);
            if (item != null)
            {
                item.Name = modified.Name;
                item.CurrentStock = modified.CurrentStock;
                item.Unit = modified.Unit;
                item.MinThreshold = modified.MinThreshold;
                item.CostPerUnit = modified.CostPerUnit;
                StockDataChanged?.Invoke();
            }
        }

        public bool DeleteIngredient(int ingredientId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                if (_ingredientRepository.IsIngredientInUse(ingredientId, out var usedInMenuNames))
                {
                    errorMessage = $"Cannot delete this ingredient because it is currently used by the following menu item(s):\n• " +
                                   string.Join("\n• ", usedInMenuNames) +
                                   "\n\nPlease remove it from those menu items first.";
                    return false;
                }

                bool ok = _ingredientRepository.DeleteIngredient(ingredientId);
                if (ok)
                {
                    AuditService.Instance.Log("Delete Ingredient", $"Ingredient #{ingredientId} deleted from inventory.");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to delete ingredient.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool AddNewIngredient(Ingredient ingredient, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                bool ok = _ingredientRepository.InsertIngredient(ingredient);
                if (ok)
                {
                    AuditService.Instance.Log("Add Ingredient", $"Added new ingredient: {ingredient?.Name} ({ingredient?.CurrentStock} {ingredient?.Unit})");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to add ingredient.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateIngredient(Ingredient ingredient, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                bool ok = _ingredientRepository.UpdateIngredient(ingredient);
                if (ok)
                {
                    AuditService.Instance.Log("Edit Ingredient", $"Updated ingredient parameters: {ingredient?.Name}");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to update ingredient.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public void CancelChanges()
        {
            // Revert working copy back to original database state
            _workingStock = _originalStock.Select(i => i.Clone()).ToList();
            StockDataChanged?.Invoke();
        }

        public bool SaveChanges(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                bool ok = _ingredientRepository.BatchSaveIngredients(_workingStock);
                if (ok)
                {
                    AuditService.Instance.Log("Stock Update", "Batch updated inventory stock and parameters.");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to save stock changes.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private bool CheckPendingChanges()
        {
            if (_originalStock == null || _workingStock == null) return false;
            if (_originalStock.Count != _workingStock.Count) return true;

            for (int i = 0; i < _originalStock.Count; i++)
            {
                var orig = _originalStock[i];
                var work = _workingStock.FirstOrDefault(w => w.IngredientId == orig.IngredientId);
                if (work == null) return true;
                if (work.CurrentStock != orig.CurrentStock ||
                    work.Name != orig.Name ||
                    work.Unit != orig.Unit ||
                    work.MinThreshold != orig.MinThreshold ||
                    work.CostPerUnit != orig.CostPerUnit)
                {
                    return true;
                }
            }
            return false;
        }

        public void RefreshIfClean()
        {
            if (!HasPendingChanges)
            {
                ReloadFromDatabase();
            }
        }

        public int GetAvailableStockForMenuItem(MenuItem item, string doughSizeName = null)
        {
            if (item == null || !item.IsAvailable) return 0;

            if (_workingStock == null || _workingStock.Count == 0)
            {
                ReloadFromDatabase();
            }

            // Ensure recipes and dough sizes are loaded
            if ((item.RecipeIngredients == null || item.RecipeIngredients.Count == 0) && item.ItemId > 0)
            {
                try
                {
                    var repo = new MenuItemRepository();
                    item.RecipeIngredients = repo.GetRecipeIngredients(item.ItemId);
                    if (item.DoughSizes == null || item.DoughSizes.Count == 0)
                    {
                        item.DoughSizes = repo.GetMenuItemDoughSizes(item.ItemId);
                    }
                }
                catch { }
            }

            int minPortions = int.MaxValue;
            bool hasIngredientConstraints = false;

            // 1. Check Recipe Ingredients
            if (item.RecipeIngredients != null && item.RecipeIngredients.Count > 0)
            {
                string targetSizeName = doughSizeName;
                if (string.IsNullOrEmpty(targetSizeName) && item.DoughSizes != null && item.DoughSizes.Count > 0)
                {
                    var defSize = item.DoughSizes.FirstOrDefault(ds => ds.IsDefault) ?? item.DoughSizes[0];
                    targetSizeName = defSize?.SizeName;
                }

                List<MenuItemIngredient> recipesToCheck = null;
                if (!string.IsNullOrEmpty(targetSizeName))
                {
                    var sizeRecipes = item.RecipeIngredients.Where(r => string.Equals(r.SizeName, targetSizeName, StringComparison.OrdinalIgnoreCase)).ToList();
                    if (sizeRecipes.Count > 0)
                    {
                        recipesToCheck = sizeRecipes;
                    }
                }

                if (recipesToCheck == null)
                {
                    var defaultRecipes = item.RecipeIngredients.Where(r => string.IsNullOrEmpty(r.SizeName)).ToList();
                    if (defaultRecipes.Count > 0)
                    {
                        recipesToCheck = defaultRecipes;
                    }
                    else
                    {
                        recipesToCheck = item.RecipeIngredients.GroupBy(r => r.SizeName).FirstOrDefault()?.ToList() ?? item.RecipeIngredients;
                    }
                }

                if (recipesToCheck != null && recipesToCheck.Count > 0)
                {
                    hasIngredientConstraints = true;
                    foreach (var recipe in recipesToCheck)
                    {
                        var ing = _workingStock.FirstOrDefault(i => i.IngredientId == recipe.IngredientId);
                        if (ing == null || ing.CurrentStock <= 0 || recipe.QuantityRequired <= 0)
                        {
                            return 0; // Missing or depleted ingredient
                        }

                        int portions = (int)Math.Floor(ing.CurrentStock / recipe.QuantityRequired);
                        if (portions < minPortions)
                        {
                            minPortions = portions;
                        }
                    }
                }
            }

            // 2. Check Dough Size Ingredient
            if (item.DoughSizes != null && item.DoughSizes.Count > 0)
            {
                MenuItemDoughSize sizeToUse = null;
                if (!string.IsNullOrEmpty(doughSizeName))
                {
                    sizeToUse = item.DoughSizes.FirstOrDefault(ds => string.Equals(ds.SizeName, doughSizeName, StringComparison.OrdinalIgnoreCase));
                }
                if (sizeToUse == null)
                {
                    sizeToUse = item.DoughSizes.FirstOrDefault(ds => ds.IsDefault) ?? item.DoughSizes[0];
                }

                if (sizeToUse != null && sizeToUse.IngredientId.HasValue && sizeToUse.DoughPortionQty > 0)
                {
                    hasIngredientConstraints = true;
                    var doughIng = _workingStock.FirstOrDefault(i => i.IngredientId == sizeToUse.IngredientId.Value);
                    if (doughIng == null || doughIng.CurrentStock <= 0)
                    {
                        return 0;
                    }

                    int doughPortions = (int)Math.Floor(doughIng.CurrentStock / sizeToUse.DoughPortionQty);
                    if (doughPortions < minPortions)
                    {
                        minPortions = doughPortions;
                    }
                }
            }

            if (!hasIngredientConstraints)
            {
                return item.IsAvailable ? 99 : 0;
            }

            return Math.Max(0, minPortions == int.MaxValue ? 0 : minPortions);
        }
    }
}
