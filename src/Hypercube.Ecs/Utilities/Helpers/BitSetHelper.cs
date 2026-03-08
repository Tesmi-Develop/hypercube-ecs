using Hypercube.Ecs.Components;
using Hypercube.Utilities.Collections.Bit;

namespace Hypercube.Ecs.Utilities.Helpers;

public static class BitSetHelper
{
    public static void Set<TBitSet>(ref TBitSet bitset, Span<ComponentMeta> components) where TBitSet : IBitSet
    {
        foreach (var component in components)
        {
            bitset.Set(component.Id);
        }
    }
}