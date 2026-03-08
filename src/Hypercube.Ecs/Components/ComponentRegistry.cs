using System.Runtime.CompilerServices;
using Hypercube.Utilities;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Components;

[PublicAPI]
public static class ComponentRegistry
{
    private static readonly Dictionary<Type, ComponentMeta> InternalMapping = new(128);
    
    public static int Index { get; private set; }
    public static IReadOnlyDictionary<Type, ComponentMeta> Mapping => InternalMapping;

    #region Add
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ComponentMeta Add<T>() =>
        Add(typeof(T), HyperUnsafe.SizeOf<T>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ComponentMeta Add(Type type) =>
        Add(type, HyperUnsafe.SizeOf(type));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ComponentMeta Add(Type type, int size)
    {
        if (TryGet(type, out var meta))
            return meta;

        meta = new ComponentMeta(Index++, size);
        InternalMapping[type] = meta;

        return meta;
    }
    
    #endregion
    
    #region Remove
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Remove<T>() =>
        Remove(typeof(T));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Remove(Type type) =>
        Remove(type, out _);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Remove(Type type, out ComponentMeta element) =>
        InternalMapping.Remove(type, out element);

    #endregion
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet(Type type, out ComponentMeta componentMeta) =>
        Mapping.TryGetValue(type, out componentMeta);
}