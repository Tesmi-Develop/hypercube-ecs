namespace Hypercube.Ecs.UnitTests;

[TestFixture]
public sealed class WorldEntityTests
{
    private readonly World _world = new();

    [Test]
    public void CreateMany_DeleteMany_ValidateCorrectly()
    {
        const int entityCount = 10_000;
        
        var entities = new List<Entity>(entityCount);
        
        for (var i = 0; i < entityCount; i++)
        {
            var entity = _world.Create();
            entities.Add(entity);
        }
        
        foreach (var entity in entities)
        {
            Assert.That(_world.Validate(entity), Is.True);
        }
        
        for (var i = 0; i < entities.Count; i += 2)
        {
            _world.Delete(entities[i]);
        }
        
        for (var i = 0; i < entities.Count; i++)
        {
            if (i % 2 == 0)
            {
                Assert.That(_world.Validate(entities[i]), Is.False);
            }
            else
            {
                Assert.That(_world.Validate(entities[i]), Is.True);
            }
        }
    }
}