namespace Hypercube.Ecs.Pool;

public interface IComponentPool
{
    void Remove(Entity entity);
    void Flush();
}
