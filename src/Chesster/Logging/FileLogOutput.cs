using System;
using System.IO;
using System.Text;

namespace Chesster.Logging
{
    public class FileLogOutput : LogOutput
    {
        public string LogfilePath => Path.Combine(LogfileDirectory, LogfileName);

        public string LogfileName { get; }
        public string LogfileDirectory { get; }

        private StreamWriter _streamWriter;

        public FileLogOutput() : this("chesster.log") { }
        public FileLogOutput(string fileName) : this(fileName, Environment.CurrentDirectory) { }
        public FileLogOutput(string fileName, string directory)
        {
            LogfileName = fileName;
            LogfileDirectory = directory;

            _streamWriter = new StreamWriter(LogfilePath);
        }

        public override void Log(LogMessage message, LoggerSettings settings)
        {
            StringBuilder builder = new StringBuilder();

            if (settings.IncludeDate)
                builder.Append($"{Date.ToString(settings.DateFormat)} ");
            if (settings.IncludeTime)
                builder.Append($"{Date.ToString(settings.TimeFormat)} ");
            if (settings.IncludeDate || settings.IncludeTime)
                builder.Append(' ');

            builder.Append(message.Level.ToString().PadRight(7));
            
            if (message.SenderType != null)
                builder.Append($"{message.SenderType.FullName}: ");

            int left = builder.Length;
            builder.Append(message.Message);
            
            if (!string.IsNullOrEmpty(message.AdditionalInfo))
            {
                foreach (string line in message.AdditionalInfo.Split('\n'))
                {
                    builder.AppendLine();
                    builder.Append(' ', left);
                    builder.Append(line);
                }
            }

            _streamWriter.WriteLine(builder.ToString());
        }

        public override void Dispose()
        {
            _streamWriter.Close();
        }
    }
}
