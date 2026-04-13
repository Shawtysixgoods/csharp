using System;

namespace HashSystem.Models
{
    public class HashLog
    {
        public string Id { get; set; }
        public string Operation { get; set; }
        public string Algorithm { get; set; }
        public string InputPreview { get; set; }
        public string ResultHash { get; set; }
        public bool Success { get; set; }
        public DateTime Timestamp { get; set; }
        public string ErrorMessage { get; set; }

        public HashLog(string id, string operation,
                       string algorithm, string inputPreview)
        {
            Id           = id;
            Operation    = operation;
            Algorithm    = algorithm;
            InputPreview = inputPreview;
            Timestamp    = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {Operation} ({Algorithm}) " +
                   $"— {(Success ? "OK" : "FAIL")}: {ErrorMessage ?? ResultHash}";
        }
    }
}