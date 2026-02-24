using System.Collections;
using System.Text;

namespace Hypercube.Ecs.System.Collections;

public sealed class SystemSequence : ISystemCollection
{
    private readonly List<ISystem> _systems = [];
    
    public SystemSequence(params ISystem[] systems)
    {
        foreach (var system in systems)
            _systems.Add(system);
    }
    
    public void Initialize()
    {
        for (var index = 0; index < _systems.Count; index++)
            _systems[index].Initialize();
    }
    
    public void BeforeUpdate(float deltaTime)
    {
        for (var index = 0; index < _systems.Count; index++)
            _systems[index].BeforeUpdate(deltaTime);
    }

    public void Update(float deltaTime)
    {
        for (var index = 0; index < _systems.Count; index++)
            _systems[index].Update(deltaTime);
    }
    
    public void AfterUpdate(float deltaTime)
    {
        for (var index = 0; index < _systems.Count; index++)
            _systems[index].AfterUpdate(deltaTime);
    }
    
    public void Dispose()
    {
        for (var index = 0; index < _systems.Count; index++)
            _systems[index].Dispose();
    }


    public void Add<T>() where T : ISystem, new()
    {
        _systems.Add(new T());
    }

    public void Add(params ISystem[] systems)
    {
        foreach (var system in systems)
            Add(system);
    }
    
    public void Add(ISystem system)
    {
        _systems.Add(system);
    }
    
    public T Get<T>() where T : ISystem
    {
        foreach (var entry in _systems)
        {
            switch (entry)
            {
                case T system:
                    return system;
               
                case ISystemCollection collection:
                    return collection.Get<T>();
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
    public IEnumerator<ISystem> GetEnumerator() => ((IEnumerable<ISystem>) _systems).GetEnumerator();
    
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}