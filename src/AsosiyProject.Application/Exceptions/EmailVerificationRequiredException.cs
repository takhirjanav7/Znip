namespace AsosiyProject.Application.Exceptions;

public class EmailVerificationRequiredException : Exception
{
    public Guid UserId { get; }

    public EmailVerificationRequiredException(Guid userId)
        : base("Elektron pochta manzili tasdiqlanmagan. Iltimos, emailingizni tekshiring.")
    {
        UserId = userId;
    }
}