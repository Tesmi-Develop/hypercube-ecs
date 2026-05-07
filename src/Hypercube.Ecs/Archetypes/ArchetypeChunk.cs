using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Archetypes;

/// <summary>
/// A chunk of entities that share the same archetype.
/// Stores component data in column-oriented format for cache efficiency.
/// Uses arrays for minimal allocations.
/// </summary>
[PublicAPI]
public sealed class ArchetypeChunk
{
    public const int Capacity = 256;
    
    public readonly Archetype Archetype;
    public readonly int ArchetypeIndex;

    private readonly EntityId[] _entities = new EntityId[Capacity];
    
    public int Count { get; private set; }
    
    public ReadOnlySpan<EntityId> Entities => new(_entities, 0, Count);
    
    public bool Full => Count >= Capacity;
    
    public ArchetypeChunk(Archetype archetype, int index)
    {
        Archetype = archetype;
        ArchetypeIndex = index;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Add(EntityId entity)
    {
        var index = Count++;
        
        _entities[index] = entity;
        
        return index;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int RemoveAt(int index)
    {
        // Swap deletion, last element replace index
        // Example: [A, B, C, D]
        // Remove B -> [A, D, C]
        
        var lastIndex = --Count;
        if (index == lastIndex)
            return -1;

        _entities[index] = _entities[lastIndex];
        return lastIndex;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new(this);
    
    [PublicAPI]
    public ref struct Enumerator
    {
        private readonly ArchetypeChunk _chunk;
        private readonly bool _initialized;
        
        private int _index;
        
        public int CurrentEntityId => _chunk._entities[_index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator(ArchetypeChunk chunk)
        {
            _chunk = chunk;
            _index = -1;
            _initialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _initialized && ++_index < _chunk.Count;
    }
}
