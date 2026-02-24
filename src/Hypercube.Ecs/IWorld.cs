using Hypercube.Ecs.Components;

namespace Hypercube.Ecs;

public interface IWorld
{
    Entity Create();
    void Destroy(Entity entity);
    bool IsAlive(Entity entity);

    
    ref T Add<T>(Entity entity) where T : IComponent;
    ref T Get<T>(Entity entity) where T : IComponent;
    bool Has<T>(Entity entity) where T : IComponent;
}