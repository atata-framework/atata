using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Specifies that a control should be found within the table column (<c>&lt;td&gt;</c>) that has the nth index.
    /// </summary>
    public class FindByColumnIndexAttribute : FindAttribute
    {
        public FindByColumnIndexAttribute(int columnIndex)
        {
            ColumnIndex = columnIndex;
        }

        public int ColumnIndex { get; }

        protected override Type DefaultStrategy => typeof(FindByColumnIndexStrategy);

        protected override IEnumerable<object> GetStrategyArguments()
        {
            yield return ColumnIndex;
        }

        public override string BuildComponentName() =>
            BuildComponentNameWithArgument(ColumnIndex);
    }
}
