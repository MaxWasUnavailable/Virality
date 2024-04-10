namespace Virality.State;

/// <summary>
///     Tracks whether there is a Photon lobby limit.
/// </summary>
public static class PhotonLobbyLimitTracker
{
    /// <summary>
    ///     Limit on the number of players in the lobby, forced by Photon servers.
    /// </summary>
    public static int? PlayerLimit { get; set; } = null;
}