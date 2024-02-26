namespace Overoom.IntegrationEvents.Users;

public class UserChangedIntegrationEvent
{
    public required Guid Id { get; init; }
    
    public required Uri PhotoUrl { get; init; }
    
    public required string Name { get; init; }
}