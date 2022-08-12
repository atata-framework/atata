namespace Atata
{
    /// <summary>
    /// Sets <see cref="FindSettingsAttribute.Visibility"/> property to <see cref="Visibility.Visible"/>.
    /// Also can define additional finding settings to apply to the targeted control(s).
    /// Adds to or overrides properties of <see cref="FindAttribute"/>.
    /// </summary>
    public class FindOnlyVisibleAttribute : FindSettingsAttribute
    {
        public FindOnlyVisibleAttribute() =>
            Visibility = Visibility.Visible;
    }
}
