#nullable enable

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
                $"The {nameof(AtataContext)} with Id={_context.Id} is already disposed.");
}
