using Hypercube.Ecs.Components;
using Hypercube.Ecs.Entities;

namespace Hypercube.Ecs;

public partial class World
{
    private readonly EntityFactory _entityFactory = new();
    
    public Entity Create()
    {
        var entity = _entityFactory.Create();
        _archetypes.Add(entity.Id, Signature.Empty);
        return entity;
    }

    public void Delete(Entity entity)
    {
        if (!_entityFactory.Validate(entity))
            return;

        _archetypes.Remove(entity.Id);

        foreach (var pool in _pools.Values)
            pool.Remove(entity);

        _entityFactory.Delete(entity);
    }

    public bool Validate(Entity entity)
        => _entityFactory.Validate(entity);

    /// <summary>
    /// Gets the current version for an entity id. Used by QueryEnumerator to construct valid Entity.
    /// </summary>
    public int GetEntityVersion(EntityId id) => _entityFactory.GetVersion(id);
}