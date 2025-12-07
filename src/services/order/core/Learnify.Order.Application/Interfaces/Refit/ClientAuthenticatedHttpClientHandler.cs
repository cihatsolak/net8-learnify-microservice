namespace Learnify.Order.Application.Interfaces.Refit;

internal sealed class ClientAuthenticatedHttpClientHandler(
        IServiceProvider serviceProvider,
        IHttpClientFactory httpClientFactory) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        using var scope = serviceProvider.CreateScope();
        var identityOptions = scope.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>();
        var clientSecretOptions = scope.ServiceProvider.GetRequiredService<IOptions<ClientSecretOptions>>();

        var discoveryRequest = new DiscoveryDocumentRequest
        {
            Address = identityOptions.Value.Address,
            Policy = { RequireHttps = false }
        };

        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(identityOptions.Value.Address);

        var discoveryResponse = await client.GetDiscoveryDocumentAsync(discoveryRequest, cancellationToken);
        if (discoveryResponse.IsError)
        {
            throw new EndpointException(client.BaseAddress, $"Discovery document request failed: {discoveryResponse.Error}");
        }

        var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = clientSecretOptions.Value.Id,
            ClientSecret = clientSecretOptions.Value.Secret,
        };

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest, cancellationToken);
        if (tokenResponse.IsError)
        {
            throw new EndpointException(client.BaseAddress, $"Token request failed: {tokenResponse.Error}");
        }

        request.SetBearerToken(tokenResponse.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
