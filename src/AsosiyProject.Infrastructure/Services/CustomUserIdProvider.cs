using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AsosiyProject.Infrastructure.Services;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // JWT ichidagi UserId ni oladi
        return connection.User?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value;
    }
}