namespace Atata
{
    public interface IQualifierMatchAttribute
    {
        QualifierMatch GetQualifierMatch(UIPropertyMetadata metadata);
    }
}
