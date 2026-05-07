using System.Diagnostics;
using Hypercube.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Archetypes;

[PublicAPI]
public sealed class ArchetypeContainer
{
    public const int DefaultArchetypeCount = 32;
    
    private EntityLocation[] _locations = new EntityLocation[1024];
    
    private Archetype[] _archetypes = new Archetype[DefaultArchetypeCount];
    private int _archetypeCount;
    private int _archetypeVersion;
    
    public ReadOnlySpan<Archetype> Archetypes => new(_archetypes, 0, _archetypeCount);
    public int Version => _archetypeVersion;

    public EntityLocation Add(EntityId entityId, Signature signature)
    {
        Reserve(entityId);

        var archetype = GetOrCreateArchetype(signature, out var archetypeIndex);
        var (chunkIndex, index) = archetype.Add(entityId);
        
        var location = new EntityLocation(archetypeIndex, chunkIndex, index);
        _locations[entityId] = location;

        return location;
    }

    public EntityLocation AddSignature(EntityId entityId, Signature signature)
    {
        var archetype = _archetypes[_locations[entityId].ArchetypeIndex];
        var archetypeNext = GetOrCreateArchetype(archetype.Signature + signature);
        return Move(entityId, archetype, archetypeNext);
    }
    
    public EntityLocation RemoveSignature(EntityId entityId, Signature signature)
    {
        var archetype = _archetypes[_locations[entityId].ArchetypeIndex];
        var archetypeNext = GetOrCreateArchetype(archetype.Signature - signature);
        return Move(entityId, archetype, archetypeNext);
    }

    public void Remove(EntityId entityId)
    {
        Debug.Assert(entityId >= 0);
        Debug.Assert(entityId < _locations.Length);
        
        var location = _locations[entityId];
        
        Debug.Assert(location.ArchetypeIndex < _archetypes.Length);
        
        Remove(_locations[entityId]);
    }

    public EntityLocation Move(EntityId entityId, Archetype from, Archetype to)
    {
        Debug.Assert(from != to);
        Debug.Assert(entityId >= 0);
        Debug.Assert(entityId < _locations.Length);
        
        if (from == to)
            return _locations[entityId];
        
        Remove(_locations[entityId]);
        
        var newChunkEntity = to.Add(entityId);
        var newArchetypeIndex = -1;
        
        for (var i = 0; i < _archetypeCount; i++)
        {
            if (_archetypes[i] != to)
                continue;
            
            newArchetypeIndex = i;
            break;
        }
        
        Debug.Assert(newArchetypeIndex >= 0);
        
        var location = new EntityLocation(newArchetypeIndex, newChunkEntity.ChunkIndex, newChunkEntity.Index);
        _locations[entityId] = location;

        return location;
    }

    public Archetype GetOrCreateArchetype(Signature signature) => GetOrCreateArchetype(signature, out _);

    public Archetype GetOrCreateArchetype(Signature signature, out int index)
    {
        index = 0;
        
        for (var i = 0; i < _archetypeCount; i++)
        {
            if (_archetypes[i].Signature != signature)
                continue;
            
            index = i;
            return _archetypes[i];
        }

        var archetype = new Archetype(signature);
        index = _archetypeCount++;
        
        while (index >= _archetypes.Length)
            Array.Resize(ref _archetypes, _archetypes.Length * 2);
        
        _archetypes[index] = archetype;
        _archetypeVersion++;

        return archetype;
    }

    private void Remove(EntityLocation location)
    {
        var archetype = _archetypes[location.ArchetypeIndex];
        if (!archetype.RemoveAt(location.ChunkIndex, location.LocalIndex, out var movedEntity,  out var movedChunkEntity))
            return;
        
        ref var movedLocation = ref _locations[movedEntity];
        movedLocation = new EntityLocation(movedLocation.ArchetypeIndex, movedChunkEntity.ChunkIndex, movedChunkEntity.Index);
    }
    
    private void Reserve(EntityId entityId)
    {
        while (entityId >= _locations.Length)
            Array.Resize(ref _locations, _locations.Length * 2);
    }

    private Archetype GetEntityArchetype(Entity entity)
    {
        Debug.Assert(entity.Id < _locations.Length);
        var location = _locations[entity.Id];
        Debug.Assert(location.ArchetypeIndex < _archetypeCount);
        return _archetypes[location.ArchetypeIndex];
    }
}