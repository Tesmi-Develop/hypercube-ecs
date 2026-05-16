using JetBrains.Annotations;

namespace Hypercube.Ecs.Analyzers;

// NOTE: Shared?
[PublicAPI]
public static class Link
{
    public const string ComponentInterfaceFullName = "Hypercube.Ecs.Components.IComponent";
    public const string ComponentInterfaceName = "IComponent";

    public const string ComponentAttributeFullName = "Hypercube.Ecs.Attributes.ComponentAttribute";
    public const string ComponentAttributeName = "ComponentAttribute";
}
