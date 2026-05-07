using Hypercube.Ecs.Archetypes;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

/// <summary>
/// Packed entity world location data.
/// </summary>
[PublicAPI]
public readonly struct EntityLocation : IEquatable<EntityLocation>
{
    public readonly ChunkLocation Chunk;
    public readonly int LocalIndex;
    
    public int ArchetypeIndex => Chunk.ArchetypeIndex;
    public int ChunkIndex => Chunk.ChunkIndex;
    
    public EntityLocation(int archetypeIndex, int chunkIndex, int localIndex)
    {
        Chunk = new ChunkLocation(archetypeIndex, chunkIndex);
        LocalIndex = localIndex;
    }

    public bool Equals(EntityLocation other)
        => Chunk.Equals(other.Chunk) && LocalIndex == other.LocalIndex;

    public override bool Equals(object? obj)
        => obj is EntityLocation other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Chunk, LocalIndex);
    
    public static bool operator ==(EntityLocation left, EntityLocation right)
        => left.Equals(right);

    public static bool operator !=(EntityLocation left, EntityLocation right)
        => !(left == right);
}

public readonly record struct ChunkLocation(int ArchetypeIndex, int ChunkIndex)
{
    public static readonly ChunkLocation Null = new(-1, -1);
}

public readonly record struct ChunkEntity(int ChunkIndex, int Index)
{
    public static readonly ChunkEntity Null = new(-1, -1);
}
