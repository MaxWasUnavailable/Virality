# Virality

*A better multiplayer mod for Content Warning*

[![Build](https://img.shields.io/github/actions/workflow/status/MaxWasUnavailable/Virality/build.yml?style=for-the-badge&logo=github&branch=master)](https://github.com/MaxWasUnavailable/Virality/actions/workflows/build.yml)
[![Latest Version](https://img.shields.io/thunderstore/v/MaxWasUnavailable/Virality?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/content-warning/p/MaxWasUnavailable/Virality)
[![Thunderstore Downloads](https://img.shields.io/thunderstore/dt/MaxWasUnavailable/Virality?style=for-the-badge&logo=thunderstore&logoColor=white)](https://thunderstore.io/c/content-warning/p/MaxWasUnavailable/Virality)

## Features

- Bigger lobby sizes (configurable limit)
- Late joining (configurable on/off)

## Notes

- Required by all players to work properly!
- Supports all vanilla player features (i.e. showing up in video comments, hospital bills, etc...)
- Only 4 players need to sleep to progress the day

## Photon server limit

At the time of writing, the Photon server limit is 4 players. This was set a couple of days after the game's release due
to heavy Photon server loads - presumably due to bigger lobby mods. There is **no** way to bypass this limit on the base
game's servers.

If you do not use the workaround mentioned below, you will be limited to 4 players, and will have to change your
Virality config to match this limit.

### Workaround

**However**, you can run your own Photon servers by following the instructions on the [Self Sufficient](
https://thunderstore.io/c/content-warning/p/Computery/Self_Sufficient/) mod page, and using said mod together with
Virality. Doing this will bump the maximum player limit to 16.

## Requirements

- BepInEx LTS (5.4.22 or 5.4.21)
- Self Sufficient (optional, but currently necessary for bigger lobbies)

## Credits

- Day -- Bed patches, support, testing
- sovenance -- Mod name idea