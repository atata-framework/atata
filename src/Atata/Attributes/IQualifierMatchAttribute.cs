namespace Atata
{
    public interface IQualifierMatchAttribute
    {
        QualifierMatch GetQualifierMatch(UIComponentMetadata metadata);
    }
}
