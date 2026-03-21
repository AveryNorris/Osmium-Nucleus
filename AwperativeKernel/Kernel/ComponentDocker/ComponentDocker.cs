using System;
using System.Collections;
using System.Collections.Generic;


namespace OsmiumNucleus;


/// <summary>
/// Base class for all Osmium objects. Responsible for Managing hierarchy between Components and Scenes, has Extensive Component Manipulation Available.
/// Also transfers Time and Carries most of the responsibilities akin to the Component. NOT FOR INHERITING, please use docker by inheriting component.
/// </summary>
/// <author> Avery Norris </author>
public abstract partial class ComponentDocker : IEnumerable<Component>, IEquatable<Component>, IEquatable<ComponentDocker>, IEquatable<Scene>
{
    
    //Blocks external inheritance
    internal ComponentDocker() {}
    
    
    
    /// <summary> Core of the Docker, holds all of the Components, sorted by update priority.</summary>
    [MarkerAttributes.UnsafeInternal] internal readonly List<Component> _components = [];
    /// <summary> Holds a list of Components at each of their types. This optimizes Get&lt;Type&gt; to O(1) </summary>
    [MarkerAttributes.UnsafeInternal] internal readonly Dictionary<Type, HashSet<Component>> _componentTypeDictionary = new();
    /// <summary> Stores a Component in a list at each of their tags. This optimizes Get(string tag) to O(1)</summary>
    [MarkerAttributes.UnsafeInternal] internal readonly Dictionary<string, HashSet<Component>> _componentTagDictionary = new();
    
    
        
    /// <summary>  All children belonging to the Component. </summary>
    public IEnumerable<Component> Children => _components;
    
    
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
        get => !Guard.ValueFitsRange(__index, 0, _components.Count) ? null : _components[__index];
        set { if (!Guard.ValueFitsRange(__index, 0, _components.Count)) return; _components[__index] = value; }
    }
    
    
    
    //Enumerators that allow convenient foreach loops.
    IEnumerator IEnumerable.GetEnumerator() { return  GetEnumerator(); } 
    public IEnumerator<Component> GetEnumerator() { return new ComponentDockEnum([.._components]); }
    
    
    
    /// <summary>Compares the Docker to another Scene.</summary>
    public bool Equals(Scene __other) {
        if (this is Scene scene)
            return scene == __other;
        return false;
    }
    
    /// <summary>Compares the Docker to another Component.</summary>
    public bool Equals(Component __other) {
        if (this is Component component)
            return component == __other;
        return false;
    }
    
    /// <summary>Compares the Docker to another Docker.</summary>
    public bool Equals(ComponentDocker __other) {
        return this == __other;
    }





    /// <summary> Resorts the component list to order of priority </summary>
    [MarkerAttributes.UnsafeInternal]
    internal void UpdatePriority(Component __component, int __priority) {
        _components.Sort(Component._prioritySorter);
    }
    
    /// <summary> Sends an event to all Children and tells them to continue it. </summary>
    /// <param name="__timeEvent"> Integer ID of the event to send. </param>
    [MarkerAttributes.UnsafeInternal]
    internal void ChainEvent(int __timeEvent) {
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
        if(!_componentTagDictionary.ContainsKey(__tag)) _componentTagDictionary[__tag].Remove(__component);
    }
}