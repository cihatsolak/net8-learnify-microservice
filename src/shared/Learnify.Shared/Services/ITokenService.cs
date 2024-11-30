namespace Learnify.Shared.Services;

public interface ITokenService
{
    public Guid UserId { get; }
    public string UserName { get; }
}

public class FakeTokenService : ITokenService
{
    public Guid UserId => Guid.Parse("332ee8cd-f3f6-49fa-92e2-5fdb188b3377");
    public string UserName => "Cihat34";
}