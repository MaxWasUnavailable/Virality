using Photon.Pun;

namespace Virality.Helpers;

/// <summary>
///     Helper class for late join functionality.
/// </summary>
public static class LateJoinHelper
{
    /// <summary>
    ///     Whether late joining is currently allowed.
    /// </summary>
    public static bool IsLateJoinAllowed { get; private set; }
    
    /// <summary>
    ///     Enables late joining.
    /// </summary>
    public static void EnableLateJoin()
    {
        Virality.Logger?.LogDebug("Enabling late join.");

        SteamLobbyHelper.LobbyHandler!.OpenLobby();
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
        IsLateJoinAllowed = true;
    }
    
    /// <summary>
    ///     Disables late joining.
    /// </summary>
    public static void DisableLateJoin()
    {
        Virality.Logger?.LogDebug("Disabling late join.");
        
        SteamLobbyHelper.LobbyHandler!.HideLobby();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        IsLateJoinAllowed = false;
    }
}