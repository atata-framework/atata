using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the file input control (<c>&lt;input type="file"&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='file']", Visibility = Visibility.Any)]
    public class FileInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override void SetValue(string value)
        {
            string valueAsString = ConvertValueToString(value);
            Scope.SendKeys(valueAsString);
        }

        protected override void OnClear()
        {
            var element = Scope;

            try
            {
                element.Clear();
            }
            catch (InvalidElementStateException)
            {
                Owner.Driver.ExecuteScript("arguments[0].value = null;", element);
            }
        }
    }
}
