using Hypercube.Ecs.Pool;

namespace Hypercube.Ecs;

public sealed class World
{
    private readonly Entities _entities = new();
    
    private readonly Dictionary<Type, IComponentPool> _pools = new();

    public Entity Create()
    {
        return _entities.Create();
    }

    public void Destroy(Entity entity) {
        foreach (var pool in _pools.Values)
            pool.Remove(entity);
        
        _entities.Destroy(entity);
    }

    public bool IsAlive(Entity entity)
    {
        return _entities.IsAlive(entity);
    }
    
    public ref T Add<T>(Entity entity) where T : struct =>
        ref GetPool<T>().Add(entity);

    public ref T Get<T>(Entity entity) where T : struct =>
        ref GetPool<T>().Get(entity);

    public bool Has<T>(Entity entity) where T : struct =>
        GetPool<T>().Has(entity);

    private ComponentPool<T> GetPool<T>() where T : struct {
        if (_pools.TryGetValue(typeof(T), out var pool))
            return (ComponentPool<T>) pool;

        var created = new ComponentPool<T>();
        _pools.Add(typeof(T), created);
        
        return created;
    }
}