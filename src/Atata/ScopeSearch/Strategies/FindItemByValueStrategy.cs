using Humanizer;

namespace Atata
{
    public class FindItemByValueStrategy : IItemElementFindStrategy
    {
        public string Find(string xPath, object parameter)
        {
            return xPath + "[@value='{0}']".FormatWith(parameter);
        }
    }
}
