using Virality.State;

namespace Virality.Helpers;

/// <summary>
///     Helper class for lobby-related functionality.
/// </summary>
public static class LobbyHelper
{
    /// <summary>
    ///     Gets the maximum number of players allowed in a new lobby from the Virality config or the PlayerLimit override.
    /// </summary>
    /// <returns> The maximum number of players allowed in a new lobby. </returns>
    public static int GetLobbyMaxConfig()
    {
        return PhotonLobbyLimitTracker.PlayerLimit ?? Virality.MaxPlayers;
    }
}