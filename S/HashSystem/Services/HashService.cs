using System;
using System.Security.Cryptography;
using System.Text;
using HashSystem.Models;

namespace HashSystem.Services
{
    public class HashService
    {
        private List<HashLog> _logs;

        public string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash  = sha.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public string ComputeSha512(string input)
        {
            using var sha = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash  = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public string ComputeMd5(string input)
        {
            using var md5  = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash  = md5.ComputeHash(bytes);
            return BitConverter.ToString(hash);
        }

        public string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes     = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            var hash = hmac.ComputeHash(messageBytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public bool VerifyHash(string input, string expectedHash, string algorithm)
        {
            string actualHash = algorithm switch
            {
                "SHA256" => ComputeSha256(input),
                "SHA512" => ComputeSha512(input),
                "MD5"    => ComputeMd5(input),
                _        => throw new NotSupportedException($"Алгоритм {algorithm} не поддерживается")
            };

            return actualHash == expectedHash;
        }

        public string GenerateSalt(int length = 16)
        {
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public string HashWithSalt(string input, string salt, string algorithm)
        {
            string combined = salt + input;

            return algorithm switch
            {
                "SHA256" => ComputeSha256(combined),
                "SHA512" => ComputeSha512(combined),
                "MD5"    => ComputeMd5(combined),
                _        => throw new NotSupportedException()
            };
        }

        public bool VerifyWithSalt(string input, string salt,
                                   string expectedHash, string algorithm)
        {
            string actualHash = HashWithSalt(input, salt, algorithm);
            return actualHash == expectedHash;
        }

        public void LogOperation(string id, string operation,
                                 string algorithm, string preview,
                                 string resultHash, bool success,
                                 string error = null)
        {
            var log = new HashLog(id, operation, algorithm, preview)
            {
                ResultHash   = resultHash,
                Success      = success,
                ErrorMessage = error
            };

            _logs.Add(log);
        }

        public List<HashLog> GetLogs()
        {
            return _logs;
        }
    }
}