using AsosiyProject.Domain.Entities;

namespace AsosiyProject.Application.DTOs.SignInDtos;

public class UserTokenDto
{
    public Guid UserId { get; set; }

    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }

}