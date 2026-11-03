using System;
using System.Collections.Generic;
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
            _originalUsers = _userRepository.GetAllUsers();
            _workingUsers = _originalUsers.Select(u => u.Clone()).ToList();
            UserDataChanged?.Invoke();
        }

        public void UpdateUserRole(int userId, string newRole)
        {
            var user = _workingUsers.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.Role = newRole;
                UserDataChanged?.Invoke();
            }
        }

        public bool DeleteUser(int userId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                bool ok = _userRepository.DeleteUser(userId);
                if (ok)
                {
                    AuditService.Instance.Log("Delete User", $"User ID #{userId} was deleted from system.");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to delete user.";
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
            _workingUsers = _originalUsers.Select(u => u.Clone()).ToList();
            UserDataChanged?.Invoke();
        }

        public bool SaveChanges(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                bool ok = _userRepository.BatchUpdateUserRoles(_workingUsers);
                if (ok)
                {
                    AuditService.Instance.Log("User Management", "Updated user roles / profile information.");
                    ReloadFromDatabase();
                    return true;
                }
                errorMessage = "Failed to save user role changes.";
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
                    work.Address != orig.Address)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
