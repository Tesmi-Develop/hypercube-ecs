using Hypercube.Ecs.Queries;

namespace Hypercube.Ecs;

public partial class World
{
    public Query CreateQuery(in QueryMeta meta) => new(this, meta);
}
