using System;
using HarmonyLib;
using Photon.Pun;
using Unity.Mathematics;
using Virality.Core;
using Zorro.Settings;

namespace Virality;

/// <summary>
///    Information about the plugin.
/// </summary>
public static class PluginInfo
{
    /// <summary>
    ///     The GUID of the plugin.
    /// </summary>
    public const string PluginGuid = "MaxWasUnavailable.Virality";
    /// <summary>
    ///    The version of the plugin.
    /// </summary>
    public const string PluginVersion = "1.5.3";
}

/// <summary>
///     Main plugin class for Virality.
/// </summary>
[ContentWarningPlugin(PluginInfo.PluginGuid, PluginInfo.PluginVersion, false)]
public class Virality
{
    private bool _isPatched;

    static Virality()
    {
        // Init logger
        Logger = new Logger();

        // Create new instance
        Instance = new Virality();
    }

    /// <summary>
    ///     Constructor for the Virality plugin.
    /// </summary>
    public Virality()
    {
        Logger?.LogInfo($"Plugin {PluginInfo.PluginGuid} is loaded!");
    }

    private Harmony? Harmony { get; set; }
    internal static Logger? Logger { get; }
    internal static int MaxPlayers { get; private set; } = 12;
    internal static bool AllowLateJoin { get; private set; } = true;
    internal static bool EnableVoiceFix { get; private set; } = true;

    /// <summary>
    ///     Singleton instance of the Virality plugin.
    /// </summary>
    public static Virality Instance { get; }

    private void PatchAll()
    {
        if (_isPatched)
        {
            Logger?.LogWarning("Already patched!");
            return;
        }

        Logger?.LogDebug("Patching...");

        Harmony ??= new Harmony(PluginInfo.PluginGuid);

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

    /// <summary>
    ///     Unpatches all patches applied by the plugin.
    /// </summary>
    public void UnpatchAll()
    {
        if (!_isPatched)
        {
            Logger?.LogWarning("Already unpatched!");
            return;
        }

        Logger?.LogDebug("Unpatching...");

        try
        {
            Harmony?.UnpatchSelf();
            _isPatched = false;
            Logger?.LogDebug("Unpatched!");
        }
        catch (Exception e)
        {
            Logger?.LogError($"Failed to unpatch: {e}");
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
            MaxPlayers = ValidateValue(Value);
        }

        public override int GetDefaultValue()
        {
            return 12;
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
            AllowLateJoin = Value;
        }

        public override bool GetDefaultValue()
        {
            return true;
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
            EnableVoiceFix = Value;
        }

        public override bool GetDefaultValue()
        {
            return true;
        }

        public string GetDescription()
        {
            return "Whether or not to enable the voice fix.";
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}