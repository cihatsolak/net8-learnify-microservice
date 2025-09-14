namespace Learnify.Bus.Events;

public sealed record OrderCreatedEvent(Guid OrderId, Guid UserId);
