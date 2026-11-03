using System;
using System.Collections.Generic;
using System.Linq;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Services
{
    public class OrderService
    {
        private static OrderService _instance;
        private readonly OrderRepository _orderRepository;
        private readonly PromoVoucherRepository _promoRepository;
        private readonly List<OrderItem> _cart;
        private decimal _serviceFee = 2.00m;
        private decimal _discountAmount = 0m;

        public static OrderService Instance => _instance ?? (_instance = new OrderService());

        public List<OrderItem> Cart => _cart;
        public decimal ServiceFee => _cart.Count > 0 ? _serviceFee : 0m;
        public decimal DiscountAmount => _discountAmount;

        public decimal Subtotal => _cart.Sum(i => i.Subtotal);
        public decimal Total => Math.Max(0, Subtotal + ServiceFee - DiscountAmount);

        public event Action CartUpdated;

        public void NotifyCartUpdated()
        {
            CartUpdated?.Invoke();
        }

        public OrderService()
        {
            _orderRepository = new OrderRepository();
            _promoRepository = new PromoVoucherRepository();
            _cart = new List<OrderItem>();
        }

        public void AddToCart(MenuItem item, int quantity = 1)
        {
            if (item == null) return;
            var existing = _cart.FirstOrDefault(i => i.ItemId == item.ItemId && string.IsNullOrEmpty(i.SelectedDoughSize) && string.IsNullOrEmpty(i.SelectedAddOns));
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _cart.Add(new OrderItem(item, quantity));
            }
            CartUpdated?.Invoke();
        }

        public void AddToCart(OrderItem orderItem)
        {
            if (orderItem == null) return;
            var existing = _cart.FirstOrDefault(i => i.ItemId == orderItem.ItemId && i.SelectedDoughSize == orderItem.SelectedDoughSize && i.SelectedAddOns == orderItem.SelectedAddOns);
            if (existing != null)
            {
                existing.Quantity += orderItem.Quantity;
            }
            else
            {
                _cart.Add(orderItem);
            }
            CartUpdated?.Invoke();
        }

        public void UpdateQuantity(OrderItem item, int newQuantity)
        {
            if (item == null) return;
            if (newQuantity <= 0)
            {
                _cart.Remove(item);
            }
            else
            {
                item.Quantity = newQuantity;
            }
            CartUpdated?.Invoke();
        }

        public void RemoveFromCart(OrderItem item)
        {
            if (item != null)
            {
                _cart.Remove(item);
                CartUpdated?.Invoke();
            }
        }

        public void ClearCart()
        {
            _cart.Clear();
            _discountAmount = 0m;
            CartUpdated?.Invoke();
        }

        public bool ApplyPromoCode(string promoCode, out string message)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
            {
                message = "Please enter a valid promo code.";
                return false;
            }

            if (Subtotal <= 0)
            {
                message = "Your cart is empty. Add delicious pizza first!";
                return false;
            }

            string code = promoCode.Trim().ToUpper();

            try
            {
                var voucher = _promoRepository.GetVoucherByCode(code);
                if (voucher != null)
                {
                    if (voucher.MinOrderAmount > 0 && Subtotal < voucher.MinOrderAmount)
                    {
                        message = $"This voucher requires a minimum order of ₱{voucher.MinOrderAmount:N2}.";
                        return false;
                    }

                    if (voucher.DiscountType == "Percentage")
                    {
                        decimal disc = Math.Round(Subtotal * (voucher.DiscountValue / 100m), 2);
                        if (voucher.MaxDiscountAmount.HasValue && disc > voucher.MaxDiscountAmount.Value)
                        {
                            disc = voucher.MaxDiscountAmount.Value;
                        }
                        _discountAmount = disc;
                        message = $"{voucher.DisplayDiscount} voucher applied! (Saved ₱{_discountAmount:N2})";
                    }
                    else
                    {
                        _discountAmount = Math.Min(Subtotal, voucher.DiscountValue);
                        message = $"₱{_discountAmount:N2} voucher discount applied!";
                    }

                    CartUpdated?.Invoke();
                    return true;
                }
            }
            catch
            {
                // Fallback to defaults
            }

            if (code == "TONYO25")
            {
                _discountAmount = Math.Round(Subtotal * 0.25m, 2);
                message = $"25% OFF applied! (Saved ₱{_discountAmount:N2})";
                CartUpdated?.Invoke();
                return true;
            }
            else if (code == "FREEPIZZA")
            {
                _discountAmount = 5.00m;
                message = "₱5.00 discount applied!";
                CartUpdated?.Invoke();
                return true;
            }

            message = "Invalid or expired promo code.";
            return false;
        }

        public bool PlaceOrder(User customer, string address, string phone, string notes, out string errorMessage)
        {
            return Checkout(customer, phone, address, notes, out errorMessage);
        }

        public bool Checkout(User customer, string phone, string address, string notes, out string errorMessage, List<OrderItem> selectedItems = null)
        {
            errorMessage = string.Empty;
            var itemsToOrder = selectedItems ?? _cart;
            if (itemsToOrder.Count == 0)
            {
                errorMessage = "Please select at least one item to checkout.";
                return false;
            }

            try
            {
                decimal itemsSubtotal = itemsToOrder.Sum(i => i.Subtotal);
                decimal fee = itemsToOrder.Count > 0 ? _serviceFee : 0m;
                decimal orderTotal = Math.Max(0, itemsSubtotal + fee - _discountAmount);

                Order order = new Order
                {
                    CustomerId = customer?.UserId,
                    CustomerName = customer != null ? customer.FullName : "Walk-in Guest",
                    Phone = phone,
                    Address = address,
                    TotalAmount = orderTotal,
                    Status = "Pending",
                    Notes = notes,
                    CreatedAt = DateTime.Now,
                    Items = new List<OrderItem>(itemsToOrder)
                };

                bool ok = _orderRepository.CreateOrder(order);
                if (ok)
                {
                    AuditService.Instance.Log("Create Order", $"Order #{order.OrderId} placed for {order.CustomerName} - Total: ₱{order.TotalAmount:N2}");
                    // Remove only the items that were ordered
                    foreach (var itm in itemsToOrder.ToList())
                    {
                        _cart.Remove(itm);
                    }
                    _discountAmount = 0m;
                    CartUpdated?.Invoke();
                    return true;
                }
                errorMessage = "Failed to save order.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool CancelOrder(int orderId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.CancelOrder(orderId);
                if (ok)
                {
                    AuditService.Instance.Log("Cancel Order", $"Order #{orderId} was cancelled.");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool ArchiveOrder(int orderId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.ArchiveOrder(orderId);
                if (ok)
                {
                    AuditService.Instance.Log("Archive Order", $"Order #{orderId} moved to 30-day archive.");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool ArchiveOrders(List<int> orderIds, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.ArchiveOrders(orderIds);
                if (ok)
                {
                    AuditService.Instance.Log("Batch Archive Orders", $"Archived {orderIds.Count} order(s): {string.Join(", ", orderIds)}");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool CancelOrders(List<int> orderIds, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.CancelOrders(orderIds);
                if (ok)
                {
                    AuditService.Instance.Log("Batch Cancel Orders", $"Cancelled {orderIds.Count} order(s): {string.Join(", ", orderIds)}");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool ArchiveOrderForCustomer(int orderId, int customerId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.ArchiveOrderForCustomer(orderId, customerId);
                if (ok)
                {
                    AuditService.Instance.Log("Customer Archive Order", $"Customer #{customerId} moved Order #{orderId} to personal archive.");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool UnarchiveOrderForCustomer(int orderId, int customerId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.UnarchiveOrderForCustomer(orderId, customerId);
                if (ok)
                {
                    AuditService.Instance.Log("Customer Unarchive Order", $"Customer #{customerId} restored Order #{orderId} to active orders.");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool ArchiveOrdersForCustomer(List<int> orderIds, int customerId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.ArchiveOrdersForCustomer(orderIds, customerId);
                if (ok)
                {
                    AuditService.Instance.Log("Customer Batch Archive", $"Customer #{customerId} moved {orderIds.Count} order(s) to personal archive: {string.Join(", ", orderIds)}");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool UnarchiveOrdersForCustomer(List<int> orderIds, int customerId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.UnarchiveOrdersForCustomer(orderIds, customerId);
                if (ok)
                {
                    AuditService.Instance.Log("Customer Batch Restore", $"Customer #{customerId} restored {orderIds.Count} order(s): {string.Join(", ", orderIds)}");
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool DeleteOrderForCustomer(int orderId, int customerId, out string error)
        {
            error = string.Empty;
            try
            {
                bool ok = _orderRepository.DeleteOrderForCustomer(orderId, customerId);
                if (ok)
                {
                    AuditService.Instance.Log("Customer Delete Order", $"Customer #{customerId} deleted Order #{orderId} from order history (sales report preserved).");
                }
                else
                {
                    error = "Only completed or cancelled orders can be deleted.";
                }
                return ok;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool DeleteOrdersForCustomer(List<int> orderIds, int customerId, out int deletedCount, out string error)
        {
            error = string.Empty;
            deletedCount = 0;
            try
            {
                deletedCount = _orderRepository.DeleteOrdersForCustomer(orderIds, customerId);
                if (deletedCount > 0)
                {
                    AuditService.Instance.Log("Customer Batch Delete", $"Customer #{customerId} deleted {deletedCount} order(s) from order history (sales report preserved).");
                }
                return deletedCount > 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool DeleteCancelledOrdersForCustomer(List<int> orderIds, int customerId, out int deletedCount, out string error)
        {
            return DeleteOrdersForCustomer(orderIds, customerId, out deletedCount, out error);
        }
    }
}
