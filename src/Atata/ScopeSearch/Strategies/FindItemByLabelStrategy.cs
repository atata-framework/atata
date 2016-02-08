using Humanizer;

namespace Atata
{
    public class FindItemByLabelStrategy : IItemElementFindStrategy
    {
        public string Find(string xPath, object parameter)
        {
            return xPath + "[ancestor::label[contains(., '{0}']]".FormatWith(parameter);
        }
    }
}
