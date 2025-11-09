namespace Learnify.Web.Services;

public sealed class UserService(IHttpContextAccessor httpContextAccessor)
{
    private ClaimsPrincipal User => httpContextAccessor.HttpContext?.User
        ?? throw new UnauthorizedAccessException("HttpContext is not available.");

    public Guid UserId
    {
        get
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Guid.Empty;

            return Guid.Parse(User.Claims.First(c => c.Type == "sub").Value);
        }
    }

    public string UserName
    {
        get
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return default;

            return User.Identity!.Name!;
        }
    }

    public IReadOnlyList<string> Roles
    {
        get
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return [];

            return User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList()
                .AsReadOnly();
        }
    }
}

