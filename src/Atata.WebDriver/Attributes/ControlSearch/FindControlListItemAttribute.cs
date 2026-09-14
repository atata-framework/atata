namespace Atata.WebDriver;

internal class FindControlListItemAttribute : FindAttribute
{
    protected override Type DefaultStrategy => typeof(FindFirstDescendantStrategy);
}
