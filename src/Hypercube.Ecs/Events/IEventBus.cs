using Hypercube.Ecs.Components;
using Hypercube.Ecs.Events.Handling;

namespace Hypercube.Ecs.Events;

public interface IEventBus
{
    void Subscribe<TComponent, TEvent>(Handling.EventHandler<TComponent, TEvent> handler, int priority = EventHandlerList.NoPriority)
        where TComponent : IComponent
        where TEvent : struct, IEvent;

    void Unsubscribe<TComponent, TEvent>(Handling.EventHandler<TComponent, TEvent> handler)
        where TComponent : IComponent
        where TEvent : struct, IEvent;

    void Raise<TComponent, TEvent>(Entity entity, ref TComponent component, TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent;
    
    void Raise<TComponent, TEvent>(Entity entity, ref TComponent component, ref TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent;
}
