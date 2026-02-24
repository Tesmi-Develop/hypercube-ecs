using Hypercube.Ecs.Components;

namespace Hypercube.Ecs.System;

public abstract partial class EntitySystem
{
    private readonly IWorld _world = null!;

    protected Entity EntityCreate() =>
        _world.Create();
    
    protected void EntityDestroy(Entity entity) =>
        _world.Destroy(entity);
    
    protected bool EntityAlive(Entity entity) =>
        _world.IsAlive(entity);
    
    protected ref T GetComponent<T>(Entity entity) where T : IComponent
        => ref _world.Get<T>(entity);

    protected ref T AddComponent<T>(Entity entity) where T : IComponent
        => ref _world.Add<T>(entity);

    protected bool HasComponent<T>(Entity entity) where T : IComponent
        => _world.Has<T>(entity);
}
