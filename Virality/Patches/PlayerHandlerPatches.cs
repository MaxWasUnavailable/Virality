using HarmonyLib;
using Photon.Pun;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(PlayerHandler))]
[HarmonyPriority(Priority.First)]
internal static class PlayerHandlerPatches
{
    /// <summary>
    ///     Postfix patch for the AddPlayer method.
    ///     Runs the open door RPC to fix a late join issue.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AddPlayer))]
    private static void AddPlayerPostfix()
    {
        if (SurfaceNetworkHandler.m_Started && PhotonLobbyHelper.IsOnSurface())
            SurfaceNetworkHandler.Instance.m_View.RPC("RPCA_OpenDoor", RpcTarget.All);
    }
}