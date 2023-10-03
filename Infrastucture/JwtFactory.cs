using TourAndTravels.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace TourAndTravels.Infrastucture
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedTokenAsync(ClaimsIdentity identity, int timeOut);
        ClaimsIdentity GenerateClaimsIdentity(User user);
        Task<string> GenerateEncodedTokenAsync(List<Claim> claims, int timeOut);
        ClaimsIdentity GetCookieIdentity(User user, string userName);
    }

    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(JwtIssuerOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public ClaimsIdentity GenerateClaimsIdentity(User user)
        {
            return new ClaimsIdentity(new GenericIdentity(user.Username, "Token"), new[]
            {
                new Claim(JwtClaimIdentifiers.Username, user.Username),
                new Claim(JwtClaimIdentifiers.Firstname, user.Firstname),
                new Claim(JwtClaimIdentifiers.Email, user.Email),
                new Claim(JwtClaimIdentifiers.Lastname, user.Lastname),
                new Claim(JwtClaimIdentifiers.Id, user.UserId.ToString())
            });
        }

        public ClaimsIdentity GetCookieIdentity(User user, string userName)
        {
            return new ClaimsIdentity(new[]
            {
                new Claim(JwtClaimIdentifiers.Username, user.Username),
                new Claim(JwtClaimIdentifiers.Firstname, user.Firstname),
                new Claim(JwtClaimIdentifiers.Email, user.Email),
                new Claim(JwtClaimIdentifiers.Lastname, user.Lastname),
                new Claim(JwtClaimIdentifiers.Id, user.UserId.ToString())

            }, "TourAndTravels");
        }


        public async Task<string> GenerateEncodedTokenAsync(ClaimsIdentity identity, int timeOut)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.FindFirst(JwtClaimIdentifiers.Username)?.Value?.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(JwtClaimIdentifiers.Username),
                identity.FindFirst(JwtClaimIdentifiers.Email),
                identity.FindFirst(JwtClaimIdentifiers.Firstname),
                identity.FindFirst(JwtClaimIdentifiers.Lastname),
                identity.FindFirst(JwtClaimIdentifiers.Id)
            };
            _jwtOptions.ValidFor = TimeSpan.FromMinutes(timeOut + 5);
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<string> GenerateEncodedTokenAsync(List<Claim> claims, int timeOut)
        {
            claims.Remove(claims.Find(f => f.Type.Equals(JwtRegisteredClaimNames.Iat)));
            claims.Insert(2, new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64));

            _jwtOptions.ValidFor = TimeSpan.FromMinutes(timeOut + 5);
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return await Task.FromResult(encodedJwt);
        }

        private static long ToUnixEpochDate(DateTime date)
         => (long)Math.Round((date.ToUniversalTime() -
                              new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                             .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
