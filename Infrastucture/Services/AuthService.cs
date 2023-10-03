using TourAndTravels.Data.Repositories;
using TourAndTravels.Domain;
using System.Security.Claims;

namespace TourAndTravels.Infrastucture.Services
{
    public interface IAuthService
    {
        Task<ResponseContext<string>> Authenticate(LoginViewModel model);
    }

    public class AuthService : IAuthService
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;

        public AuthService(IJwtFactory jwtFactory, IHttpContextAccessor httpContext, IUserRepository userRepository)
        {
            _jwtFactory = jwtFactory;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        public async Task<ResponseContext<string>> Authenticate(LoginViewModel model)
        {
            var user = await this._userRepository.Authenticate(model);
            if (user == null)
                return new ResponseContext<string>("Invalid Username and Password", System.Net.HttpStatusCode.BadRequest);

            var identity = _jwtFactory.GenerateClaimsIdentity(user);

            var principal = new ClaimsPrincipal(identity);

            Thread.CurrentPrincipal = principal;
            _httpContext.HttpContext.User = principal;

            return ResponseContext<string>.Success(await _jwtFactory.GenerateEncodedTokenAsync(identity, 120));
        }
    }
}
