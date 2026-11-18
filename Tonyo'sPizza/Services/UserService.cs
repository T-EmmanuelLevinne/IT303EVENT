using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tonyo_sPizza.Data.Repositories;
using Tonyo_sPizza.Models;

namespace Tonyo_sPizza.Services
{
    public class UserService
    {
        private static UserService _instance;
        private readonly UserRepository _userRepository;
        private List<User> _originalUsers;
        private List<User> _workingUsers;

        public static UserService Instance => _instance ?? (_instance = new UserService());

        public List<User> WorkingUsers => _workingUsers;
        public bool HasPendingChanges => CheckPendingChanges();

        public event Action UserDataChanged;

        public UserService()
        {
            _userRepository = new UserRepository();
            ReloadFromDatabase();
        }

        public void ReloadFromDatabase()
        {
            try
            {
                _originalUsers = _userRepository.GetAllUsers();
                _workingUsers = _originalUsers.Select(u => u.Clone()).ToList();
                UserDataChanged?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.ReloadFromDatabase");
                _originalUsers = new List<User>();
                _workingUsers = new List<User>();
            }
        }

        public void UpdateUserRole(int userId, string newRole)
        {
            AuthService.Instance.RequireRole("Admin");
            var user = _workingUsers.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.Role = newRole;
                UserDataChanged?.Invoke();
            }
        }

        public bool CreateUser(string username, string plainPassword, string fullName, string role, string phone, string address, bool isActive, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword) || string.IsNullOrWhiteSpace(fullName))
                {
                    errorMessage = "Username, password, and full name are required.";
                    return false;
                }

                if (plainPassword.Length < 4)
                {
                    errorMessage = "Password must be at least 4 characters long.";
                    return false;
                }

                if (_userRepository.UsernameExists(username))
                {
                    errorMessage = $"Username '{username}' is already in use.";
                    return false;
                }

                string salt = AuthService.Instance.GenerateSalt();
                var user = new User
                {
                    Username = username.Trim(),
                    Salt = salt,
                    PasswordHash = AuthService.Instance.HashPasswordWithSalt(plainPassword, salt),
                    FullName = fullName.Trim(),
                    Role = role,
                    Phone = phone,
                    Address = address,
                    IsActive = isActive,
                    CreatedAt = DateTime.Now
                };

                if (_userRepository.Register(user))
                {
                    AuditService.Instance.Log("Add User", $"Admin created {user.Role} user '{user.Username}'.", "Users", user.UserId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Could not save user account.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.CreateUser");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool UpdateUserProfile(User user, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                if (user == null)
                {
                    errorMessage = "User cannot be null.";
                    return false;
                }

                bool ok = _userRepository.UpdateUser(user);
                if (ok)
                {
                    AuditService.Instance.Log("Edit User", $"Updated user '{user.Username}' profile (Role: {user.Role}, Active: {user.IsActive}).", "Users", user.UserId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to update user.";
                return false;
            }
            catch (DBConcurrencyException cex)
            {
                errorMessage = cex.Message;
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.UpdateUserProfile");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool ResetUserPassword(int userId, string newPlainPassword, bool alsoUnlock, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");

                if (string.IsNullOrWhiteSpace(newPlainPassword) || newPlainPassword.Length < 4)
                {
                    errorMessage = "New password must be at least 4 characters long.";
                    return false;
                }

                var target = _userRepository.GetUserById(userId);
                if (target == null)
                {
                    errorMessage = "User account was not found.";
                    return false;
                }

                string salt = AuthService.Instance.GenerateSalt();
                string hash = AuthService.Instance.HashPasswordWithSalt(newPlainPassword, salt);

                if (_userRepository.UpdatePasswordHashAndSalt(userId, hash, salt))
                {
                    AuditService.Instance.Log("Reset Password", $"Admin reset password for user '{target.Username}'{(alsoUnlock ? " and cleared account lockout" : "")}.", "Users", userId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Could not reset password.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.ResetUserPassword");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool SetUserActiveStatus(int userId, bool isActive, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                if (AuthService.Instance.CurrentUser?.UserId == userId && !isActive)
                {
                    errorMessage = "You cannot deactivate your own active administrator account.";
                    return false;
                }

                var target = _userRepository.GetUserById(userId);
                if (target == null)
                {
                    errorMessage = "User not found.";
                    return false;
                }

                if (_userRepository.SetUserActiveStatus(userId, isActive))
                {
                    AuditService.Instance.Log(isActive ? "Reactivate User" : "Deactivate User", $"User '{target.Username}' set to {(isActive ? "Active" : "Inactive")}.", "Users", userId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Could not update account status.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.SetUserActiveStatus");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool UnlockUser(int userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                var target = _userRepository.GetUserById(userId);
                if (target == null)
                {
                    errorMessage = "User not found.";
                    return false;
                }

                _userRepository.ResetLoginLockout(userId);
                AuditService.Instance.Log("Unlock User", $"Admin unlocked account '{target.Username}'.", "Users", userId.ToString());
                ReloadFromDatabase();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.UnlockUser");
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool HasUserTransactions(int userId) => _userRepository.HasUserTransactions(userId);

        public bool DeleteUser(int userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");

                if (AuthService.Instance.CurrentUser?.UserId == userId)
                {
                    errorMessage = "You cannot delete the currently signed-in account.";
                    return false;
                }

                if (_userRepository.HasUserTransactions(userId))
                {
                    errorMessage = "Cannot permanently delete this user because they have existing order history.\nPlease use 'Deactivate' instead to preserve historical order records.";
                    return false;
                }

                bool ok = _userRepository.DeleteUser(userId);
                if (ok)
                {
                    AuditService.Instance.Log("Delete User", $"User ID #{userId} permanently removed from system.", "Users", userId.ToString());
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to delete user.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.DeleteUser");
                errorMessage = ex.Message;
                return false;
            }
        }

        public void CancelChanges()
        {
            _workingUsers = _originalUsers.Select(u => u.Clone()).ToList();
            UserDataChanged?.Invoke();
        }

        public bool SaveChanges(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                AuthService.Instance.RequireRole("Admin");
                bool ok = _userRepository.BatchUpdateUserRoles(_workingUsers);
                if (ok)
                {
                    AuditService.Instance.Log("User Management", "Saved batch user role and account updates.", "Users");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to save user role changes.";
                return false;
            }
            catch (DBConcurrencyException cex)
            {
                errorMessage = cex.Message;
                return false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserService.SaveChanges");
                errorMessage = ex.Message;
                return false;
            }
        }

        private bool CheckPendingChanges()
        {
            if (_originalUsers == null || _workingUsers == null) return false;
            if (_originalUsers.Count != _workingUsers.Count) return true;

            for (int i = 0; i < _originalUsers.Count; i++)
            {
                var orig = _originalUsers[i];
                var work = _workingUsers.FirstOrDefault(w => w.UserId == orig.UserId);
                if (work == null) return true;
                if (work.Role != orig.Role ||
                    work.FullName != orig.FullName ||
                    work.Phone != orig.Phone ||
                    work.Address != orig.Address ||
                    work.IsActive != orig.IsActive)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
