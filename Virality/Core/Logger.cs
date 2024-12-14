using UnityEngine;

namespace Virality.Core;

/// <summary>
///     Logger class for Virality to replace the BepInEx logger.
/// </summary>
public class Logger
{
    private string MessageFormat(string message) => $"[{PluginInfo.PluginGuid}:{PluginInfo.PluginVersion}] " + message;
    
    /// <summary>
    ///     Log a debug message.
    /// </summary>
    /// <param name="message"></param>
    public void LogDebug(string message)
    {
        Debug.Log(MessageFormat(message));
    }

    /// <summary>
    ///     Log an info message.
    /// </summary>
    /// <param name="message"></param>
    public void LogInfo(string message)
    {
        Debug.Log(MessageFormat(message));
    }

    /// <summary>
    ///     Log a warning message.
    /// </summary>
    /// <param name="message"></param>
    public void LogWarning(string message)
    {
        Debug.LogWarning(MessageFormat(message));
    }

    /// <summary>
    ///     Log an error message.
    /// </summary>
    /// <param name="message"></param>
    public void LogError(string message)
    {
        Debug.LogError(MessageFormat(message));
    }
}