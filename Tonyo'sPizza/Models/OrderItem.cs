using System;

namespace Tonyo_sPizza.Models
{
    public class OrderItem
    {
        private int _orderItemId;
        private int _orderId;
        private int? _itemId;
        private string _itemName;
        private string _selectedDoughSize;
        private string _selectedAddOns;
        private decimal _unitPrice;
        private int _quantity;
        private decimal _subtotal;
        private string _imageBase64;

        public int OrderItemId
        {
            get => _orderItemId;
            set => _orderItemId = value;
        }

        public int OrderId
        {
            get => _orderId;
            set => _orderId = value;
        }

        public int? ItemId
        {
            get => _itemId;
            set => _itemId = value;
        }

        public string ItemName
        {
            get => _itemName;
            set => _itemName = value?.Trim() ?? string.Empty;
        }

        public string SelectedDoughSize
        {
            get => _selectedDoughSize;
            set => _selectedDoughSize = value;
        }

        public string SelectedAddOns
        {
            get => _selectedAddOns;
            set => _selectedAddOns = value;
        }

        public string DisplayTitle
        {
            get
            {
                string title = _itemName;
                if (!string.IsNullOrEmpty(_selectedDoughSize))
                {
                    title += $" ({_selectedDoughSize})";
                }
                return title;
            }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value;
                RecalculateSubtotal();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be at least 1.");
                _quantity = value;
                RecalculateSubtotal();
            }
        }

        public decimal Subtotal
        {
            get => _subtotal;
            set => _subtotal = value;
        }

        public string ImageBase64
        {
            get => _imageBase64;
            set => _imageBase64 = value;
        }

        private void RecalculateSubtotal()
        {
            _subtotal = _unitPrice * _quantity;
        }

        public OrderItem()
        {
            _quantity = 1;
        }

        public OrderItem(int orderItemId, int orderId, int? itemId, string itemName, decimal unitPrice, int quantity, decimal subtotal, string imageBase64 = null, string doughSize = null, string addOns = null)
        {
            _orderItemId = orderItemId;
            _orderId = orderId;
            _itemId = itemId;
            _itemName = itemName;
            _unitPrice = unitPrice;
            _quantity = quantity;
            _subtotal = subtotal;
            _imageBase64 = imageBase64;
            _selectedDoughSize = doughSize;
            _selectedAddOns = addOns;
        }

        public OrderItem(MenuItem item, int quantity = 1, string doughSize = null, decimal? customPrice = null, string addOns = null)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _itemId = item.ItemId;
            _itemName = item.Name;
            _unitPrice = customPrice ?? item.Price;
            _quantity = quantity;
            _imageBase64 = item.ImageBase64;
            _selectedDoughSize = doughSize;
            _selectedAddOns = addOns;
            RecalculateSubtotal();
        }
    }
}
