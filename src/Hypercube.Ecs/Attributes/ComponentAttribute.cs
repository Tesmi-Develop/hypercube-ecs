using Hypercube.Ecs.Components;

namespace Hypercube.Ecs.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class ComponentAttribute : Attribute
{
    public readonly ComponentSpecification Specification;

    public ComponentAttribute(ComponentSpecification specification)
    {
        Specification = specification;
    }
}
