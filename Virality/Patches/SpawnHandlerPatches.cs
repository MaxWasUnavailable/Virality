using System;
using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(SpawnHandler))]
[HarmonyPriority(Priority.First)]
internal static class SpawnHandlerPatches
{
    /// <summary>
    ///     Postfix for the FindLocalSpawnIndex method in the SpawnHandler class.
    ///     This method ensures that the local spawn index is always within the bounds of the spawn arrays, and that
    ///     only the host can be index 0.
    /// </summary>
    /// <param name="__instance"> The instance of the SpawnHandler class. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SpawnHandler.FindLocalSpawnIndex))]
    private static void FindLocalSpawnIndexPostfix(ref SpawnHandler __instance)
    {
        if (__instance.m_LocalSpawnIndex == 0) return;

        var minNumberOfSpawns = Math.Min(
            Math.Min(__instance.m_HospitalSpawns.Length, __instance.m_HouseSpawns.Length),
            __instance.m_DiveBellSpawns.Length) - 1;

        __instance.m_LocalSpawnIndex %= minNumberOfSpawns;
        __instance.m_LocalSpawnIndex++;
    }
}