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

    /// <summary>
    ///     Prefix for the ShowError method in the Modal class.
    ///     Intercepts the body, and in case of a Photon lobby limit error, adds some additional information and sets
    ///     a temporary max player override to the error's max player limit.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Modal.ShowError))]
    private static void ShowErrorPrefix(string title, ref string body)
    {
        if (title.ToLower() != PhotonRoomErrorTitle)
            return;

        if (!body.ToLower().Contains(PhotonLobbyLimitBodyPartial))
            return;

        var lobbyLimit = int.Parse(body.Split(':').Last());

        Virality.Logger?.LogWarning(
            $"Photon lobby limit error detected. Setting temporary max player override to {lobbyLimit}. You may use a mod such as \"Self Sufficient\" to increase this limit.");

        PhotonLobbyLimitTracker.PlayerLimit = lobbyLimit;
        
        body += $"\n\nThe lobby player limit has been temporarily set to {lobbyLimit} for the duration of this session.";
        body += "\nYou may use a mod such as \"Self Sufficient\" to host your own Photon server with a higher player limit.";
    }
}