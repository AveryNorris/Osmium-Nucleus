using System.Collections.Generic;


namespace AwperativeKernel;


public abstract partial class Component
{
    /// <summary> Scene the Component resides in. To save ram it's not stored but calculated on the fly. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public Scene Scene => __QueryScene();
    private Scene __QueryScene() {
        if (ComponentDocker is Scene scene) return scene;
        if (ComponentDocker is Component Component) return Component.__QueryScene();
            
        return null;
    }
    
    
    
    /// <summary> Returns the Parent Component. Will be null if the Component's parent is a Scene. </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.VeryLow), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public Component Parent => __QueryParent();
    private Component __QueryParent() {
        if (ComponentDocker is Component Component)
            return Component;
        return null;
    }
    
    
    
    /// <summary> All parent Components and the parents of the parents up until the Scene. Will only list parents of parents, not uncle Components. And will not list the Scene </summary>
    [MarkerAttributes.CalculatedProperty, MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public IEnumerable<Component> AllParents => __QueryComponents();
    private IEnumerable<Component> __QueryComponents() {
        List<Component> returnValue = [];
        ComponentDocker currentComponentDocker = ComponentDocker;

        while (currentComponentDocker is not AwperativeKernel.Scene) {
            if (currentComponentDocker is Component Component) {
                returnValue.Add(Component);
                currentComponentDocker = Component.ComponentDocker;
            }
        }

        return returnValue;
    }

    
}