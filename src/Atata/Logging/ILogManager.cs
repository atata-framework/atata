using System;

namespace Atata
{
    public interface ILogManager
    {
        void Trace(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warn(string message, params object[] args);

        void Error(string message, Exception exception);

        void Fatal(string message, Exception exception);

        void Start(LogSection section);

        void EndSection();

        void Screenshot(string title = null);
    }
}
