namespace Atata
{
    public interface IQualifierAttribute
    {
        string[] GetQualifiers(UIPropertyMetadata metadata);
    }
}
