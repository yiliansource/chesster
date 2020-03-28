using System;
using System.IO;
using System.Text;

namespace Chesster.Logging
{
    /// <summary>
    /// Represents a logger that can output the messages to a logfile.
    /// </summary>
    public class FileLogOutput : LogOutput
    {
        /// <summary>
        /// The path that the file will be created at.
        /// </summary>
        public string LogfilePath => Path.Combine(LogfileDirectory, LogfileName);

        /// <summary>
        /// The name of the logfile.
        /// </summary>
        public string LogfileName { get; }
        /// <summary>
        /// The directory of the logfile.
        /// </summary>
        public string LogfileDirectory { get; }

        private StreamWriter _streamWriter;

        /// <summary>
        /// Creates a new logfile output, with the default filename of 'chesster.log'.
        /// </summary>
        public FileLogOutput() : this("chesster.log") { }
        /// <summary>
        /// Creates a new logfile output with the specified filename, in the executing directory.
        /// </summary>
        public FileLogOutput(string fileName) : this(fileName, Environment.CurrentDirectory) { }
        /// <summary>
        /// Creates a new logfile output, with a specified file and directory.
        /// </summary>
        public FileLogOutput(string fileName, string directory)
        {
            LogfileName = fileName;
            LogfileDirectory = directory;

            _streamWriter = new StreamWriter(LogfilePath);
        }

        /// <summary>
        /// Appends the given message to the logfile, using the specified settings.
        /// </summary>
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
                builder.Append($"{FormatTypeName(message.SenderType)}: ");

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

        /// <summary>
        /// Closes the filestream to the logfile.
        /// </summary>
        public override void Dispose()
        {
            _streamWriter.Close();
        }
    }
}
