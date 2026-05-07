using Hypercube.Ecs.Archetypes;

namespace Hypercube.Ecs;

public partial class World
{
    private readonly ArchetypeContainer _archetypes = new();
    
    public ReadOnlySpan<Archetype> Archetypes => _archetypes.Archetypes;
    public int ArchetypeVersion => _archetypes.Version;
}
