using System.Reflection;
using Films.Domain.Rooms.BaseRoom;

namespace Films.Infrastructure.Storage.Mappers.StaticMethods;

public class RoomFields
{
    private static readonly Type RoomType = typeof(Room);

    public static readonly FieldInfo Code =
        RoomType.GetField("<Code>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo Viewers =
        RoomType.GetField("_viewers", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo Banned =
        RoomType.GetField("_bannedUsers", BindingFlags.Instance | BindingFlags.NonPublic)!;
    
    public static readonly FieldInfo ServerId =
        RoomType.GetField("<ServerId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;
}