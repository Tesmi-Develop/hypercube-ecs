using System.Runtime.InteropServices;
using Hypercube.Ecs.Attributes;

namespace Hypercube.Ecs;

[StructLayout(LayoutKind.Sequential), GenerateEntityPack(15), Serializable]
public readonly partial struct Entity : IEquatable<Entity>
{
    public const int InvalidId = -1;
    
    public const int InvalidVersion = -1;
    public const int QueryVersion = -2;
    
    public static readonly Entity Invalid = new(InvalidId, InvalidVersion);
    
    public readonly int Id;
    public readonly int Version;

    public Entity(int id, int version)
    {
        Id = id;
        Version = version;
    }

    public bool Equals(Entity other)
    {
        return Id == other.Id && Version == other.Version;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (Id << 16) ^ Version;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
