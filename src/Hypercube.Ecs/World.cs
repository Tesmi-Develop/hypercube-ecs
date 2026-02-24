using Hypercube.Ecs.Components;
using Hypercube.Ecs.Components.Pool;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[PublicAPI]
public sealed class World : IWorld
{
    private readonly Entities _entities = new();
    private readonly Dictionary<Type, IComponentPool> _pools = new();

    public Entity Create() => _entities.Create();

    public void Destroy(Entity entity) {
        foreach (var pool in _pools.Values)
            pool.Remove(entity);
        
        _entities.Destroy(entity);
    }

    public bool IsAlive(Entity entity) => _entities.IsAlive(entity);

    public ref T Add<T>(Entity entity) where T : IComponent => ref GetPool<T>().Add(entity);
    public ref T Get<T>(Entity entity) where T : IComponent => ref GetPool<T>().Get(entity);
    public bool Has<T>(Entity entity) where T : IComponent => GetPool<T>().Has(entity);

    private ComponentPool<T> GetPool<T>() where T : IComponent {
        if (_pools.TryGetValue(typeof(T), out var pool))
            return (ComponentPool<T>) pool;

        var created = new ComponentPool<T>();
        _pools.Add(typeof(T), created);
        
        return created;
    }
}