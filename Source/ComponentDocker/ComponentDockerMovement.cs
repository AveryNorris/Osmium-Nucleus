using System.Collections.Generic;
using System.Linq;


namespace OsmiumNucleus;


public abstract partial class ComponentDocker
{

    
    
    /// <summary> Moves a component belonging to the docker to another docker</summary>
    public void Move(Component __component, ComponentDocker __componentDocker) {
        if(!Guard.ComponentNotNull(__component)) return;
        if(!Guard.DockerOwns(this, __component)) return;
        if(!Guard.DockerNotNull(__componentDocker)) return;
        if(!Guard.DifferentDocker(this, __componentDocker)) return;
        
        RemoveComponentFromLists(__component);
        __componentDocker.AddComponentToLists(__component);
        
        __component.Parent = __componentDocker;
    }
        

    /// <summary> Moves all components in a list to another docker</summary>
    public void MoveAll(IEnumerable<Component> __Components, ComponentDocker __componentDocker) {
        if(!Guard.EnumerableNotNull(__Components)) return;
        if(!Guard.DockerNotNull(__componentDocker)) return;
        if(!Guard.DifferentDocker(this, __componentDocker)) return;

        foreach (Component Component in __Components) {
            if(!Guard.DockerOwns(__componentDocker, Component)) return;
            Move(Component, __componentDocker);
        }
    }
    
    /// <summary> Gets all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)] 
    public void MoveAll(ComponentDocker __componentDocker) => MoveAll(GetAll(), __componentDocker);

    /// <summary> Moves the first component with a given Type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Move<__Type>(ComponentDocker __componentDocker) where __Type : Component => Move(Get<__Type>(), __componentDocker);



    /// <summary> Moves all components of a given type</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>(ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(), __componentDocker);



    /// <summary> Moves all components that have all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll(IEnumerable<string> __tags, ComponentDocker __componentDocker) => MoveAll(GetAll(__tags), __componentDocker);
    



    /// <summary> Moves all Components that have the given type, and all the given tags</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>(IEnumerable<string> __tags, ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>(__tags), __componentDocker);
    
    
    
    
    /// <summary> Moves all the components with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void MoveAll(string __tag, ComponentDocker __componentDocker) => MoveAll(GetAll([__tag]), __componentDocker);
    
    
    
    /// <summary> Moves all the components that have a certain type, and a certain tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void MoveAll<__Type>(string __tag, ComponentDocker __componentDocker) where __Type : Component => MoveAll(GetAll<__Type>([__tag]), __componentDocker);
    
    

    /// <summary> Moves the first component with the given tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public void Move(string __tag, ComponentDocker __componentDocker) => Move(GetAll([__tag]).FirstOrDefault(), __componentDocker);


    
    /// <summary> Moves the moves component with the given type and tag</summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public void Move<__Type>(string __tag, ComponentDocker __componentDocker) where __Type : Component => Move(GetAll<__Type>(__tag).FirstOrDefault(), __componentDocker);
    
    
}