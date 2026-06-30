using AsosiyProject.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AsosiyProject.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public Guid? UserId
        {
            get
            {
                var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User?.FindFirst("UserId")?.Value;
                return Guid.TryParse(id, out var g) ? g : (Guid?)null;
            }
        }

        public string? UserName => User?.FindFirst(ClaimTypes.Name)?.Value;
        public string? FullName => User?.FindFirst("FullName")?.Value;
        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;
        public string? ProfilePictureUrl => User?.FindFirst("ProfilePictureUrl")?.Value;
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
