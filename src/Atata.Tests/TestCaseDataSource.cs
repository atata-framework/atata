using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Atata.Tests
{
    public abstract class TestCaseDataSource : IEnumerable
    {
        private readonly List<TestCaseData> items = new List<TestCaseData>();

        protected void Add(params object[] arguments)
        {
            items.Add(new TestCaseData(arguments));
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
