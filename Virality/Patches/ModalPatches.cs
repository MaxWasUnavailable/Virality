using System.Linq;
using HarmonyLib;
using Virality.State;

namespace Virality.Patches;

[HarmonyPatch(typeof(Modal))]
[HarmonyPriority(Priority.First)]
internal static class ModalPatches
{
    private const string PhotonRoomErrorTitle = "failed to create photon room";
    private const string PhotonLobbyLimitBodyPartial = "max players peer room value is too big";

    private static bool IsPhotonLobbyLimitError(string title, string body)
    {
        return title.ToLower() == PhotonRoomErrorTitle && body.ToLower().Contains(PhotonLobbyLimitBodyPartial);
    }

    /// <summary>
    ///     Prefix for the ShowError method in the Modal class.
    ///     Intercepts the body, and in case of a Photon lobby limit error, adds some additional information and sets
    ///     a temporary max player override to the error's max player limit.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Modal.ShowError))]
    private static void ShowErrorPrefix(string title, ref string body)
    {
        if (!IsPhotonLobbyLimitError(title, body))
            return;

        var lobbyLimit = int.Parse(body.Split(':').Last());

        Virality.Logger?.LogWarning(
            $"Photon lobby limit error detected. Setting temporary max player override to {lobbyLimit}. You may use a mod such as \"Self Sufficient\" to increase this limit.");

        PhotonLobbyLimitTracker.PlayerLimit = lobbyLimit;

        body += $"\n\nThe lobby player limit will temporarily be set to {lobbyLimit} for the duration of this session.";
        body +=
            "\nYou may use a mod such as \"Self Sufficient\" to host your own Photon server with a higher player limit.";
    }

    /// <summary>
    ///     Prefix for the Show method in the Modal class.
    ///     Adds a "Cancel" option to prevent the player limit override.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Modal.Show))]
    private static void ShowPostfix(string title, ref string body, ref ModalOption[] options)
    {
        if (!IsPhotonLobbyLimitError(title, body))
            return;

        var cancelOption = new ModalOption("Cancel", CancelPhotonLobbyLimitOverride);
        options = options.Append(cancelOption).ToArray();
    }

    /// <summary>
    ///     Callback for the "Cancel" option in the Show method in the Modal class.
    ///     Resets the temporary max player override.
    /// </summary>
    private static void CancelPhotonLobbyLimitOverride()
    {
        PhotonLobbyLimitTracker.PlayerLimit = null;
        Virality.Logger?.LogWarning("Photon lobby limit override cancelled.");
    }
}