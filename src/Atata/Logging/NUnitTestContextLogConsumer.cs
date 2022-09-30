using System;
using System.Reflection;

namespace Atata
{
    public class NUnitTestContextLogConsumer : TextOutputLogConsumer
    {
        private readonly MethodInfo _writeMethod;

        public NUnitTestContextLogConsumer()
        {
            Type testContextType = Type.GetType("NUnit.Framework.TestContext,nunit.framework", true);

            _writeMethod = testContextType.GetMethodWithThrowOnError("WriteLine", typeof(string));
        }

        protected override void Write(string completeMessage) =>
            _writeMethod.InvokeStaticAsLambda(completeMessage);
    }
}
