using System;
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
    private static void AddPlayerPostfix(Player player)
    {
        if (!Virality.AllowLateJoin!.Value)
            return;

        if (!SurfaceNetworkHandler.m_Started || !PhotonLobbyHelper.IsOnSurface())
            return;

        if (player.IsLocal)
            return;

        Virality.Logger?.LogDebug("Running open door RPC.");
        SurfaceNetworkHandler.Instance.m_View.RPC("RPCA_OpenDoor", RpcTarget.OthersBuffered);

        if (PhotonNetwork.IsMasterClient)
            CurrentObjectiveTracker.SendCurrentObjective();
    }

    /// <summary>
    ///     Changes check for players being in bed to require at most 4 players to be in bed.
    /// </summary>
    /// <param name="__instance"> The PlayerHandler instance. </param>
    /// <param name="__result"> The original result. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AllPlayersInBed))]
    private static void AllPlayersInBedPostfix(PlayerHandler __instance, ref bool __result)
    {
        var numberInBed = 0;
        for (var i = 0; i < __instance.playerAlive.Count; i++)
            if (__instance.playerAlive[i].data.bed != null)
                numberInBed++;

        __result = numberInBed >= Math.Min(__instance.playerAlive.Count, 4);
    }

    /// <summary>
    ///     Changes check for players being asleep to require at most 4 players to be asleep.
    /// </summary>
    /// <param name="__instance"> The PlayerHandler instance. </param>
    /// <param name="__result"> The original result. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AllPlayersAsleep))]
    private static void AllPlayersAsleepPostfix(PlayerHandler __instance, ref bool __result)
    {
        var numberAsleep = 0;
        for (var i = 0; i < __instance.playerAlive.Count; i++)
            if (__instance.playerAlive[i].data.sleepAmount >= 0.9f)
                numberAsleep++;

        __result = numberAsleep >= Math.Min(__instance.playerAlive.Count, 4);
    }
}