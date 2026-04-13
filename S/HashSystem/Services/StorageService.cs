using System;
using System.Collections.Generic;
using System.IO;
using HashSystem.Models;

namespace HashSystem.Services
{
    public class StorageService
    {
        public void SaveCredentials(string filePath,
                                    List<UserCredential> users)
        {
            var lines = new List<string>();

            foreach (var user in users)
            {
                lines.Add($"{user.Username}|{user.PasswordHash}|" +
                          $"{user.Salt}|{user.Algorithm}|" +
                          $"{user.CreatedAt}|{user.FailedAttempts}|{user.IsLocked}");
            }

            File.WriteAllLines(filePath, lines);
        }

        public List<UserCredential> LoadCredentials(string filePath)
        {
            var users = new List<UserCredential>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split('|');

                var user = new UserCredential(
                    parts[0],
                    parts[1],
                    parts[2],
                    parts[3]
                );

                user.FailedAttempts = int.Parse(parts[5]);
                user.IsLocked       = bool.Parse(parts[6]);

                users.Add(user);
            }

            return users;
        }

        public void SaveFileRecords(string filePath,
                                    List<FileRecord> records)
        {
            var lines = new List<string>();

            foreach (var record in records)
            {
                lines.Add($"{record.FilePath}|{record.OriginalHash}|" +
                          $"{record.Algorithm}|{record.FileSize}|" +
                          $"{record.RegisteredAt}");
            }

            File.WriteAllLines(filePath, lines);
        }

        public List<FileRecord> LoadFileRecords(string filePath)
        {
            var records = new List<FileRecord>();
            var lines   = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts  = line.Split('|');
                var record = new FileRecord(
                    parts[0],
                    parts[1],
                    parts[2],
                    long.Parse(parts[3])
                );

                records.Add(record);
            }

            return records;
        }

        public void SaveLogs(string filePath, List<HashLog> logs)
        {
            var lines = new List<string>();

            foreach (var log in logs)
            {
                lines.Add($"{log.Id}|{log.Operation}|{log.Algorithm}|" +
                          $"{log.Success}|{log.Timestamp}|" +
                          $"{log.ResultHash}|{log.ErrorMessage}");
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}