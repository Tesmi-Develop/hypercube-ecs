using System.Diagnostics.CodeAnalysis;
using Hypercube.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Queries;

[PublicAPI]
public struct QueryMeta()
{
    public static readonly QueryMeta Empty = new();
    
    public Signature All { get; private set; } = Signature.Empty;
    public Signature Any { get; private set; } = Signature.Empty;
    public Signature None { get; private set; } = Signature.Empty;

    #region All
    
    [UnscopedRef]
    public ref QueryMeta WithAll<T>()
        where T : struct, IComponent
    {
        All += ComponentStaticMeta<T>.Signature;
        return ref this;
    }

    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        return ref this;
    }

    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        All += ComponentStaticMeta<T3>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2, T3, T4>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        All += ComponentStaticMeta<T3>.Signature;
        All += ComponentStaticMeta<T4>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2, T3, T4, T5>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        All += ComponentStaticMeta<T3>.Signature;
        All += ComponentStaticMeta<T4>.Signature;
        All += ComponentStaticMeta<T5>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2, T3, T4, T5, T6>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        All += ComponentStaticMeta<T3>.Signature;
        All += ComponentStaticMeta<T4>.Signature;
        All += ComponentStaticMeta<T5>.Signature;
        All += ComponentStaticMeta<T6>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        All += ComponentStaticMeta<T3>.Signature;
        All += ComponentStaticMeta<T4>.Signature;
        All += ComponentStaticMeta<T5>.Signature;
        All += ComponentStaticMeta<T6>.Signature;
        All += ComponentStaticMeta<T7>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAll<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        All += ComponentStaticMeta<T1>.Signature;
        All += ComponentStaticMeta<T2>.Signature;
        All += ComponentStaticMeta<T3>.Signature;
        All += ComponentStaticMeta<T4>.Signature;
        All += ComponentStaticMeta<T5>.Signature;
        All += ComponentStaticMeta<T6>.Signature;
        All += ComponentStaticMeta<T7>.Signature;
        All += ComponentStaticMeta<T8>.Signature;
        return ref this;
    }
    
    #endregion

    #region Any

    [UnscopedRef]
    public ref QueryMeta WithAny<T>() where T : struct, IComponent
    {
        Any += ComponentStaticMeta<T>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        Any += ComponentStaticMeta<T3>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2, T3, T4>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        Any += ComponentStaticMeta<T3>.Signature;
        Any += ComponentStaticMeta<T4>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2, T3, T4, T5>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        Any += ComponentStaticMeta<T3>.Signature;
        Any += ComponentStaticMeta<T4>.Signature;
        Any += ComponentStaticMeta<T5>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2, T3, T4, T5, T6>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        Any += ComponentStaticMeta<T3>.Signature;
        Any += ComponentStaticMeta<T4>.Signature;
        Any += ComponentStaticMeta<T5>.Signature;
        Any += ComponentStaticMeta<T6>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        Any += ComponentStaticMeta<T3>.Signature;
        Any += ComponentStaticMeta<T4>.Signature;
        Any += ComponentStaticMeta<T5>.Signature;
        Any += ComponentStaticMeta<T6>.Signature;
        Any += ComponentStaticMeta<T7>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        Any += ComponentStaticMeta<T1>.Signature;
        Any += ComponentStaticMeta<T2>.Signature;
        Any += ComponentStaticMeta<T3>.Signature;
        Any += ComponentStaticMeta<T4>.Signature;
        Any += ComponentStaticMeta<T5>.Signature;
        Any += ComponentStaticMeta<T6>.Signature;
        Any += ComponentStaticMeta<T7>.Signature;
        Any += ComponentStaticMeta<T8>.Signature;
        return ref this;
    }

    #endregion

    #region None

    [UnscopedRef]
    public ref QueryMeta WithNone<T>() where T : struct, IComponent
    {
        None += ComponentStaticMeta<T>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2, T3>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        None += ComponentStaticMeta<T3>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2, T3, T4>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        None += ComponentStaticMeta<T3>.Signature;
        None += ComponentStaticMeta<T4>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2, T3, T4, T5>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        None += ComponentStaticMeta<T3>.Signature;
        None += ComponentStaticMeta<T4>.Signature;
        None += ComponentStaticMeta<T5>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2, T3, T4, T5, T6>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        None += ComponentStaticMeta<T3>.Signature;
        None += ComponentStaticMeta<T4>.Signature;
        None += ComponentStaticMeta<T5>.Signature;
        None += ComponentStaticMeta<T6>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2, T3, T4, T5, T6, T7>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        None += ComponentStaticMeta<T3>.Signature;
        None += ComponentStaticMeta<T4>.Signature;
        None += ComponentStaticMeta<T5>.Signature;
        None += ComponentStaticMeta<T6>.Signature;
        None += ComponentStaticMeta<T7>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T1, T2, T3, T4, T5, T6, T7, T8>()
        where T1 : struct, IComponent
        where T2 : struct, IComponent
        where T3 : struct, IComponent
        where T4 : struct, IComponent
        where T5 : struct, IComponent
        where T6 : struct, IComponent
        where T7 : struct, IComponent
        where T8 : struct, IComponent
    {
        None += ComponentStaticMeta<T1>.Signature;
        None += ComponentStaticMeta<T2>.Signature;
        None += ComponentStaticMeta<T3>.Signature;
        None += ComponentStaticMeta<T4>.Signature;
        None += ComponentStaticMeta<T5>.Signature;
        None += ComponentStaticMeta<T6>.Signature;
        None += ComponentStaticMeta<T7>.Signature;
        None += ComponentStaticMeta<T8>.Signature;
        return ref this;
    }

    #endregion
}
