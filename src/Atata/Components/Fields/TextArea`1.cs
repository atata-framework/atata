namespace Atata
{
    [ControlDefinition("textarea", IgnoreNameEndings = "TextArea")]
    public class TextArea<_> : EditableField<string, _>
        where _ : PageObject<_>
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
