using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace App_Api.Authentication;

public sealed class InternalApiKeyAuthenticationHandler
    : AuthenticationHandler<InternalApiKeyAuthenticationOptions>
{
    public InternalApiKeyAuthenticationHandler(
        IOptionsMonitor<InternalApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(
                InternalApiKeyAuthenticationDefaults.HeaderName,
                out var providedValues))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var providedKey = providedValues.ToString();
        if (string.IsNullOrWhiteSpace(providedKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("API key is empty."));
        }

        var expectedBytes = Encoding.UTF8.GetBytes(Options.ApiKey);
        var providedBytes = Encoding.UTF8.GetBytes(providedKey);
        if (expectedBytes.Length != providedBytes.Length
            || !CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes))
        {
            return Task.FromResult(AuthenticateResult.Fail("API key is invalid."));
        }

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "app-view"),
                new Claim(ClaimTypes.Name, "Bazaizai internal service")
            },
            InternalApiKeyAuthenticationDefaults.Scheme);
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(identity),
            InternalApiKeyAuthenticationDefaults.Scheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
