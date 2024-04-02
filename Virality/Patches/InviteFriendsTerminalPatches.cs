using DefaultNamespace;
using HarmonyLib;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(InviteFriendsTerminal))]
[HarmonyPriority(Priority.First)]
internal static class InviteFriendsTerminalPatches
{
    /// <summary>
    ///     Prefix patch for the IsGameFull property's getter.
    ///     Overrides the check with one that uses the max players value from the Virality config.
    /// </summary>
    /// <param name="__result"> The original result of the method. </param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(InviteFriendsTerminal.IsGameFull), MethodType.Getter)]
    private static bool IsGameFullPrefix(ref bool __result)
    {
        if (SteamLobbyHelper.LobbyHandler == null)
            return true;

        __result = PlayerHandler.instance.players.Count > SteamLobbyHelper.LobbyHandler.m_MaxPlayers;
        return false;
    }

    /// <summary>
    ///     Prefix patch for the IsGameStarted property's getter.
    ///     Overrides the check with one that allows late joining if the config is enabled.
    /// </summary>
    /// <param name="__result"></param>
    /// <returns></returns>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(InviteFriendsTerminal.IsGameStarted), MethodType.Getter)]
    private static bool IsGameStartedPrefix(ref bool __result)
    {
        if (!Virality.AllowLateJoin!.Value)
            return true;

        __result = false;
        return false;
    }
}