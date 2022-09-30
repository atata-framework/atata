using System;
using System.Drawing;

namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's size (width and height).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentSizeProvider<TOwner> : ValueProvider<Size, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> _component;

        public UIComponentSizeProvider(UIComponent<TOwner> component, Func<Size> valueGetFunction, string providerName)
            : base(component.Owner, DynamicObjectSource.Create(valueGetFunction), providerName) =>
            _component = component;

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the width of the component.
        /// </summary>
        public ValueProvider<int, TOwner> Width =>
            _component.CreateValueProvider("width", GetWidth);

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the height of the component.
        /// </summary>
        public ValueProvider<int, TOwner> Height =>
            _component.CreateValueProvider("height", GetHeight);

        private int GetWidth() =>
            Value.Width;

        private int GetHeight() =>
            Value.Height;
    }
}
