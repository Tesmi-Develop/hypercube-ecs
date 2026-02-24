namespace Hypercube.Ecs.Components.Pool;

public interface IComponentPool
{
    void Remove(Entity entity);
    void Flush();
}
