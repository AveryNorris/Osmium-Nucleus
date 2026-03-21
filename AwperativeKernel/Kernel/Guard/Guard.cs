using System;
using System.Collections.Generic;
using System.Linq;


namespace OsmiumNucleus;


/// <summary>
/// Holds a myriad of useful attributes which you are more than welcome to use! These attributes are not meant for reflection, but merely as a means of straightforward debugging.
/// Each attribute gives has a method called VerifyOrThrow() which can differ parameter wise. VerifyOrThrow() returns a bool based on if the condition is true or not.
/// If it is false it will try to throw an error, and returns false as well.The only time this behavior differs, is if Osmium is set to IgnoreErrors. In that case it will return true no matter what.
/// (However it will still debug unless that is disabled too).
/// </summary>
/// <usage>
/// The attributes have been designed to be used in methods like so : if(!Attribute.VerifyOrThrow()) return; This usage allows the attribute to control the flow of output, and halt any unsafe process.
/// However, nothing is stopping you from using them any other way, so go wild. Feel free to make more, or use these in your own code!
/// </usage>
/// <author> Avery Norris </author>
public static class Guard
{
    #region Docker/Entity
    
    /// <summary> Requires that any Component is owned by the Docker</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static bool DockerOwns(ComponentDocker __docker, Component __component) {
        if (__docker.Contains(__component)) return true;

        Debug.LogError("Docker does not own the Component!",
            ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
                __component.GetType().Name,
                __component.Name,
                __component.GetHashCode().ToString("N0"),
                __docker.GetType().Name,
                __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                __docker.GetHashCode().ToString("N0")
            ]);

        return Debug.IgnoreErrors;
    }



    /// <summary> Requires that the Docker does not own the given Component</summary>
    public static bool DockerDoesntOwn(ComponentDocker __docker, Component __component) {
        if (!__docker.Contains(__component)) return true;

        Debug.LogError("Docker owns the Component!",
            ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
                __component.GetType().Name,
                __component.Name,
                __component.GetHashCode().ToString("N0"),
                __docker.GetType().Name,
                __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                __docker.GetHashCode().ToString("N0")
            ]);

        return Debug.IgnoreErrors;
    }



    /// <summary> Requires that the Component does not belong to a Docker</summary>
    public static bool OrphanComponent(Component __component) {
        if (__component.ComponentDocker == null) return true;

        Debug.LogError("Component is already owned!",
            ["ComponentType", "ComponentName", "ComponentHash", "DockerType", "DockerName", "DockerHash"], [
                __component.GetType().Name,
                __component.Name,
                __component.GetHashCode().ToString("N0"),
                __component.ComponentDocker.GetType().Name,
                __component.ComponentDocker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
                __component.ComponentDocker.GetHashCode().ToString("N0")
            ]);

        return Debug.IgnoreErrors;
    }
    
    
    
    /// <summary> Requires that a given Docker is not the same</summary>
    public static bool DifferentDocker(ComponentDocker __docker, ComponentDocker __other) {
        if (!__docker.Equals(__other)) return true;

        Debug.LogError("The dockers are the same!", ["DockerType", "DockerName", "DockerHash"], [
            __docker.GetType().Name,
            __docker switch { Scene scene => scene.Name, Component component => component.Name, _ => "unknown" },
            __docker.GetHashCode().ToString("N0")
        ]);

        return Debug.IgnoreErrors;
    }



    /// <summary> Requires that the Component is not null</summary>
    /// <inheritdoc cref="DockerOwns.VerifyOrThrow"/>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static bool ComponentNotNull(Component __component) {
        if (__component != null) return true;

        Debug.LogError("Component is null!");

        return Debug.IgnoreErrors;
    }



    /// <summary> Requires that the Docker is not null</summary>
    public static bool DockerNotNull(ComponentDocker __componentDocker) {
        if (__componentDocker != null) return true;

        Debug.LogError("Docker is null!");

        return Debug.IgnoreErrors;
    }



    /// <summary> Requires that a given Scene is not null</summary>
    public static bool SceneNotNull(Scene __scene) {
        if (__scene != null) return true;

        Debug.LogError("Scene is null!");

        return Debug.IgnoreErrors;
    }
    
    /// <summary> Requires that a given Scene does not exist</summary>
    public static bool SceneDoesntExist(Scene __scene) {
        if (!Osmium._scenes.Contains(__scene)) return true;

        Debug.LogError("Scene already exists!");

        return Debug.IgnoreErrors;
    }
    
    /// <summary> Requires that a given Scene exists</summary>
    public static bool SceneExists(string __scene) {
        if (Osmium.GetScene(__scene) != null) return true;

        Debug.LogError("Scene does not exist!");

        return Debug.IgnoreErrors;
    }
    
    #endregion

    #region Null/Collection



    /// <summary> Requires all elements in an Enumerator are not null</summary>
    public static bool EnumerableNotNull(IEnumerable<object> __enumerator) {
        if (__enumerator == null) { Debug.LogError("A given enumerator is null!"); return Debug.IgnoreErrors; }
            
        foreach (object obj in __enumerator) {
            if (obj == null) {
                Debug.LogError("A given enumerator has null members!", ["Type"], [__enumerator.GetType().Name]);
                return Debug.IgnoreErrors;
            }
        }

        return true;
    }
    
    
    
    /// <summary> Requires that the enumerator contains a certain element.</summary>
    public static bool EnumerableContains(IEnumerable<object> __enumerator, object __object) {
        if (__enumerator.Contains(__object)) return true;
            
        Debug.LogError("A given enumerator does not contains an object!", ["EnumeratorType", "ObjectType", "Value"], [__enumerator.GetType().Name, __object.GetType().Name, __object.ToString()]);

        return Debug.IgnoreErrors;
    }
    
    
    
    /// <summary> Requires that the enumerator does not contain a certain element.</summary>
    public static bool EnumerableDoesntContain(IEnumerable<object> __enumerator, object __object) {
            if (!__enumerator.Contains(__object)) return true;
            
            Debug.LogError("A given enumerator already contains the object object!", ["EnumeratorType", "ObjectType", "Value"], [__enumerator.GetType().Name, __object.GetType().Name, __object.ToString()]);

            return Debug.IgnoreErrors;
    }



    /// <summary> Requires a given object is not null </summary>
    public static bool NotNull(Object __object) {
        if (__object != null) return true;

        Debug.LogError("A given object is null!");

        return Debug.IgnoreErrors;
    }



    /// <summary> Requires that an integer fits a range</summary>
    public static bool ValueFitsRange(int __index, int __min, int __max) {
        if (__index >= __min && __index <= __max) return true;

        Debug.LogError("Value does not fit range!", ["Index"], [__index.ToString("N0")]);

        return Debug.IgnoreErrors;
    }
    
    #endregion
}