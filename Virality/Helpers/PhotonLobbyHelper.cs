using Photon.Pun;
using Virality.State;

namespace Virality.Helpers;

/// <summary>
///     Helper class for Photon lobby functionality.
/// </summary>
public static class PhotonLobbyHelper
{
    /// <summary>
    ///     Checks if the current scene is the surface.
    /// </summary>
    /// <returns> True if the current scene is the surface, false otherwise. </returns>
    public static bool IsOnSurface()
    {
        return PhotonGameLobbyHandler.IsSurface;
    }

    /// <summary>
    ///     Sets the maximum number of players allowed in the lobby.
    /// </summary>
    /// <param name="maxPlayers"> The maximum number of players allowed in the lobby. </param>
    public static void SetLobbyMaxPlayers(int maxPlayers)
    {
        PhotonNetwork.CurrentRoom.MaxPlayers = maxPlayers;
    }

    /// <summary>
    ///     Sets the maximum number of players allowed in the lobby to the value in the config.
    /// </summary>
    public static void SetLobbyMaxToConfig()
    {
        SetLobbyMaxPlayers(LobbyHelper.GetLobbyMaxConfig());
    }

    /// <summary>
    ///     Checks if the local player is the master client.
    /// </summary>
    /// <returns></returns>
    public static bool IsMasterClient()
    {
        return PhotonNetwork.IsMasterClient;
    }
}