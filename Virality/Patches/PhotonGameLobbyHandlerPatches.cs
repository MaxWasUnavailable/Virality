using HarmonyLib;
using Photon.Pun;
using Virality.Helpers;
using Virality.State;

namespace Virality.Patches;

[HarmonyPatch(typeof(PhotonGameLobbyHandler))]
[HarmonyPriority(Priority.First)]
internal static class PhotonGameLobbyHandlerPatches
{
    /// <summary>
    ///     Set
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.Start))]
    private static void OnSteamHostedPrefix()
    {
        // Override voice server app id
        if (Virality.EnableVoiceFix!.Value)
            OverrideVoiceServerAppId();
    }

    /// <summary>
    ///     Overrides the voice server app id with the realtime server app id, in order to fix voice issues.
    /// </summary>
    private static void OverrideVoiceServerAppId()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice =
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

        Virality.Logger?.LogDebug(
            $"Voice server app id set to realtime server app id ({PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice})");
    }

    /// <summary>
    ///     Hide the lobby when the PhotonGameLobbyHandler is started and we're not on the surface.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.Start))]
    private static void StartPostfixHide()
    {
        if (!PhotonLobbyHelper.IsMasterClient())
            return;

        if (!Virality.AllowLateJoin!.Value)
            return;

        if (PhotonLobbyHelper.IsOnSurface())
            return;

        LateJoinHelper.DisableLateJoin();
    }

    /// <summary>
    ///     Postfix patch for the SetCurrentObjective method.
    /// </summary>
    /// <param name="objective"> The objective to set. </param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PhotonGameLobbyHandler.SetCurrentObjective))]
    private static bool SetCurrentObjectivePostfix(ref Objective objective)
    {
        if (!PhotonLobbyHelper.IsMasterClient())
            return true;

        if (objective is LeaveHouseObjective &&
            CurrentObjectiveTracker.CurrentObjective is not (InviteFriendsObjective or LeaveHouseObjective))
            return false; // Redundant RPC calls if we're already past these objectives.

        Virality.Logger?.LogDebug($"Setting current objective to {objective}.");

        CurrentObjectiveTracker.CurrentObjective = objective;

        return true;
    }
}