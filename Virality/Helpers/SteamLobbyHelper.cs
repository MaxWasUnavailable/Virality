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

    /// <summary>
    ///     Gets the current lobby ID.
    /// </summary>
    /// <returns> The current lobby ID. </returns>
    public static CSteamID GetLobbyId()
    {
        return LobbyHandler!.m_CurrentLobby;
    }

    /// <summary>
    ///     Adds rich presence for Steam to allow right-click join.
    /// </summary>
    public static void SetRichPresenceJoinable()
    {
        SteamFriends.SetRichPresence("connect",
            $"steam://joinlobby/{GetAppId()}/{GetLobbyId()}/{GetUserId()}");
    }
}