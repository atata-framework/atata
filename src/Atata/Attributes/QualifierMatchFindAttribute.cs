namespace Atata
{
    public abstract class QualifierMatchFindAttribute : QualifierFindAttribute, IQualifierMatchAttribute
    {
        protected QualifierMatchFindAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
            : base(format)
        {
            Match = match;
        }

        protected QualifierMatchFindAttribute(params string[] values)
            : base(values)
        {
        }

        public new QualifierMatch Match { get; set; }

        public QualifierMatch GetQualifierMatch(UIPropertyMetadata metadata)
        {
            return Match != QualifierMatch.Inherit ? Match : GetQualifierMatchFromMetadata(metadata);
        }

        protected abstract QualifierMatch GetQualifierMatchFromMetadata(UIPropertyMetadata metadata);
    }
}
