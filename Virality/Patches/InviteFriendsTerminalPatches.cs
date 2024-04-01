using DefaultNamespace;
using HarmonyLib;

namespace Virality.Patches;

[HarmonyPatch(typeof(InviteFriendsTerminal))]
[HarmonyPriority(Priority.First)]
[HarmonyWrapSafe]
internal static class InviteFriendsTerminalPatches
{
    /// <summary>
    ///     Prefix patch for the IsGameFull property's getter.
    ///     Overrides the check with one that uses the max players value from the Virality config.
    /// </summary>
    /// <param name="__result"> The original result of the method. </param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(InviteFriendsTerminal.IsGameFull), "get")]
    private static bool IsGameFullPrefix(ref bool __result)
    {
        __result = PlayerHandler.instance.players.Count > Virality.MaxPlayers!.Value;
        return false;
    }
}