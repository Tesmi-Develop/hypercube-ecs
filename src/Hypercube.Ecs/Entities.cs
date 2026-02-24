using Hypercube.Utilities.Collections;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

[PublicAPI]
public sealed class Entities
{
    private readonly NumPool<int> _pool = new();
    
    private int[] _versions = new int[1024];

    public Entity Create() {
        var id = _pool.Next;
        if (id >= _versions.Length)
            Array.Resize(ref _versions, _versions.Length * 2);

        return new Entity(id, _versions[id]);
    }

    public void Destroy(Entity entity) {
        if (!IsAlive(entity))
            return;

        _versions[entity.Id]++;
        _pool.Release(entity.Id);
    }

    public bool IsAlive(Entity entity)
    {
        return entity.Id >= 0 &&
               entity.Id < _versions.Length &&
               _versions[entity.Id] == entity.Version;
    }
}