namespace Chesster.Logging
{
    /// <summary>
    /// Provides settings for logging.
    /// </summary>
    public class LoggerSettings
    {
        /// <summary>
        /// Should the date be included in the log message?
        /// </summary>
        public bool IncludeDate { get; set; } = false;
        /// <summary>
        /// Should the time be included in the log message?
        /// </summary>
        public bool IncludeTime { get; set; } = true;

        /// <summary>
        /// If the date should be included, this format string will be used.
        /// </summary>
        public string DateFormat { get; set; } = "yyyy/MM/dd";
        /// <summary>
        /// If the time should be included, this format string will be used.
        /// </summary>
        public string TimeFormat { get; set; } = "HH:mm:ss";

        /// <summary>
        /// The minimum log level that messages need to have to be logged.
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Error;
    }
}
