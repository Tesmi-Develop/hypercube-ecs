namespace Hypercube.Ecs.System;

public interface ISystem<in T> : IDisposable
{
    void Initialize();
    void BeforeUpdate(T args);
    void Update(T args);
    void AfterUpdate(T args);
}

public interface ISystem : ISystem<float>;
