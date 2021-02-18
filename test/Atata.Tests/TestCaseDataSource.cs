using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

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
