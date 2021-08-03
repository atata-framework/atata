namespace Atata
{
    public static class LayerScopeContextResolverFactory
    {
        private static readonly ILayerScopeContextResolver s_parentResolver = new PlainLayerScopeContextResolver("./");

        private static readonly ILayerScopeContextResolver s_siblingResolver = new PlainLayerScopeContextResolver("../");

        private static readonly ILayerScopeContextResolver s_ancestorResolver = new PlainLayerScopeContextResolver(".//");

        private static readonly ILayerScopeContextResolver s_shadowHostResolver = new ShadowHostLayerScopeContextResolver();

        public static ILayerScopeContextResolver Create(FindAs findAs)
        {
            switch (findAs)
            {
                case FindAs.Parent:
                    return s_parentResolver;
                case FindAs.Sibling:
                    return s_siblingResolver;
                case FindAs.Ancestor:
                    return s_ancestorResolver;
                case FindAs.ShadowHost:
                    return s_shadowHostResolver;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(findAs, nameof(findAs));
            }
        }
    }
}
