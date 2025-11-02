namespace Learnify.Web.Services;

public sealed class TokenService(IHttpClientFactory httpClientFactory, IdentityOption identityOption, ILogger<TokenService> logger)
{
    public List<Claim> ExtractClaims(string accessToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(accessToken);

        return [.. jwt.Claims];
    }

    public AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse)
    {
        ArgumentNullException.ThrowIfNull(tokenResponse);

        List<AuthenticationToken> tokens = 
        [
            new()
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = tokenResponse.AccessToken
            },
            new()
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = tokenResponse.RefreshToken
            },
            new()
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("O")
            }
        ];

        var properties = new AuthenticationProperties { IsPersistent = true };
        properties.StoreTokens(tokens);

        return properties;
    }

    public async Task<TokenResponse> GetTokensByRefreshToken(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        var client = httpClientFactory.CreateClient("GetTokensByRefreshToken");
        client.BaseAddress = new Uri(identityOption.Address);

        var discoveryResponse = await client.GetDiscoveryDocumentAsync();
        if (discoveryResponse.IsError)
        {
            logger.LogError("Discovery document request failed: {Error}", discoveryResponse.Error);
            throw new InvalidOperationException($"Discovery document request failed: {discoveryResponse.Error}");
        }

        var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = identityOption.Web.ClientId,
            ClientSecret = identityOption.Web.ClientSecret,
            RefreshToken = refreshToken
        });

        if (tokenResponse.IsError)
        {
            logger.LogError("Refresh token request failed: {Error}", tokenResponse.Error);
            throw new InvalidOperationException($"Refresh token request failed: {tokenResponse.Error}");
        }

        return tokenResponse;
    }

    public async Task<TokenResponse> GetClientAccessToken()
    {
        var client = httpClientFactory.CreateClient("GetClientAccessToken");
        client.BaseAddress = new Uri(identityOption.Address);
        var discoveryResponse = await client.GetDiscoveryDocumentAsync();

        if (discoveryResponse.IsError)
        {
            logger.LogError("Discovery document request failed: {Error}", discoveryResponse.Error);
            throw new InvalidOperationException($"Discovery document request failed: {discoveryResponse.Error}");
        }

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = identityOption.Web.ClientId,
            ClientSecret = identityOption.Web.ClientSecret
        });

        if (tokenResponse.IsError)
        {
            logger.LogError("Client credentials token request failed: {Error}", tokenResponse.Error);
            throw new InvalidOperationException($"Client credentials token request failed: {tokenResponse.Error}");
        }

        return tokenResponse;
    }
}