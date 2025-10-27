namespace Learnify.Web.Pages.Auth.SignUp;

public sealed class SignUpService(IdentityOption identityOption, HttpClient client, ILogger<SignUpService> logger)
{
    public async Task<ServiceResult> CreateAccountAsync(SignUpViewModel model)
    {
        try
        {
            string token = await GetAdminAccessTokenAsync();
            string requestUrl = $"{identityOption.BaseAddress}/admin/realms/LearnifyTenant/users";

            client.SetBearerToken(token);

            var userRequest = BuildUserCreateRequest(model);
            var response = await client.PostAsJsonAsync(requestUrl, userRequest);

            if (!response.IsSuccessStatusCode)
            {
                return await HandleCreateAccountErrorAsync(response);
            }

            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred during account creation.");
            return ServiceResult.Error("An unexpected error occurred. Please try again later.");
        }
    }

    private static UserCreateRequest BuildUserCreateRequest(SignUpViewModel model)
    {
        return new UserCreateRequest(
            model.UserName,
            true,
            model.FirstName,
            model.LastName,
            model.Email,
            Credentials: [new Credential("password", model.Password, false)]);
    }

    private async Task<ServiceResult> HandleCreateAccountErrorAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            string internalError = await response.Content.ReadAsStringAsync();
            logger.LogError("Internal server error during user creation: {Error}", internalError);
            return ServiceResult.Error("A system error occurred. Please try again later.");
        }

        var keycloakError = await response.Content.ReadFromJsonAsync<KeycloakErrorResponse>();
        string errorMessage = keycloakError?.ErrorMessage ?? "User creation failed.";
        return ServiceResult.Error(errorMessage);
    }

    private async Task<string> GetAdminAccessTokenAsync()
    {
        client.BaseAddress = new Uri(identityOption.Address);

        var discovery = await client.GetDiscoveryDocumentAsync();
        if (discovery.IsError)
        {
            logger.LogError("Discovery document request failed: {Error}", discovery.Error);
            throw new InvalidOperationException("Failed to obtain Keycloak discovery document.");
        }

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discovery.TokenEndpoint,
            ClientId = identityOption.Admin.ClientId,
            ClientSecret = identityOption.Admin.ClientSecret
        });

        if (tokenResponse.IsError)
        {
            logger.LogError("Token request failed: {Error}", tokenResponse.Error);
            throw new InvalidOperationException("Failed to obtain access token from Keycloak.");
        }

        return tokenResponse.AccessToken;
    }

}
