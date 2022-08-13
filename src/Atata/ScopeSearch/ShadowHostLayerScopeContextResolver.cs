using OpenQA.Selenium;

namespace Atata
{
    public class ShadowHostLayerScopeContextResolver : ILayerScopeContextResolver
    {
        private const string GetShadowRootChildElementsScript =
@"if (!arguments[0].shadowRoot) {
    throw 'Element doesn\'t have shadowRoot value. Element: ' + arguments[0].outerHTML.replace(arguments[0].innerHTML, '...');
}
var shadowChildren = arguments[0].shadowRoot.children;

for (var i = 0; i < shadowChildren.length; i++) {
    var nodeName = shadowChildren[i].nodeName;

    if (nodeName !== 'STYLE' && nodeName !== 'SCRIPT') {
        return shadowChildren[i];
    }
}

throw 'Element\'s shadowRoot doesn\'t contain any elements.';";

        public string DefaultOuterXPath => "..//";

        public ISearchContext Resolve(IWebElement element) =>
            (IWebElement)AtataContext.Current.Driver.AsScriptExecutor()
                .ExecuteScriptWithLogging(GetShadowRootChildElementsScript, element);
    }
}
