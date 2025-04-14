namespace Atata;

public static class LayerScopeContextResolverFactory
{
    private static readonly ILayerScopeContextResolver s_parentResolver = new PlainLayerScopeContextResolver("./");

    private static readonly ILayerScopeContextResolver s_siblingResolver = new PlainLayerScopeContextResolver("../");

    private static readonly ILayerScopeContextResolver s_ancestorResolver = new PlainLayerScopeContextResolver(".//");

    private static readonly ILayerScopeContextResolver s_shadowHostResolver = new ShadowHostLayerScopeContextResolver();

    public static ILayerScopeContextResolver Create(FindAs findAs) =>
        findAs switch
        {
            FindAs.Parent => s_parentResolver,
            FindAs.Sibling => s_siblingResolver,
            FindAs.Ancestor => s_ancestorResolver,
            FindAs.ShadowHost => s_shadowHostResolver,
            _ => throw Guard.CreateArgumentExceptionForUnsupportedValue(findAs)
        };
}
