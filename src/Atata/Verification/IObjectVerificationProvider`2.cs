namespace Atata;

public interface IObjectVerificationProvider<out TObject, out TOwner> : IVerificationProvider<TOwner>
{
    /// <summary>
    /// Gets the object provider that is verified.
    /// </summary>
    IObjectProvider<TObject, TOwner> ObjectProvider { get; }
}
