using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Unity.Mathematics;
using Zorro.Settings;

namespace Virality;

/// <summary>
///     Main plugin class for Virality.
/// </summary>
[ContentWarningPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_VERSION, false)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Virality : BaseUnityPlugin
{
    private bool _isPatched;
    private Harmony? Harmony { get; set; }
    internal new static ManualLogSource? Logger { get; private set; }
    internal static ConfigEntry<int>? MaxPlayers { get; private set; }
    internal static ConfigEntry<bool>? AllowLateJoin { get; private set; }
    internal static ConfigEntry<bool>? EnableVoiceFix { get; private set; }

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

        AllowLateJoin = Config.Bind("General", "AllowLateJoin", true,
            "Whether or not to allow players to join your lobby after the game has started.");

        EnableVoiceFix = Config.Bind("General", "EnableVoiceFix", true,
            "Whether or not to enable the voice fix.");

        // Patch using Harmony
        PatchAll();

        // Report plugin loaded
        if (_isPatched)
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        else
            Logger.LogError($"Plugin {PluginInfo.PLUGIN_GUID} failed to load correctly!");
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

        try
        {
            Harmony.PatchAll();
            _isPatched = true;
            Logger?.LogDebug("Patched!");
        }
        catch (Exception e)
        {
            Logger?.LogError($"Failed to patch: {e}");
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    [ContentWarningSetting]
    public class MaxPlayersSetting : IntSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Mods;
        }

        public string GetDisplayName()
        {
            return "Max Players";
        }

        public override void ApplyValue()
        {
            MaxPlayers!.Value = ValidateValue(Value);
        }

        public override int GetDefaultValue()
        {
            return MaxPlayers?.Value ?? 12;
        }

        private static int ValidateValue(int value)
        {
            return math.clamp(value, 1, 100);
        }

        public string GetDescription()
        {
            return "The maximum number of players allowed in your lobby.";
        }
    }

    [ContentWarningSetting]
    public class LateJoinSetting : BoolSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Mods;
        }

        public string GetDisplayName()
        {
            return "Allow Late Join";
        }

        public override void ApplyValue()
        {
            AllowLateJoin!.Value = Value;
        }

        public override bool GetDefaultValue()
        {
            return AllowLateJoin?.Value ?? true;
        }

        public string GetDescription()
        {
            return "Whether or not to allow players to join your lobby after the game has started.";
        }
    }

    [ContentWarningSetting]
    public class VoiceFixSetting : BoolSetting, IExposedSetting
    {
        public SettingCategory GetSettingCategory()
        {
            return SettingCategory.Mods;
        }

        public string GetDisplayName()
        {
            return "Enable Voice Fix";
        }

        public override void ApplyValue()
        {
            EnableVoiceFix!.Value = Value;
        }

        public override bool GetDefaultValue()
        {
            return EnableVoiceFix?.Value ?? true;
        }

        public string GetDescription()
        {
            return "Whether or not to enable the voice fix.";
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}