using System;
using System.Collections.Generic;

namespace Chesster.Logging
{
    /// <summary>
    /// Provides logging features.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// The settings that the logger should use.
        /// </summary>
        public static LoggerSettings Settings { get; } = new LoggerSettings();

        private static List<LogOutput> _outputs = new List<LogOutput>();

        static Logger()
        {
            // Ensure that the loggers are all disposed on domain exit
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Dispose();
        }

        /// <summary>
        /// Registers an output of the specified type to the logger.
        /// </summary>
        public static void RegisterOutput<T>() where T : LogOutput
            => RegisterOutput(Activator.CreateInstance<T>() as LogOutput);
        /// <summary>
        /// Registers the specified log output ot the logger.
        /// </summary>
        public static void RegisterOutput<T>(T output) where T : LogOutput
            => _outputs.Add(output);

        public static void Debug(string message)
            => Append(CreateMessage(message, LogLevel.Debug));
        public static void Debug<T>(string message)
            => Append(CreateMessage<T>(message, LogLevel.Debug));

        public static void Info(string message)
            => Append(CreateMessage(message, LogLevel.Info));
        public static void Info<T>(string message)
            => Append(CreateMessage<T>(message, LogLevel.Info));

        public static void Warn(string message)
            => Append(CreateMessage(message, LogLevel.Warn));
        public static void Warn<T>(string message)
            => Append(CreateMessage<T>(message, LogLevel.Warn));

        public static void Error(string message)
            => Append(CreateMessage(message, LogLevel.Error));
        public static void Error<T>(string message)
            => Append(CreateMessage<T>(message, LogLevel.Error));

        public static void Fatal(string message)
            => Append(CreateMessage(message, LogLevel.Fatal));
        public static void Fatal<T>(string message)
            => Append(CreateMessage<T>(message, LogLevel.Fatal));

        public static void Exception(Exception e)
             => Append(CreateMessage(e));
        public static void Exception<T>(Exception e)
            => Append(CreateMessage<T>(e));

        /// <summary>
        /// Appends the specified log message to the outputs.
        /// </summary>
        public static void Append(LogMessage message)
        {
            // If the message level is lower than the minimum level, ignore the message.
            if (message.Level < Settings.MinimumLevel)
                return;

            foreach (LogOutput output in _outputs)
            {
                output.Log(message, Settings);
            }
        }

        private static LogMessage CreateMessage(string message, LogLevel level)
        {
            return new LogMessage
            {
                Message = message,
                Level = level
            };
        }
        private static LogMessage CreateMessage<T>(string message, LogLevel level)
        {
            LogMessage lm = CreateMessage(message, level);
            lm.SenderType = typeof(T);
            return lm;
        }
        private static LogMessage CreateMessage(Exception ex)
        {
            return new LogMessage
            {
                Message = ex.Message,
                Level = LogLevel.Error,
                AdditionalInfo = ex.StackTrace
            };
        }
        private static LogMessage CreateMessage<T>(Exception ex)
        {
            LogMessage lm = CreateMessage(ex);
            lm.SenderType = typeof(T);
            return lm;
        }

        /// <summary>
        /// Disposes all outputs. Note that some loggers will most likely not react anymore after they are disposed.
        /// </summary>
        public static void Dispose()
        {
            foreach (LogOutput output in _outputs)
            {
                output.Dispose();
            }
        }
    }
}
