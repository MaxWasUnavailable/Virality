using System;
using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(PlayerHandler))]
[HarmonyPriority(Priority.First)]
internal static class AllPlayersInBed
{
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
}