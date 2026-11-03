using System;
using System.Collections.Generic;

namespace Tonyo_sPizza.Models
{
    public class Order
    {
        private int _orderId;
        private int? _customerId;
        private string _customerName;
        private string _phone;
        private string _address;
        private decimal _totalAmount;
        private string _status; // "Pending", "Preparing", "Ready to Serve", "Completed", "Cancelled"
        private string _notes;
        private DateTime _createdAt;
        private List<OrderItem> _items;

        public int OrderId
        {
            get => _orderId;
            set => _orderId = value;
        }

        public int? CustomerId
        {
            get => _customerId;
            set => _customerId = value;
        }

        public string CustomerName
        {
            get => _customerName ?? string.Empty;
            set => _customerName = value?.Trim() ?? string.Empty;
        }

        public string Phone
        {
            get => _phone ?? string.Empty;
            set => _phone = value?.Trim() ?? string.Empty;
        }

        public string Address
        {
            get => _address ?? string.Empty;
            set => _address = value?.Trim() ?? string.Empty;
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set => _totalAmount = Math.Max(0, value);
        }

        public string Status
        {
            get => _status ?? "Pending";
            set => _status = value;
        }

        public string Notes
        {
            get => _notes ?? string.Empty;
            set => _notes = value?.Trim() ?? string.Empty;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public List<OrderItem> Items
        {
            get
            {
                if (_items == null)
                    _items = new List<OrderItem>();
                return _items;
            }
            set => _items = value;
        }

        public bool IsArchived { get; set; } = false;
        public DateTime? ArchivedAt { get; set; } = null;
        public bool IsCustomerArchived { get; set; } = false;
        public DateTime? CustomerArchivedAt { get; set; } = null;
        public bool IsCustomerDeleted { get; set; } = false;
        public DateTime? CustomerDeletedAt { get; set; } = null;
        public bool IsStockDeducted { get; set; } = false;

        public Order()
        {
            _status = "Pending";
            _createdAt = DateTime.Now;
            _items = new List<OrderItem>();
        }

        public Order(int orderId, int? customerId, string customerName, string phone, string address, decimal totalAmount, string status, string notes, DateTime createdAt)
        {
            _orderId = orderId;
            _customerId = customerId;
            CustomerName = customerName;
            Phone = phone;
            Address = address;
            TotalAmount = totalAmount;
            Status = status;
            Notes = notes;
            CreatedAt = createdAt;
            _items = new List<OrderItem>();
        }

        public void CalculateTotal(decimal serviceFee = 2.00m)
        {
            decimal sum = 0;
            if (_items != null)
            {
                foreach (var itm in _items)
                {
                    sum += itm.Subtotal;
                }
            }
            _totalAmount = sum + (sum > 0 ? serviceFee : 0);
        }
    }
}
