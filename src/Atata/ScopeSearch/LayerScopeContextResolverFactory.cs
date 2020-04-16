namespace Atata
{
    public static class LayerScopeContextResolverFactory
    {
        private static readonly ILayerScopeContextResolver ParentResolver = new PlainLayerScopeContextResolver("./");

        private static readonly ILayerScopeContextResolver SiblingResolver = new PlainLayerScopeContextResolver("../");

        private static readonly ILayerScopeContextResolver AncestorResolver = new PlainLayerScopeContextResolver(".//");

        private static readonly ILayerScopeContextResolver ShadowHostResolver = new ShadowHostLayerScopeContextResolver();

        public static ILayerScopeContextResolver Create(FindAs findAs)
        {
            switch (findAs)
            {
                case FindAs.Parent:
                    return ParentResolver;
                case FindAs.Sibling:
                    return SiblingResolver;
                case FindAs.Ancestor:
                    return AncestorResolver;
                case FindAs.ShadowHost:
                    return ShadowHostResolver;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(findAs, nameof(findAs));
            }
        }
    }
}
