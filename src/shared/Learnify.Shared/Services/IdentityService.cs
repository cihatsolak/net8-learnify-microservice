namespace Learnify.Shared.Services;

internal sealed class 
    IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public Guid UserId => Guid.Parse(GetRequiredClaim(ClaimTypes.NameIdentifier, "User ID"));

    public string UserName => GetRequiredClaim(ClaimTypes.Name, "User name");

    public IReadOnlyList<string> GetRoles() =>
        [.. User.FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))];

    private ClaimsPrincipal User =>
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
            ? httpContextAccessor.HttpContext.User
            : throw new InvalidOperationException("User is not authenticated.");

    private string GetRequiredClaim(string claimType, string claimDescription)
    {
        var value = User.FindFirst(claimType)?.Value;

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"Authenticated user's {claimDescription} claim is missing.");

        return value;
    }
}
