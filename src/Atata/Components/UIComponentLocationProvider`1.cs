using System;
using System.Drawing;

namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's location (X and Y).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentLocationProvider<TOwner> : DataProvider<Point, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public UIComponentLocationProvider(UIComponent<TOwner> component, Func<Point> valueGetFunction)
            : base(component, valueGetFunction, "location")
        {
        }

        /// <summary>
        /// Gets the X location coordinate.
        /// </summary>
        public DataProvider<int, TOwner> X =>
            Component.GetOrCreateDataProvider("X location", GetX);

        /// <summary>
        /// Gets the Y location coordinate.
        /// </summary>
        public DataProvider<int, TOwner> Y =>
            Component.GetOrCreateDataProvider("Y location", GetY);

        private int GetX() =>
            Value.X;

        private int GetY() =>
            Value.Y;
    }
}
