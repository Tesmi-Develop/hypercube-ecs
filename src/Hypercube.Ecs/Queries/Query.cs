using System.Runtime.CompilerServices;
using Hypercube.Ecs.Archetypes;
using Hypercube.Ecs.Components;
using Hypercube.Ecs.Entities;
using Hypercube.Utilities.Collections.Bit;
using JetBrains.Annotations;

namespace Hypercube.Ecs.Queries;

/// <summary>
/// Optimized query that uses archetypes for fast entity matching.
/// Caches matching archetypes in a FrozenSet to avoid allocations on repeated access.
/// </summary>
[PublicAPI]
public sealed class Query
{
    private readonly World _world;
    private readonly QueryMeta _meta;
    
    // BitSets for matching (precomputed once)
    private readonly BitSet _any;
    private readonly BitSet _all;
    private readonly BitSet _none;

    // Cached matching archetypes - FrozenSet for zero allocations on iteration
    private List<Archetype>? _cachedArchetypes;
    private int? _cachedCount;

    private readonly int _hashCode;
    private int _archetypesHashCode;
        
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _cachedCount ??= MatchingArchetypes.Sum(archetype => archetype.EntityCount);
    }
    
    public List<Archetype> MatchingArchetypes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _cachedArchetypes ??= BuildCachedArchetypes();
    }
    
    public Query(World world, QueryMeta meta)
    {
        _world = world;
        _meta = meta;
        
        _all = meta.All.AsBitSet();
        _any = meta.Any.AsBitSet();
        _none = meta.None.AsBitSet();
        
        if (!meta.All.IsEmpty)
            _world.GetOrCreateArchetype(meta.All);
        
        _hashCode = GetHashCode(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Matches(Archetype archetype)
        => Matches(archetype.BitSet);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Matches(BitSet bitset)
        => _all.All(bitset) && _any.AnyOrEmpty(bitset) && _none.None(bitset);

    private void Match()
    {
        var newArchetypesHashCode = _world.ArchetypesCache;
        if (newArchetypesHashCode == _archetypesHashCode)
            return;

        _cachedArchetypes = BuildCachedArchetypes();
        _archetypesHashCode = newArchetypesHashCode;
    }
    
    public void Invalidate()
    {
        _cachedArchetypes = null;
        _cachedCount = 0;
    }

    private List<Archetype> BuildCachedArchetypes()
    {
        var matches = new List<Archetype>();
        
        foreach (var archetype in _world.Archetypes)
        {
            if (Matches(archetype))
                matches.Add(archetype);
        }
        
        return matches;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator()
    {
        Match();
        return new Enumerator(MatchingArchetypes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ForEach(Action<Entity> action)
    {
        foreach (var entity in this)
            action(entity);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void With<T1>(EntityRefAction<T1> action) where T1 : struct, IComponent
    {
        foreach (var entity in this)
            action(entity, ref _world.Get<T1>(entity));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void With<T1, T2>(EntityRefAction<T1, T2> action) 
        where T1 : struct, IComponent where T2 : struct, IComponent
    {
        foreach (var entity in this)
            action(entity, ref _world.Get<T1>(entity), ref _world.Get<T2>(entity));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void With<T1, T2, T3>(EntityRefAction<T1, T2, T3> action) 
        where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent
    {
        foreach (var entity in this)
            action(entity, ref _world.Get<T1>(entity), ref _world.Get<T2>(entity), ref _world.Get<T3>(entity));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void With<T1, T2, T3, T4>(EntityRefAction<T1, T2, T3, T4> action) 
        where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
    {
        foreach (var entity in this)
            action(entity, ref _world.Get<T1>(entity), ref _world.Get<T2>(entity), ref _world.Get<T3>(entity), ref _world.Get<T4>(entity));
    }

    public override string ToString()
    {
        return $"All 0x{_all} Any 0x{_any} None 0x{_none}";
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

    public ref struct Enumerator
    {
        private List<Archetype>.Enumerator _archetypeEnumerator;
        private Archetype.Enumerator _currentArchetypeEnumerator;

        public Entity Current => new(_currentArchetypeEnumerator.CurrentEntityId, Entity.QueryVersion);

        public Enumerator(List<Archetype> archetypes)
        {
            _archetypeEnumerator = archetypes.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            while (true)
            {
                // Try current archetype
                if (_currentArchetypeEnumerator.MoveNext())
                    return true;

                // Move to next archetype
                if (!_archetypeEnumerator.MoveNext())
                    return false;

                _currentArchetypeEnumerator = _archetypeEnumerator.Current.GetEnumerator();
            }
        }

        [PublicAPI]
        public void Reset() => throw new NotSupportedException();
    }
}

file sealed class ArchetypeEqualityComparer : IEqualityComparer<Archetype>
{
    public static readonly ArchetypeEqualityComparer Instance = new();

    public bool Equals(Archetype? x, Archetype? y)
        => x?.HashCode == y?.HashCode;
    
    public int GetHashCode(Archetype obj)
        => obj.HashCode;
}
