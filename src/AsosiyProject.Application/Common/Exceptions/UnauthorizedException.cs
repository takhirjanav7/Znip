namespace AsosiyProject.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Autentifikatsiya talab qilinadi")
        : base(message) { }
}