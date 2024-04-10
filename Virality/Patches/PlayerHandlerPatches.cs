using System;
using HarmonyLib;
using Virality.Helpers;
using Virality.State;

namespace Virality.Patches;

[HarmonyPatch(typeof(PlayerHandler))]
[HarmonyPriority(Priority.First)]
internal static class PlayerHandlerPatches
{
    /// <summary>
    ///     Postfix patch for the AddPlayer method.
    ///     Opens the door for late joiners.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AddPlayer))]
    private static void AddPlayerPostfixOpenDoor(Player player)
    {
        if (!Virality.AllowLateJoin!.Value)
            return;

        if (!DoorOpenTracker.IsDoorOpen || !PhotonLobbyHelper.IsOnSurface())
            return;

        if (!PhotonLobbyHelper.IsMasterClient())
            return;

        if (player.IsLocal)
            return;

        SendDoorOpenRPC(player);
    }

    /// <summary>
    ///     Postfix patch for the AddPlayer method.
    ///     Syncs the current objective with late joiners.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AddPlayer))]
    private static void AddPlayerPostfixSyncObjective(Player player)
    {
        if (!PhotonLobbyHelper.IsMasterClient())
            return;

        if (!Virality.AllowLateJoin!.Value)
            return;

        SyncObjectiveRPC(player);
    }

    /// <summary>
    ///     Sends an RPC to open the door for the target player.
    /// </summary>
    /// <param name="player"> The player to open the door for. </param>
    private static void SendDoorOpenRPC(Player player)
    {
        SurfaceNetworkHandler.Instance.m_View.RPC("RPCA_OpenDoor", player.refs.view.Controller);
    }

    /// <summary>
    ///     Syncs the current objective with the target player.
    /// </summary>
    /// <param name="player"> The player to sync the objective with. </param>
    private static void SyncObjectiveRPC(Player player)
    {
        var currentObjective = CurrentObjectiveTracker.CurrentObjective;
        if (currentObjective == null)
            return;

        PhotonGameLobbyHandler.Instance.photonView.RPC("RPC_SetCurrentObjective", player.refs.view.Controller,
            ObjectiveDatabase.GetIndex(currentObjective));
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