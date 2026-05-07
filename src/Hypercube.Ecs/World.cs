using Hypercube.Ecs.Components;
using Hypercube.Ecs.Components.Pool;
using Hypercube.Ecs.Lifetime;
using Hypercube.Utilities.Debugging.Logger;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[PublicAPI]
public partial class World : IWorld
{
    // Legacy pools for direct component access (kept for compatibility)
    private readonly Dictionary<Type, IComponentPool> _pools = new();
    private readonly ILogger _logger;
    
    public World(ILogger logger)
    {
        _logger = logger;
    }

    public ref T Add<T>(Entity entity) where T : struct, IComponent
    {
        if (Has<T>(entity))
            return ref GetPool<T>().Get(entity);
        
        _archetypes.AddSignature(entity.Id, ComponentStaticMeta<T>.Signature);
        
        ref var component = ref GetPool<T>().Add(entity, new T());
        _eventBus.Raise(entity, ref component, new AddedEvent());

        return ref GetPool<T>().Get(entity);
    }

    public ref T Add<T>(Entity entity, in T value) where T : struct, IComponent
    {
        if (Has<T>(entity))
            return ref GetPool<T>().Get(entity);
        
        _archetypes.AddSignature(entity.Id, ComponentStaticMeta<T>.Signature);
        
        ref var component = ref GetPool<T>().Add(entity, value);
        _eventBus.Raise(entity, ref component, new AddedEvent());

        return ref component;
    }
    
    public ref T Get<T>(Entity entity) where T : struct, IComponent =>
        ref GetPool<T>().Get(entity);

    public bool Has<T>(Entity entity) where T : struct, IComponent =>
        GetPool<T>().Has(entity);

    /// <summary>
    /// Removes a component from an entity, potentially moving it to a different archetype.
    /// </summary>
    public void Remove<T>(Entity entity)
        where T : struct, IComponent
    {
        if (!Has<T>(entity))
            return;
        
        _archetypes.RemoveSignature(entity.Id, ComponentStaticMeta<T>.Signature);

        ref var component = ref GetPool<T>().Get(entity);
        _eventBus.Raise(entity, ref component, new RemovedEvent());
        
        GetPool<T>().Remove(entity);
    }

    private ComponentPool<T> GetPool<T>() where T : struct, IComponent
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
            return (ComponentPool<T>)pool;

        var created = new ComponentPool<T>();
        _pools.Add(typeof(T), created);

        return created;
    }
}
