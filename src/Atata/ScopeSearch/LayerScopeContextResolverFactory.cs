namespace Atata
{
    public static class LayerScopeContextResolverFactory
    {
        private static readonly ILayerScopeContextResolver PlainResolver = new PlainLayerScopeContextResolver();

        public static ILayerScopeContextResolver Create(FindAttribute findAttribute)
        {
            return findAttribute.As == FindAs.ShadowHost
                ? new ShadowHostLayerScopeContextResolver(findAttribute.ShadowIndex)
                : PlainResolver;
        }
    }
}
