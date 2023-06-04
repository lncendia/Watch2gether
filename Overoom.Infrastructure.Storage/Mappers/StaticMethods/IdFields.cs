using System.Reflection;
using Overoom.Domain.Abstractions;

namespace Overoom.Infrastructure.Storage.Mappers.StaticMethods;

internal static class IdFields
{
    public static readonly FieldInfo AggregateId =
        typeof(AggregateRoot).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo EntityId =
        typeof(Entity).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static readonly FieldInfo DomainEvents =
        typeof(AggregateRoot).GetField("_domainEvents", BindingFlags.Instance | BindingFlags.NonPublic)!;
}