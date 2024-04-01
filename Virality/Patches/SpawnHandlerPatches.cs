using System;
using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(SpawnHandler))]
[HarmonyPriority(Priority.First)]
internal static class SpawnHandlerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SpawnHandler.FindLocalSpawnIndex))]
    private static void FindLocalSpawnIndexPostfix(ref SpawnHandler __instance)
    {
        var minNumberOfSpawns = Math.Min(
            Math.Min(__instance.m_HospitalSpawns.Length, __instance.m_HouseSpawns.Length), __instance.m_DiveBellSpawns.Length);
        __instance.m_LocalSpawnIndex %= minNumberOfSpawns;
    }
}