#nullable enable

namespace Atata;

public static class ButtonDelegate1Extensions
{
    public static Button<TOwner> GetControl<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        (Button<TOwner>)UIComponentResolver.GetControlByDelegate<TOwner>(clickable);

    public static TOwner Click<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Click();

    public static TOwner Hover<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Hover();

    public static TOwner Focus<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Focus();

    public static TOwner DoubleClick<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().DoubleClick();

    public static TOwner RightClick<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().RightClick();

    public static TOwner ScrollTo<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().ScrollTo();

    public static bool IsEnabled<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().IsEnabled;

    public static bool Exists<TOwner>(this ButtonDelegate<TOwner> clickable, SearchOptions? options = null)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Exists(options);

    public static bool Missing<TOwner>(this ButtonDelegate<TOwner> clickable, SearchOptions? options = null)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Missing(options);

    public static ValueProvider<string, TOwner> Content<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Content;

    public static UIComponentVerificationProvider<Control<TOwner>, TOwner> Should<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().Should;

    public static UIComponentVerificationProvider<Control<TOwner>, TOwner> ExpectTo<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().ExpectTo;

    public static UIComponentVerificationProvider<Control<TOwner>, TOwner> WaitTo<TOwner>(this ButtonDelegate<TOwner> clickable)
        where TOwner : PageObject<TOwner>
        =>
        clickable.GetControl().WaitTo;
}
