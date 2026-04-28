using System.Collections;
using System.Text;

namespace Hypercube.Ecs.System.Collections;

public sealed class SystemSequence<T> : ISystemCollection<T>
{
    private readonly List<ISystem<T>> _systems = [];
    
    public SystemSequence(params ISystem<T>[] systems)
    {
        foreach (var system in systems)
            _systems.Add(system);
    }
    
    public void Initialize()
    {
        foreach (var t in _systems)
            t.Initialize();
    }
    
    public void BeforeUpdate(T deltaTime)
    {
        foreach (var t in _systems)
            t.BeforeUpdate(deltaTime);
    }

    public void Update(T deltaTime)
    {
        foreach (var t in _systems)
            t.Update(deltaTime);
    }
    
    public void AfterUpdate(T deltaTime)
    {
        foreach (var t in _systems)
            t.AfterUpdate(deltaTime);
    }
    
    public void Dispose()
    {
        foreach (var t in _systems)
            t.Dispose();
    }
    
    public void Add<TSystem>() where TSystem : ISystem<T>, new()
    {
        _systems.Add(new TSystem());
    }

    public void Add(params ISystem[] systems)
    {
        foreach (var system in systems)
            Add(system);
    }
    
    public void Add(ISystem<T> system)
    {
        _systems.Add(system);
    }
    
    public TSystem Get<TSystem>() where TSystem : ISystem<T>
    {
        foreach (var entry in _systems)
        {
            switch (entry)
            {
                case TSystem system:
                    return system;
               
                case ISystemCollection<T> collection:
                    return collection.Get<TSystem>();
            }
        }

        return default!;
    }
    
    /// <inheritdoc/>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        
        foreach (var system in _systems)
            stringBuilder.Append($"{system.GetType().Name},");
        
        // Remove las comma
        if (_systems.Count > 0)
            stringBuilder.Length--;
        
        return $"Linear System Sequence ({stringBuilder})";
    }

    /// <inheritdoc/>
    public IEnumerator<ISystem<T>> GetEnumerator() => ((IEnumerable<ISystem<T>>) _systems).GetEnumerator();
    
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}