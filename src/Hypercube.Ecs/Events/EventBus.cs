using System.Collections.Concurrent;
using Hypercube.Ecs.Components;
using Hypercube.Ecs.Events.Handling;

namespace Hypercube.Ecs.Events;

public sealed class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, EventHandlerList> _broadcastHandlers = new();
    private readonly ConcurrentDictionary<EventSignature, EventHandlerList> _handlers = new();

    #region Broadcast
    
    public void Subscribe<TEvent>(Handling.EventHandler<TEvent> handler, int priority = EventHandlerList.NoPriority)
        where TEvent : struct, IEvent
    {
        var signature = typeof(TEvent);
        GetBroadcastHandlers(signature).Add(handler, priority);
    }
    

    public void Unsubscribe<TEvent>(Handling.EventHandler<TEvent> handler)
        where TEvent : struct, IEvent
    {
        var signature = typeof(TEvent);
        if (!_broadcastHandlers.TryGetValue(signature, out var list))
            return;

        list.Remove(handler);
    }
    
    public void Raise<TEvent>(TEvent args)
        where TEvent : struct, IEvent
    {
        Raise(ref args);
    }

    public void Raise<TEvent>(ref TEvent args)
        where TEvent : struct, IEvent
    {
        var signature = typeof(TEvent);
        if (!_broadcastHandlers.TryGetValue(signature, out var list))
            return;
        
        list.Invoke(ref args);
    }

    #endregion
    
    #region Component direct

    public void Subscribe<TComponent, TEvent>(Handling.EventHandler<TComponent, TEvent> handler, int priority = EventHandlerList.NoPriority)
        where TComponent : IComponent
        where TEvent : struct, IEvent
    {
        var signature = new EventSignature(ComponentStaticMeta<TComponent>.Type, typeof(TEvent));
        GetHandlers(signature).Add(handler, priority);
    }
    
    public void Unsubscribe<TComponent, TEvent>(Handling.EventHandler<TComponent, TEvent> handler)
        where TComponent : IComponent
        where TEvent : struct, IEvent
    {
        var signature = new EventSignature(ComponentStaticMeta<TComponent>.Type, typeof(TEvent));
        if (!_handlers.TryGetValue(signature, out var list))
            return;

        list.Remove(handler);
    }
    
    public void Raise<TComponent, TEvent>(Entity entity, ref TComponent component, TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent
    {
        Raise(entity, ref component, ref args);
    }

    public void Raise<TComponent, TEvent>(Entity entity, ref TComponent component, ref TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent
    {
        var signature = new EventSignature(ComponentStaticMeta<TComponent>.Type, typeof(TEvent));
        if (!_handlers.TryGetValue(signature, out var list))
            return;
        
        list.Invoke(entity, ref component, ref args);
    }

    #endregion

    private EventHandlerList GetHandlers(in EventSignature signature)
        => _handlers.GetOrAdd(signature, static _ => new EventHandlerList());

    private EventHandlerList GetBroadcastHandlers(in Type eventType)
        => _broadcastHandlers.GetOrAdd(eventType, static _ => new EventHandlerList());
}