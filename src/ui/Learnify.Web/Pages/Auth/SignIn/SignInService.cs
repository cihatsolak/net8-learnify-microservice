namespace Learnify.Web.Pages.Auth.SignIn;

public class SignInService(
    IHttpContextAccessor contextAccessor,
    TokenService tokenService,
    IdentityOption identityOption,
    HttpClient client,
    ILogger<SignInService> logger)
{
    public async Task<ServiceResult> AuthenticateAsync(SignInViewModel signInViewModel)
    {
        var tokenResponse = await RequestAccessTokenAsync(signInViewModel);
        if (tokenResponse.IsError)
        {
            logger.LogWarning("Access token alınamadı. Hata: {Error}, Açıklama: {Description}",
                tokenResponse.Error, tokenResponse.ErrorDescription);

            return ServiceResult.Error(tokenResponse.Error, tokenResponse.ErrorDescription);
        }

        var userClaims = tokenService.ExtractClaims(tokenResponse.AccessToken);
        var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse);

        var claimsIdentity = new ClaimsIdentity(
            userClaims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name,
            ClaimTypes.Role
        );

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await contextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            authenticationProperties
        );

        return ServiceResult.Success();
    }

    private async Task<TokenResponse> RequestAccessTokenAsync(SignInViewModel signInViewModel)
    {
        var discoveryRequest = new DiscoveryDocumentRequest
        {
            Address = identityOption.Address,
            Policy = { RequireHttps = false }
        };

        client.BaseAddress = new Uri(identityOption.Address);

        var discoveryResponse = await client.GetDiscoveryDocumentAsync(discoveryRequest);
        if (discoveryResponse.IsError)
        {
            logger.LogError("Discovery document alınamadı. Hata: {Error}", discoveryResponse.Error);
            throw new InvalidOperationException($"Discovery document alınamadı: {discoveryResponse.Error}");
        }

        var tokenRequest = new PasswordTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = identityOption.Web.ClientId,
            ClientSecret = identityOption.Web.ClientSecret,
            UserName = signInViewModel.Email,
            Password = signInViewModel.Password,
            Scope = "offline_access"
        };

        return await client.RequestPasswordTokenAsync(tokenRequest);
    }
}
