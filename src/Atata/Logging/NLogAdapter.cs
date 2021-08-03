using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    internal static class NLogAdapter
    {
        private const string NLogAssemblyName = "NLog";

        private static readonly Lazy<Type> s_fileTargetType = new Lazy<Type>(
            () => GetType("NLog.Targets.FileTarget"));

        private static readonly Lazy<Type> s_layoutType = new Lazy<Type>(
            () => GetType("NLog.Layouts.Layout"));

        private static readonly Lazy<Type> s_loggingConfigurationType = new Lazy<Type>(
            () => GetType("NLog.Config.LoggingConfiguration"));

        private static readonly Lazy<Type> s_logManagerType = new Lazy<Type>(
            () => GetType("NLog.LogManager"));

        private static readonly Lazy<Type> s_logEventInfoType = new Lazy<Type>(
            () => GetType("NLog.LogEventInfo"));

        private static readonly Lazy<Type> s_logLevelType = new Lazy<Type>(
            () => GetType("NLog.LogLevel"));

        private static readonly object s_configurationSyncLock = new object();

        private static readonly Lazy<Dictionary<LogLevel, dynamic>> s_logLevelsMap = new Lazy<Dictionary<LogLevel, dynamic>>(
            CreateLogLevelsMap);

        private static Dictionary<LogLevel, dynamic> CreateLogLevelsMap()
        {
            PropertyInfo allLevelsProperty = s_logLevelType.Value.GetPropertyWithThrowOnError("AllLoggingLevels");
            IEnumerable allLevels = (IEnumerable)allLevelsProperty.GetStaticValue();

            var result = new Dictionary<LogLevel, dynamic>();

            foreach (LogLevel level in Enum.GetValues(typeof(LogLevel)))
            {
                result[level] = allLevels.
                    Cast<dynamic>().
                    First(x => x.Name == Enum.GetName(typeof(LogLevel), level));
            }

            return result;
        }

        private static Type GetType(string typeName)
        {
            return Type.GetType($"{typeName},{NLogAssemblyName}", true);
        }

        internal static dynamic CreateFileTarget(string name, string filePath, string layout)
        {
            dynamic fileTarget = Activator.CreateInstance(s_fileTargetType.Value, name);

            MethodInfo layoutFromStringMethod = s_layoutType.Value.GetMethodWithThrowOnError("FromString", typeof(string));
            fileTarget.FileName = layoutFromStringMethod.InvokeStaticAsLambda<dynamic>(filePath);
            fileTarget.Layout = layoutFromStringMethod.InvokeStaticAsLambda<dynamic>(layout);

            return fileTarget;
        }

        internal static void AddConfigurationRuleForAllLevels(dynamic target, string loggerNamePattern)
        {
            var logManagerConfigurationProperty = s_logManagerType.Value.GetPropertyWithThrowOnError("Configuration");

            lock (s_configurationSyncLock)
            {
                dynamic configuration = logManagerConfigurationProperty.GetStaticValue();

                if (configuration is null)
                {
                    configuration = Activator.CreateInstance(s_loggingConfigurationType.Value);
                    logManagerConfigurationProperty.SetStaticValue(configuration as object);
                }

                configuration.AddRuleForAllLevels(target, loggerNamePattern);

                s_logManagerType.Value.GetMethodWithThrowOnError("ReconfigExistingLoggers")
                    .InvokeStaticAsLambda();
            }
        }

        internal static dynamic GetLogger(string name) =>
            s_logManagerType.Value.GetMethodWithThrowOnError("GetLogger", typeof(string))
                .InvokeStaticAsLambda<dynamic>(name);

        internal static dynamic GetCurrentClassLogger() =>
            s_logManagerType.Value.GetMethodWithThrowOnError("GetCurrentClassLogger")
                .InvokeStaticAsLambda<dynamic>();

        internal static dynamic CreateLogEventInfo(LogEventInfo eventInfo)
        {
            dynamic otherEventInfo = Activator.CreateInstance(s_logEventInfoType.Value);

            otherEventInfo.TimeStamp = eventInfo.Timestamp;
            otherEventInfo.Level = s_logLevelsMap.Value[eventInfo.Level];
            otherEventInfo.Message = eventInfo.Message;
            otherEventInfo.Exception = eventInfo.Exception;

            var properties = (IDictionary<object, object>)otherEventInfo.Properties;

            properties["build-start"] = eventInfo.BuildStart;
            properties["test-name"] = eventInfo.TestName;
            properties["test-name-sanitized"] = eventInfo.TestNameSanitized;
            properties["test-suite-name"] = eventInfo.TestSuiteName;
            properties["test-suite-name-sanitized"] = eventInfo.TestSuiteNameSanitized;
            properties["test-start"] = eventInfo.TestStart;
            properties["driver-alias"] = eventInfo.DriverAlias;

            return otherEventInfo;
        }
    }
}
