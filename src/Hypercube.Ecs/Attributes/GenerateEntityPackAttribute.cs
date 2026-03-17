using JetBrains.Annotations;

namespace Hypercube.Ecs.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class GenerateEntityPackAttribute : Attribute
{
    [UsedImplicitly]
    public int Index { get; }

    public GenerateEntityPackAttribute(int index = 5)
    {
        Index = index;
    }
}
