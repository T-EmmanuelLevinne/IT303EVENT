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
        public const int MaxFailedAttempts = 5;
        public const int LockoutMinutes = 15;

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

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
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

        public string HashPasswordWithSalt(string password, string salt)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            if (string.IsNullOrEmpty(salt)) return HashPassword(password);

            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes($"{salt}:{password}"));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void RequireRole(params string[] allowedRoles)
        {
            if (_currentUser == null)
            {
                throw new UnauthorizedAccessException("Please sign in to perform this action.");
            }
            if (allowedRoles != null && allowedRoles.Length > 0 && !allowedRoles.Any(r => string.Equals(r, _currentUser.Role, StringComparison.OrdinalIgnoreCase)))
            {
                AuditService.Instance.Log("Unauthorized Access", $"User '{_currentUser.Username}' ({_currentUser.Role}) attempted restricted action requiring [{string.Join(", ", allowedRoles)}].", "Users", _currentUser.UserId.ToString());
                throw new UnauthorizedAccessException("You do not have permission to perform this action.");
            }
        }

        public bool Login(string username, string plainPassword, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword))
            {
                errorMessage = "Please enter both your username and password.";
                return false;
            }

            try
            {
                string trimmedUser = username.Trim();
                User user = _userRepository.GetUserByUsername(trimmedUser);

                if (user == null)
                {
                    AuditService.Instance.LogForUser(trimmedUser, null, "Failed Login", $"Failed login attempt for unrecognized username '{trimmedUser}'.", "Users");
                    errorMessage = "Invalid username or password.";
                    return false;
                }

                if (!user.IsActive)
                {
                    AuditService.Instance.LogForUser(user.Username, user.UserId, "Failed Login", $"Blocked sign-in for deactivated account '{user.Username}'.", "Users", user.UserId.ToString());
                    errorMessage = "This account has been deactivated. Please contact an administrator.";
                    return false;
                }

                // Check Lockout Status
                if (user.IsLocked)
                {
                    if (user.LockoutUntil.HasValue && user.LockoutUntil.Value > DateTime.Now)
                    {
                        int minsLeft = Math.Max(1, (int)Math.Ceiling((user.LockoutUntil.Value - DateTime.Now).TotalMinutes));
                        AuditService.Instance.LogForUser(user.Username, user.UserId, "Failed Login", $"Sign-in rejected: account '{user.Username}' is locked ({minsLeft} min remaining).", "Users", user.UserId.ToString());
                        errorMessage = $"Account is temporarily locked due to too many failed login attempts. Try again in {minsLeft} minute(s) or ask an administrator to unlock it.";
                        return false;
                    }
                    else if (!user.LockoutUntil.HasValue)
                    {
                        AuditService.Instance.LogForUser(user.Username, user.UserId, "Failed Login", $"Sign-in rejected: account '{user.Username}' is locked by administrator.", "Users", user.UserId.ToString());
                        errorMessage = "This account is locked due to consecutive failed login attempts. Please ask an administrator to unlock it.";
                        return false;
                    }
                    else
                    {
                        // Lockout window elapsed cleanly; reset counters before verifying password
                        _userRepository.ResetLoginLockout(user.UserId);
                        user.FailedLoginAttempts = 0;
                        user.IsLocked = false;
                        user.LockoutUntil = null;
                    }
                }

                // Verify Password (salted SHA-256 preferred; transparent upgrade for seeded legacy unsalted hashes)
                bool isValid;
                bool needsSaltUpgrade = false;

                if (!string.IsNullOrEmpty(user.Salt))
                {
                    string expectedHash = HashPasswordWithSalt(plainPassword, user.Salt);
                    isValid = string.Equals(user.PasswordHash, expectedHash, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    string legacyHash = HashPassword(plainPassword);
                    isValid = string.Equals(user.PasswordHash, legacyHash, StringComparison.OrdinalIgnoreCase);
                    needsSaltUpgrade = isValid;
                }

                if (isValid)
                {
                    if (needsSaltUpgrade)
                    {
                        string newSalt = GenerateSalt();
                        string saltedHash = HashPasswordWithSalt(plainPassword, newSalt);
                        _userRepository.UpdatePasswordHashAndSalt(user.UserId, saltedHash, newSalt);
                        user.Salt = newSalt;
                        user.PasswordHash = saltedHash;
                    }
                    else if (user.FailedLoginAttempts > 0 || user.IsLocked)
                    {
                        _userRepository.ResetLoginLockout(user.UserId);
                    }

                    user.FailedLoginAttempts = 0;
                    user.IsLocked = false;
                    user.LockoutUntil = null;

                    _currentUser = user;
                    AuditService.Instance.LogForUser(user.Username, user.UserId, "Login", $"Successful login as {user.Role}.", "Users", user.UserId.ToString());
                    UserLoggedIn?.Invoke(_currentUser);
                    return true;
                }
                else
                {
                    int nextAttempts = user.FailedLoginAttempts + 1;
                    bool shouldLock = nextAttempts >= MaxFailedAttempts;
                    DateTime? lockUntil = shouldLock ? (DateTime?)DateTime.Now.AddMinutes(LockoutMinutes) : null;

                    _userRepository.RecordFailedLoginAttempt(user.UserId, nextAttempts, shouldLock, lockUntil);

                    if (shouldLock)
                    {
                        AuditService.Instance.LogForUser(user.Username, user.UserId, "Failed Login", $"Account '{user.Username}' locked for {LockoutMinutes} minutes after {nextAttempts} consecutive failed attempts.", "Users", user.UserId.ToString());
                        errorMessage = $"Too many failed login attempts ({MaxFailedAttempts}). Your account has been locked for {LockoutMinutes} minutes.";
                    }
                    else
                    {
                        int remaining = MaxFailedAttempts - nextAttempts;
                        AuditService.Instance.LogForUser(user.Username, user.UserId, "Failed Login", $"Incorrect password for '{user.Username}' (Attempt {nextAttempts} of {MaxFailedAttempts}).", "Users", user.UserId.ToString());
                        errorMessage = $"Invalid username or password. {remaining} attempt(s) remaining before lockout.";
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuthService.Login");
                errorMessage = "Unable to connect to the database. Please verify SQL Server settings and try again.";
                return false;
            }
        }

        public bool VerifyCurrentAdminPassword(string plainPassword)
        {
            if (_currentUser == null || string.IsNullOrWhiteSpace(plainPassword)) return false;
            string hash = !string.IsNullOrEmpty(_currentUser.Salt)
                ? HashPasswordWithSalt(plainPassword, _currentUser.Salt)
                : HashPassword(plainPassword);
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

            if (plainPassword.Length < 4)
            {
                errorMessage = "Password must be at least 4 characters long.";
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

                string salt = GenerateSalt();
                User newUser = new User
                {
                    Username = username,
                    Salt = salt,
                    PasswordHash = HashPasswordWithSalt(plainPassword, salt),
                    FullName = fullName,
                    Role = role,
                    Phone = phone,
                    Address = address,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                bool success = _userRepository.Register(newUser);
                if (success)
                {
                    _currentUser = newUser;
                    AuditService.Instance.LogForUser(newUser.Username, newUser.UserId, "Register User", $"New {newUser.Role} account registered: '{newUser.Username}'.", "Users", newUser.UserId.ToString());
                    UserLoggedIn?.Invoke(_currentUser);
                    return true;
                }
                errorMessage = "Could not register user.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuthService.Register");
                errorMessage = "Registration could not be completed due to a system error. Please check your input or database connection.";
                return false;
            }
        }

        public bool LoginAsGuest(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                User dbGuest = _userRepository.GetUserByUsername("guest");
                if (dbGuest != null)
                {
                    if (!dbGuest.IsActive)
                    {
                        errorMessage = "Guest kiosk ordering is currently disabled by administrator.";
                        return false;
                    }
                    dbGuest.IsGuest = true;
                    _currentUser = dbGuest;
                }
                else
                {
                    _currentUser = new User
                    {
                        UserId = 0,
                        Username = "guest",
                        PasswordHash = string.Empty,
                        FullName = "Walk-in Guest",
                        Role = "Customer",
                        IsActive = true,
                        IsGuest = true,
                        CreatedAt = DateTime.Now
                    };
                }

                AuditService.Instance.LogForUser("guest", _currentUser.UserId > 0 ? (int?)_currentUser.UserId : null, "Guest Login", "Walk-in guest started self-service Order Kiosk session.", "Users", _currentUser.UserId > 0 ? _currentUser.UserId.ToString() : null);
                UserLoggedIn?.Invoke(_currentUser);
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AuthService.LoginAsGuest");
                errorMessage = "Unable to start Guest Kiosk session right now. Please check database settings.";
                return false;
            }
        }

        public void Logout()
        {
            if (_currentUser != null)
            {
                int? uid = _currentUser.UserId > 0 ? (int?)_currentUser.UserId : null;
                AuditService.Instance.LogForUser(_currentUser.Username, uid, "Logout", $"User '{_currentUser.Username}' signed out.", "Users", uid?.ToString());
            }
            _currentUser = null;
            UserLoggedOut?.Invoke();
        }
    }
}
