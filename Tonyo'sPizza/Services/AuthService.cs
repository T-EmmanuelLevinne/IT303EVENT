using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Services
{
    public class AuthService
    {
        private static AuthService _instance;
        private readonly UserRepository _userRepository;
        private User _currentUser;

        public static AuthService Instance => _instance ?? (_instance = new AuthService());

        public User CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public event Action<User> UserLoggedIn;
        public event Action UserLoggedOut;

        public AuthService()
        {
            _userRepository = new UserRepository();
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool Login(string username, string plainPassword, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword))
            {
                errorMessage = "Username and password cannot be empty.";
                return false;
            }

            try
            {
                string hash = HashPassword(plainPassword);
                User user = _userRepository.Authenticate(username, hash);
                if (user != null)
                {
                    _currentUser = user;
                    UserLoggedIn?.Invoke(_currentUser);
                    return true;
                }
                else
                {
                    errorMessage = "Invalid username or password.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Database error: " + ex.Message;
                return false;
            }
        }

        public bool VerifyCurrentAdminPassword(string plainPassword)
        {
            if (_currentUser == null || string.IsNullOrWhiteSpace(plainPassword)) return false;
            string hash = HashPassword(plainPassword);
            return string.Equals(_currentUser.PasswordHash, hash, StringComparison.OrdinalIgnoreCase);
        }

        public bool Register(string username, string plainPassword, string fullName, string role, string phone, string address, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword))
            {
                errorMessage = "Username and password are required.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!phone.StartsWith("09") || phone.Length != 11 || !phone.All(char.IsDigit))
                {
                    errorMessage = "Phone number must start with '09' and be exactly 11 digits.";
                    return false;
                }
            }

            try
            {
                if (_userRepository.UsernameExists(username))
                {
                    errorMessage = "Username is already taken. Please choose another.";
                    return false;
                }

                User newUser = new User
                {
                    Username = username,
                    PasswordHash = HashPassword(plainPassword),
                    FullName = fullName,
                    Role = role,
                    Phone = phone,
                    Address = address,
                    CreatedAt = DateTime.Now
                };

                bool success = _userRepository.Register(newUser);
                if (success)
                {
                    _currentUser = newUser;
                    UserLoggedIn?.Invoke(_currentUser);
                    return true;
                }
                errorMessage = "Could not register user.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = "Registration error: " + ex.Message;
                return false;
            }
        }

        public void Logout()
        {
            _currentUser = null;
            UserLoggedOut?.Invoke();
        }
    }
}
