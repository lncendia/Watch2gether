namespace Overoom.Domain.Film.ValueObject;

public class ActorData
{
    internal ActorData(string actorName, string actorDescription)
    {
        ActorName = actorName;
        ActorDescription = actorDescription;
    }

    public string ActorName { get; }
    public string ActorDescription { get; }
}