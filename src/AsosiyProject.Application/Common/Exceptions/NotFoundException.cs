namespace AsosiyProject.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Resurs topilmadi") { }
    public NotFoundException(string name, object key)
        : base($"{name} ({key}) topilmadi") { }

    public NotFoundException(string message) : base(message) { }
}