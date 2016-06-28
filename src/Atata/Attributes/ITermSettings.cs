namespace Atata
{
    public interface ITermSettings
    {
        TermCase Case { get; }
        TermMatch Match { get; }
        string StringFormat { get; }
    }
}
