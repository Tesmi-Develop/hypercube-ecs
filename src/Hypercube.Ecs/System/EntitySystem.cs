namespace Hypercube.Ecs.System;

public abstract partial class EntitySystem : ISystem
{
    public virtual void Initialize() { }
    public virtual void BeforeUpdate(float deltaTime) { }
    public virtual void Update(float deltaTime) { }
    public virtual void AfterUpdate(float deltaTime) { }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }
}
