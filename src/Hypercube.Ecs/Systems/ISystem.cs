namespace Hypercube.Ecs.Systems;

public interface ISystem
{
    void Update(World world, float deltaTime);
}
