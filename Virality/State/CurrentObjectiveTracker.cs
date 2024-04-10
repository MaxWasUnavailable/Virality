namespace Virality.State;

/// <summary>
///     Helper class for tracking the current objective.
/// </summary>
public static class CurrentObjectiveTracker
{
    /// <summary>
    ///     Keeps track of the current objective.
    /// </summary>
    public static Objective? CurrentObjective { get; internal set; }
}