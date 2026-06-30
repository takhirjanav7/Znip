namespace AsosiyProject.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }           // login qilmagan bo‘lsa null
    string? UserName { get; }
    string? FullName { get; }
    string? Email { get; }
    string? ProfilePictureUrl { get; }
    bool IsAuthenticated { get; }   // login qilganmi?
}