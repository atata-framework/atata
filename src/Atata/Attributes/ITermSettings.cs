namespace Atata
{
    public interface ITermSettings
    {
        TermFormat Format { get; }
        TermMatch Match { get; }
        string StringFormat { get; }
    }
}
