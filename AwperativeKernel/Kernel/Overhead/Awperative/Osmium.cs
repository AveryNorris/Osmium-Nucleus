using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace OsmiumNucleus;

/// <summary>
/// Main class of Osmium, allows you to Create() scenes and Start() the game
/// </summary>
/// <author> Avery Norris </author>
public static class Osmium
{

    
    /// <summary> Current Version of Osmium </summary>
    public const string Version = "1.2C";



    /// <summary> Bottom class of Osmium. Contains the OpenTK Instance. </summary>
    public static Base Base { get; private set; }

    
    
    /// <summary> List of all scenes currently loaded in the kernel. </summary>
    public static IReadOnlySet<Scene> Scenes => _scenes;
    [MarkerAttributes.UnsafeInternal] internal static readonly HashSet<Scene> _scenes = [];


    
    /// <summary> Displays if Osmium has Started or not </summary>
    public static bool IsStarted { get; private set; } = false;
    /// <summary> Displays if the update loop is active</summary>
    public static bool IsRunning { get; private set; } = false;
    
    
    
    
    
    /// <summary> Gets Osmium ready to begin and Compiles Component functions. Please call before doing anything Osmium related! </summary>
    public static void Start() {
        if (IsRunning) { Debug.LogError("Osmium is already Running!"); return; }
        if (IsStarted) { Debug.LogError("Osmium has already Started!"); return; }
        
        IsStarted = true;
        
        //initialize here to allow any required access before Run()
        Base = new Base();

        ReflectionManager.ResolveAllModules();
        
        Debug.LogAction("Successfully Started Osmium!");
    }
    
    
    
    /// <summary> Starts Osmium up! This method runs until the game is closed. </summary>
    public static void Run() {
        if(!IsStarted) { Debug.LogError("Osmium has not been Started yet!"); return; }
        if(IsRunning) { Debug.LogError("Osmium is already Running!"); return; }
        
        IsRunning = true;
        
        Debug.LogAction("Beginning Update Loop!");
        Base.Run();
    }
    
    
    
    /// <summary> Closes Osmium! </summary>
    public static void Close() => Base.Close();

    
    
    

    /// <summary> Creates a new Scene with the given name </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static Scene AddScene(string __name) {
        if (!Guard.NotNull(__name)) return null;
        if (!Guard.SceneDoesntExist(GetScene(__name))) return null;
        
        Scene newScene = new (__name);
        _scenes.Add(newScene);
        return newScene;
    }
    
    
    
    /// <summary>Adds a new scene that you construct. </summary>
    /// <param name="__scene"></param>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Low), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.O1)]
    public static void AddScene(Scene __scene) {
        if (!Guard.SceneNotNull(__scene)) return;
        if (!Guard.SceneDoesntExist(__scene)) return;
        
        _scenes.Add(__scene);
    }



    /// <summary> Finds a Scene from a given name </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static Scene GetScene(string __name) => !Guard.NotNull(__name) ? null : _scenes.FirstOrDefault(scene => scene.Name == __name, null);



    /// <summary> Returns bool based on whether there a scene with the given name or not. </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static bool ContainsScene(string __name) => Guard.NotNull(__name) && _scenes.Any(scene => scene.Name == __name);



    /// <summary> Closes a Scene </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void CloseScene(Scene __scene) {
        if(!Guard.NotNull(__scene)) return;
        if(!Guard.SceneExists(__scene.Name)) return;
        
        _scenes.Remove(__scene);
    }



    /// <summary> Closes a Scene </summary>
    [MarkerAttributes.Expense(MarkerAttributes.Expense.ExpenseLevel.Medium), MarkerAttributes.Complexity(MarkerAttributes.Complexity.TimeComplexity.ON)]
    public static void CloseScene(string __name) {
        if(!Guard.NotNull(__name)) return;
        if(!Guard.SceneExists(__name)) return;

        _scenes.Remove(GetScene(__name));
    }



}