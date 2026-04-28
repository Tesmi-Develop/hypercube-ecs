namespace Hypercube.Ecs.System;

public abstract partial class EntitySystem<T> : ISystem<T>
{
    public virtual void Initialize() { }
    public virtual void BeforeUpdate(T args) { }
    public virtual void Update(T args) { }
    public virtual void AfterUpdate(T args) { }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }
}
