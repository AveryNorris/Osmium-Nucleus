using System.Reflection;



namespace OsmiumNucleus;


/// <summary>Manages all Osmium reflection based activities, right now limited to registering events.</summary>
/// <author> Avery Norris </author>
internal static class ReflectionManager
{
    
    
    
    /// <summary> Resolves all the modules from the calling assembly and module manager</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAllModules() {
        ResolveAllModules(AppDomain.CurrentDomain.GetAssemblies());
        
        Debug.LogAction("Finished Resolving!");
    }
    
    
    
    /// <summary> Resolves all the modules from the given assemblies</summary>
    [MarkerAttributes.EditorPipeline]
    internal static void ResolveAllModules(IEnumerable<Assembly> __sources) {
        foreach (Assembly assembly in __sources) { ResolveModule(assembly); }
    }
    
    
    
    /// <summary> Resolves all the types in an assembly.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveModule(Assembly __assembly) {
        foreach (Type type in __assembly.GetTypes()) EventManager.CompileType(type);
    }
    
    
    
}