using Humanizer;

namespace Atata
{
    public class FindItemByIndexStrategy : IItemElementFindStrategy
    {
        public string Find(string xPath, object parameter)
        {
            int index = (int)parameter;

            return xPath + "[{0}]".FormatWith(parameter);
        }
    }
}
