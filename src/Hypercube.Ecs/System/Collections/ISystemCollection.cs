namespace Hypercube.Ecs.System.Collections;

public interface ISystemCollection : ISystem, IEnumerable<ISystem>
{
    T Get<T>() where T : ISystem;
}