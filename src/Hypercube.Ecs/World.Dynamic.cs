using System.Reflection;
using JetBrains.Annotations;

namespace Hypercube.Ecs;

/// <summary>
/// Provides a dynamic bridge for component manipulation using runtime <see cref="Type"/> information.
/// </summary>
public partial class World
{
    /// <summary>
    /// Caches fully constructed generic methods to avoid repeated <see cref="MethodInfo.MakeGenericMethod"/> calls.
    /// Key: (Method Name, Component Type).
    /// </summary>
    private readonly Dictionary<(string Name, Type GenericType), MethodInfo> _methodCache = new();
    
    /// <summary>
    /// Caches open generic method definitions to avoid expensive <see cref="Type.GetMethods"/> lookups and LINQ filtering.
    /// Key: (Method Name, Parameter Count).
    /// </summary>
    private readonly Dictionary<(string Name, int ParamCount), MethodInfo> _genericDefinitions = new();

    /// <summary>
    /// Dynamically adds a component of the specified <paramref name="type"/> to an entity.
    /// Uses the default constructor for the component type.
    /// </summary>
    /// <param name="entity">The target entity.</param>
    /// <param name="type">The runtime type of the component to add.</param>
    /// <returns>The newly created component instance.</returns>
    public object Add(Entity entity, Type type)
    {
        var method = GetGenericMethod(nameof(Add), type, Type.EmptyTypes);
        return method.Invoke(this, [entity])!;
    }

    /// <summary>
    /// Dynamically adds a specific component instance to an entity.
    /// </summary>
    /// <param name="entity">The target entity.</param>
    /// <param name="value">The component instance to add. If null, the operation is ignored.</param>
    public void Add(Entity entity, object? value)
    {
        if (value is null) 
            return;
        
        var type = value.GetType();
        // Finds the overload: Add<T>(Entity entity, ref T component)
        var method = GetGenericMethod(nameof(Add), type, [typeof(Entity), type.MakeByRefType()]);
        method.Invoke(this, [entity, value]);
    }

    /// <summary>
    /// Dynamically retrieves a component instance of the specified <paramref name="type"/> from an entity.
    /// </summary>
    /// <param name="entity">The target entity.</param>
    /// <param name="type">The runtime type of the component to retrieve.</param>
    /// <returns>The component instance.</returns>
    public object Get(Entity entity, Type type)
    {
        var method = GetGenericMethod(nameof(Get), type, [typeof(Entity)]);
        return method.Invoke(this, [entity])!;
    }

    /// <summary>
    /// Dynamically checks if an entity has a component of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="entity">The target entity.</param>
    /// <param name="type">The runtime type of the component to check.</param>
    /// <returns>True if the entity possesses the component; otherwise, false.</returns>
    public bool Has(Entity entity, Type type)
    {
        var method = GetGenericMethod(nameof(Has), type, [typeof(Entity)]);
        return (bool)method.Invoke(this, [entity])!;
    }

    /// <summary>
    /// Dynamically removes a component of the specified <paramref name="type"/> from an entity.
    /// </summary>
    /// <param name="entity">The target entity.</param>
    /// <param name="type">The runtime type of the component to remove.</param>
    public void Remove(Entity entity, Type type)
    {
        var method = GetGenericMethod(nameof(Remove), type, [typeof(Entity)]);
        method.Invoke(this, [entity]);
    }
    
    /// <summary>
    /// Resolves and caches a generic method using a two-tier caching strategy.
    /// </summary>
    /// <param name="name">The name of the method to resolve.</param>
    /// <param name="genericType">The type argument for the generic method.</param>
    /// <param name="parameterTypes">The types of the method parameters used to resolve overloads.</param>
    /// <returns>A constructed <see cref="MethodInfo"/> ready for invocation.</returns>
    private MethodInfo GetGenericMethod(string name, Type genericType, Type[] parameterTypes)
    {
        var cacheKey = (name, genericType);
        if (_methodCache.TryGetValue(cacheKey, out var cached))
            return cached;

        var definitionKey = (name, parameterTypes.Length);
        if (!_genericDefinitions.TryGetValue(definitionKey, out var methodDefinition))
        {
            methodDefinition = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .First(m => m.Name == name && 
                            m.IsGenericMethod && 
                            m.GetGenericArguments().Length == 1 &&
                            MatchParameters(m.GetParameters(), parameterTypes));
            
            _genericDefinitions[definitionKey] = methodDefinition;
        }
        
        var genericMethod = methodDefinition.MakeGenericMethod(genericType);
        _methodCache[cacheKey] = genericMethod;
        
        return genericMethod;
    }

    /// <summary>
    /// Validates if a method's parameters match the expected types, 
    /// accounting for generic placeholders and reference types.
    /// </summary>
    /// <param name="parameters">The parameters from the <see cref="MethodInfo"/>.</param>
    /// <param name="parameterTypes">The expected runtime types.</param>
    /// <returns>True if the signatures match; otherwise, false.</returns>
    private bool MatchParameters(ParameterInfo[] parameters, Type[] parameterTypes)
    {
        if (parameters.Length != parameterTypes.Length)
            return false;

        for (var i = 0; i < parameters.Length; i++)
        {
            var paramType = parameters[i].ParameterType;
            
            if (paramType.IsGenericParameter || (paramType.IsByRef && paramType.GetElementType()!.IsGenericParameter))
                continue;

            if (paramType != parameterTypes[i])
                return false;
        }

        return true;
    }
}