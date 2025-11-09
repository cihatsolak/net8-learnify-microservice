namespace Learnify.Web.DelegateHandlers;

public sealed class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor, TokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
            return await base.SendAsync(request, cancellationToken);

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
            return await base.SendAsync(request, cancellationToken);

        var accessToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new UnauthorizedAccessException("Access token is missing.");

        request.SetBearerToken(accessToken);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        return await HandleUnauthorizedAsync(request, httpContext, cancellationToken);
    }

    private async Task<HttpResponseMessage> HandleUnauthorizedAsync(HttpRequestMessage request, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var refreshToken = await httpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new UnauthorizedAccessException("Refresh token is missing.");

        var tokenResponse = await tokenService.GetTokensByRefreshToken(refreshToken);
        if (tokenResponse.IsError)
            throw new UnauthorizedAccessException("Failed to refresh access token.");

        await UpdateAuthenticationCookiesAsync(httpContext, tokenResponse);

        request.SetBearerToken(tokenResponse.AccessToken!);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task UpdateAuthenticationCookiesAsync(HttpContext httpContext, TokenResponse tokenResponse)
    {
        var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse);
        var existingClaims = httpContext.User.Claims;

        var identity = new ClaimsIdentity(
            existingClaims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ClaimTypes.Role);

        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authenticationProperties);
    }
}

