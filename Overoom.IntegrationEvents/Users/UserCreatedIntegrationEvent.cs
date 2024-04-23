namespace Overoom.IntegrationEvents.Users;

public class UserCreatedIntegrationEvent
{
    public required Guid Id { get; init; }

    public Uri? PhotoUrl { get; init; }

    public required string Name { get; init; }

    public required string Email { get; init; }
    
    public required string Locale { get; init; }
}