namespace Chesster.Logging
{
    public class LoggerSettings
    {
        public bool IncludeDate { get; set; } = false;
        public bool IncludeTime { get; set; } = true;

        public string DateFormat { get; set; } = "yyyy/MM/dd";
        public string TimeFormat { get; set; } = "HH:mm:ss";

        public LogLevel MinimumLevel { get; set; } = LogLevel.Error;
    }
}
