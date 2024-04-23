using System.Reflection;
using Room.Domain.Abstractions;

namespace Room.Infrastructure.Storage.Mappers.StaticMethods;

internal static class IdFields
{
    public static readonly FieldInfo AggregateId =
        typeof(AggregateRoot).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo DomainEvents =
        typeof(AggregateRoot).GetField("_domainEvents", BindingFlags.Instance | BindingFlags.NonPublic)!;
}