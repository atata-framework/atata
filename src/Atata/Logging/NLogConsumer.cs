using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public class NLogConsumer : ILogConsumer
    {
        private readonly dynamic logger;
        private readonly Type logEventInfoType;
        private readonly Dictionary<LogLevel, dynamic> logLevelsMap = new Dictionary<LogLevel, dynamic>();

        public NLogConsumer(string loggerName = null)
        {
            Type logManagerType = Type.GetType("NLog.LogManager,NLog", true);

            logger = loggerName != null
                ? logManagerType.GetMethodWithThrowOnError("GetLogger", typeof(string)).Invoke(null, new[] { loggerName })
                : logManagerType.GetMethodWithThrowOnError("GetCurrentClassLogger").Invoke(null, new object[0]);

            if (logger == null)
                throw new InvalidOperationException("Failed to create NLog logger.");

            logEventInfoType = Type.GetType("NLog.LogEventInfo,NLog", true);

            InitLogLevelsMap();
        }

        private void InitLogLevelsMap()
        {
            Type logLevelType = Type.GetType("NLog.LogLevel,NLog", true);

            PropertyInfo allLevelsProperty = logLevelType.GetPropertyWithThrowOnError("AllLoggingLevels");
            IEnumerable allLevels = (IEnumerable)allLevelsProperty.GetStaticValue();

            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                logLevelsMap[level] = allLevels.
                    Cast<dynamic>().
                    First(x => x.Name == Enum.GetName(typeof(LogLevel), level));
            }
        }

        public void Log(LogEventInfo eventInfo)
        {
            dynamic otherEventInfo = ActivatorEx.CreateInstance(logEventInfoType);

            otherEventInfo.TimeStamp = eventInfo.Timestamp;
            otherEventInfo.Level = logLevelsMap[eventInfo.Level];
            otherEventInfo.Message = eventInfo.Message;
            otherEventInfo.Exception = eventInfo.Exception;
            otherEventInfo.Properties["build-start"] = eventInfo.BuildStart;
            otherEventInfo.Properties["test-name"] = eventInfo.TestName;
            otherEventInfo.Properties["test-start"] = eventInfo.TestStart;

            logger.Log(otherEventInfo);
        }
    }
}
