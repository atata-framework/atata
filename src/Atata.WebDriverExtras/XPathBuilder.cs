namespace Atata
{
    public class XPathBuilder : XPathBuilder<XPathBuilder>
    {
        public static implicit operator string(XPathBuilder builder)
        {
            return builder.XPath;
        }

        protected override XPathBuilder CreateInstance()
        {
            return new XPathBuilder();
        }
    }
}
