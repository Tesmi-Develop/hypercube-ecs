using System.Runtime.InteropServices;

namespace Hypercube.Ecs.Components;

/// <summary>
/// Metadata describing a single ECS component type.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct ComponentMeta : IEquatable<ComponentMeta>
{
    /// <summary>
    /// Unique identifier for this component type.
    /// </summary>
    public readonly int Id;
    
    /// <summary>
    /// Size in bytes of the component.
    /// </summary>
    public readonly int Size;
    
    /// <summary>
    /// Specifies the type of component.
    /// </summary>
    public readonly ComponentSpecification Specification;

    public Type Type => ComponentRegistry.ResolveType(this);
    
    public ComponentMeta(int id, int size, ComponentSpecification specification = ComponentSpecification.Dynamic)
    {
        Id = id;
        Size = size;
        Specification = specification;
    }

    /// <inheritdoc />
    public bool Equals(ComponentMeta other)
        => Id == other.Id && Size == other.Size;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is ComponentMeta other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(Id, Size);
    
    public static bool operator ==(ComponentMeta left, ComponentMeta right)
        => left.Equals(right);

    public static bool operator !=(ComponentMeta left, ComponentMeta right)
        => !(left == right);
}