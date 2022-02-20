using System.Timers;

namespace Atata.Tests
{
    using _ = StubPage;

    public class StubPage : Page<_>
    {
        private bool _isTrueInASecond;

        private Timer _isTrueInASecondTimer;

        public ValueProvider<bool, _> IsTrue =>
            CreateValueProvider(nameof(IsTrue), () => true);

        public ValueProvider<bool, _> IsTrueInASecond =>
            CreateValueProvider(nameof(IsTrue), GetIsTrueInASecond);

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
