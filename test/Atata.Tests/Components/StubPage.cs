using System.Timers;

namespace Atata.Tests
{
    using _ = StubPage;

    public class StubPage : Page<_>
    {
        private bool isTrueInASecond;

        private Timer isTrueInASecondTimer;

        public DataProvider<bool, _> IsTrue => GetOrCreateDataProvider(nameof(IsTrue), () => true);

        public DataProvider<bool, _> IsTrueInASecond => GetOrCreateDataProvider(nameof(IsTrue), GetIsTrueInASecond);

        private bool GetIsTrueInASecond()
        {
            if (!isTrueInASecond && isTrueInASecondTimer == null)
            {
                isTrueInASecondTimer = new Timer(1000)
                {
                    AutoReset = false,
                    Enabled = true
                };

                isTrueInASecondTimer.Elapsed += (s, e) =>
                {
                    isTrueInASecond = true;
                    isTrueInASecondTimer.Dispose();
                    isTrueInASecondTimer = null;
                };
            }

            return isTrueInASecond;
        }
    }
}
