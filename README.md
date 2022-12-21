# cs576-block-master-game

## Work Breakdown
Tatyana:
* Wrote UIManager.cs, ChangeScenes.cs, and WhenTreasureFound.cs
* Created the dungeon including adding textures and props
* Created the the MainMenu, Level1, and WonGameMenu scenes
* Added sound to the treasure chest

Griffin:
* Implemented BlockKeys.cs, TrollAI.cs, TrollMace.cs, keyslot.cs, door.cs (except sound programmjng by Spencer on lines 12,13,18,20,30–33,36), and block_enemies.cs (unused)
* Made geometry for keys, key slots, and doors.  
* Configured NavMesh and NavMeshAgents (setting up physics and AI for enemies and their attacks)

Spencer:
* Worked on the Claire Gameobject, including the cameras and inventory objects, as well as the pause menu
* Wrote Claire.cs, FollowCharacter.cs, inventoryScript.cs, and PauseScript.cs plus integrated them with the objects 
* Added background music and sound effects



## Assets
* The Claire prefab is based on the one used in class for homework assignment 3.

* The BGM "Bog Creatures On the Move" uses a Soundimage International Public License and is available royalty free from: https://soundimage.org/fantasywonder/
* The "Grass 1 22" AudioSource (footsteps sound) is from the Footstep(Snow and Grass) pack by MGWSoundDesign available on the Unity Asset Store.
* The "vibe2" AudioSource was sampled from Yu Yeonjoo's performance of Jan Freicher's "Soaring in the sky" (https://www.youtube.com/watch?v=aVL_pdrqX0A)
* The "success" sound clip 

* Treasure chest prefab: https://assetstore.unity.com/packages/3d/props/interior/treasure-set-free-chest-72345
* Troll/ogre (enemy) prefab: https://assetstore.unity.com/packages/3d/characters/creatures/creature-cave-troll-115707
* Torch prefab: https://assetstore.unity.com/packages/3d/environments/historic/historic-environment-142116
* Ground and roof texture: https://assetstore.unity.com/packages/2d/textures-materials/floors/pbr-ground-materials-1-dirt-grass-85402
* Wall texture: https://assetstore.unity.com/packages/2d/textures-materials/brick/tileable-bricks-wall-24530
* Door texture: https://assetstore.unity.com/packages/2d/textures-materials/free-sample-materials-112184
* Keyhole structure texture: https://assetstore.unity.com/packages/2d/textures-materials/wood/mc-old-wod-free-sample-178802

* The fire particle system for the torches was taken from https://www.mediafire.com/file/5v3mlk7p5pqp1r1/08_Basics_Fire.unitypackage
* The script FlickeringLight.cs which adds flickering lights to the fire was taken from https://assetstore.unity.com/packages/tools/particles-effects/flickering-light-effect-57324#content
