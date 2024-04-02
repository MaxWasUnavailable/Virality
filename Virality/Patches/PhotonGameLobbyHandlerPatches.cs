using HarmonyLib;
using Photon.Pun;
using Steamworks;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(PhotonGameLobbyHandler))]
[HarmonyPriority(Priority.First)]
internal static class PhotonGameLobbyHandlerPatches
{
    /// <summary>
    ///     Hide the lobby when the PhotonGameLobbyHandler is started and we're not on the surface.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.Start))]
    private static void StartPostfixHide()
    {
        if (!Virality.AllowLateJoin!.Value)
            return;

        if (PhotonLobbyHelper.IsOnSurface())
            return;

        Virality.Logger?.LogDebug("Hiding lobby.");

        SteamLobbyHelper.LobbyHandler?.HideLobby();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        SteamFriends.ClearRichPresence();
    }

    /// <summary>
    ///     Enable late joining when the PhotonGameLobbyHandler is started and we're on the surface.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.Start))]
    private static void StartPostfixOpen()
    {
        if (!Virality.AllowLateJoin!.Value)
            return;

        if (!PhotonLobbyHelper.IsOnSurface())
            return;

        if (!PhotonNetwork.CurrentRoom.IsOpen && !PhotonNetwork.CurrentRoom.IsVisible)
            return;

        Virality.Logger?.LogDebug("Showing lobby.");

        SteamLobbyHelper.LobbyHandler?.OpenLobby();
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
        SteamLobbyHelper.SetRichPresenceJoinable();
    }
}