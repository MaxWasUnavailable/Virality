using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(SteamLobbyHandler))]
[HarmonyPriority(Priority.First)]
internal static class SteamLobbyHandlerPatches
{
    /// <summary>
    ///     Postfix patch for the Awake method.
    ///     Overrides the max players value with the one from the Virality config.
    /// </summary>
    /// <param name="__instance"> Instance of the SteamLobbyHandler. </param>
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    private static void AwakePostfix(ref SteamLobbyHandler __instance)
    {
        __instance.m_MaxPlayers = Virality.MaxPlayers!.Value;
    }
}