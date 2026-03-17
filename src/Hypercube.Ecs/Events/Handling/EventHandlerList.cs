using Hypercube.Ecs.Components;

namespace Hypercube.Ecs.Events.Handling;

public readonly struct EventHandlerList()
{
    public const int NoPriority = -1;
    
    private readonly List<HandlerEntry> _handlers = [];
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);

    public void Add<TComponent, TEvent>(
        EventHandler<TComponent, TEvent> handler,
        int priority)
        where TComponent : IComponent
        where TEvent : struct, IEvent
    {
        _lock.EnterWriteLock();
        
        try
        {
            var entry = new HandlerEntry(priority, handler);
            if (priority == NoPriority)
            {
                _handlers.Add(entry);
                return;
            }

            var index = _handlers.FindIndex(h => priority > h.Priority);
            _handlers.Insert(index, entry);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Remove(object handler)
    {
        _lock.EnterWriteLock();
        
        try
        {
            _handlers.RemoveAll(h => ReferenceEquals(h.Handler, handler));
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Invoke<TComponent, TEvent>(Entity entity, ref TComponent component, ref TEvent args)
        where TComponent : IComponent
        where TEvent : struct, IEvent
    {
        _lock.EnterReadLock();
        
        try
        {
            for (var i = 0; i < _handlers.Count; i++)
            {
                var handler = (EventHandler<TComponent, TEvent>) _handlers[i].Handler;
                handler.Invoke(entity, ref component, ref args);
            }
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }
}