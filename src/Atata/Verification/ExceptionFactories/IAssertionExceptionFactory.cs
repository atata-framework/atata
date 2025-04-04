namespace Atata;

public interface IAssertionExceptionFactory
{
    Exception Create(string message, Exception? innerException);
}
