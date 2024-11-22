namespace Atata;

/// <summary>
/// Defines the modes of <see cref="AtataContext.Current"/> property.
/// </summary>
public enum AtataContextModeOfCurrent
{
    /// <summary>
    /// The <see cref="AtataContext.Current"/> value is thread-static (unique for each thread).
    /// </summary>
    ThreadStatic = 1,

    /// <summary>
    /// The <see cref="AtataContext.Current"/> value is static (same for all threads).
    /// </summary>
    Static,

    /// <summary>
    /// The <see cref="AtataContext.Current"/> value is unique for each given asynchronous control flow.
    /// Basically, <c>AsyncLocal&lt;AtataContext&gt;</c> construction is used under the hood.
    /// </summary>
    AsyncLocal,

    /// <summary>
    /// The <see cref="AtataContext.Current"/> value is boxed for asynchronous control flow.
    /// Basically, <c>AsyncLocal&lt;StrongBox&lt;AtataContext&gt;&gt;</c> construction is used under the hood.
    /// </summary>
    AsyncLocalBoxed
}
