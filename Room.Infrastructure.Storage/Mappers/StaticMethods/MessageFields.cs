using System.Reflection;
using Room.Domain.Messages.Messages;

namespace Room.Infrastructure.Storage.Mappers.StaticMethods;

public class MessageFields
{
    public static readonly Type MessageType = typeof(Message);
    
    public static readonly FieldInfo RoomId =
        MessageType.GetField("<RoomId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo UserId =
        MessageType.GetField("<UserId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo CreatedAt =
        MessageType.GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo Text =
        MessageType.GetField("<Text>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
}