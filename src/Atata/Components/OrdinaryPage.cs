namespace Atata;

/// <summary>
/// Represents the ordinary page.
/// </summary>
[Name(DefaultName)]
public class OrdinaryPage : Page<OrdinaryPage>
{
    private const string DefaultName = "<ordinary>";

    public OrdinaryPage(string name = DefaultName) =>
        ComponentName = name.CheckNotNullOrEmpty(nameof(name));
}
