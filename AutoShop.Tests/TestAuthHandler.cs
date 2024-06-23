using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AutoShop.Tests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "Test user"),
                new Claim(ClaimTypes.Email, "testuser@gmail.com")
            };

            var headerClaims = GetClaimsBasedOnHttpHeaders(Context);
            claims.AddRange(headerClaims);

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }

        private static IEnumerable<Claim> GetClaimsBasedOnHttpHeaders(HttpContext context)
        {
            const string headerPrefix = "X-Test-";

            var claims = new List<Claim>();

            var claimHeaders = context.Request.Headers.Keys.Where(k => k.StartsWith(headerPrefix));
            foreach (var header in claimHeaders)
            {
                var value = context.Request.Headers[header];
                var claimType = header[headerPrefix.Length..];
                if (!string.IsNullOrEmpty(value))
                {
                    claims.Add(new Claim(claimType == "role" ? ClaimTypes.Role : claimType, value!));
                }
            }
            return claims;
        }

    }
}
