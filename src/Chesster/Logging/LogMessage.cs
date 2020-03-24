using System;

namespace Chesster.Logging
{
    public struct LogMessage
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }

        public Type SenderType { get; set; }
        public string AdditionalInfo { get; set; }

        public override string ToString()
            => $"[{Level}] {Message}";
    }
}
