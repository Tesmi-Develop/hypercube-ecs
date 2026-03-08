using Hypercube.Ecs.Components;
using Hypercube.Ecs.Utilities.Helpers;
using Hypercube.Utilities.Collections.Bit;

namespace Hypercube.Ecs.Utilities.Extensions;

public static class BitSetExtension
{
    public static BitSet Set(this BitSet context, Span<ComponentMeta> components)
    {
        BitSetHelper.Set(ref context, components);
        return context;
    }
}