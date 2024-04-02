using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(BedBoss))]
[HarmonyPriority(Priority.First)]
internal static class BedBossPatches
{
    /// <summary>
    ///     Prefix patch for the OnPlayerJoined method.
    ///     Disables the method to assign beds if there are more than 4 players.
    /// </summary>
    /// <returns> Whether or not to run the original method. </returns>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(BedBoss.OnPlayerJoined))]
    private static bool OnPlayerJoinedPrefix()
    {
        return PlayerHandler.instance.players.Count <= 4;
    }
}