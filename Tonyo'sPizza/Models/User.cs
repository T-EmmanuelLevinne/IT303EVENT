using System;

namespace Tonyo_sPizza.Models
{
    public class User
    {
        private int _userId;
        private string _username;
        private string _passwordHash;
        private string _fullName;
        private string _role; // "Admin", "Crew", "Customer"
        private string _phone;
        private string _address;
        private DateTime _createdAt;

        public int UserId
        {
            get => _userId;
            set => _userId = value;
        }

        public string Username
        {
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username cannot be empty.");
                _username = value.Trim();
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value;
        }

        public string FullName
        {
            get => _fullName;
            set => _fullName = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }

        public string Role
        {
            get => _role;
            set
            {
                if (value != "Admin" && value != "Crew" && value != "Customer")
                    throw new ArgumentException("Role must be Admin, Crew, or Customer.");
                _role = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim() ?? string.Empty;
        }

        public string Address
        {
            get => _address;
            set => _address = value?.Trim() ?? string.Empty;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public bool IsAdmin => _role == "Admin";
        public bool IsCrew => _role == "Crew";
        public bool IsCustomer => _role == "Customer";

        public User()
        {
            _createdAt = DateTime.Now;
            _role = "Customer";
        }

        public User(int userId, string username, string passwordHash, string fullName, string role, string phone, string address, DateTime createdAt)
        {
            _userId = userId;
            Username = username;
            _passwordHash = passwordHash;
            FullName = fullName;
            Role = role;
            Phone = phone;
            Address = address;
            _createdAt = createdAt;
        }

        public User Clone()
        {
            return new User(_userId, _username, _passwordHash, _fullName, _role, _phone, _address, _createdAt);
        }
    }
}
