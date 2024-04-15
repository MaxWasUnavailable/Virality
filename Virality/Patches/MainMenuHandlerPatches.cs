using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Photon.Pun;
using Virality.Helpers;

namespace Virality.Patches;

[HarmonyPatch(typeof(MainMenuHandler))]
[HarmonyPriority(Priority.First)]
internal static class MainMenuHandlerPatches
{
    /// <summary>
    ///     Transpiler patch for the OnSteamHosted method.
    ///     Replaces the max players value with the one from the Virality config.
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(MainMenuHandler.OnSteamHosted))]
    private static IEnumerable<CodeInstruction> OnSteamHostedTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .SearchForward(instruction => instruction.opcode == OpCodes.Ldc_I4_4)
            .ThrowIfInvalid("Could not find max players constant")
            .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(LobbyHelper), nameof(LobbyHelper.GetLobbyMaxConfig))))
            .InstructionEnumeration();
    }

    /// <summary>
    ///     Prefix patch for the ConnectToPhoton method.
    ///     Overrides the voice server app id with the realtime server app id, in order to fix voice issues.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(MainMenuHandler.ConnectToPhoton))]
    private static void ConnectToPhotonPrefix()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice =
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

        Virality.Logger?.LogDebug(
            $"Voice server app id set to realtime server app id ({PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice})");
    }
}