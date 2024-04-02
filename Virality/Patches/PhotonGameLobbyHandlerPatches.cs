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

        if (Virality.AllowFriendJoining!.Value)
            SteamFriends.ClearRichPresence();
    }

    /// <summary>
    ///     Enable late joining when the PhotonGameLobbyHandler is started and we're on the surface.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.Start))]
    private static void StartPostfixOpen()
    {
        Virality.Logger?.LogDebug("Checking if we should show the lobby.");

        if (!Virality.AllowLateJoin!.Value)
            return;

        Virality.Logger?.LogDebug("Checking if we're on the surface.");
        if (!PhotonLobbyHelper.IsOnSurface())
            return;

        Virality.Logger?.LogDebug("Showing lobby.");

        SteamLobbyHelper.LobbyHandler?.OpenLobby();
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;

        if (Virality.AllowFriendJoining!.Value)
            SteamLobbyHelper.SetRichPresenceJoinable();
    }

    /// <summary>
    ///     Postfix patch for the SetCurrentObjective method.
    /// </summary>
    /// <param name="objective"> The objective to set. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.SetCurrentObjective))]
    private static bool SetCurrentObjectivePostfix(Objective objective)
    {
        if (!PhotonNetwork.IsMasterClient)
            return true;

        if (objective is LeaveHouseObjective &&
            CurrentObjectiveTracker.CurrentObjective is not (InviteFriendsObjective or LeaveHouseObjective))
            return false;   // Redundant RPC calls if we're already past these objectives.

        Virality.Logger?.LogDebug($"Setting current objective to {objective}.");

        CurrentObjectiveTracker.CurrentObjective = objective;

        return true;
    }
}