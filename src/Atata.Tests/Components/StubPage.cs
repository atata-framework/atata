namespace Atata.Tests
{
    using _ = StubPage;

    public class StubPage : Page<_>
    {
        public DataProvider<bool, _> IsTrue => GetOrCreateDataProvider(nameof(IsTrue), () => true);
    }
}
