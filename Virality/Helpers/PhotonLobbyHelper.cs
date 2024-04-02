namespace Virality.Helpers;

/// <summary>
///     Helper class for Photon lobby functionality.
/// </summary>
public static class PhotonLobbyHelper
{
    /// <summary>
    ///     Checks if the current scene is the surface.
    /// </summary>
    /// <returns> True if the current scene is the surface, false otherwise. </returns>
    public static bool IsOnSurface()
    {
        return PhotonGameLobbyHandler.IsSurface;
    }
}