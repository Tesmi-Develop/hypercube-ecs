using System.Diagnostics.CodeAnalysis;
using Hypercube.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Query;

[PublicAPI]
public struct QueryMeta()
{
    public Signature All { get; private set; } = Signature.Empty;
    public Signature Any { get; private set; } = Signature.Empty;
    public Signature None { get; private set; } = Signature.Empty;

    [UnscopedRef]
    public ref QueryMeta WithAll<T>() where T : IComponent
    {
        All += ComponentStaticMeta<T>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithAny<T>() where T : IComponent
    {
        Any += ComponentStaticMeta<T>.Signature;
        return ref this;
    }
    
    [UnscopedRef]
    public ref QueryMeta WithNone<T>() where T : IComponent
    {
        None += ComponentStaticMeta<T>.Signature;
        return ref this;
    }
}
