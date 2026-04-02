using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;


namespace OsmiumNucleus;


/// <summary> Registers <see cref="Component"/> events for Osmium! Builds them into a hyper fast dictionary for quick access. </summary>
/// <author> Avery Norris </author>
internal static class EventManager
{
    
    
    
    /// <summary> Holds an associated action for each component and a time event. Is built with ResolveTypeEvents() during Initialize().
    /// There is little to no error checking; it is assumed that any Component trying to index this dictionary, at least has null at a key/value! There cannot be empty
    /// entries even if a Component receives no events. </summary>
    [MarkerAttributes.UnsafeInternal]
    internal static Dictionary<Type, Action<Component>[]> _TypeAssociatedTimeEvents = [];
    
    
    
    /// <summary> All types of time based events in Osmium. Stored at their respective ID's by index.</summary>
    internal static readonly ImmutableArray<string> Events = [
        "Load", "Unload", "Update", "Draw", "Create", "Remove"
    ];
    
    
    
    /// <summary> Searches all assemblies in the current App Domain; and automatically resolves all Components with a given type! This is necessary for Osmium's function
    /// and part of the reason why Initialize() must be called so early.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAllModules() {
        ResolveAllModules(AppDomain.CurrentDomain.GetAssemblies());
        Debug.LogAction("Finished Resolving!");
    }
    
    
    
    /// <summary> Resolves all the components from only the given assemblies</summary>
    [MarkerAttributes.EditorPipeline]
    internal static void ResolveAllModules(IEnumerable<Assembly> __sources) {
        foreach (Assembly assembly in __sources) foreach (Type type in assembly.GetTypes()) 
            EventManager.ResolveTypeEvents(type);
    }
    
    
    
    /// <summary> Compiles a single type, and stores its events in the dictionary.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveTypeEvents(Type __type) {
        if (!__type.IsSubclassOf(typeof(Component))) return;

        List<Action<Component>> timeEvents = [];

        foreach (MethodInfo eventMethod in Events.Select(__type.GetMethod)) {
            if (eventMethod == null) { timeEvents.Add(null); continue; }

            //create a new delegate expression that calls the Components associated method.
            ParameterExpression ComponentInstanceParameter = Expression.Parameter(typeof(Component), "__component");
            UnaryExpression Casting = Expression.Convert(ComponentInstanceParameter, __type);
            MethodCallExpression Call = Expression.Call(Casting, eventMethod);
            Expression<Action<Component>> Lambda = Expression.Lambda<Action<Component>>(Call, ComponentInstanceParameter);
            timeEvents.Add(Lambda.Compile());
        }
        
        Debug.LogAction("Found and Resolved events attached to : " + __type.Name + " In " + __type.Namespace);

        _TypeAssociatedTimeEvents.Add(__type, timeEvents.ToArray());
    }
    
    
    
}