using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Atata
{
    public class ControlList<T, TOwner> : IEnumerable<T>
        where T : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected UIComponent<TOwner> Component { get; private set; }

        public T this[int index]
        {
            get { return null; }
        }

        public T this[Expression<Func<T, bool>> predicateExpression]
        {
            get { return null; }
        }

        public DataProvider<int, TOwner> Count => Component.GetOrCreateDataProvider(nameof(Count).ToString(TermCase.Lower), GetCount);

        protected virtual int GetCount()
        {
            return 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
