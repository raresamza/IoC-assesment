
namespace IoC_asesment.Exceptions;

public class NoSessionsApprovedException : Exception
{
    public NoSessionsApprovedException(string message) : base(message) { }
}
