namespace Learnify.Bus;

public sealed class BusOptions
{
    public required string HostAddress { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required int Port { get; set; }
}
