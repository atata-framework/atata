using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atata
{
    /// <summary>
    /// Represents the log consumer for log4net.
    /// </summary>
    public class Log4NetConsumer : LazyInitializableLogConsumer, INamedLogConsumer
    {
        private static readonly Lazy<Dictionary<LogLevel, dynamic>> LazyLogLevelsMap = new Lazy<Dictionary<LogLevel, dynamic>>(CreateLogLevelsMap);

        private static readonly Lazy<dynamic> LazyThreadContextProperties = new Lazy<dynamic>(GetThreadContextProperties);

        private string repositoryName;

        private Assembly repositoryAssembly;

        /// <summary>
        /// Gets or sets the name of the logger repository.
        /// </summary>
        public string RepositoryName
        {
            get => repositoryName;
            set
            {
                repositoryName = value;
                repositoryAssembly = null;
            }
        }

        /// <summary>
        /// Gets or sets the assembly to use to lookup the repository.
        /// </summary>
        public Assembly RepositoryAssembly
        {
            get => repositoryAssembly;
            set
            {
                repositoryAssembly = value;
                repositoryName = null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the logger.
        /// </summary>
        public string LoggerName { get; set; }

        private static Dictionary<LogLevel, dynamic> CreateLogLevelsMap()
        {
            Dictionary<LogLevel, dynamic> logLevelsMap = new Dictionary<LogLevel, dynamic>();
            Type logLevelType = Type.GetType("log4net.Core.Level,log4net", true);

            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                FieldInfo levelField = logLevelType.GetFieldWithThrowOnError(
                    level.ToString(),
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField);

                logLevelsMap[level] = levelField.GetValue(null);
            }

            return logLevelsMap;
        }

        private static dynamic GetThreadContextProperties()
        {
            return Type.GetType("log4net.ThreadContext,log4net", true).
                GetPropertyWithThrowOnError("Properties", BindingFlags.Public | BindingFlags.Static).
                GetStaticValue();
        }

        private static MethodInfo GetGetLoggerMethod(params Type[] argumentTypes)
        {
            return Type.GetType("log4net.LogManager,log4net", true).
                GetMethodWithThrowOnError("GetLogger", BindingFlags.Public | BindingFlags.Static, argumentTypes);
        }

        protected override void OnLog(LogEventInfo eventInfo)
        {
            var properties = LazyThreadContextProperties.Value;

            properties["build-start"] = eventInfo.BuildStart;
            properties["test-name"] = eventInfo.TestName;
            properties["test-name-sanitized"] = eventInfo.TestNameSanitized;
            properties["test-start"] = eventInfo.TestStart;
            properties["driver-alias"] = eventInfo.DriverAlias;

            var level = LazyLogLevelsMap.Value[eventInfo.Level];

            Logger.Log(null, level, eventInfo.Message, eventInfo.Exception);
        }

        protected override dynamic GetLogger()
        {
            string loggerName = LoggerName ?? GetType().FullName;

            dynamic log = GetLog(loggerName);

            return log.Logger;
        }

        private dynamic GetLog(string loggerName)
        {
            if (RepositoryName != null)
            {
                return GetGetLoggerMethod(typeof(string), typeof(string)).
                    InvokeStaticAsLambda<dynamic>(RepositoryName, loggerName);
            }
            else if (RepositoryAssembly != null)
            {
                return GetGetLoggerMethod(typeof(Assembly), typeof(string)).
                    InvokeStaticAsLambda<dynamic>(RepositoryAssembly, loggerName);
            }
            else
            {
                return GetGetLoggerMethod(typeof(string)).
                    InvokeStaticAsLambda<dynamic>(loggerName);
            }
        }
    }
}
