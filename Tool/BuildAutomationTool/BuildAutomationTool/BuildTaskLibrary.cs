using System.Collections;
using System.Reflection;
using BuildAutomationTool.Tasks;
using System.Linq;

namespace BuildAutomationTool;

/// <summary>
///     The library that loads and manages task libraries.,
/// </summary>
public sealed class BuildTaskLibrary : IEnumerable<BuildTask>
{
    /// <summary>
    ///     
    /// </summary>
    public Dictionary<string, Type> BuildTaskTypes { get; private set; } = new Dictionary<string, Type>();


    /// <summary>
    ///     
    /// </summary>
    public List<string> BuildTaskLibraries { get; private set; } = new List<string>();

    /// <summary>
    ///     
    /// </summary>
    public BuildTaskLibrary()
    {
    }

    public void GetBuildTaskType(string buildTaskName)
    {
        if (buildTaskName == null) throw new ArgumentNullException(nameof(buildTaskName));
    }

    /// <summary>
    ///     Add a library from the path specified.
    /// </summary>
    /// <param name="libraryPath"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddLibrary(string libraryPath)
    {
        if (libraryPath == null) throw new ArgumentNullException(nameof(libraryPath));
        if (!File.Exists(libraryPath))
        {
            throw new FileNotFoundException("Library file not found.", libraryPath);
        }

        if (!libraryPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The library path must be a .dll file.");
        }

        BuildTaskLibraries.Add(libraryPath);
    }

    /// <summary>
    ///     Load task types the libraries specified. 
    /// </summary>
    public void LoadTaskTypes()
    {
        foreach (string libraryPath in BuildTaskLibraries)
        {
            Assembly assembly = Assembly.LoadFrom(libraryPath);
            var buildTasks = assembly.GetTypes().Where(n => n.IsSubclassOf(typeof(BuildTask)));
            foreach (var buildTaskType in buildTasks)
            {
                BuildTaskTypes.Add(buildTaskType.Name, buildTaskType);
            }
        }
    }


    /// <summary>
    ///     The IEnumerator generated from yielding
    /// </summary>
    /// <returns></returns>
    public IEnumerator<BuildTask> GetEnumerator()
    {
        return null;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}