using Microsoft.AspNetCore.Mvc;
using TourAndTravels.Domain;
using TourAndTravels.Infrastucture.Services;

namespace TourAndTravels.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet("health")]
        public async Task<IActionResult> Get() =>
            Ok(await Task.FromResult("Running!!!"));

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _authService.Authenticate(model);
            if (response.IsError)
                return BadRequest(response);

            this._logger.LogInformation($"The user {model.Domain}/{model.Username} logged into system at {DateTime.Now}");

            SetTokenCookie(response.Content);

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                IsEssential = true,
                Expires = DateTime.Now.AddDays(-1)
            };

            Response.Cookies.Delete(CookieNames.Auth, cookieOptions);

            await Task.CompletedTask;
            return Ok();
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                IsEssential = true,
                Secure = false,
            };
            Response.Cookies.Append(CookieNames.Auth, token, cookieOptions);
        }
    }
}
