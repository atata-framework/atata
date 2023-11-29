namespace Atata;

public sealed class SubsequentComponentScopeFindResult : ComponentScopeFindResult
{
    public SubsequentComponentScopeFindResult(ISearchContext scopeSource, IComponentScopeFindStrategy strategy, ComponentScopeFindOptions scopeFindOptions = null)
        : this([scopeSource ?? throw new ArgumentNullException(nameof(scopeSource))], strategy, scopeFindOptions)
    {
    }

    public SubsequentComponentScopeFindResult(IEnumerable<ISearchContext> scopeSources, IComponentScopeFindStrategy strategy, ComponentScopeFindOptions scopeFindOptions = null)
        : this(strategy, scopeFindOptions) =>
        ScopeSources = scopeSources ?? throw new ArgumentNullException(nameof(scopeSources));

    public SubsequentComponentScopeFindResult(By scopeSourceBy, IComponentScopeFindStrategy strategy, ComponentScopeFindOptions scopeFindOptions = null)
        : this(strategy, scopeFindOptions) =>
        ScopeSourceBy = scopeSourceBy ?? throw new ArgumentNullException(nameof(scopeSourceBy));

    private SubsequentComponentScopeFindResult(IComponentScopeFindStrategy strategy, ComponentScopeFindOptions scopeFindOptions)
    {
        Strategy = strategy;
        ScopeFindOptions = scopeFindOptions;
    }

    public IEnumerable<ISearchContext> ScopeSources { get; } = Enumerable.Empty<IWebElement>();

    public By ScopeSourceBy { get; }

    public IComponentScopeFindStrategy Strategy { get; }

    public ComponentScopeFindOptions ScopeFindOptions { get; }
}
