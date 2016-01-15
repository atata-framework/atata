namespace Atata
{
    public interface IQualifierAttribute
    {
        string[] GetQualifiers(UIComponentMetadata metadata);
    }
}
