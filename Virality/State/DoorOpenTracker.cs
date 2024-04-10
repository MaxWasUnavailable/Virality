namespace Virality.State;

/// <summary>
///     Helper class for tracking the door open state.
/// </summary>
public static class DoorOpenTracker
{
    /// <summary>
    ///     Keeps track of the door open state.
    /// </summary>
    public static bool IsDoorOpen { get; internal set; }
}