using Hypercube.Ecs.Events;

namespace Hypercube.Ecs;

public partial class World
{
    private readonly EventBus _eventBus = new();
    public IEventBus Events => _eventBus;
}
