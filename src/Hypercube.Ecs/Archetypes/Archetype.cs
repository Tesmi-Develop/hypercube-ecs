using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hypercube.Ecs.Components;
using Hypercube.Utilities.Collections.Bit;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Archetypes;

/// <summary>
/// Represents a unique combination of components (archetype).
/// Entities with the same set of components belong to the same archetype.
/// Uses arrays instead of lists to minimize allocations.
/// </summary>
[PublicAPI]
[DebuggerDisplay("{Signature.ComponentNames}")]
public sealed class Archetype
{
    public const int DefaultChuckCount = 4;
    
    public readonly Signature Signature;
    public readonly BitSet BitSet;
    
    private ArchetypeChunk[] _chunks = new ArchetypeChunk[DefaultChuckCount];
    private int _chunkCount;
        
    private int[] _availableChunks = new int[DefaultChuckCount];
    private int _availableChunkCount;
    
    public int EntityCount { get; private set; }
    
    public ReadOnlySpan<ArchetypeChunk> Chunks => new(_chunks, 0, _chunkCount);
    
    public Archetype(Signature signature)
    {
        Signature = signature;
        BitSet = signature.AsBitSet();
    }

    /// <summary>
    /// Adds an entity to this archetype, returning the chunk and local index.
    /// </summary>
    public ChunkEntity Add(EntityId entityId)
    {
        var chunk = _availableChunkCount == 0 
            ? CreateChunk() 
            : _chunks[_availableChunks[0]];
        
        var localIndex = chunk.Add(entityId);
        
        if (chunk.Full)
            RemoveAvailable(chunk.ArchetypeIndex);

        EntityCount++;
        return new ChunkEntity(chunk.ArchetypeIndex, localIndex);
    }

    /// <summary>
    /// Removes an entity from this archetype.
    /// </summary>
    public bool RemoveAt(int chunkIndex, int index, out EntityId movedEntity)
    {
        var chunk = _chunks[chunkIndex];
        var wasFull = chunk.Full;

        EntityCount--;
        
        if (chunk.RemoveAt(index, out movedEntity))
        {
            if (wasFull)
                AddAvailable(chunk.ArchetypeIndex);

            return true;
        }

        if (wasFull)
            AddAvailable(chunk.ArchetypeIndex);
        
        return false;
    }
    
    /// <summary>
    /// Returns an enumerator over all entities in this archetype.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new(this);

    private ArchetypeChunk CreateChunk()
    {
        var chunkId = _chunkCount++;
        var instance = new ArchetypeChunk(this, chunkId);

        while (_chunkCount >= _chunks.Length)
            Array.Resize(ref _chunks, _chunks.Length * 2);
        
        _chunks[chunkId] = instance;
        AddAvailable(chunkId);
        
        return instance;
    }

    private void AddAvailable(int chunkIndex)
    {
        Debug.Assert(_chunks[chunkIndex] is not null);
        Debug.Assert(!_chunks[chunkIndex].Full);

        while (chunkIndex >= _availableChunks.Length)
            Array.Resize(ref _availableChunks, _availableChunks.Length * 2);

        var index = _availableChunkCount++;
        
        _availableChunks[index] = chunkIndex;
    }

    private void RemoveAvailable(int chunkIndex)
    {
        Debug.Assert(_chunks[chunkIndex] is not null);
        
        for (var i = 0; i < _availableChunkCount; i++)
        {
            var element = _availableChunks[i];
            if (element != chunkIndex)
                continue;

            var lastIndex = --_availableChunkCount;
            if (lastIndex == i)
                continue;
            
            _availableChunks[i] = _availableChunks[lastIndex];
            return;
        }

        // We're skipping the receipt that wasn't available
        // it's still unavailable - that's normal
    }

    /// <summary>
    /// Fast enumerator over all entities in the archetype.
    /// </summary>
    [PublicAPI]
    public ref struct Enumerator
    {
        private readonly Archetype _archetype;
        private readonly bool _initialized;
        
        private int _chunkIndex;
        private ArchetypeChunk.Enumerator _chunkEnumerator;

        public int CurrentEntityId => _chunkEnumerator.CurrentEntityId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator(Archetype archetype)
        {
            _archetype = archetype;
            _initialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (!_initialized)
                return false;
            
            while (true)
            {
                if (_chunkEnumerator.MoveNext())
                    return true;

                if (_chunkIndex >= _archetype._chunkCount)
                    return false;

                _chunkEnumerator = _archetype._chunks[_chunkIndex++].GetEnumerator();
            }
        }
    }
}