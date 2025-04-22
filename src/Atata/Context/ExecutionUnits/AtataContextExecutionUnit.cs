namespace Atata;

internal sealed class AtataContextExecutionUnit : IAtataExecutionUnit
{
    private readonly AtataContext _context;

    internal AtataContextExecutionUnit(AtataContext context) =>
        _context = context;

    public AtataContext Context =>
        EnsureIsActive();

    public ILogManager Log =>
        EnsureIsActive().Log;

    public bool IsActive =>
        _context.IsActive;

    private AtataContext EnsureIsActive() =>
        _context.IsActive
            ? _context
            : throw new InvalidOperationException(
                $"The {_context} is already disposed.");
}
