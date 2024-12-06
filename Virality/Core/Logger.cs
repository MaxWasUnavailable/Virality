using UnityEngine;

namespace Virality.Core;

/// <summary>
///     Logger class for Virality to replace the BepInEx logger.
/// </summary>
public class Logger
{
    /// <summary>
    ///     Log a debug message.
    /// </summary>
    /// <param name="message"></param>
    public void LogDebug(string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    ///     Log an info message.
    /// </summary>
    /// <param name="message"></param>
    public void LogInfo(string message)
    {
        Debug.Log(message);
    }

    /// <summary>
    ///     Log a warning message.
    /// </summary>
    /// <param name="message"></param>
    public void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    /// <summary>
    ///     Log an error message.
    /// </summary>
    /// <param name="message"></param>
    public void LogError(string message)
    {
        Debug.LogError(message);
    }
}