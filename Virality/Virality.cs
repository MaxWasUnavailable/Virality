using BepInEx;
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