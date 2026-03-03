namespace AwperativeKernel;


public abstract partial class Component
{
    /// <inheritdoc cref="Awperative.CreateScene"/>
    [MarkerAttributes.MethodPointer]
    public static Scene CreateScene(string __name) => Awperative.CreateScene(__name);



    /// <inheritdoc cref="Awperative.GetScene"/>
    [MarkerAttributes.MethodPointer]
    public static Scene GetScene(string __name) => Awperative.GetScene(__name);



    /// <inheritdoc cref="Awperative.CloseScene(AwperativeKernel.Scene)"/>
    [MarkerAttributes.MethodPointer]
    public void RemoveScene(Scene __scene) => Awperative.CloseScene(__scene);



    /// <inheritdock cref="Awperative.CloseScene(string)" />
    [MarkerAttributes.MethodPointer]
    public void RemoveScene(string __name) => Awperative.CloseScene(__name);
    
    
    
    /// <inheritdoc cref="ComponentDocker.Move"/>
    [MarkerAttributes.MethodPointer]
    public void Move(ComponentDocker __newDocker) => ComponentDocker.Move(this, __newDocker);



    /// <summary> Makes the Component destroy itself </summary>
    [MarkerAttributes.MethodPointer]
    public void DestroySelf() => ComponentDocker.Destroy(this);



    /// <inheritdoc cref="Awperative.Base"/>
    public Base AwperativeBase => Awperative.Base;
}