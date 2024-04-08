
namespace IoC_asesment.Exceptions;

public class SpeakerRegistrationFailedException : Exception
{
    public SpeakerRegistrationFailedException(string message, Exception innerException) : base(message, innerException) { }
}
