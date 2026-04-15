using Hypercube.Ecs.Components;
using Hypercube.Ecs.Events;
using Hypercube.Ecs.Events.Handling;
using Hypercube.Ecs.Queries;
using JetBrains.Annotations;

namespace Hypercube.Ecs.System;

public abstract partial class EntitySystem
{
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public World World { get; private set; } = null!;
    
    protected Entity EntityCreate() =>
        World.Create();
    
    protected void EntityDestroy(Entity entity) =>
        World.Delete(entity);
    
    protected bool EntityAlive(Entity entity) =>
        World.Validate(entity);
    
    protected ref T GetComponent<T>(Entity entity) where T : struct, IComponent
        => ref World.Get<T>(entity);

    protected ref T AddComponent<T>(Entity entity) where T : struct, IComponent
        => ref World.Add<T>(entity);
    
    protected bool HasComponent<T>(Entity entity) where T : struct, IComponent
        => World.Has<T>(entity);

    protected bool TryGetComponent<T>(Entity entity, out T component) where T : struct, IComponent
    {
        component = default;
        
        if (!World.Has<T>(entity))
            return false;
        
        component = World.Get<T>(entity);
        return true;
    }
    
    protected void RemoveComponent<T>(Entity entity) where T : struct, IComponent
        => World.Remove<T>(entity);

    protected Query CreateQuery(in QueryMeta meta) =>
        World.CreateQuery(meta);

    protected void Subscribe<TComponent, TEvent>(Events.Handling.EventHandler<TComponent, TEvent> handler, int priority = EventHandlerList.NoPriority)
        where TComponent : IComponent
        where TEvent : struct, IEvent
        => World.Events.Subscribe(handler, priority);

    protected void Unsubscribe<TComponent, TEvent>(Events.Handling.EventHandler<TComponent, TEvent> handler)
        where TComponent : IComponent
        where TEvent : struct, IEvent
        => World.Events.Unsubscribe(handler);

    protected void Raise<TComponent, TEvent>(Entity entity, ref TComponent component, TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent
        => World.Events.Raise(entity, ref component, args);

    protected void Raise<TComponent, TEvent>(Entity entity, ref TComponent component, ref TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent
        => World.Events.Raise(entity, ref component, ref args);
}
