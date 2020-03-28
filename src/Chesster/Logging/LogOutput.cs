using System;
using System.Text;
using System.Linq;

namespace Chesster.Logging
{
    /// <summary>
    /// Represents the base for a logger.
    /// </summary>
    public abstract class LogOutput : IDisposable
    {
        protected DateTime Date => DateTime.Now;

        /// <summary>
        /// Logs the specified message, using the specified settings.
        /// </summary>
        public abstract void Log(LogMessage message, LoggerSettings settings);
        /// <summary>
        /// Safely disposes the logger.
        /// </summary>
        public virtual void Dispose() { }

        protected string FormatTypeName(Type type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(type.FullName);

            if (type.ContainsGenericParameters)
            {
                builder.Append('<');
                builder.AppendJoin(", ", type.GetGenericArguments().Select(arg => arg.Name));
                builder.Append('>');
            }

            return builder.ToString();
        }
    }
}
