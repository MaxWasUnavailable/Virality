using System;
using HarmonyLib;
using Photon.Pun;
using Unity.Mathematics;
using Virality.Core;
using Zorro.Settings;

namespace Virality;

/// <summary>
///     Main plugin class for Virality.
/// </summary>
[ContentWarningPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_VERSION, false)]
public class Virality
{
    private static bool _isPatched;

    static Virality()
    {
        // Init logger
        Logger = new Logger();

        // Patch using Harmony
        PatchAll();

        // Override voice server app id
        if (EnableVoiceFix)
            OverrideVoiceServerAppId();

        // Report plugin loaded
        if (_isPatched)
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        else
            Logger.LogError($"Plugin {PluginInfo.PLUGIN_GUID} failed to load correctly!");
    }

    private static Harmony? Harmony { get; set; }
    internal static Logger? Logger { get; }
    internal static int MaxPlayers { get; private set; } = 12;
    internal static bool AllowLateJoin { get; private set; } = true;
    internal static bool EnableVoiceFix { get; private set; } = true;

    private static void PatchAll()
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

    /// <summary>
    ///     Overrides the voice server app id with the realtime server app id, in order to fix voice issues.
    /// </summary>
    private static void OverrideVoiceServerAppId()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice =
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;

        Logger?.LogDebug(
            $"Voice server app id set to realtime server app id ({PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice})");
    }

    [ContentWarningSetting] // TODO: Turn into IntSetting when available
    public class MaxPlayersSetting : FloatSetting, IExposedSetting
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
            MaxPlayers = (int)Value;
        }

        public override float GetDefaultValue()
        {
            return 12;
        }

        public override float2 GetMinMaxValue()
        {
            return new float2(1, 100);
        }

        public string GetDescription()
        {
            return "The maximum number of players allowed in your lobby.";
        }
    }

    [ContentWarningSetting]
    public class LateJoinSetting : FloatSetting, IExposedSetting
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
            AllowLateJoin = Value > 0;
        }

        public override float GetDefaultValue()
        {
            return 1;
        }

        public override float2 GetMinMaxValue()
        {
            return new float2(0, 1);
        }

        public string GetDescription()
        {
            return "Whether or not to allow players to join your lobby after the game has started.";
        }
    }

    [ContentWarningSetting]
    public class VoiceFixSetting : FloatSetting, IExposedSetting
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
            EnableVoiceFix = Value > 0;
        }

        public override float GetDefaultValue()
        {
            return 1;
        }

        public override float2 GetMinMaxValue()
        {
            return new float2(0, 1);
        }

        public string GetDescription()
        {
            return "Whether or not to enable the voice fix.";
        }
    }
}