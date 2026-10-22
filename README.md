# BlueLevelStudios

An Arkanoid-style brick-breaker built with **Unity and C#**, exploring game optimization and reusable gameplay systems.

Move the paddle, keep the ball in play, and clear the bricks before time runs out. The project includes level progression, multiball power-ups, and a complete set of menu, victory, and game-over scenes.

## Gameplay

- Ten level prefabs with progression between levels.
- Multiball power-ups that launch two additional balls.
- A life system and a countdown timer.
- Sound effects, pause controls, and menu screens.

| Action | Control |
| --- | --- |
| Move the paddle | A / D or Left / Right arrows |
| Launch the ball | Space |

## Technical focus

- **Custom collision handling:** ball movement and collisions with walls, the paddle, and bricks are handled in C#.
- **Object pooling:** bricks are reused when generating subsequent levels.
- **ScriptableObjects:** gameplay configuration is separated from runtime logic.
- **Addressables:** assets and level prefabs are loaded through a shared asset manager.
- **Central update loop:** gameplay controllers implement `ICustomUpdate` and are updated by the game manager.
- **Rendering and UI:** Universal Render Pipeline, texture atlases, and TextMesh Pro.

## Open the project

1. Clone or download this repository.
2. In Unity Hub, add the **`BlueLevelStudioss`** folder as the project root.
3. Open it with **Unity 2023.2.20f1**, the version recorded in the project settings.
4. Allow Unity to import the assets and restore the packages.
5. Open `Assets/Scenes/SplashScreen.unity`, the first scene in the build settings.

Addressables settings are stored in `Assets/AddressableAssetsData`. Review the active profile and asset load paths for your environment before creating a build.

## Explore the source

| Area | Location |
| --- | --- |
| Game flow and level progression | [GameMnager.cs](BlueLevelStudioss/Assets/Scripts/GameMnager.cs) |
| Ball movement and collisions | [BallController.cs](BlueLevelStudioss/Assets/Scripts/Ball/BallController.cs) |
| Brick pooling | [BrickPool.cs](BlueLevelStudioss/Assets/Scripts/BrickPool.cs) |
| Multiball power-up | [PowerUpMultiball.cs](BlueLevelStudioss/Assets/Scripts/PowerUpMultiball.cs) |
| Asset and level loading | [AddressableScripts](BlueLevelStudioss/Assets/Scripts/AddressableScripts) |
| Gameplay configuration | [ScriptableObjects](BlueLevelStudioss/Assets/Scripts/ScriptableObjects) |
| Scenes | [Assets/Scenes](BlueLevelStudioss/Assets/Scenes) |
