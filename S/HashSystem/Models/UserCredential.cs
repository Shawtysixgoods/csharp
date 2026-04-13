using System;

namespace HashSystem.Models
{
    public class UserCredential
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Algorithm { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }

        public UserCredential(string username, string passwordHash,
                              string salt, string algorithm)
        {
            Username     = username;
            PasswordHash = passwordHash;
            Salt         = salt;
            Algorithm    = algorithm;
            CreatedAt    = DateTime.Now;
            FailedAttempts = 0;
            IsLocked     = false;
        }

        public void RegisterFailedAttempt()
        {
            FailedAttempts++;
            if (FailedAttempts > 5)
                IsLocked = true;
        }

        public void ResetFailedAttempts()
        {
            FailedAttempts = 0;
            IsLocked = false;
        }

        public override string ToString()
        {
            return $"[{Username}] алг: {Algorithm}, " +
                   $"заблокирован: {IsLocked}, " +
                   $"попыток: {FailedAttempts}";
        }
    }
}