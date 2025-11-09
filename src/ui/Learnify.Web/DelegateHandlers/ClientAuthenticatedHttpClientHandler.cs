namespace Learnify.Web.DelegateHandlers;

public sealed class ClientAuthenticatedHttpClientHandler(
    IHttpContextAccessor httpContextAccessor,
    TokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            return await base.SendAsync(request, cancellationToken);

        var tokenResponse = await tokenService.GetClientAccessToken();
        if (tokenResponse.IsError)
            throw new UnauthorizedAccessException($"Failed to obtain client access token: {tokenResponse.Error}");

        request.SetBearerToken(tokenResponse.AccessToken!);

        return await base.SendAsync(request, cancellationToken);
    }
}
