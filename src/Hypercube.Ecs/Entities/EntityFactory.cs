using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hypercube.Utilities.Collections;

namespace Hypercube.Ecs.Entities;

/// <summary>
/// It is a wrapper around <see cref="NumPool{int}"/> that supports versioning
/// and hides the actual process of creating the entity from <see cref="World"/>,
/// in order to separate responsibilities.
/// </summary>
public sealed class EntityFactory
{
    private readonly NumPool<int> _generator = new();
    private int[] _versions;

    public ReadOnlySpan<int> Versions => _versions;

    public EntityFactory(int size = 1024)
    {
        _versions = new int[size];
    }
    
    #region Management

    public Entity Create()
    {
        var id = _generator.Next;
        ExpandEnsure(ref _versions, id);
        return new Entity(id, _versions[id]);
    }

    public void Delete(Entity entity) 
    {
        if (!Validate(entity))
            return;

        _versions[entity.Id]++;
        _generator.Release(entity.Id);
    }

    public int GetVersion(EntityId entityId)
    {
        Debug.Assert(!_generator.Invalid(entityId));
        
        if (entityId < 0 || entityId >= _versions.Length)
            return Entity.InvalidVersion;
        
        return _versions[entityId];
    }

    #endregion

    #region Validation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Validate(Entity entity) => ValidateId(entity.Id) && ValidateVersion(entity.Id, entity.Version);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ValidateId(EntityId entityId) => entityId != Entity.InvalidId && !_generator.Invalid(entityId);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ValidateVersion(EntityId entityId, int version) => _versions[entityId] == version;
    
    #endregion

    #region Utilities

    private static void ExpandEnsure(ref int[] array, EntityId entityId)
    {
        while (entityId >= array.Length)
            Expand(ref array);
    }

    private static void Expand(ref int[] array)
    {
        Array.Resize(ref array, array.Length * 2);
    }

    #endregion
}