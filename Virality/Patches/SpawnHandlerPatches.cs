using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(SpawnHandler))]
[HarmonyPriority(Priority.First)]
internal static class SpawnHandlerPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SpawnHandler.GetSpawnPoint))]
    private static void GetSpawnPointPrefix(ref SpawnHandler __instance, Spawns state)
    {
        switch (state)
        {
            case Spawns.House:
                __instance.m_LocalSpawnIndex %= __instance.m_HouseSpawns.Length;
                break;
            case Spawns.Hospital:
                __instance.m_LocalSpawnIndex %= __instance.m_HospitalSpawns.Length;
                break;
            case Spawns.DiveBell:
                __instance.m_LocalSpawnIndex %= __instance.m_DiveBellSpawns.Length;
                break;
            default:
                break;
                
        }
    }
}