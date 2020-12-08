namespace Atata
{
    public class WaitForElementLogSection : UIComponentLogSection
    {
        public WaitForElementLogSection(UIComponent component, WaitBy waitBy, string selector, WaitUnit waitUnit)
            : base(component)
        {
            Message = $"Wait until element by \"{selector}\" {ConvertToString(waitBy)} is {waitUnit.GetWaitingText()}";
        }

        private static string ConvertToString(WaitBy waitBy)
        {
            switch (waitBy)
            {
                case WaitBy.Id:
                    return "id";
                case WaitBy.Name:
                    return "name";
                case WaitBy.Class:
                    return "class";
                case WaitBy.Css:
                    return "CSS selector";
                case WaitBy.XPath:
                    return "XPath";
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(waitBy, nameof(waitBy));
            }
        }
    }
}
