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
        private readonly StockTransactionRepository _stockTxRepository;
        private List<Ingredient> _originalStock;
        private List<Ingredient> _workingStock;
        private bool _includeArchived = false;

        public static StockService Instance => _instance ?? (_instance = new StockService());

        public List<Ingredient> WorkingStock => _workingStock;
        public bool HasPendingChanges => CheckPendingChanges();
        public bool IncludeArchived
        {
            get => _includeArchived;
            set
            {
                if (_includeArchived != value)
                {
                    _includeArchived = value;
                    ReloadFromDatabase();
                }
            }
        }

        public event Action StockDataChanged;

        public StockService()
        {
            _ingredientRepository = new IngredientRepository();
            _stockTxRepository = new StockTransactionRepository();
            ReloadFromDatabase();
        }

        public void ReloadFromDatabase()
        {
            try
            {
                _originalStock = _ingredientRepository.GetAllIngredients(_includeArchived);
                _workingStock = _originalStock.Select(i => i.Clone()).ToList();
                StockDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.ReloadFromDatabase");
                _originalStock = _originalStock ?? new List<Ingredient>();
                _workingStock = _workingStock ?? new List<Ingredient>();
            }
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

        public bool RestockAtomic(int ingredientId, decimal addQty, string notes, out int createdTxId, out string errorMessage)
        {
            createdTxId = 0;
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin", "Crew");
                var user = AuthService.Instance.CurrentUser;
                bool ok = _stockTxRepository.CreateRestockTransaction(
                    ingredientId,
                    addQty,
                    user?.UserId,
                    user?.Username ?? "Admin",
                    notes,
                    out createdTxId
                );

                if (ok)
                {
                    var ing = _ingredientRepository.GetIngredientById(ingredientId);
                    AuditService.Instance.Log("Stock Restock", $"Restocked +{addQty:N2} {ing?.Unit} of '{ing?.Name}' (Tx #{createdTxId}).", "StockTransactions", createdTxId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Restock transaction failed.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.RestockAtomic");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool VoidStockTransaction(int stockTxId, byte[] rowVersion, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                if (_stockTxRepository.VoidStockTransaction(stockTxId, rowVersion))
                {
                    AuditService.Instance.Log("Stock Void", $"Voided Stock Transaction #{stockTxId} and reversed inventory quantities.", "StockTransactions", stockTxId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to void stock transaction.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.VoidStockTransaction");
                errorMessage = ex.Message;
                return false;
            }
        }

        public List<StockTransaction> GetStockTransactions(DateTime? fromDate = null, DateTime? toDate = null, string status = "All", string search = null)
        {
            return _stockTxRepository.GetTransactions(fromDate, toDate, status, search);
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
                AuthService.Instance.RequireRole("Admin");
                if (_ingredientRepository.IsIngredientInUse(ingredientId, out var usedInMenuNames))
                {
                    errorMessage = $"Cannot permanently delete this ingredient because it is currently used by:\n• " +
                                   string.Join("\n• ", usedInMenuNames) +
                                   "\n\nYou can Archive this ingredient instead or remove it from those menu items first.";
                    return false;
                }

                if (_ingredientRepository.HasStockTransactions(ingredientId))
                {
                    errorMessage = "Cannot permanently delete this ingredient because it has recorded stock transactions.\nPlease Archive it instead to preserve inventory audit history.";
                    return false;
                }

                bool ok = _ingredientRepository.DeleteIngredient(ingredientId);
                if (ok)
                {
                    AuditService.Instance.Log("Delete Ingredient", $"Ingredient #{ingredientId} deleted from inventory.", "Ingredients", ingredientId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to delete ingredient.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.DeleteIngredient");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool SetIngredientActiveStatus(int ingredientId, bool isActive, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                var ing = _ingredientRepository.GetIngredientById(ingredientId);
                if (ing == null)
                {
                    errorMessage = "Ingredient not found.";
                    return false;
                }

                if (_ingredientRepository.SetIngredientActiveStatus(ingredientId, isActive))
                {
                    AuditService.Instance.Log(isActive ? "Restore Ingredient" : "Archive Ingredient", $"Ingredient '{ing.Name}' set to {(isActive ? "Active" : "Archived")}.", "Ingredients", ingredientId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Could not update ingredient status.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.SetIngredientActiveStatus");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool AddNewIngredient(Ingredient ingredient, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                bool ok = _ingredientRepository.InsertIngredient(ingredient);
                if (ok)
                {
                    AuditService.Instance.Log("Add Ingredient", $"Added new ingredient: {ingredient?.Name} ({ingredient?.CurrentStock} {ingredient?.Unit})", "Ingredients", ingredient?.IngredientId.ToString());
                    if (ingredient.CurrentStock > 0)
                    {
                        var u = AuthService.Instance.CurrentUser;
                        var delta = new List<StockTransactionItem>
                        {
                            new StockTransactionItem
                            {
                                IngredientId = ingredient.IngredientId,
                                IngredientName = ingredient.Name,
                                QuantityChange = ingredient.CurrentStock,
                                Unit = ingredient.Unit,
                                StockBefore = 0m,
                                StockAfter = ingredient.CurrentStock
                            }
                        };
                        _stockTxRepository.RecordBatchAdjustmentTransaction(delta, u?.UserId, u?.Username ?? "Admin", $"Initial stock for {ingredient.Name}", out _);
                    }
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to add ingredient.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.AddNewIngredient");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateIngredient(Ingredient ingredient, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                var orig = _originalStock?.FirstOrDefault(i => i.IngredientId == ingredient?.IngredientId);
                bool ok = _ingredientRepository.UpdateIngredient(ingredient);
                if (ok)
                {
                    if (orig != null && orig.CurrentStock != ingredient.CurrentStock)
                    {
                        var u = AuthService.Instance.CurrentUser;
                        var delta = new List<StockTransactionItem>
                        {
                            new StockTransactionItem
                            {
                                IngredientId = ingredient.IngredientId,
                                IngredientName = ingredient.Name,
                                QuantityChange = ingredient.CurrentStock - orig.CurrentStock,
                                Unit = ingredient.Unit,
                                StockBefore = orig.CurrentStock,
                                StockAfter = ingredient.CurrentStock
                            }
                        };
                        _stockTxRepository.RecordBatchAdjustmentTransaction(delta, u?.UserId, u?.Username ?? "Admin", $"Stock edited for {ingredient.Name}", out _);
                    }
                    AuditService.Instance.Log("Edit Ingredient", $"Updated ingredient parameters: {ingredient?.Name}", "Ingredients", ingredient?.IngredientId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to update ingredient.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.UpdateIngredient");
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
                var deltas = new List<StockTransactionItem>();
                foreach (var work in _workingStock)
                {
                    var orig = _originalStock?.FirstOrDefault(o => o.IngredientId == work.IngredientId);
                    if (orig != null && work.CurrentStock != orig.CurrentStock)
                    {
                        deltas.Add(new StockTransactionItem
                        {
                            IngredientId = work.IngredientId,
                            IngredientName = work.Name,
                            QuantityChange = work.CurrentStock - orig.CurrentStock,
                            Unit = work.Unit,
                            StockBefore = orig.CurrentStock,
                            StockAfter = work.CurrentStock
                        });
                    }
                }

                bool ok = _ingredientRepository.BatchSaveIngredients(_workingStock);
                if (ok)
                {
                    if (deltas.Count > 0)
                    {
                        var u = AuthService.Instance.CurrentUser;
                        _stockTxRepository.RecordBatchAdjustmentTransaction(deltas, u?.UserId, u?.Username ?? "Admin", "Batch stock update", out _);
                    }
                    AuditService.Instance.Log("Stock Update", "Batch updated inventory stock and parameters.", "Ingredients");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to save stock changes.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StockService.SaveChanges");
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
