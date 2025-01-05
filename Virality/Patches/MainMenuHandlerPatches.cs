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
}