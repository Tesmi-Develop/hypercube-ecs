using Hypercube.Ecs.Components;
using Hypercube.Mathematics.Vectors;
using Hypercube.Utilities.Debugging.Logger;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Test;

public static class Program
{
    public static void Main()
    {
        var world = new World(new HeadlessLogger());
        
        var entity = world.Create();
        
        world.Add<Position>(entity);
        world.Add<Velocity>(entity);
    }
    
    [PublicAPI] private record struct Position(Vector2 Value) : IComponent;
    [PublicAPI] private record struct Velocity(Vector2 Value) : IComponent;
}
