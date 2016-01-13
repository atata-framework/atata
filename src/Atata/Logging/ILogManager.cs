using System;

namespace Atata
{
    public interface ILogManager
    {
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, Exception excepton);
        void Screenshot(string title = null, int waitMilliseconds = 0);
    }
}
