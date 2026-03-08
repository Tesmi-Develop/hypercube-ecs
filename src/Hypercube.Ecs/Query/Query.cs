using Hypercube.Utilities.Collections.Bit;

namespace Hypercube.Ecs.Query;

public class Query
{
    private readonly BitSet _any;
    private readonly BitSet _all;
    private readonly BitSet _none;

    private readonly QueryMeta _meta;
    private readonly int _hashCode;

    public Query(QueryMeta meta)
    {
        _all = meta.All.AsBitSet();
        _any = meta.Any.AsBitSet();
        _none = meta.None.AsBitSet();
        
        _meta = meta;
        _hashCode = GetHashCode(this);
    }
    
    public bool Match(BitSet bitset)
    {
        return _all.All(bitset) && _any.Any(bitset) && _none.None(bitset);
    }

    public override int GetHashCode() => _hashCode;

    private static int GetHashCode(Query query)
    {
        var hash = 17;
        hash = hash * 23 + query._all.GetHashCode();
        hash = hash * 23 + query._any.GetHashCode();
        hash = hash * 23 + query._none.GetHashCode();
        return hash;
    }
}
