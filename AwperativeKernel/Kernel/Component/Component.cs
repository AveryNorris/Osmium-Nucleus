using System.Collections.Generic;


namespace OsmiumNucleus;


/// <summary>
/// The backing class for components in Osmium; provides access to a few important methods, and inheriting this is required to receive time based events.
/// When Start() is called, Osmium reflectively analyzes all classes that inherits component, and checks for given events. See the wiki for more information!
/// </summary>
/// <author> Avery Norris </author>
public abstract partial class Component : ComponentDocker
{



    /// <summary> Current parent of the Component. Can be either a Scene or another Component.</summary>
    [MarkerAttributes.UnsafeInternal]
    public ComponentDocker ComponentDocker { get; internal set; } = null;

    
    /// <summary> Component's name </summary>
    public string Name {
        get => _name;
        set { if (!Guard.NotNull(value)) return; _name = value; }
    } [MarkerAttributes.UnsafeInternal] private string _name;

    
    /// <summary> Represents the state of this Component, The largest bit represents if the Component is enabled or not, while the
    /// next 7 represent its priority like so : (Enabled -> 0 | Priority -> 0000000) </summary>
    [MarkerAttributes.UnsafeInternal] private byte OrderProfile;

    /// <summary> If the component receives time events or not. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public bool Enabled {
        get => (OrderProfile & 128) > 0;
        set => OrderProfile = (byte)((OrderProfile & 127) | (value ? 128 : 0));
    }
    
    /// <summary> Represents the Component's Update priority; higher priorities get updated first. Can be any integer in the range -64 -> 63. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public int Priority {
        get => (sbyte)(OrderProfile << 1) >> 1;
        set {
            if(!Guard.ValueFitsRange(value, -64, 63)) return;
            OrderProfile = (byte)((OrderProfile & 0x80) | (value & 0x7F));
            ComponentDocker.UpdatePriority(this, value);
        }
    }
    
    /// <summary> Algorithm for how Components are sorted via Priority </summary>
    internal static readonly Comparer<Component> _prioritySorter = Comparer<Component>.Create((a, b) => {
        int result = b.Priority.CompareTo(a.Priority); 
        return (result != 0) ? result : a.GetHashCode().CompareTo(b.GetHashCode());
    });


    
    /// <summary> A list of all tags belonging to the component. Use AddTag() to modify it.</summary>
    public IReadOnlySet<string> Tags => _tags;
    [MarkerAttributes.UnsafeInternal] internal HashSet<string> _tags = [];
    
    
    /// <summary> Attempts to send an event to the component, and quietly exits if not.</summary>
    [MarkerAttributes.UnsafeInternal]
    internal void TryEvent(int __timeEvent) {
        EventManager._TypeAssociatedTimeEvents[GetType()][__timeEvent]?.Invoke(this);
    }



    /// <summary> Adds a new tag to the component</summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void AddTag(string __tag) {
        if(!Guard.NotNull(__tag)) return;
        if(!Guard.EnumerableDoesntContain(_tags, __tag)) return;

        _tags.Add(__tag);
        ComponentDocker.HashTaggedComponent(__tag, this);
    }
    
    
    
    
    
    /// <summary> Removes a tag from the component.</summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void RemoveTag(string __tag) {
        if (!Guard.NotNull(__tag)) return;
        if(!Guard.EnumerableContains(_tags, __tag)) return;

        _tags.Remove(__tag);
        ComponentDocker.UnhashTaggedComponent(__tag, this);
    }

    /// <summary> Just gives the Component's name. But makes debugging a little easier :).</summary>
    public override string ToString() {
        return this.Name;
    }
}