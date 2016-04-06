namespace Atata
{
    [PageObjectDefinition(ComponentTypeName = "page", IgnoreNameEndings = "Page,PageObject")]
    public class Page<T> : PageObject<T>
        where T : Page<T>
    {
    }
}
