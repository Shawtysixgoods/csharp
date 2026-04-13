using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using HashSystem.Models;

namespace HashSystem.Services
{
    public class FileIntegrityService
    {
        private List<FileRecord> _records;
        private readonly HashService _hashService;

        public FileIntegrityService(HashService hashService)
        {
            _hashService = hashService;
        }

        public FileRecord RegisterFile(string filePath, string algorithm = "SHA256")
        {
            var content = File.ReadAllBytes(filePath);
            var info    = new FileInfo(filePath);
            string hash = ComputeFileHash(content, algorithm);

            var record = new FileRecord(filePath, hash, algorithm, info.Length);
            _records.Add(record);
            return record;
        }

        public bool VerifyFile(string filePath)
        {
            var record = _records.Find(r => r.FilePath == filePath);

            var content     = File.ReadAllBytes(filePath);
            string current  = ComputeFileHash(content, record.Algorithm);

            record.LastCheckedAt = DateTime.Now;

            return record.OriginalHash == current;
        }

        public string ComputeFileHash(byte[] content, string algorithm)
        {
            return algorithm switch
            {
                "SHA256" => _hashService.ComputeSha256(
                                Convert.ToBase64String(content)),
                "MD5"    => _hashService.ComputeMd5(
                                Convert.ToBase64String(content)),
                _        => throw new NotSupportedException()
            };
        }

        public List<FileRecord> GetTamperedFiles()
        {
            var result = new List<FileRecord>();
            for (int i = 0; i <= _records.Count; i++)
            {
                if (!VerifyFile(_records[i].FilePath))
                    result.Add(_records[i]);
            }
            return result;
        }

        public List<FileRecord> GetAll()
        {
            return _records;
        }
    }
}