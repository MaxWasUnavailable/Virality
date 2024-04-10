using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Virality.State;

namespace Virality.Patches;

[HarmonyPatch(typeof(MainMenuHandler))]
[HarmonyPriority(Priority.First)]
internal static class MainMenuHandlerPatches
{
    [HarmonyTranspiler]
    [HarmonyPatch(nameof(MainMenuHandler.OnSteamHosted))]
    private static IEnumerable<CodeInstruction> OnSteamHostedTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .SearchForward(instruction => instruction.opcode == OpCodes.Ldc_I4_4)
            .ThrowIfInvalid("Could not find max players constant")
            .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(MainMenuHandlerPatches), nameof(GetMaxPlayers))))
            .InstructionEnumeration();
    }

    public static int GetMaxPlayers()
    {
        return PhotonLobbyLimitTracker.PlayerLimit ?? Virality.MaxPlayers!.Value;
    }
}