using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Atata.Tests
{
    public abstract class TestCaseDataSource : IEnumerable
    {
        private readonly List<TestCaseData> _items = new List<TestCaseData>();

        protected void Add(params object[] arguments)
        {
            _items.Add(new TestCaseData(arguments));
        }

        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
