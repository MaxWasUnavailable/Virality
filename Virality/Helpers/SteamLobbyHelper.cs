using Steamworks;
using Virality.State;

// ReSharper disable MemberCanBePrivate.Global

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
    ///     Sets the maximum number of players allowed in the lobby.
    /// </summary>
    /// <param name="maxPlayers"> The maximum number of players allowed in the lobby. </param>
    public static void SetLobbyMaxPlayers(int maxPlayers)
    {
        Virality.Logger?.LogInfo($"Setting lobby max players to {maxPlayers}.");
        LobbyHandler!.m_MaxPlayers = maxPlayers;
    }

    /// <summary>
    ///     Sets the maximum number of players allowed in the lobby to the value from the Virality config.
    /// </summary>
    public static void SetLobbyMaxToConfig()
    {
        SetLobbyMaxPlayers(LobbyHelper.GetLobbyMaxConfig());
    }
}