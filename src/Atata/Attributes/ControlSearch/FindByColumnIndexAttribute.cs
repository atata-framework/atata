using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found within the table column (&lt;td&gt;) that has the nth index.
    /// </summary>
    public class FindByColumnIndexAttribute : FindAttribute
    {
        public FindByColumnIndexAttribute(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        public int ColumnIndex { get; private set; }

        protected override Type DefaultStrategy
        {
            get { return typeof(FindByColumnIndexStrategy); }
        }

        protected override IEnumerable<object> GetStrategyArguments()
        {
            yield return ColumnIndex;
        }
    }
}
