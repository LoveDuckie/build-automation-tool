using System.Reflection;
using BuildAutomationTool.Attributes;
using BuildAutomationTool.Tasks;

namespace BuildAutomationTool;

/// <summary>
///     The Build Task Graph.
/// </summary>
public sealed class BuildTaskGraph
{
    /// <summary>
    ///
    /// </summary>
    private readonly Dictionary<Type, List<Type>> _taskDependencies;

    /// <summary>
    /// The base constructor
    /// </summary>
    public BuildTaskGraph()
    {
        _taskDependencies = new Dictionary<Type, List<Type>>();
    }

    /// <summary>
    ///     Generate the graph.
    /// </summary>
    public void GenerateGraph()
    {
        foreach (var taskType in BuildTask.GetBuildTaskTypes())
        {
            AddTask(taskType.Value);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buildTaskType"></param>
    private void AddTask(Type buildTaskType)
    {
        if (!_taskDependencies.ContainsKey(buildTaskType))
        {
            _taskDependencies[buildTaskType] = [];
        }

        // Look for dependencies in the task's DependsOn attributes
        var buildTaskDependenciesFromAttribute = buildTaskType.GetCustomAttributes<DependsOnAttribute>()
            .Select(attr => attr.DependsOnType)
            .ToList();

        foreach (var currentBuildTaskDependency in buildTaskDependenciesFromAttribute)
        {
            if (!_taskDependencies.TryGetValue(currentBuildTaskDependency, out var currentBuildTaskDependents))
            {
                currentBuildTaskDependents = [];
                _taskDependencies[currentBuildTaskDependency] = currentBuildTaskDependents;
            }

            currentBuildTaskDependents.Add(buildTaskType);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>Returns the task graph.</returns>
    public Dictionary<Type, List<Type>> GetGraph()
    {
        return _taskDependencies;
    }

    /// <summary>
    ///  Find all tasks that depend on the given start task (directly or indirectly)
    /// </summary>
    /// <param name="startBuildTaskType">The task to start from</param>
    /// <returns>Returns a list of tasks that are dependent on the start task</returns>
    private HashSet<Type> GetSubgraphFromTask(Type startBuildTaskType)
    {
        var visited = new HashSet<Type>();
        var stack = new Stack<Type>();
        stack.Push(startBuildTaskType);

        while (stack.Count > 0)
        {
            var currentTask = stack.Pop();
            if (!visited.Add(currentTask)) continue;

            if (!_taskDependencies.TryGetValue(currentTask, out var currentTaskDependencies)) continue;
            foreach (var currentTaskDependency in currentTaskDependencies.Where(dependentTask =>
                         !visited.Contains(dependentTask)))
            {
                stack.Push(currentTaskDependency);
            }
        }

        return visited;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="startTaskType">The start task type.</param>
    /// <returns>Returns the list of levels that can be processed in parallel</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public List<List<Type>> GetSortedSubgraphFromTaskAsLevels(Type startTaskType)
    {
        ArgumentNullException.ThrowIfNull(startTaskType);
        var subGraph = GetSubgraphFromTask(startTaskType) ??
                       throw new ArgumentNullException(nameof(startTaskType));
        var sortedTasks = new List<List<Type>>();
        var taskDependenciesCount = new Dictionary<Type, int>();
        var taskDependents = GetGraph();

        // Initialize indegree of all vertices in the subgraph
        foreach (var task in subGraph)
        {
            taskDependenciesCount[task] = 0;
        }

        // Calculate the indegree of each vertex in the subgraph
        foreach (var currentTaskType in subGraph)
        {
            if (!taskDependents.TryGetValue(currentTaskType, out var currentTaskDependents)) continue;
            foreach (var currentTaskDependent in currentTaskDependents.Where(dependentTask =>
                         subGraph.Contains(dependentTask)))
            {
                taskDependenciesCount[currentTaskDependent]++;
            }
        }

        // Create a queue of vertices with 0 indegree
        var zeroDependentsTasks =
            new Queue<Type>(taskDependenciesCount.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key));

        // Perform the topological sort
        while (zeroDependentsTasks.Count > 0)
        {
            var currentTasks = zeroDependentsTasks.ToList();
            zeroDependentsTasks.Clear();
            sortedTasks.Add(currentTasks);

            foreach (var currentTaskType in currentTasks)
            {
                // Update dependents' indegrees
                if (!taskDependents.TryGetValue(currentTaskType, out var currentTaskDependents)) continue;
                foreach (var currentTaskDependent in currentTaskDependents.Where(taskDependency =>
                             subGraph.Contains(taskDependency)))
                {
                    taskDependenciesCount[currentTaskDependent]--;
                    if (taskDependenciesCount[currentTaskDependent] == 0)
                    {
                        zeroDependentsTasks.Enqueue(currentTaskDependent);
                    }
                }
            }
        }

        // If all tasks are not sorted, there is a cycle
        if (sortedTasks.SelectMany(x => x).Count() != subGraph.Count)
        {
            throw new InvalidOperationException("Graph contains a cycle, topological sorting is not possible.");
        }

        return sortedTasks;
    }

    /// <summary>
    ///     Get a sorted "subgraph" of tasks from the start task
    /// </summary>
    /// <param name="startTaskType">The task to start from</param>
    /// <returns>Returns the list of sorted tasks to invoke.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public List<Type> GetSortedSubgraphFromTask(Type startTaskType)
    {
        ArgumentNullException.ThrowIfNull(startTaskType);
        var subGraph = GetSubgraphFromTask(startTaskType) ?? throw new ArgumentNullException(nameof(startTaskType));
        var sortedTasks = new List<Type>();
        var taskDependenciesCount = new Dictionary<Type, int>();
        var taskDependents = GetGraph();

        // Initialize indegree of all vertices in the subgraph
        foreach (var task in subGraph)
        {
            taskDependenciesCount[task] = 0;
        }

        // Calculate the indegree of each vertex in the subgraph
        foreach (var currentTaskType in subGraph)
        {
            if (!taskDependents.TryGetValue(currentTaskType, out var currentTaskDependencies)) continue;
            foreach (var currentTaskDependency in currentTaskDependencies.Where(dependentTask =>
                         subGraph.Contains(dependentTask)))
            {
                taskDependenciesCount[currentTaskDependency]++;
            }
        }

        // Create a queue of vertices with 0 indegree
        var zeroDependentsTasks =
            new Queue<Type>(taskDependenciesCount.Where(kvp => kvp.Value == 0).Select(kvp => kvp.Key));

        // Perform the topological sort
        while (zeroDependentsTasks.Count > 0)
        {
            var currentTaskType = zeroDependentsTasks.Dequeue();
            sortedTasks.Add(currentTaskType);

            if (!taskDependents.TryGetValue(currentTaskType, out var currentTaskDependents)) continue;
            foreach (var currentTaskDependent in currentTaskDependents.Where(taskDependency =>
                         subGraph.Contains(taskDependency)))
            {
                taskDependenciesCount[currentTaskDependent]--;
                if (taskDependenciesCount[currentTaskDependent] == 0)
                {
                    zeroDependentsTasks.Enqueue(currentTaskDependent);
                }
            }
        }

        // If all tasks are not sorted, there is a cycle
        if (sortedTasks.Count != subGraph.Count)
        {
            throw new InvalidOperationException("Graph contains a cycle, topological sorting is not possible.");
        }

        return sortedTasks;
    }
}