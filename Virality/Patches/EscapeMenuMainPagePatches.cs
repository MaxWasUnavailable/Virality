using HarmonyLib;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(EscapeMenuMainPage))]
[HarmonyPriority(Priority.First)]
internal static class EscapeMenuMainPagePatches
{
    /// <summary>
    ///     Prefix for the Update method. This method currently only checks whether to hide the invite button.
    /// </summary>
    /// <returns> Whether to run the original method. </returns>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(EscapeMenuMainPage.Update))]
    private static bool UpdatePrefix(EscapeMenuMainPage __instance)
    {
        // Whenever we allow late joining, or during vanilla rules, we can show the invite button.
        var showButton = LateJoinHelper.IsLateJoinAllowed || VanillaCheck();
        __instance.inviteButton.gameObject.SetActive(showButton);
        return false;
    }
    
    private static bool VanillaCheck()
    {
        return MainMenuHandler.SteamLobbyHandler != null && SurfaceNetworkHandler.Instance != null &&
               !SurfaceNetworkHandler.HasStarted;
    }
}