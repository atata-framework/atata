using System;
using System.Reflection;

namespace Atata
{
    public class NUnitTestContextLogConsumer : TextOutputLogConsumer
    {
        private readonly MethodInfo writeMethod;

        public NUnitTestContextLogConsumer()
        {
            Type type = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);
            writeMethod = type.GetMethod("WriteLine", new[] { typeof(string) });
        }

        protected override void Write(string completeMessage)
        {
            writeMethod.Invoke(null, new[] { completeMessage });
        }
    }
}
