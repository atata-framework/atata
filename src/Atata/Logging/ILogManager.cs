using System;

namespace Atata
{
    public interface ILogManager
    {
        void Trace(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warn(string message, params object[] args);

        void Warn(Exception exception);

        void Warn(string message, Exception exception = null);

        void Error(Exception exception);

        void Error(string message, Exception exception = null);

        void Fatal(Exception exception);

        void Fatal(string message, Exception exception = null);

        void Start(LogSection section);

        void EndSection();

        void Screenshot(string title = null);
    }
}
