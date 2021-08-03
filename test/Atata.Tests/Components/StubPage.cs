using System.Timers;

namespace Atata.Tests
{
    using _ = StubPage;

    public class StubPage : Page<_>
    {
        private bool _isTrueInASecond;

        private Timer _isTrueInASecondTimer;

        public DataProvider<bool, _> IsTrue => GetOrCreateDataProvider(nameof(IsTrue), () => true);

        public DataProvider<bool, _> IsTrueInASecond => GetOrCreateDataProvider(nameof(IsTrue), GetIsTrueInASecond);

        private bool GetIsTrueInASecond()
        {
            if (!_isTrueInASecond && _isTrueInASecondTimer == null)
            {
                _isTrueInASecondTimer = new Timer(1000)
                {
                    AutoReset = false,
                    Enabled = true
                };

                _isTrueInASecondTimer.Elapsed += (s, e) =>
                {
                    _isTrueInASecond = true;
                    _isTrueInASecondTimer.Dispose();
                    _isTrueInASecondTimer = null;
                };
            }

            return _isTrueInASecond;
        }
    }
}
