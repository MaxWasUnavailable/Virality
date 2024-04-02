namespace Virality.Helpers;

/// <summary>
///     Helper class for tracking the current objective.
/// </summary>
public static class CurrentObjectiveTracker
{
    /// <summary>
    ///     Keeps track of the current objective.
    /// </summary>
    public static Objective? CurrentObjective { get; internal set; }

    /// <summary>
    ///     Sends the current objective to all clients.
    /// </summary>
    public static void SendCurrentObjective()
    {
        if (CurrentObjective == null)
            return;

        PhotonGameLobbyHandler.Instance.SetCurrentObjective(CurrentObjective);
    }
}