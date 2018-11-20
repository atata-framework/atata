namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's size (width and height).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentSizeProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataProvider<int, TOwner> Width => Component.GetOrCreateDataProvider("width", GetWidth);

        public DataProvider<int, TOwner> Height => Component.GetOrCreateDataProvider("height", GetHeight);

        private int GetWidth()
        {
            return Component.Scope.Size.Width;
        }

        private int GetHeight()
        {
            return Component.Scope.Size.Height;
        }
    }
}
