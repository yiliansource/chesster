using System;

namespace Chesster.Logging
{
    public abstract class LogOutput : IDisposable
    {
        protected DateTime Date => DateTime.Now;

        public abstract void Log(LogMessage message, LoggerSettings settings);
        public virtual void Dispose() { }
    }
}
