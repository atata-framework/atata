namespace Atata
{
    /// <summary>
    /// Provides the navigation functionality between pages and windows.
    /// </summary>
    public static class Go
    {
        /// <inheritdoc cref="AtataNavigator.To{T}(T, string, bool, bool)"/>
        public static T To<T>(T pageObject = null, string url = null, bool navigate = true, bool temporarily = false)
            where T : PageObject<T>
            =>
            ResolveAtataContext().Go.To(pageObject, url, navigate, temporarily);

        /// <inheritdoc cref="AtataNavigator.ToWindow{T}(T, string, bool)"/>
        public static T ToWindow<T>(T pageObject, string windowName, bool temporarily = false)
            where T : PageObject<T>
            =>
            ResolveAtataContext().Go.ToWindow(pageObject, windowName, temporarily);

        /// <inheritdoc cref="AtataNavigator.ToWindow{T}(string, bool)"/>
        public static T ToWindow<T>(string windowName, bool temporarily = false)
            where T : PageObject<T>
            =>
            ResolveAtataContext().Go.ToWindow<T>(windowName, temporarily);

        /// <inheritdoc cref="AtataNavigator.ToNextWindow{T}(T, bool)"/>
        public static T ToNextWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
            =>
            ResolveAtataContext().Go.ToNextWindow(pageObject, temporarily);

        /// <inheritdoc cref="AtataNavigator.ToPreviousWindow{T}(T, bool)"/>
        public static T ToPreviousWindow<T>(T pageObject = null, bool temporarily = false)
            where T : PageObject<T>
            =>
            ResolveAtataContext().Go.ToPreviousWindow(pageObject, temporarily);

        /// <inheritdoc cref="AtataNavigator.ToUrl(string)"/>
        public static void ToUrl(string url) =>
            ResolveAtataContext().Go.ToUrl(url);

        private static AtataContext ResolveAtataContext() =>
            AtataContext.Current
            ?? AtataContext.Configure().Build();
    }
}
