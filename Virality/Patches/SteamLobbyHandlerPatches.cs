using HarmonyLib;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(SteamLobbyHandler))]
[HarmonyPriority(Priority.First)]
internal static class SteamLobbyHandlerPatches
{
    /// <summary>
    ///     Prefix patch for the HostMatch method.
    ///     Overrides the max players value with the one from the Virality config.
    /// </summary>
    /// <param name="__instance"> Instance of the SteamLobbyHandler. </param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SteamLobbyHandler.HostMatch))]
    private static void HostMatchPrefix(ref SteamLobbyHandler __instance)
    {
        SteamLobbyHelper.LobbyHandler = __instance;
        SteamLobbyHelper.SetLobbyMaxToConfig();
    }
}