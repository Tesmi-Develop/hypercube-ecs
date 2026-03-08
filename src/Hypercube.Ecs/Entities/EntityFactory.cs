using Hypercube.Utilities.Collections;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Entities;

[PublicAPI]
public sealed class EntityFactory
{
    private readonly NumPool<int> _generator = new();
    private int[] _versions = new int[1024];

    public Entity Create()
    {
        var id = _generator.Next;
        
        EnsureExpand(ref _versions, id);
        return new Entity(id, _versions[id]);
    }

    public void Delete(Entity entity) 
    {
        if (!Valid(entity))
            return;

        _versions[entity.Id]++;
        _generator.Release(entity.Id);
    }

    public bool Valid(Entity entity)
    {
        return entity.Id >= 0 &&
               entity.Id < _versions.Length &&
               _versions[entity.Id] == entity.Version;
    }

    private void EnsureExpand(ref int[] array, int index)
    {
        if (index < array.Length)
            return;
        
        Expand(ref array);
    }

    private void Expand(ref int[] array)
    {
        Array.Resize(ref _versions, _versions.Length * 2);
    }
}