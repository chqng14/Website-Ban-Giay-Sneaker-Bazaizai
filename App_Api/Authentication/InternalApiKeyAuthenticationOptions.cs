using Microsoft.AspNetCore.Authentication;

namespace App_Api.Authentication;

public sealed class InternalApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = string.Empty;
}
