using System;
using System.Collections.Generic;
using System.Reflection;
using OsmiumNucleus;


namespace OsmiumNucleus;


/// <summary>
/// Manages all Osmium reflection based activities, right now limited to registering events.
/// </summary>
/// <author> Avery Norris </author>
internal static class ReflectionManager
{
    /// <summary> Resolves all the modules from the calling assembly and module manager</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveAllModules() {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) { ResolveModule(assembly); }
        
        Debug.LogAction("Finished Resolving!");
    }
    
    /// <summary> Resolves all the types in an assembly.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal static void ResolveModule(Assembly __assembly) {
        foreach (Type type in __assembly.GetTypes()) {
            EventManager.CompileType(type);
        }
    }
}