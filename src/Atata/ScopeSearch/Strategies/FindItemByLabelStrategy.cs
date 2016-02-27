using Humanizer;
using System;

namespace Atata
{
    public class FindItemByLabelStrategy : IItemElementFindStrategy
    {
        private readonly TermMatch match;

        public FindItemByLabelStrategy(TermMatch match)
        {
            this.match = match;
        }

        // TODO: Get rid of 'contains' in FindItemByLabelStrategy.Find method.
        public string Find(object parameter)
        {
            string parameterAsString = parameter is Enum ? ((Enum)parameter).ToTitleString() : parameter.ToString();
            return "[ancestor::label[{0}]]".FormatWith(match.CreateXPathCondition(parameterAsString));
        }
    }
}
