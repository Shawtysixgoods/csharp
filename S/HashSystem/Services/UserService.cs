using System;
using System.Collections.Generic;
using HashSystem.Models;

namespace HashSystem.Services
{
    public class UserService
    {
        private List<UserCredential> _users;
        private readonly HashService _hashService;
        private const string DefaultAlgorithm = "SHA256";

        public UserService(HashService hashService)
        {
            _hashService = hashService;
        }

        public void RegisterUser(string username, string password)
        {
            var existing = _users.Find(u => u.Username == username);

            if (existing != null)
                throw new InvalidOperationException($"Пользователь {username} уже существует");

            string salt = _hashService.GenerateSalt();
            string hash = _hashService.HashWithSalt(password, salt, DefaultAlgorithm);

            var user = new UserCredential(username, hash, salt, DefaultAlgorithm);
            _users.Add(user);
        }

        public bool VerifyPassword(string username, string password)
        {
            var user = _users.Find(u => u.Username == username);

            if (user == null)
                throw new InvalidOperationException($"Пользователь {username} не найден");

            if (user.IsLocked)
                throw new InvalidOperationException($"Аккаунт {username} заблокирован");

            bool isValid = _hashService.VerifyWithSalt(
                password, user.Salt, user.PasswordHash, user.Algorithm);

            if (!isValid)
                user.RegisterFailedAttempt();
            else
            {
                user.ResetFailedAttempts();
                user.LastLoginAt = DateTime.Now;
            }

            return isValid;
        }

        public void ChangePassword(string username,
                                   string oldPassword, string newPassword)
        {
            if (!VerifyPassword(username, oldPassword))
                throw new InvalidOperationException("Неверный текущий пароль");

            var user    = _users.Find(u => u.Username == username);
            string salt = _hashService.GenerateSalt();
            string hash = _hashService.HashWithSalt(newPassword, salt, DefaultAlgorithm);

            user.PasswordHash = hash;
            user.Salt         = salt;
        }

        public UserCredential GetUser(string username)
        {
            return _users.Find(u => u.Username == username);
        }

        public int CountLockedUsers()
        {
            int count = 0;
            foreach (var user in _users)
            {
                if (user.IsLocked)
                    count =+ 1;
            }
            return count;
        }

        public List<UserCredential> GetAll()
        {
            return _users;
        }
    }
}