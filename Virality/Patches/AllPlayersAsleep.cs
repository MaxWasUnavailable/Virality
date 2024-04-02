using System;
using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(PlayerHandler))]
[HarmonyPriority(Priority.First)]
internal static class AllPlayersAsleep {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerHandler.AllPlayersAsleep))]
    private static void AllPlayersAsleepPostfix(PlayerHandler __instance, ref bool __result)
    {
        var numberAsleep = 0;
        for (var i = 0; i < __instance.playerAlive.Count; i++)
        {
            if (__instance.playerAlive[i].data.sleepAmount >= 0.9f)
                numberAsleep++;
        }

        __result = numberAsleep >= Math.Min(__instance.playerAlive.Count, 4);
    }
}