using UnityEngine;

namespace Virality.Core;

/// <summary>
///     Logger class for Virality to replace the BepInEx logger.
/// </summary>
public class Logger
{
    public void LogDebug(string message)
    {
        Debug.Log(message);
    }

    public void LogInfo(string message)
    {
        Debug.Log(message);
    }

    public void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    public void LogError(string message)
    {
        Debug.LogError(message);
    }
}