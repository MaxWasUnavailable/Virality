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
        
        LateJoinHelper.DisableLateJoin();

        if (Virality.AllowFriendJoining!.Value)
            SteamFriends.ClearRichPresence();
    }

    /// <summary>
    ///     Postfix patch for the SetCurrentObjective method.
    /// </summary>
    /// <param name="objective"> The objective to set. </param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.SetCurrentObjective))]
    private static bool SetCurrentObjectivePostfix(ref Objective objective)
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