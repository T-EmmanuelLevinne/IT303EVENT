# Tonyo's Pizza Restaurant Management System
**Project Write-up & System Architecture Report**

---

## 1. Problem Statement & System Purpose
Traditional quick-service and artisan pizza parlors often suffer from manual operation bottlenecks: handwritten order slips getting misplaced, kitchen staff misunderstanding custom topping requests, untracked inventory leading to unexpected stock shortages, and lack of accountability regarding cash flow and user privileges.

**Tonyo's Pizza Restaurant Management System** is a unified, desktop-based enterprise solution built in **C# Windows Forms** and **Microsoft SQL Server**. It automates the entire order-to-table lifecycle:
* **Eliminates Order Discrepancies:** Allows customers and cashiers to configure multi-tier pizzas (dough types, crust sizes, and custom ingredient toppings) with instant price calculations.
* **Streamlines Kitchen Execution:** Directly pushes placed orders into an interactive Kitchen Queue with color-coded preparation statuses (`Pending`, `Preparing`, `Ready to Serve`, `Completed`).
* **Automates Inventory Control:** Links menu recipes directly to raw stock quantities (grams, milliliters, slices), alerting staff when ingredients fall below minimum threshold levels.
* **Ensures Security & Accountability:** Implements strict Role-Based Access Control (RBAC) with password hashing and comprehensive audit trails for all sensitive actions.

---

## 2. Main System Modules

1. **Authentication & Role-Based Access Control (RBAC):**
   * Supports three dedicated user roles: `Admin`, `Crew`, and `Customer`.
   * Enforces secure credential validation with SHA-256 password hashing.
   * Features client-side input validation (e.g., standard 11-digit Philippine mobile numbers starting with `09`).
   * Includes an elevated security dialog requiring admin re-authentication before modifying staff permissions.

2. **Customer Ordering & Interactive Customization:**
   * Visual food catalog categorized by Signature Pizzas, Classic Pizzas, Sides, Beverages, and Desserts.
   * Interactive customization dialog allowing customers to select dough styles/sizes (10", 12", 14", 16") and dynamic add-on toppings.
   * Promo voucher engine supporting percentage-based and fixed-amount discounts.

3. **Live Kitchen Order Queue:**
   * Real-time ticket board enabling kitchen crew to view order customizations, update order progress, and notify service staff when orders are ready.
   * 30-day order archiving to keep the active production screen responsive and uncluttered.

4. **Ingredient Stock & Recipe Inventory Management:**
   * Real-time stock tracking with visual status indicators (In Stock, Low Stock, Depleted).
   * Safe batch-editing workflow allowing managers to make multiple stock modifications with **Save Changes** and **Cancel Changes** controls.
   * Recipe mapping associating ingredients with menu items for precise food costing.

5. **Analytics, Sales Reports & Audit Trail:**
   * Financial reporting module summarizing Gross Sales, Net Revenue, Total Orders, and Average Ticket Size across customizable date ranges.
   * System-wide Audit Log recording user logins, order actions, stock restocks, and security role modifications.

---

## 3. Database & Table Design

The database (**`TonyosPizzaDB`**) follows a normalized relational structure (Third Normal Form - 3NF) on Microsoft SQL Server to ensure data integrity, eliminate redundancy, and support high transaction volume:

| Table Name | Primary Key | Key Relationships / Purpose |
| :--- | :--- | :--- |
| **`Users`** | `UserId` | Stores login credentials, hashed passwords, contact details (11-digit phone), and user roles (`Admin`, `Crew`, `Customer`). |
| **`Categories`** | `CategoryId` | Food menu categories (Signature Pizza, Sides, Beverages, etc.). |
| **`IngredientCategories`** | `IngredientCategoryId` | Inventory groupings for raw ingredients (Dough, Cheese, Meat, Veggie, Sauce). |
| **`Ingredients`** | `IngredientId` | Raw inventory tracking: `CurrentStock`, `MinThreshold`, `Unit` (g, ml, pcs), and `CostPerUnit`. |
| **`MenuItems`** | `ItemId` | Base food items linked via `CategoryId` foreign key. Stores descriptions, pricing, and availability. |
| **`MenuItemDoughSizes`** | `SizeId` | Supports up to 4 crust sizes per pizza (Small, Medium, Large, Extra Large) with scaled pricing. |
| **`MenuItemIngredients`** | `(ItemId, IngredientId)` | Composite junction table linking recipes to inventory ingredients with required portion quantities. |
| **`MenuItemAddOns`** | `AddOnId` | Available customer add-ons per item linked to specific ingredients for automatic inventory deduction. |
| **`Orders`** | `OrderId` | Order header table tracking `CustomerId`, `TotalAmount`, `Status`, `CreatedAt`, and `IsArchived`. |
| **`OrderItems`** | `OrderItemId` | Order line item detail linked to `OrderId`, storing selected dough sizes, custom toppings, and subtotal. |
| **`PromoVouchers`** | `PromoId` | Discount codes with expiry dates, minimum spend constraints, and discount percentage/values. |
| **`AuditLogs`** | `LogId` | Tamper-evident operational logging: `Username`, `Action`, `Details`, and server `Timestamp`. |

* **Relational Integrity:** Cascading deletes (`ON DELETE CASCADE`) are utilized on child transaction records (`OrderItems`, `MenuItemIngredients`, `MenuItemDoughSizes`) to prevent orphaned rows, while customer references use `SET NULL` to retain historic order data upon customer account removal.
