using Hypercube.Ecs.Archetypes;
using Hypercube.Ecs.Components;

namespace Hypercube.Ecs;

public partial class World
{
    // Archetype system - uses arrays for better cache locality
    private Archetype[] _archetypes = new Archetype[16];
    private int _archetypeCount;
    
    // Empty archetype for entities with no components
    private readonly Archetype _emptyArchetype;
    
    /// <summary>
    /// Gets the archetype for an entity.
    /// </summary>
    public Archetype GetEntityArchetype(Entity entity)
    {
        if (entity.Id >= _entityLocations.Length)
            return _emptyArchetype;
        
        var location = _entityLocations[entity.Id];
        
        return location.ArchetypeIndex < _archetypeCount
            ? _archetypes[location.ArchetypeIndex]
            : _emptyArchetype;
    }

    /// <summary>
    /// Gets or creates an archetype with the given signature.
    /// </summary>
    public Archetype GetOrCreateArchetype(Signature signature)
    {
        // Linear search - archetypes are typically few
        for (var i = 0; i < _archetypeCount; i++)
        {
            if (_archetypes[i].Signature == signature)
                return _archetypes[i];
        }

        // Create new archetype
        var archetype = new Archetype(signature);
        
        // Expand if needed
        if (_archetypeCount >= _archetypes.Length)
        {
            var newArchetypes = new Archetype[_archetypes.Length * 2];
            Array.Copy(_archetypes, newArchetypes, _archetypes.Length);
            _archetypes = newArchetypes;
        }
        
        _archetypes[_archetypeCount++] = archetype;
        
        // Empty archetype is always at index 0
        if (signature != Signature.Empty)
        {
            // Move empty archetype to front if needed
            if (_archetypeCount > 1 && _archetypes[0] != _emptyArchetype)
            {
                // Swap
                for (int i = 0; i < _archetypeCount; i++)
                {
                    if (_archetypes[i] == _emptyArchetype)
                    {
                        (_archetypes[0], _archetypes[i]) = (_archetypes[i], _archetypes[0]);
                        break;
                    }
                }
            }
        }
        
        return archetype;
    }

    /// <summary>
    /// Gets all archetypes in the world.
    /// </summary>
    public ReadOnlySpan<Archetype> Archetypes => new(_archetypes, 0, _archetypeCount);
    
    public int ArchetypesCache => _archetypes.GetHashCode();

    /// <summary>
    /// Moves an entity from one archetype to another.
    /// </summary>
    private void MoveToArchetype(Entity entity, Archetype fromArchetype, Signature newSignature)
    {
        var toArchetype = GetOrCreateArchetype(newSignature);
        
        if (fromArchetype == toArchetype)
            return;

        // Get current location
        var oldLocation = _entityLocations[entity.Id];
        
        // Remove from old archetype
        var oldChunk = fromArchetype.Chunks[oldLocation.ChunkIndex];
        fromArchetype.RemoveEntity(oldChunk, oldLocation.LocalIndex, entity);
        
        // Add to new archetype
        var (newChunk, newLocalIndex) = toArchetype.AddEntity(entity);
        
        // Find new chunk index
        var newChunkIndex = 0;
        foreach (var c in toArchetype.Chunks)
        {
            if (c == newChunk) break;
            newChunkIndex++;
        }
        
        // Find new archetype index
        var newArchetypeIndex = 0;
        for (int i = 0; i < _archetypeCount; i++)
        {
            if (_archetypes[i] == toArchetype)
            {
                newArchetypeIndex = i;
                break;
            }
        }
        
        // Update mapping
        _entityLocations[entity.Id] = new EntityLocation(newArchetypeIndex, newChunkIndex, newLocalIndex);
    }

    private void EnsureEntityLocationCapacity(int entityId)
    {
        if (entityId < _entityLocations.Length)
            return;

        Array.Resize(ref _entityLocations, _entityLocations.Length * 2);
    }
}