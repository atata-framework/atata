using OpenQA.Selenium;

namespace Atata
{
    [UIComponent("input[@type='checkbox']", IgnoreNameEndings = "Checkbox,CheckBox")]
    public class Checkbox<TOwner> : EditableField<bool, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override bool GetValue()
        {
            return Scope.Selected;
        }

        protected override void SetValue(bool value)
        {
            IWebElement element = Scope;
            if (element.Selected != value)
                element.Click();
        }

        public TOwner Check()
        {
            return Set(true);
        }

        public TOwner Uncheck()
        {
            return Set(false);
        }
    }
}
