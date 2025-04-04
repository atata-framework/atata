namespace Atata;

/// <summary>
/// Represents the local date/time input control (<c>&lt;input type="datetime-local"&gt;</c>).
/// Default search is performed by the label.
/// The default format is <c>"yyyy-MM-ddTHH:mm"</c>.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='datetime-local']", ComponentTypeName = "local date/time input")]
[Format("g")]
[ValueGetFormat("yyyy-MM-ddTHH:mm")]
[ValueSetFormat("yyyy-MM-ddTHH:mm")]
[SetsValueUsingScript]
public class LocalDateTimeInput<TOwner> : Input<DateTime?, TOwner>
    where TOwner : PageObject<TOwner>
{
    protected override DateTime? ConvertStringToValueUsingGetFormat(string? value)
    {
        try
        {
            return base.ConvertStringToValueUsingGetFormat(value);
        }
        catch
        {
            return null;
        }
    }
}
