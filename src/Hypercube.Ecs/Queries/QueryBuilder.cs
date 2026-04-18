using System.Diagnostics.CodeAnalysis;
using Hypercube.Ecs.Components;

namespace Hypercube.Ecs.Queries;

public struct QueryBuilder
{
    private readonly IWorld _world;
    private QueryMeta _meta;

    public QueryBuilder(IWorld world)
    {
        _world = world;
        _meta = QueryMeta.Empty;
    }
    
    public QueryBuilder(IWorld world, QueryMeta meta)
    {
        _world = world;
        _meta = meta;
    }

    #region All
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T>()
        where T : struct, IComponent
    {
        _meta.WithAll<T>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        _meta.WithAll<T1, T2>();
        return ref this;
    }

    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        _meta.WithAll<T1, T2, T3>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2, T3, T4>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        _meta.WithAll<T1, T2, T3, T4>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2, T3, T4, T5>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        _meta.WithAll<T1, T2, T3, T4, T5>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2, T3, T4, T5, T6>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        _meta.WithAll<T1, T2, T3, T4, T5, T6>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        _meta.WithAll<T1, T2, T3, T4, T5, T6, T7>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAll<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        _meta.WithAll<T1, T2, T3, T4, T5, T6, T7, T8>();
        return ref this;
    }
    
    #endregion

    #region Any
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T>()
        where T : struct, IComponent
    {
        _meta.WithAny<T>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        _meta.WithAny<T1, T2>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        _meta.WithAny<T1, T2, T3>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2, T3, T4>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        _meta.WithAny<T1, T2, T3, T4>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2, T3, T4, T5>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        _meta.WithAny<T1, T2, T3, T4, T5>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2, T3, T4, T5, T6>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        _meta.WithAny<T1, T2, T3, T4, T5, T6>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        _meta.WithAny<T1, T2, T3, T4, T5, T6, T7>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithAny<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        _meta.WithAny<T1, T2, T3, T4, T5, T6, T7, T8>();
        return ref this;
    }
    
    #endregion

    #region None

    [UnscopedRef]
    public ref QueryBuilder WithNone<T>()
        where T : struct, IComponent
    {
        _meta.WithNone<T>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        _meta.WithNone<T1, T2>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        _meta.WithNone<T1, T2, T3>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2, T3, T4>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        _meta.WithNone<T1, T2, T3, T4>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2, T3, T4, T5>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        _meta.WithNone<T1, T2, T3, T4, T5>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2, T3, T4, T5, T6>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        _meta.WithNone<T1, T2, T3, T4, T5, T6>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        _meta.WithNone<T1, T2, T3, T4, T5, T6, T7>();
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryBuilder WithNone<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        _meta.WithNone<T1, T2, T3, T4, T5, T6, T7, T8>();
        return ref this;
    }

    #endregion

    public Query Build() => _world.CreateQuery(_meta);
}