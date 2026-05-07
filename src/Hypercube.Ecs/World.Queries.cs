using Hypercube.Ecs.Queries;

namespace Hypercube.Ecs;

public partial class World
{
    private Dictionary<QueryMeta, Query> _queries = new();
    
    public Query Query(in QueryMeta meta)
    {
        if (_queries.TryGetValue(meta, out var query))
            return query;
        
        query = new Query(this, meta);
        _logger.Trace($"New query created: {query}");
        
        _queries[meta] = query;

        return query;
    }
}
