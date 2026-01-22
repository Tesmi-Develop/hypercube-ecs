using Hypercube.Mathematics.Vectors;

namespace Hypercube.Ecs.Test;

public static class Program
{
    public static void Main(string[] args)
    {
        var world = new World();
        var entity = world.Create();
        world.Add<Position>(entity);
        world.Add<Velocity>(entity);
        
        
    }

    public record struct Position(Vector2 Value);
    public record struct Velocity(Vector2 Value);
}