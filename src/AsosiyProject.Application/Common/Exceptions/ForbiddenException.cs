namespace AsosiyProject.Application.Common.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "Bu amalni bajarishga ruxsatingiz yo‘q")
        : base(message) { }
}