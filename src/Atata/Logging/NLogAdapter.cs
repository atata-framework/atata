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

        private static readonly Lazy<Type> FileTargetType = new Lazy<Type>(
            () => GetType("NLog.Targets.FileTarget"));

        private static readonly Lazy<Type> LayoutType = new Lazy<Type>(
            () => GetType("NLog.Layouts.Layout"));

        private static readonly Lazy<Type> LoggingConfigurationType = new Lazy<Type>(
            () => GetType("NLog.Config.LoggingConfiguration"));

        private static readonly Lazy<Type> LogManagerType = new Lazy<Type>(
            () => GetType("NLog.LogManager"));

        private static readonly Lazy<Type> LogEventInfoType = new Lazy<Type>(
            () => GetType("NLog.LogEventInfo"));

        private static readonly Lazy<Type> LogLevelType = new Lazy<Type>(
            () => GetType("NLog.LogLevel"));

        private static readonly object ConfigurationSyncLock = new object();

        private static readonly Lazy<Dictionary<LogLevel, dynamic>> LogLevelsMap = new Lazy<Dictionary<LogLevel, dynamic>>(
            CreateLogLevelsMap);

        private static Dictionary<LogLevel, dynamic> CreateLogLevelsMap()
        {
            PropertyInfo allLevelsProperty = LogLevelType.Value.GetPropertyWithThrowOnError("AllLoggingLevels");
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
            dynamic fileTarget = Activator.CreateInstance(FileTargetType.Value, name);

            MethodInfo layoutFromStringMethod = LayoutType.Value.GetMethodWithThrowOnError("FromString", typeof(string));
            fileTarget.FileName = layoutFromStringMethod.InvokeStaticAsLambda<dynamic>(filePath);
            fileTarget.Layout = layoutFromStringMethod.InvokeStaticAsLambda<dynamic>(layout);

            return fileTarget;
        }

        internal static void AddConfigurationRuleForAllLevels(dynamic target, string loggerNamePattern)
        {
            var logManagerConfigurationProperty = LogManagerType.Value.GetPropertyWithThrowOnError("Configuration");

            lock (ConfigurationSyncLock)
            {
                dynamic configuration = logManagerConfigurationProperty.GetStaticValue();

                if (configuration is null)
                {
                    configuration = Activator.CreateInstance(LoggingConfigurationType.Value);
                    logManagerConfigurationProperty.SetStaticValue(configuration as object);
                }

                configuration.AddRuleForAllLevels(target, loggerNamePattern);

                LogManagerType.Value.GetMethodWithThrowOnError("ReconfigExistingLoggers")
                    .InvokeStaticAsLambda();
            }
        }

        internal static dynamic GetLogger(string name) =>
            LogManagerType.Value.GetMethodWithThrowOnError("GetLogger", typeof(string))
                .InvokeStaticAsLambda<dynamic>(name);

        internal static dynamic GetCurrentClassLogger() =>
            LogManagerType.Value.GetMethodWithThrowOnError("GetCurrentClassLogger")
                .InvokeStaticAsLambda<dynamic>();

        internal static dynamic CreateLogEventInfo(LogEventInfo eventInfo)
        {
            dynamic otherEventInfo = Activator.CreateInstance(LogEventInfoType.Value);

            otherEventInfo.TimeStamp = eventInfo.Timestamp;
            otherEventInfo.Level = LogLevelsMap.Value[eventInfo.Level];
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
