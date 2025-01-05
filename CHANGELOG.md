# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.5.2] - 14/12/2024 - (1.19.d)

### Removed

- Voice fix, since it was causing an infinite hosting loop.

## [1.4.0] - 15/05/2024 - (1.15.a)

### Fixed

- Updated for game version 1.11.a
- Fixed SteamLobbyHandler constructor patch since signature changed (Not backwards compatible due to this)

## [1.3.0] - 02/05/2024 - (1.11.a)

### Fixed

- Updated for game version 1.11.a
- Fixed sleeping being broken due to game update

## [1.2.0] - 15/04/2024

### Added

- Patch max player count on SteamLobbyHandler constructor
- Config toggle for the voice app id override

### Fixed

- Set Photon voice app id to the realtime app id (attempt to fix voice issues)

### Changed

- Use base game static MainMenuHandler.SteamLobbyHandler instead of saving it on lobby creation

### Removed

## [1.1.0] - 10/04/2024

### Added

- Added IsLateJoinAllowed property that keeps track of the current state of late joining
- Enabled invite button when late joining is allowed or when vanilla allows it
- ContentWarningPlugin attribute
- Modal popup in case of Photon Lobby Limit error
- Override for lobby size limit in case of Photon Lobby Limit error

### Fixed

- Some more MasterClient checks to prevent potential issues
- Target individual player instead of all players when performing late game join RPCs

### Changed

- Common lobby functionality moved to helpers
- Use lobby size limit override if set instead of Virality config limit

### Removed

- Steam Rich Presence, since base game now handles this

## [1.0.2] - 02/04/2024

### Fixed

- Added transpiler patch to fix new MainMenuHandler 4 player limit

## [1.0.0-pre.2] - 02/04/2024

### Changed

- Changed default max lobby size from 10 to 12

## [1.0.0-pre.1] - 02/04/2024

### Added

- Configurable max player limit
- Late joining
- Right click Steam join