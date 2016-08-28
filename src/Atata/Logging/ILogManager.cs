using System;

namespace Atata
{
    public interface ILogManager
    {
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, Exception exception);
        void Screenshot(string title = null);
    }
}
