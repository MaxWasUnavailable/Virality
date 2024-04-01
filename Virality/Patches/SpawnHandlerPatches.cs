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
        __instance.m_LocalSpawnIndex %= 4;
    }
}