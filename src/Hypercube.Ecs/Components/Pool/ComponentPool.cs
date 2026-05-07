using JetBrains.Annotations;

namespace Hypercube.Ecs.Components.Pool;

/// <summary>
/// Component storage based on a sparse set.
/// </summary>
[PublicAPI]
public sealed class ComponentPool<T> : IComponentPool where T : struct, IComponent
{
    // Dense component storage (index -> component)
    private T[] _components;

    // Dense entities array (index -> entityId)
    private int[] _entities;

    // Sparse indices array (entityId -> index)
    private int[] _indices;

    // Used only for deferred removal
    private bool[] _enabled;

    // Number of active components
    public int Count { get; private set; }

    /// <summary>
    /// Dense list of entities owning this component.
    /// </summary>
    public ReadOnlySpan<int> DenseEntities => new(_entities, 0, Count);

    public ComponentPool(int capacity = 64)
    {
        _components = new T[capacity];
        _entities   = new int[capacity];
        _indices    = new int[capacity];
        _enabled    = new bool[capacity];
    }

    /// <summary>
    /// Checks whether the entity owns this component.
    /// O(1)
    /// </summary>
    public bool Has(Entity entity)
    {
        var id = entity.Id;
        if (id >= _indices.Length)
            return false;

        var index = _indices[id];
        return index < Count && _entities[index] == id;
    }

    /// <summary>
    /// Returns a reference to the component.
    /// Throws if the component is missing.
    /// </summary>
    public ref T Get(Entity entity)
    {
        if (!Has(entity))
            throw new InvalidOperationException($"Component {typeof(T)} is missing on entity {entity.Id}");

        return ref _components[_indices[entity.Id]];
    }

    /// <summary>
    /// Adds a component or returns an existing one.
    /// O(1)
    /// </summary>
    public ref T Add(Entity entity, in T component)
    {
        EnsureSparseCapacity(entity.Id);

        if (Has(entity))
            return ref _components[_indices[entity.Id]];

        EnsureDenseCapacity();

        var index = Count++;

        _entities[index] = entity.Id;
        _indices[entity.Id] = index;
        _enabled[index] = true;
        _components[index] = component;

        return ref _components[index];
    }

    /// <summary>
    /// Immediately removes the component using swap-remove.
    /// O(1)
    /// </summary>
    public void Remove(Entity entity)
    {
        if (!Has(entity))
            return;

        RemoveAt(_indices[entity.Id]);
    }

    /// <summary>
    /// Marks the component for removal without modifying storage.
    /// Safe during iteration.
    /// </summary>
    public void RemoveDeferred(Entity entity)
    {
        if (!Has(entity))
            return;

        _enabled[_indices[entity.Id]] = false;
    }

    /// <summary>
    /// Applies all deferred removals.
    /// Must be called after iteration.
    /// </summary>
    public void Flush()
    {
        for (var i = Count - 1; i >= 0; i--)
        {
            if (_enabled[i])
                continue;

            RemoveAt(i);
        }
    }

    /// <summary>
    /// Returns component by dense index.
    /// Intended for system iteration.
    /// </summary>
    public ref T GetByIndex(int index)
    {
        return ref _components[index];
    }

    /// <summary>
    /// Removes an element by dense index using swap-remove.
    /// </summary>
    private void RemoveAt(int index)
    {
        var lastIndex   = --Count;
        var movedEntity = _entities[lastIndex];

        _entities[index] = movedEntity;
        _indices[movedEntity] = index;

        _components[index] = _components[lastIndex];
        _enabled[index]    = _enabled[lastIndex];
    }

    /// <summary>
    /// Ensures that the sparse indices array
    /// can index the given entity id.
    /// </summary>
    private void EnsureSparseCapacity(int entityId)
    {
        if (entityId < _indices.Length)
            return;

        var newSize = _indices.Length;
        while (newSize <= entityId)
            newSize <<= 1;

        Array.Resize(ref _indices, newSize);
    }

    /// <summary>
    /// Ensures enough capacity for dense arrays.
    /// </summary>
    private void EnsureDenseCapacity()
    {
        if (Count < _components.Length)
            return;

        var newSize = _components.Length << 1;
        while (newSize <= _components.Length)
            newSize <<= 1;
        
        Array.Resize(ref _components, newSize);
        Array.Resize(ref _entities, newSize);
        Array.Resize(ref _enabled, newSize);
    }
}
