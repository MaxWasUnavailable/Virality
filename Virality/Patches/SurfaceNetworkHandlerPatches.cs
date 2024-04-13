using HarmonyLib;
using Virality.Helpers;
using Virality.State;

namespace Virality.Patches;

[HarmonyPatch(typeof(SurfaceNetworkHandler))]
[HarmonyPriority(Priority.First)]
internal static class SurfaceNetworkHandlerPatches
{
    /// <summary>
    ///     Postfix patch for the RPCM_StartGame method.
    ///     Allows late joining if enabled.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SurfaceNetworkHandler.RPCM_StartGame))]
    private static void RPCM_StartGamePostfix()
    {
        if (!PhotonLobbyHelper.IsMasterClient())
            return;

        if (!Virality.AllowLateJoin!.Value)
            return;

        if (!PhotonLobbyHelper.IsOnSurface())
            return; // If we're not on the surface, we don't want to allow late joining.

        DoorOpenTracker.IsDoorOpen = true;
        LateJoinHelper.EnableLateJoin();
    }

    /// <summary>
    ///     Postfix patch for the OnSlept method.
    ///     Re-enables late joining if enabled.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SurfaceNetworkHandler.OnSlept))]
    private static void OnSleptPostfix()
    {
        if (!PhotonLobbyHelper.IsMasterClient())
            return;

        if (!Virality.AllowLateJoin!.Value)
            return;

        LateJoinHelper.EnableLateJoin();
    }
}