using System;
using System.Drawing;

namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's size (width and height).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentSizeProvider<TOwner> : DataProvider<Size, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public UIComponentSizeProvider(UIComponent<TOwner> component, Func<Size> valueGetFunction)
            : base(component, valueGetFunction, "size")
        {
        }

        /// <summary>
        /// Gets the width of the component.
        /// </summary>
        public DataProvider<int, TOwner> Width =>
            Component.GetOrCreateDataProvider("width", GetWidth);

        /// <summary>
        /// Gets the height of the component.
        /// </summary>
        public DataProvider<int, TOwner> Height =>
            Component.GetOrCreateDataProvider("height", GetHeight);

        private int GetWidth() =>
            Value.Width;

        private int GetHeight() =>
            Value.Height;
    }
}
