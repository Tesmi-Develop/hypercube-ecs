using JetBrains.Annotations;

// ReSharper disable StaticMemberInGenericType

namespace Hypercube.Ecs.Components;

/// <summary>
/// Provides static metadata and a signature for a specific component type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The component type implementing <see cref="IComponent"/>.</typeparam>
[PublicAPI]
public static class ComponentStaticMeta<T> where T : IComponent
{
    public static readonly ComponentMeta Meta;
    public static readonly Signature Signature;

    static ComponentStaticMeta()
    {
        Meta = ComponentRegistry.Add<T>();
        Signature = new Signature(Meta);
    }
}
