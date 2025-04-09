namespace Atata;

/// <summary>
/// Provides the navigation functionality between pages and windows.
/// </summary>
public static class Go
{
    /// <inheritdoc cref="WebDriverSessionNavigator.On{T}"/>
    public static T On<T>()
       where T : PageObject<T> =>
        ResolveWebDriverSession().Go.On<T>();

    /// <inheritdoc cref="WebDriverSessionNavigator.OnRefreshed{T}"/>
    public static T OnRefreshed<T>()
       where T : PageObject<T> =>
        ResolveWebDriverSession().Go.OnRefreshed<T>();

    /// <inheritdoc cref="WebDriverSessionNavigator.OnOrTo{T}"/>
    public static T OnOrTo<T>()
       where T : PageObject<T> =>
        ResolveWebDriverSession().Go.OnOrTo<T>();

    /// <inheritdoc cref="WebDriverSessionNavigator.To{T}(T, string, bool, bool)"/>
    public static T To<T>(T? pageObject = null, string? url = null, bool navigate = true, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.To(pageObject, url, navigate, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToWindow{T}(T, string, bool)"/>
    public static T ToWindow<T>(T pageObject, string windowName, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.ToWindow(pageObject, windowName, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToWindow{T}(string, bool)"/>
    public static T ToWindow<T>(string windowName, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.ToWindow<T>(windowName, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToNextWindow{T}(T, bool)"/>
    public static T ToNextWindow<T>(T? pageObject = null, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.ToNextWindow(pageObject, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToPreviousWindow{T}(T, bool)"/>
    public static T ToPreviousWindow<T>(T? pageObject = null, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.ToPreviousWindow(pageObject, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToNewWindowAsTab{T}(T, string, bool)"/>
    public static T ToNewWindowAsTab<T>(T? pageObject = null, string? url = null, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.ToNewWindowAsTab(pageObject, url, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToNewWindow{T}(T, string, bool)"/>
    public static T ToNewWindow<T>(T? pageObject = null, string? url = null, bool temporarily = false)
        where T : PageObject<T> =>
        ResolveWebDriverSession().Go.ToNewWindow(pageObject, url, temporarily);

    /// <inheritdoc cref="WebDriverSessionNavigator.ToUrl(string)"/>
    public static void ToUrl(string url) =>
        ResolveWebDriverSession().Go.ToUrl(url);

    private static WebDriverSession ResolveWebDriverSession() =>
        AtataContext.ResolveCurrent().Sessions.Get<WebDriverSession>();
}
