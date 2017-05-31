namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's location (X and Y).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentLocationProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataProvider<int, TOwner> X => Component.GetOrCreateDataProvider("X location", GetX);

        public DataProvider<int, TOwner> Y => Component.GetOrCreateDataProvider("Y location", GetY);

        private int GetX()
        {
            return Component.Scope.Location.X;
        }

        private int GetY()
        {
            return Component.Scope.Location.Y;
        }
    }
}
