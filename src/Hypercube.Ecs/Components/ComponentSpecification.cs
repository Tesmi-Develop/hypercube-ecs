namespace Hypercube.Ecs.Components;

public enum ComponentSpecification : byte
{
    /// <summary>
    /// Standard component that can be added to multiple entities and stores unique data per entity.
    /// </summary>
    Dynamic  = 0,
    
    /// <summary>
    /// Singleton component: only one instance exists, typically used for shared state.
    /// </summary>
    Static   = 1,
    
    /// <summary>
    /// Marker component that carries no data. Used to tag entities for identification or filtering.
    /// </summary>
    Tag      = 2,
    
    /// <summary>
    /// Read-only static component: data cannot be modified at runtime. Always treated as a static component.
    /// </summary>
    Readonly = 3,
}
