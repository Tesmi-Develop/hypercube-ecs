using Hypercube.Ecs.Components;
using Hypercube.Ecs.Components.Pool;
using Hypercube.Ecs.Lifetime;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[PublicAPI]
public partial class World : IWorld
{
    // Legacy pools for direct component access (kept for compatibility)
    private readonly Dictionary<Type, IComponentPool> _pools = new();
    
    public World()
    {
        _emptyArchetype = GetOrCreateArchetype(Signature.Empty);
    }
    
    #region Component Operations

    public ref T Add<T>(Entity entity) where T : struct, IComponent
    {
        var signature = ComponentStaticMeta<T>.Signature;
        
        var currentArchetype = GetEntityArchetype(entity);
        var newSignature = currentArchetype.Signature + signature;
        
        if (newSignature != currentArchetype.Signature)
            MoveToArchetype(entity, currentArchetype, newSignature);
        
        ref var component = ref GetPool<T>().Add(entity, new T());
        _eventBus.Raise(entity, ref component, new AddedEvent());

        return ref GetPool<T>().Get(entity);
    }

    public ref T Add<T>(Entity entity, in T value) where T : struct, IComponent
    {
        var signature = ComponentStaticMeta<T>.Signature;
        
        var currentArchetype = GetEntityArchetype(entity);
        var newSignature = currentArchetype.Signature + signature;
        
        if (newSignature != currentArchetype.Signature)
            MoveToArchetype(entity, currentArchetype, newSignature);
        
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
        var signature = ComponentStaticMeta<T>.Signature;
        
        var currentArchetype = GetEntityArchetype(entity);
        var newSignature = currentArchetype.Signature - signature;
        
        // Get component before removal for lifecycle event
        ref var component = ref GetPool<T>().Get(entity);
        
        if (newSignature != currentArchetype.Signature)
        {
            MoveToArchetype(entity, currentArchetype, newSignature);
        }

        // Dispatch lifecycle event
        _eventBus.Raise(entity, ref component, new RemovedEvent());
        
        // Legacy: remove from pool
        GetPool<T>().Remove(entity);
    }

    #endregion
    
    #region Legacy Pool Support

    private ComponentPool<T> GetPool<T>() where T : struct, IComponent
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
            return (ComponentPool<T>)pool;

        var created = new ComponentPool<T>();
        _pools.Add(typeof(T), created);

        return created;
    }

    #endregion
}
