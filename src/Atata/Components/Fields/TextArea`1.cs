namespace Atata
{
    /// <summary>
    /// Represents the text area control (&lt;textarea&gt;).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("textarea", IgnoreNameEndings = "TextArea")]
    public class TextArea<TOwner> : EditableField<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string GetValue()
        {
            return Scope.GetValue();
        }

        protected override void SetValue(string value)
        {
            Scope.FillInWith(value);
        }
    }
}
