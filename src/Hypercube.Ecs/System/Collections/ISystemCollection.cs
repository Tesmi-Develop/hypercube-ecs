namespace Hypercube.Ecs.System.Collections;

public interface ISystemCollection<T> : ISystem<T>, IEnumerable<ISystem<T>>
{
     TSystem Get<TSystem>() where TSystem : ISystem<T>;
}
