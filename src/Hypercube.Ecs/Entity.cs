using System.Runtime.InteropServices;
using Hypercube.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[StructLayout(LayoutKind.Sequential), Serializable]
public readonly struct Entity : IEquatable<Entity>
{
    public static readonly Entity Invalid = new(-1, -1);
    
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
