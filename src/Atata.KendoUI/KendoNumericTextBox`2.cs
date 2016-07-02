using OpenQA.Selenium;

namespace Atata.KendoUI
{
    // TODO: Fix IdXPathFormat. Element used by IdXPathFormat is invisilble.
    [ControlDefinition(
        "span",
        ContainingClass = "k-numerictextbox",
        ComponentTypeName = "numeric text box",
        IdXPathFormat = "span[span/input[2][@id='{0}']]")]
    public class KendoNumericTextBox<T, TOwner> : EditableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override T GetValue()
        {
            string valueAsString = Scope.Get(By.CssSelector("input.k-input[data-role='numerictextbox']").Input().OfAnyVisibility()).GetValue();
            return ConvertStringToValue(valueAsString);
        }

        protected override void SetValue(T value)
        {
            Scope.Get(By.CssSelector("input.k-formatted-value.k-input").Input()).Click();

            Scope.Get(By.CssSelector("input.k-input")).FillInWith(value.ToString());
        }
    }
}
