using Humanizer;
using System;

namespace Atata
{
    public class FindItemByValueStrategy : IItemElementFindStrategy
    {
        private readonly TermMatch match;

        public FindItemByValueStrategy(TermMatch match)
        {
            this.match = match;
        }

        public string Find(object parameter)
        {
            string parameterAsString = parameter is Enum ? ((Enum)parameter).ToTitleString() : parameter.ToString();
            return "[{0}]".FormatWith(match.CreateXPathCondition(parameterAsString, "@value"));
        }
    }
}
