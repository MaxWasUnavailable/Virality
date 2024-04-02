using System;
using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(PlayerHandler))]
[HarmonyPriority(Priority.First)]
internal static class AllPlayersInBed {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AllPlayersInBed))]
    private static void AllPlayersInBedPostfix(PlayerHandler __instance, ref bool __result)
    {
        var numberInBed = 0;
        for (var i = 0; i < __instance.playerAlive.Count; i++)
        {
            if (__instance.playerAlive[i].data.bed != null)
                numberInBed++;
        }

        __result = numberInBed >= Math.Min(__instance.playerAlive.Count, 4);
    }
}