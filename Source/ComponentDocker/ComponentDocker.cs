using System.Collections;


namespace OsmiumNucleus;


/// <summary> Base class for both Scenes and Components. Responsible for Managing hierarchy between Components and Scenes and managing children, has Extensive Component Manipulation Available.
/// Also transfers Time and Carries most of the responsibilities akin to the Component. NOT FOR OUTSIDE INHERITANCE (this is very under the hood, and meant to be mostly hidden), please use docker by inheriting component. </summary>
/// <author> Avery Norris </author>
#nullable enable
public abstract partial class ComponentDocker : IEnumerable<Component>
{
    
    
    
    //Blocks external inheritance
    internal ComponentDocker() {}
    
    
    
    /// <summary> Core of the Docker, holds all the Components, sorted by update priority.</summary>
    [MarkerAttributes.UnsafeInternal] internal readonly List<Component> _components = [];
    /// <summary> Holds a list of Components at each of their types. This optimizes Get&lt;Type&gt; to O(1) </summary>
    [MarkerAttributes.UnsafeInternal] internal readonly Dictionary<Type, HashSet<Component>> _componentTypeDictionary = new();
    /// <summary> Stores a Component in a list at each of their tags. This optimizes Get(string tag) to O(1)</summary>
    [MarkerAttributes.UnsafeInternal] internal readonly Dictionary<string, HashSet<Component>> _componentTagDictionary = new();
    
    
    
    /// <summary> Algorithm for how Components are sorted via Priority </summary>
    internal static readonly Comparer<Component> _prioritySorter = Comparer<Component>.Create((a, b) => {
        int result = b.Priority.CompareTo(a.Priority); 
        return (result != 0) ? result : a.GetHashCode().CompareTo(b.GetHashCode());
    });

    /// <summary> Resorts the component list to order of priority </summary>
    [MarkerAttributes.UnsafeInternal]
    internal void UpdatePriority(Component __component) {
        _components.Sort(Component._prioritySorter);
    }
    
    
    
    
    
    /// <summary>  All children belonging to the Docker. </summary>
    public IReadOnlyList<Component> Children => _components;
    
    
    
    /// <summary> All children and children of children until the bottom of the scene. Uses Breadth First Search. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.High), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IEnumerable<Component> AllChildren => GetAllChildren();
    public IEnumerable<Component> GetAllChildren() {
        List<Component> returnValue = [];
        Queue<Component> queue = new(_components);
        while (queue.Count > 0) {
            Component current = queue.Dequeue();
            returnValue.Add(current);
            
            for (int i = 0; i < current.Count; i++) queue.Enqueue(current[i]);
        }

        return returnValue;
    }
    
    
    
    /// <summary>Amount of Components attached to the Docker</summary>
    public int Count => _components.Count;
    
    
    
    //Indexers to make for loops easier.
    public Component this[int __index] {
        get {
            if(__index < 0 || __index >= _components.Count) { Debug.LogError("Docker Index out of range!", ["Count", "Index"], [Count.ToString(), __index.ToString()]); return null; }
            return _components[__index];
        }
        set {
            if(__index < 0 || __index >= _components.Count) { Debug.LogError("Docker Index out of range!", ["Count", "Index"], [Count.ToString(), __index.ToString()]); return; }
            _components[__index] = value;
        }
    }
    
    
    
    //Enumerators that allow convenient foreach loops. Uses ToList() to make a new snapshot of the hashset and allow hashset modification in the loop.
    IEnumerator IEnumerable.GetEnumerator() { return  GetEnumerator(); } 
    public IEnumerator<Component> GetEnumerator() { return _components.ToList().GetEnumerator(); }
    
    
    
    /// <summary> Sends an event to all Children and tells them to continue it. Will stop if this is a Component, and it is not enabled</summary>
    /// <param name="__timeEvent"> Integer ID of the event to send. </param>
    [MarkerAttributes.UnsafeInternal]
    internal void ChainEvent(int __timeEvent) {
        if(this is Component { Enabled: false }) return;
        
        for (int i = 0; i < _components.Count; i++) {
            _components[i].TryEvent(__timeEvent);
            _components[i].ChainEvent(__timeEvent);
        }
    }
    
    
    
    /// <summary> Add a Component to the Docker. </summary>
    [MarkerAttributes.UnsafeInternal]
    private void AddComponentToLists(Component __component) {
        Type Type = __component.GetType();
        _components.Add(__component); 
        if (!_componentTypeDictionary.TryAdd(Type, [__component])) _componentTypeDictionary[Type].Add(__component);

        foreach (string tag in __component._tags) HashTaggedComponent(tag, __component);
    }

    
    
    /// <summary> Removes a Component from the Docker. </summary>
    [MarkerAttributes.UnsafeInternal]
    private void RemoveComponentFromLists(Component __component) {
        Type Type = __component.GetType();
        _components.Remove(__component); 
        
        if(_componentTypeDictionary.TryGetValue(Type, out HashSet<Component> value)) value.Remove(__component);
        
        foreach (string tag in __component._tags) UnhashTaggedComponent(tag, __component);
    }

    /// <summary> Hashes a component in the tag dictionary </summary>
    [MarkerAttributes.UnsafeInternal]
    internal void HashTaggedComponent(string __tag, Component __component) {
        if (!_componentTagDictionary.TryAdd(__tag, [__component])) 
            _componentTagDictionary[__tag].Add(__component);
    }

    /// <summary> Removes a component's hash from the tag dictionary </summary>
    [MarkerAttributes.UnsafeInternal]
    internal void UnhashTaggedComponent(string __tag, Component __component) {
        if (_componentTagDictionary.TryGetValue(__tag, out HashSet<Component>? value)) {
            value.Remove(__component); if(value.Count == 0) _componentTagDictionary.Remove(__tag);
        }
    }

}