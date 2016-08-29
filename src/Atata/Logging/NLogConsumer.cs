using System;

namespace Atata.Logging
{
    public class NLogConsumer : ILogConsumer
    {
        private readonly dynamic logger;
        private readonly Type logEventInfoType;

        public NLogConsumer(string loggerName = null)
        {
            Type logManagerType = Type.GetType("NLog.LogManager,NLog", true);

            logger = loggerName != null
                ? logManagerType.GetMethodWithThrowOnError("GetLogger", typeof(string)).Invoke(null, new[] { loggerName })
                : logManagerType.GetMethodWithThrowOnError("GetCurrentClassLogger").Invoke(null, new object[0]);

            if (logger == null)
                throw new InvalidOperationException("Failed to create NLog logger.");

            logEventInfoType = Type.GetType("NLog.LogEventInfo,NLog", true);
        }

        public void Log(LogEventInfo eventInfo)
        {
            dynamic otherEventInfo = ActivatorEx.CreateInstance(logEventInfoType);

            otherEventInfo.TimeStamp = eventInfo.Timestamp;

            // TODO: Cast Level.
            otherEventInfo.Level = eventInfo.Level;
            otherEventInfo.Message = eventInfo.Message;
            otherEventInfo.Exception = eventInfo.Exception;

            logger.Log(otherEventInfo);
        }
    }
}
