using HarmonyLib;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(SurfaceNetworkHandler))]
[HarmonyPriority(Priority.First)]
internal static class SurfaceNetworkHandlerPatches
{
    /// <summary>
    ///     Postfix patch for the RPCM_StartGame method.
    ///     Allows late joining if enabled.
    /// </summary>
    /// <param name="__instance"> Instance of the SurfaceNetworkHandler. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SurfaceNetworkHandler.RPCM_StartGame))]
    private static void RPCM_StartGamePostfix(ref SurfaceNetworkHandler __instance)
    {
        if (!Virality.AllowLateJoin!.Value)
            return;

        if (!PhotonLobbyHelper.IsOnSurface())
            return; // If we're not on the surface, we don't want to allow late joining.

        LateJoinHelper.EnableLateJoin();
    }

    /// <summary>
    ///     Postfix patch for the OnSlept method.
    ///     Re-enables late joining if enabled.
    /// </summary>
    /// <param name="__instance"> Instance of the SurfaceNetworkHandler. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SurfaceNetworkHandler.OnSlept))]
    private static void OnSleptPostfix(ref SurfaceNetworkHandler __instance)
    {
        if (!Virality.AllowLateJoin!.Value)
            return;

        LateJoinHelper.EnableLateJoin();
    }
}