using System;

namespace Chesster.Logging
{
    /// <summary>
    /// Represents a message in a log.
    /// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// The severity of the log message.
        /// </summary>
        public LogLevel Level { get; set; }
        /// <summary>
        /// The message content.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The type that the message requested as the sender type.
        /// </summary>
        public Type SenderType { get; set; }
        /// <summary>
        /// Information that is logged additionally to the message content.
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Returns a basic representation of the message.
        /// </summary>
        public override string ToString()
            => $"[{Level}] {Message}";
    }
}
