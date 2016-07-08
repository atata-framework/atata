namespace Atata
{
    public interface ITermDataProvider : ITermSettings
    {
        string[] Values { get; }
    }
}
