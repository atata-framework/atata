namespace Atata
{
    public static class LayerScopeContextResolverFactory
    {
        private static readonly ILayerScopeContextResolver PlainResolver = new PlainLayerScopeContextResolver();

        private static readonly ILayerScopeContextResolver ShadowHostResolver = new ShadowHostLayerScopeContextResolver();

        public static ILayerScopeContextResolver Create(FindAs findAs)
        {
            return findAs == FindAs.ShadowHost
                ? ShadowHostResolver
                : PlainResolver;
        }
    }
}
