using Humanizer;

namespace Atata
{
    public class FindItemByValueStrategy : IItemElementFindStrategy
    {
        public string Find(object parameter)
        {
            return "[@value='{0}']".FormatWith(parameter);
        }
    }
}
