namespace Chesster.Logging
{
    /// <summary>
    /// Represents a severity level for a log message.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// A message that contains debugging information.
        /// </summary>
        Debug,
        /// <summary>
        /// A message that contains general information.
        /// </summary>
        Info,
        /// <summary>
        /// A message that contains a warning report.
        /// </summary>
        Warn,
        /// <summary>
        /// A message that contains an error report.
        /// </summary>
        Error,
        /// <summary>
        /// A message that contains a fatal report.
        /// </summary>
        Fatal
    }
}
