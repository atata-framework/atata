using System;
using System.Reflection;

namespace Atata
{
    /// <summary>
    /// Defines the method to invoke on the specified event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class InvokeMethodAttribute : TriggerAttribute
    {
        public InvokeMethodAttribute(string methodName, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            MethodName = methodName.CheckNotNullOrWhitespace(nameof(methodName));
        }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string MethodName { get; private set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            var methodOwner = IsDefinedAtComponentLevel ? context.Component : context.Component.Parent;
            MethodInfo method = methodOwner.GetType().GetMethod(MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (method == null)
                throw new MissingMethodException(methodOwner.GetType().FullName, MethodName);

            method.Invoke(method.IsStatic ? null : methodOwner, new object[0]);
        }
    }
}
