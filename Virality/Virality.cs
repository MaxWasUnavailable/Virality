using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace Virality;

/// <summary>
///     Main plugin class for Virality.
/// </summary>
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Virality : BaseUnityPlugin
{
    private bool _isPatched;
    private Harmony? Harmony { get; set; }
    internal new static ManualLogSource? Logger { get; private set; }
    internal static ConfigEntry<int>? MaxPlayers { get; private set; }
    internal static ConfigEntry<bool>? AllowFriendJoining { get; private set; }
    internal static ConfigEntry<bool>? AllowLateJoin { get; private set; }

    /// <summary>
    ///     Singleton instance of the plugin.
    /// </summary>
    public static Virality? Instance { get; private set; }

    private void Awake()
    {
        // Set instance
        Instance = this;

        // Init logger
        Logger = base.Logger;

        // Init config entries
        MaxPlayers = Config.Bind("General", "MaxPlayers", 12,
            "The maximum number of players allowed in your lobby.");

        AllowFriendJoining = Config.Bind("General", "AllowFriendJoining", true,
            "Whether or not to allow friends to join your lobby through the Steam overlay.");

        AllowLateJoin = Config.Bind("General", "AllowLateJoin", true,
            "Whether or not to allow players to join your lobby after the game has started.");

        // Patch using Harmony
        PatchAll();

        // Report plugin loaded
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void PatchAll()
    {
        if (_isPatched)
        {
            Logger?.LogWarning("Already patched!");
            return;
        }

        Logger?.LogDebug("Patching...");

        Harmony ??= new Harmony(PluginInfo.PLUGIN_GUID);

        Harmony.PatchAll();
        _isPatched = true;

        Logger?.LogDebug("Patched!");
    }
}