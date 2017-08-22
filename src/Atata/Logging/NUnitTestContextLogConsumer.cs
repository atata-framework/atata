using System;
using System.Reflection;

namespace Atata
{
    public class NUnitTestContextLogConsumer : TextOutputLogConsumer
    {
        private readonly MethodInfo writeMethod;

        public NUnitTestContextLogConsumer()
        {
            Type testContextType = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);

            writeMethod = testContextType.GetMethodWithThrowOnError("WriteLine", typeof(string));
        }

        protected override void Write(string completeMessage)
        {
            writeMethod.InvokeStatic(completeMessage);
        }
    }
}
