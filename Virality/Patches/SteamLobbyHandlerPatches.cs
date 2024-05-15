using System.Collections.Generic;
using System.Reflection.Emit;
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
    [HarmonyPrefix]
    [HarmonyPatch(nameof(SteamLobbyHandler.HostMatch))]
    private static void HostMatchPrefix()
    {
        SteamLobbyHelper.SetLobbyMaxToConfig();
    }
    
    /// <summary>
    ///     Transpiler patch for the SteamLobbyHandler constructor.
    ///     Replaces the max players value with the one from the Virality config.
    /// </summary>
    /// <param name="instructions"> Original instructions. </param>
    /// <returns> Modified instructions. </returns>
    [HarmonyTranspiler]
    [HarmonyPatch(MethodType.Constructor, [typeof(int), typeof(bool)])]
    private static IEnumerable<CodeInstruction> ConstructorTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .SearchForward(instruction => instruction.opcode == OpCodes.Ldarg_1)
            .ThrowIfInvalid("Could not find max players argument")
            .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(LobbyHelper), nameof(LobbyHelper.GetLobbyMaxConfig))))
            .InstructionEnumeration();
    }
}