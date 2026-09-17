namespace Atata.IntegrationTests;

public sealed class StubComponent
{
    private readonly Subject<ActualComponent> _subject = new(new ActualComponent());

    public ValueProvider<bool, Subject<ActualComponent>> IsTrue =>
        _subject.DynamicValueOf(x => true);

    public ValueProvider<bool, Subject<ActualComponent>> IsTrueInASecond =>
        _subject.DynamicValueOf(x => x.IsTrueInASecond);

    public sealed class ActualComponent
    {
        private bool _isTrueInASecond;

        private System.Timers.Timer? _isTrueInASecondTimer;

        public bool IsTrueInASecond =>
            GetIsTrueInASecond();

        private bool GetIsTrueInASecond()
        {
            if (!_isTrueInASecond && _isTrueInASecondTimer is null)
            {
                _isTrueInASecondTimer = new(1000)
                {
                    AutoReset = false,
                    Enabled = true
                };

                _isTrueInASecondTimer.Elapsed += (_, _) =>
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
