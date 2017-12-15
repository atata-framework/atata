namespace Atata
{
    /// <summary>
    /// Represents the ordinary page.
    /// </summary>
    public class OrdinaryPage : Page<OrdinaryPage>
    {
        public OrdinaryPage(string name = "<ordinary>")
        {
            ComponentName = name;
        }
    }
}
