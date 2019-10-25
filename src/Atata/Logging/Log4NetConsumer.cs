using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public class Log4NetConsumer : ILogConsumer
    {
        private readonly Dictionary<LogLevel, dynamic> logLevelsMap = new Dictionary<LogLevel, dynamic>();
        private readonly string repositoryName;
        private string loggerName;
        private dynamic logger;

        public Log4NetConsumer(string repositoryName, string loggerName)
        {
			this.repositoryName = repositoryName;
			LoggerName = loggerName;
            InitLogLevelsMap();
        }

        public string LoggerName
        {
            get
            {
                return loggerName;
            }

            set
            {
                loggerName = value;
                InitLogger(value);
            }
        }

        private void InitLogLevelsMap()
        {
            var allLevels = (IEnumerable)logger.Logger.Repository.LevelMap.AllLevels;
            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                logLevelsMap[level] = allLevels.
                    Cast<dynamic>().
                    First(x => x.Name == Enum.GetName(typeof(LogLevel), level).ToUpper());
            }
        }

        private void InitLogger(string loggerName)
        {
            Type logManagerType = Type.GetType("log4net.LogManager,log4net", true);

			var _repository = logManagerType.GetMethodWithThrowOnError("GetRepository", typeof(string)).InvokeStaticAsLambda<dynamic>(repositoryName);
			if (_repository.Configured)
			{
				var currentLoggers = (IEnumerable)logManagerType.GetMethodWithThrowOnError("GetCurrentLoggers", typeof(string)).InvokeStaticAsLambda<dynamic>(repositoryName);
				logger = currentLoggers?.Cast<dynamic>().FirstOrDefault(a=>a.Logger.Name == loggerName);
				logger = logger != null ? logger : logManagerType.GetMethodWithThrowOnError("GetLogger", typeof(string), typeof(string)).InvokeStaticAsLambda<dynamic>(repositoryName, loggerName);
			}
			else
			{
				throw new ArgumentException($"Log4Net repository '{repositoryName}' is not configured.");
			}

			if (logger == null)
                throw new InvalidOperationException("Failed to create Log4Net logger.");
        }

        public void Log(LogEventInfo eventInfo)
        {
            logger.Logger.Log(null, logLevelsMap[eventInfo.Level], eventInfo.Message, eventInfo.Exception);
        }
    }
}
