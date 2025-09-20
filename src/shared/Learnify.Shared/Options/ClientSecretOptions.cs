namespace Learnify.Shared.Options;

public sealed record ClientSecretOptions
{
    public required string Id { get; set; }
    public required string Secret { get; set; }
}
