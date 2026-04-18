using Hypercube.Ecs.Queries;

namespace Hypercube.Ecs;

public partial class World
{
    private Dictionary<QueryMeta, Query> _queries = new();
    
    public Query CreateQuery(in QueryMeta meta)
    {
        if (_queries.TryGetValue(meta, out var query))
            return query;
        
        query = new Query(this, meta);
        _queries[meta] = query;

        return query;
    }
}
