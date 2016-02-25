using Humanizer;
using System;

namespace Atata
{
    public class FindItemByLabelStrategy : IItemElementFindStrategy
    {
        // TODO: Get rid of 'contains' in FindItemByLabelStrategy.Find method.
        public string Find(object parameter)
        {
            string parameterAsString = parameter is Enum ? ((Enum)parameter).ToTitleString() : parameter.ToString();
            return "[ancestor::label[contains(., '{0}')]]".FormatWith(parameterAsString);
        }
    }
}
