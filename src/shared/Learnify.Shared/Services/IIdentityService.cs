namespace Learnify.Shared.Services;

public interface IIdentityService
{
    public Guid UserId { get; }
    public string UserName { get; }
    public IReadOnlyList<string> GetRoles();
}

public class FakeTokenService : IIdentityService
{
    public Guid UserId => Guid.Parse("332ee8cd-f3f6-49fa-92e2-5fdb188b3377");
    public string UserName => "Cihat34";

    public IReadOnlyList<string> GetRoles() => [];
}