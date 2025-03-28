#nullable enable

namespace Atata;

public class PageObjectVerificationProvider<TPageObject> :
    UIComponentVerificationProvider<TPageObject, PageObjectVerificationProvider<TPageObject>, TPageObject>,
    IPageObjectVerificationProvider<TPageObject>
    where TPageObject : PageObject<TPageObject>
{
    public PageObjectVerificationProvider(TPageObject pageObject)
        : base(pageObject)
    {
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public NegationPageObjectVerificationProvider Not =>
        new(Owner, this);

    public class NegationPageObjectVerificationProvider :
        NegationUIComponentVerificationProvider<NegationPageObjectVerificationProvider>,
        IPageObjectVerificationProvider<TPageObject>
    {
        internal NegationPageObjectVerificationProvider(TPageObject pageObject, IVerificationProvider<TPageObject> verificationProvider)
            : base(pageObject, verificationProvider)
        {
        }
    }
}
