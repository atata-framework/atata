using OpenQA.Selenium;

namespace Atata
{
    [ControlDefinition("input[@type='checkbox']", IgnoreNameEndings = "Checkbox,CheckBox,Option")]
    public class CheckBox<TOwner> : EditableField<bool, TOwner>
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
