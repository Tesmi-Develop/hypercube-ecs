using Hypercube.Ecs.Components;
using Hypercube.Ecs.Components.Pool;
using Hypercube.Ecs.Entities;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[PublicAPI]
public sealed class World : IWorld
{
    private readonly EntityFactory _entityFactory = new();
    private readonly Dictionary<Type, IComponentPool> _pools = new();

    public Entity Create() => _entityFactory.Create();

    public void Delete(Entity entity) {
        foreach (var pool in _pools.Values)
            pool.Remove(entity);
        
        _entityFactory.Delete(entity);
    }

    public bool IsAlive(Entity entity) => _entityFactory.Valid(entity);

    public ref T Add<T>(Entity entity) where T : IComponent =>
        ref GetPool<T>().Add(entity);
    
    public ref T Get<T>(Entity entity) where T : IComponent =>
        ref GetPool<T>().Get(entity);
    
    public bool Has<T>(Entity entity) where T : IComponent => 
        GetPool<T>().Has(entity);

    private ComponentPool<T> GetPool<T>() where T : IComponent {
        if (_pools.TryGetValue(typeof(T), out var pool))
            return (ComponentPool<T>) pool;

        var created = new ComponentPool<T>();
        _pools.Add(typeof(T), created);
        
        return created;
    }
}