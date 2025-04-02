#nullable enable

namespace Atata;

public sealed class UseParentScopeAttribute : FindAttribute
{
    public new int Index => base.Index;

    protected override Type DefaultStrategy => typeof(UseParentScopeStrategy);
}
