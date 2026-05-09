using System.Runtime.InteropServices;
using Hypercube.Ecs.Attributes;

namespace Hypercube.Ecs;

[StructLayout(LayoutKind.Sequential), GenerateEntityPack(15), Serializable]
public readonly partial struct Entity : IEquatable<Entity>
{
    public const int InvalidId = -1;
    public const int InvalidVersion = -1;
    
    public static readonly Entity Invalid = new(InvalidId, InvalidVersion);
    
    public readonly EntityId Id;
    public readonly int Version;

    public Entity(EntityId id, int version)
    {
        Id = id;
        Version = version;
    }

    public bool Equals(Entity other) => Id == other.Id && Version == other.Version;

    public override bool Equals(object? obj) => obj is Entity other && Equals(other);

    public override int GetHashCode() => (Id << 16) ^ Version;

    public static bool operator ==(Entity left, Entity right) => left.Equals(right);

    public static bool operator !=(Entity left, Entity right) => !(left == right);
}
