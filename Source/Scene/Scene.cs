

namespace OsmiumNucleus;

/// <summary>
/// One of the main Osmium entities. Acts like a master folder for components to be stored in.
/// </summary>
/// <author> Avery Norris </author>
public class Scene : ComponentDocker
{
    
    /// <summary> Whether the scene should receive updates or not</summary>
    public bool Enabled = true;



    /// <summary> Unique identifier of the Scene</summary>
    public string Name;
    
    
    
    internal Scene(string __name) { Name = __name; }
}
