using Hypercube.Ecs.Components;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

// TODO: Roslyn Source Generator

[PublicAPI]
public readonly struct Entity<T>
    where T : IComponent
{
    public readonly Entity Id;
    public readonly T Component;

    public Entity(Entity id, T component)
    {
        Id = id;
        Component = component;
    }
    
    public static implicit operator Entity(Entity<T> arg) => arg.Id;
    public static implicit operator T(Entity<T> arg) => arg.Component;
}

[PublicAPI]
public readonly struct Entity<T1, T2>
    where T1 : IComponent
    where T2 : IComponent
{
    public readonly Entity Id;
    public readonly T1 Component1;
    public readonly T2 Component2;

    public Entity(Entity id, T1 component1, T2 component2)
    {
        Id = id;
        Component1 = component1;
        Component2 = component2;
    }
    
    public static implicit operator Entity(Entity<T1, T2> arg) => arg.Id;
    public static implicit operator T1(Entity<T1, T2> arg) => arg.Component1;
    public static implicit operator T2(Entity<T1, T2> arg) => arg.Component2;
}

[PublicAPI]
public readonly struct Entity<T1, T2, T3>
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
{
    public readonly Entity Id;
    public readonly T1 Component1;
    public readonly T2 Component2;
    public readonly T3 Component3;

    public Entity(Entity id, T1 component1, T2 component2, T3 component3)
    {
        Id = id;
        Component1 = component1;
        Component2 = component2;
        Component3 = component3;
    }
    
    public static implicit operator Entity(Entity<T1, T2, T3> arg) => arg.Id;
    public static implicit operator T1(Entity<T1, T2, T3> arg) => arg.Component1;
    public static implicit operator T2(Entity<T1, T2, T3> arg) => arg.Component2;
    public static implicit operator T3(Entity<T1, T2, T3> arg) => arg.Component3;
}

[PublicAPI]
public readonly struct Entity<T1, T2, T3, T4>
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
{
    public readonly Entity Id;
    public readonly T1 Component1;
    public readonly T2 Component2;
    public readonly T3 Component3;
    public readonly T4 Component4;

    public Entity(Entity id, T1 component1, T2 component2, T3 component3, T4 component4)
    {
        Id = id;
        Component1 = component1;
        Component2 = component2;
        Component3 = component3;
        Component4 = component4;
    }
    
    public static implicit operator Entity(Entity<T1, T2, T3, T4> arg) => arg.Id;
    public static implicit operator T1(Entity<T1, T2, T3, T4> arg) => arg.Component1;
    public static implicit operator T2(Entity<T1, T2, T3, T4> arg) => arg.Component2;
    public static implicit operator T3(Entity<T1, T2, T3, T4> arg) => arg.Component3;
    public static implicit operator T4(Entity<T1, T2, T3, T4> arg) => arg.Component4;
}

[PublicAPI]
public readonly struct Entity<T1, T2, T3, T4, T5>
    where T1 : IComponent
    where T2 : IComponent
    where T3 : IComponent
    where T4 : IComponent
    where T5 : IComponent
{
    public readonly Entity Id;
    public readonly T1 Component1;
    public readonly T2 Component2;
    public readonly T3 Component3;
    public readonly T4 Component4;
    public readonly T5 Component5;

    public Entity(Entity id, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5)
    {
        Id = id;
        Component1 = component1;
        Component2 = component2;
        Component3 = component3;
        Component4 = component4;
        Component5 = component5;
    }
    
    public static implicit operator Entity(Entity<T1, T2, T3, T4, T5> arg) => arg.Id;
    public static implicit operator T1(Entity<T1, T2, T3, T4, T5> arg) => arg.Component1;
    public static implicit operator T2(Entity<T1, T2, T3, T4, T5> arg) => arg.Component2;
    public static implicit operator T3(Entity<T1, T2, T3, T4, T5> arg) => arg.Component3;
    public static implicit operator T4(Entity<T1, T2, T3, T4, T5> arg) => arg.Component4;
    public static implicit operator T5(Entity<T1, T2, T3, T4, T5> arg) => arg.Component5;
}
