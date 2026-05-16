using Hypercube.Ecs.Events;

namespace Hypercube.Ecs.Components.Pool;

public interface IComponentPool
{
    void Remove(Entity entity);
    void Flush();
    void RaiseRemove(Entity entity, IEventBus eventBus);
}
