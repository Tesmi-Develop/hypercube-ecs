using System.Runtime.InteropServices;
using Hypercube.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[StructLayout(LayoutKind.Sequential), Serializable]
public readonly struct Entity : IEquatable<Entity>
{
    public readonly int Id;
    public readonly int Version;

    public Entity(int id, int version)
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

[PublicAPI]
public readonly struct Entity<T> where T : IComponent
{
    public readonly Entity Id;
    public readonly T Component;

    public Entity(Entity id, T component)
    {
        Id = id;
        Component = component;
    }
}

[PublicAPI]
public readonly struct Entity<T1, T2> where T1 : IComponent where T2 : IComponent
{
    public readonly Entity Id;
    public readonly T1 Component1;
    public readonly T2 Component2;

    public Entity(Entity id, T1 component1, T2 component2)
    {
        Id = id;
        Component1 = component1;
        Component2 = component2;
    }
}

// TODO: Roslyn Source Generator
