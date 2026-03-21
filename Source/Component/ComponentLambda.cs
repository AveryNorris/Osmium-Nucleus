namespace OsmiumNucleus;


public abstract partial class Component
{
    /// <inheritdoc crOsmium.AddScene(string)cene"/>
    [MarkerAttributes.MethodPointer]
    public static Scene CreateScene(string __name) => Osmium.AddScene(__name);



    /// <inheritdoc cref="Osmium.GetScene"/>
    [MarkerAttributes.MethodPointer]
    public static Scene GetScene(string __name) => Osmium.GetScene(__name);



    /// <inheritdoc cref="Osmium.CloseScene(OsmiumNucleus.Scene)"/>
    [MarkerAttributes.MethodPointer]
    public void RemoveScene(Scene __scene) => Osmium.CloseScene(__scene);



    /// <inheritdock cref="Osmium.CloseScene(string)" />
    [MarkerAttributes.MethodPointer]
    public void RemoveScene(string __name) => Osmium.CloseScene(__name);
    
    
    
    /// <inheritdoc cref="ComponentDocker.Move(Component, OsmiumNucleus.ComponentDocker)"/>
    [MarkerAttributes.MethodPointer]
    public void Move(ComponentDocker __newDocker) => ComponentDocker.Move(this, __newDocker);



    /// <summary> Makes the Component destroy itself </summary>
    [MarkerAttributes.MethodPointer]
    public void DestroySelf() => ComponentDocker.Destroy(this);



    /// <inheritdoc cref="Osmium.Base"/>
    public Base AwperativeBase => Osmium.Base;
}