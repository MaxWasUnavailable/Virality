using HarmonyLib;
using Steamworks;
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
        __instance.m_MaxPlayers = Virality.MaxPlayers!.Value;
        SteamLobbyHelper.LobbyHandler = __instance;
        Virality.Logger?.LogDebug($"Max players set to {__instance.m_MaxPlayers}.");
    }

    /// <summary>
    ///     Postfix patch for the OnLobbyCreatedCallback method.
    ///     Updates Steam rich presence.
    /// </summary>
    /// <param name="__instance"> Instance of the SteamLobbyHandler. </param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(SteamLobbyHandler.OnLobbyCreatedCallback))]
    private static void OnLobbyCreatedCallbackPostfix(ref SteamLobbyHandler __instance)
    {
        if (!Virality.AllowFriendJoining!.Value)
            return;

        SteamMatchmaking.SetLobbyType(SteamLobbyHelper.GetLobbyId(), ELobbyType.k_ELobbyTypeFriendsOnly);
        SteamLobbyHelper.SetRichPresenceJoinable();
    }
}