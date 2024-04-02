using Steamworks;

namespace Virality.Helpers;

/// <summary>
///     Helper class for Steam lobby functionality.
/// </summary>
public static class SteamLobbyHelper
{
    /// <summary>
    ///     The SteamLobbyHandler instance.
    /// </summary>
    public static SteamLobbyHandler? LobbyHandler { get; internal set; }
    
    /// <summary>
    ///     Gets the current app ID.
    /// </summary>
    /// <returns> The current app ID. </returns>
    public static AppId_t GetAppId()
    {
        return SteamUtils.GetAppID();
    }

    /// <summary>
    ///     Gets the Steam ID of the current user.
    /// </summary>
    /// <returns> The Steam ID of the current user. </returns>
    public static CSteamID GetUserId()
    {
        return SteamUser.GetSteamID();
    }
}