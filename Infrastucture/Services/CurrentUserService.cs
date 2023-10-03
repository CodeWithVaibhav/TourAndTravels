using System.Security.Claims;

namespace TourAndTravels.Infrastucture.Services
{
    public interface ICurrentUserService
    {
        public string Username { get; }
        public string Firstname { get; }
        public string Email { get; }
        public string Lastname { get; }
        public string UserId { get; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string Username => _httpContext.HttpContext?.User?.FindFirstValue(JwtClaimIdentifiers.Username);

        public string Firstname => _httpContext.HttpContext?.User?.FindFirstValue(JwtClaimIdentifiers.Firstname);

        public string Email => _httpContext.HttpContext?.User?.FindFirstValue(JwtClaimIdentifiers.Email);

        public string Lastname => _httpContext.HttpContext?.User?.FindFirstValue(JwtClaimIdentifiers.Lastname);

        public string UserId => _httpContext.HttpContext?.User?.FindFirstValue(JwtClaimIdentifiers.Id);
    }
}
