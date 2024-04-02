using HarmonyLib;
using Photon.Pun;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(PhotonGameLobbyHandler))]
[HarmonyPriority(Priority.First)]
internal static class PhotonGameLobbyHandlerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.Start))]
    private static void StartPostfix()
    {
        if (!Virality.AllowLateJoin!.Value)
            return;
        
        if (PhotonLobbyHelper.IsOnSurface())
            return;
        
        Virality.Logger?.LogDebug("Hiding lobby.");
        
        SteamLobbyHelper.LobbyHandler?.HideLobby();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }
}