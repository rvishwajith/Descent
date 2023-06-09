# Update Notes
The version history is listed below (latest-first).

## Version 0.5.2 | July 12, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-5-2-virtual-transform.png)
**Virtual transform testing.**
- VirtualTransform (now in Component namespace) is complete and seems to be working properly:
  - Tested on an object using the VirtualMatrixAlias
  - Works properly for rotation, translation, scale.
  - LookAt(point) works properly with/without world up.
  - Note: LookAt(Transform) is not implemented and will only be added if necessary.
  - World Matrix seems to match the Transform.localToWorldMatrix() exactly.
  - If necessary, a new transformation matrix will not be generated on every Get and will instead be updated only if rotation/position/scale values are changed.
- Some testing scenes and custom component scripts were moved or renamed.
- Boids are now working with virtual transforms:
  - Not noticing a signficant performance gain
  - Note: Reducing compute shader thread size to 512 (from 1024) did increase performance.
- Started testing procedural animation for flippers:
  - Seems to work fine using a 4 point verlet rope simulation.
- Worked on shader for small fish animation:
  - Works for giant trevally partically but not completely (rotation on y, z, and dependent z).
  - Does not work with GPU instancing yet!
- Added the format utility class and "Models" resource folder.

## Version 0.5.1 | July 7, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-5-1-single-spline-deform.png)
**Single spline deformation of the mesh with rotation and scaling support.**
- Boids now use GPU Instancing instead of the SRP batcher for rendering (Graphics.DrawMeshInstanced):
  - Reduced number of batches to from ~1400 (2N) to 1.
  - May be changed to use multiple sets batches in the future (for LOD support).
  - No frustum tests/occlusion culling yet.
- Created the VirtualTransform class:
  - Meant to replace a transform without any instantiation.
  - Will eventually take the place of normal transforms whenever manual GPU instancing is used.
- Changes to the player controller:
  - Settings for the controller (and future controllers) now use scriptable object assets (the boids controller already does this) to save settinggs.
  - IK was removed, may be re-added later.
  - Started switching to an animation blend tree for animations.
  - Movement and steering controllers are cleaned up and use data from the scriptable object settings instead of manually checking input types/keybinds.
- Progress on the new mesh deformation system (CPU side):
  - Mesh can now deform successfully on a single spline.
  - Rotation and scaling now work as well.
  - Note: Testing with offsets for transformations on the Transform point array has not been tested and might not work yet (will need to convert mesh points to/from world space).
  - Started working on vertex deformation on a set of splines (Curve class with a smaller spline struct)
- Progress on the new water surface shaders:
  - Objects from above the surface are now distorted when viewing them from below the water by using 2 noise textures and the view direction/distance.
  - Also works for the opposite direction (not implemented yet)
  - Water surface can now render reflections using a reflection probe.
  - Added a package for an example of a good URP water shader (didn't work well, extras will be removed later).
- Lots of project cleanup:
  - Most of the old test scenes/scripts have all been removed, such as the old controllers, boids, deformation, etc.
  - Renamed/reorganized some of the utility/helper classes, such as math and custom gizmos.
  - Doors/door triggers now use DOTween instead of the old animation delegate.
- Added placeholder skyboxes from an asset package.
- Updated README/added thumbnails.

# Milestone Version 0.5 | June 27, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-5-0-boids.png)
**The new flocking system running with collision avoidance in realtime.**
- Created a completely new rig system for the player that works properly:
  - Proper rig layering and corrected scaling/parenting on all of the bones
  - Hands and legs now have IK targets
  - Added a custom Gizmo to draw rig heirarchies with labels
- Created a new player controller
  - Functionality is the same as cleaner version of UniversalPlayerController
  - Player scripts are now in the Player namespace
  - Player movement and steering are renamed and put under the proper namespace now
  - Touch/gyroscope checks are now faster and called every frame (rejected instantly if unnecessary)
- Created a new camera controller:
  - Camera controls are now in the Camera namespace (DynamicCamera)
- Completely overhauled the boids system, which is now mostly functional:
  - Now uses a compute shader to greatly improve calculation times.
  - Now is a set of 2 components (Spawner & Manager) which takes in a settings asset for instead of using preset values.
  - Spawner/Manager components may be combined eventually and boid class may be removed when GPU mesh instancing is used.
  - All rules now work correctly with any weights.
  - Predator avoidance now works, but is not layer/distance dependent unlike before (will be fixed).
  - Collision perception distance, neighbor avoidance distance, etc. are now adjustable.
  - Collision detection now works with any LayerMask object.
  - Staying inside bounds/raycast avoidance now works.
  - Perception now has an angular FOV instead of being purely distance based.
  - Note: May switch from compute shader to jobs system later, or reduce the thread count.
- Added scripts for drawing custom Gizmos (pyramid, boid, bone)
- Major project cleanup/organization:
  - Removed all of the old player, camera, and boids scripts
  - Removed lots of testing scenes
  - Most classes are now under a namespace
- Added third party packages:
  - DOTWeen for easier animation
  - AnimationRigging (Official) for player rig IK
- Added thumbnails to About and UpdateNotes.
- Added some attributions.

## Version 0.4.3 | June 24, 2023
- Added a new hammerhead shark prototyping mesh with fixed normals and origin at the minimum Z bounds.
- Started rebuilding the spline procedural animation for large creatures:
  - Added a test program (DeformOnSpline/MeshDeformer) for deformation of a mesh using a single spline.
  - Vertex deformation works almost perfectly (tested on hammerhead mesh) when mesh origin is at minimum z (mesh pivot point is the head).
  - Vertex deformation distorts when the parent transform is rotated away from the world forward vector (will be fixed using transform's local to world direction later when splines use entity system).
  - Mesh currently scales based on the spline length, may be reversed later.
- Some project cleanup/organization:
  - Removed some of the old procedural animation files and moved others into an archive folder under "Testing".

## Version 0.4.2 | June 24, 2023
- Made some changes to the fog and caustics system:
  - Fixed a glitch where the caustics faloff would not work properly.
  - Some testing of the world position approximation for both the underside and surface of the waves.
  - Added caustics materials using the AddCaustics subgraph to different parts of the terrain.
- Started overhauling the dynamic kelp system:
  - Now uses entities and colliders/spherecasts instead of instantiated transforms for future parallel job system migration.
  - Supports (static) kelp stalks with variable dynamic kelp leaves that point in any direction.
  - Kelp leaf deformation testing using a single spline now works.
  - Leaf entities work properly with sphere collision checks even with low integration count and have adjustable deceleration
- Minor updates to the player controller.
- More progress on fast boids:
- Updated Mac test build to 0.5.
  - Started moving towards a system using entities instead of insantiation for future parallel job system migration
  - Currently still uses instantiation, but will be replaced with DrawMeshInstanced() in the future with a fish deformation material (using a property block for speed/acceleration animation) for much better performance.
- General project cleanup/organization:
  - Removed some empty/old test scenes and scripts, oncluding the entire slow boids implementation.
  - Started using namespaces in scripts.
- Added the main repostitory thumbnail.

## Version 0.4.1 | June 14, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-4-1-godrays.png)
**Faking lightrays using simple particles.**
- Added the "Identity" class:
  - Replacement for the tag system with support for multiple tags.
  - Will be used to map objects to tags.
- Started working on the new (cross-platform) camera controller.
  - Added the CameraTargetTracking class to replace adding a position history to every object that needs to be tracked.
  - Added a "mode" identifier for different types of tracking methods in the future (stationary, cutscenes, etc).
  - Will use the new tagging system instead of using the object's name.
  - Will support gimbal pivoting via both touchscreen and keyboard/mouse.
  - Will support deceleration.
- Started creating the scene delegate:
  - Will be used to cache all objects in the scene at runtime using different kinds of maps.
  - Can get objects based on their identities (from the Identity MonoBehaviour).
- Added some faked lightrays ("godrays") using a particle system that is a child of the camera:
  - Currently emitted only over time
  - Can be culled at a given y position
  - Uses a transparency mask for the distance falloff.
- Started switching boids to an octree system with (normal layered raycasting):
  - No DOTS/jobs use yet, will use parallel jobs or a compute shader in the future.
- Some more project reorganization (removed a few old scenes).
- Updated README/added thumbnails.

# Milestone Version 0.4 | June 13, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-4-0-fog2.png)
**The new fog system.**
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-4-0-verlet-kelp2.png)
**Dynamic procedural kelp using verlet integration.**
- The dynamic fog system is now mostly working:
  - Visibility distance and fog color can now change with height.
  - Visibility falloff can be adjusted by using a power function.
- Major project reorganization:
  - All life is now in subfolders under "Life".
  - Utilities folder added.
  - Camera/player controllers now under "Controller".
- Updated Mac test build to 0.4.
- Dynamic kelp is now implemented:
  - Works with both creatures and the player (layer-based)
  - Note: currently works individually for kelp deformation and leaf deformation, but not both together.
  - Kelp stalks deform based on verlet point positions.
  - Leaves deform based on point positions but mesh normals are flipped.
  - Number of integrations is reduced by setting a high deceleration value.
  - Generated height and number of points are dynamic.
- Manta ray now uses shader graph for animation.
  - Incoming change: the "time" value will be set programatically for movement-based up/down wing acceleration.
- Started working on a new version of the Boids algorithm:
  - Uses octree-based culling and a cell allication map.
  - Glitch where boids are not allocated to the proper cell when rounding their value (possible fix: use truncation instead).
- UI updates:
  - Created a system for auto-resizing a panel based on the size of its text box child using Horizontal/Vertical Layout and Content Size Fitter components.
- New player controller:
  - Player controller now works for cross-platform input.
  - If a gyroscope or touchscreen is available, those are given priority for rotation/movement. If not, the controller falls back to keyboard/mouse support.
  - Speed and rotation now use relative values from 0 - 1 for better sensitivity support.
  - Proper state and state chang detection.
  - Does not account for collision detection currently.
- New player animation system:
  - Based on the player's state, the character will switch between upright and swimming positions.
  - Currently not animated due to a glitch where rotation for euler angles is in the wrong direction (may use Quaternion.AngleAxis() to fix this).
- Switched platforms from macOS to iOS (for gyroscope/touchscreen input testing).
- Updated README/thumbnails.
- Small rocks now have procedural triplanar materials using shader graph:
  - If the normal of a face is pointing sufficiently upwards and the height is high enough, use a different texture to show moss detailing.
- Procedural worldspace caustics can now be added to any shader graph material:
  - Compatible with light direction/color, shadow attenuation, etc.
  - Strength fades between values based on the distance from the surface.
- Player skeleton is now mostly improved:
  - No longer has improper scaling.
  - Pivot points are fixed.
- Started working on a new camera controller system:
  - Can pivot around a target with clamped rotation by using touch controls.
- Started working on behaviour for dolphins/seagulls:
  - Jumping out of the water with increasing velocity + gravity deceleration works.
  - Basic particle system to suggest splashes added.
- Started working on the water underside/overhead shaders:
  - Distortion of objects behind the plane from the player's viewpoint works.
  - Vertex deformation works using noise, but gerstner waves only work using C#.
- Added environment prototyping models for large rocks:
  - Added rough colliders based on the rock bounds (to be removed).
- Added out-of-bounds detection.
  - Currently using the old player controler.
- The test level now has a substantial visual environment and out-of-bounds markers.
- Built a new (working) animation queue system:
  - Works with euler angles, local euler angles, position, and local position currently.
  - Can have a time delay.
  - Can move to a changing target position.

## Version 0.3.1 | June 2, 2023
- Built an animation/tweening system:
  - Has property, delay, duration, interpolation type (incomplete)
  - Can animate position, local position, euler angles, local euler angles.
  - Works on any transform
  - Door script now uses the new animation system.
- Changes to ocean surface shading:
  - Partially working standalone refraction shader using shader graph (works with normal map, but not facing world up).
  - Added wafe surface and underside planes with seperate materials.
  - Added gerstner wave deformation with adjustable properties + noise + wave count (C# script).
- Updated Mac test build to 0.3.
- Added a basic starting menu scene.
- General cleanup/renaming + removed old player controller class (PlayerController2 -> PlayerController).

# Milestone Version 0.3 | June 1, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/spline-animation-demo.png)
**Procedural deformation system using linked catmull-rom splines.**
- Changed Unity version from 2022.2.9 to 2022.3.0 and rebuilt library folder + removed packages.
- Nearly completed spline-based deformation system (in C# form):
  - Works with a target following deformation spine.
  - Works with any mesh length (tested with 2 shark meshes).
  - Independent of mesh/spine positions.
  - Glitch: When orientation of the spline changes the deformation is improper (normals get inverted)
  - Possible change: instead of using the position, use the tangent based on the relative rotation of the given point's forward vector on the spline compared to the pivot point's forward vector to fix the rotation issues.
- Started working on vertex deformation (using both C# and shader graph) for small fish:
  - Components: pivot on y, pivot on z, global side-to-side on x, sinusoid shift on x (based on z).
  - Speed/offset is controllable.
  - Strength can be masked for all components using gradients.
- Added the following creatures for procedural animation testing:
  - Giant trevally (uses instanced animation)
  - Great hammerhead shark (works with spline)
  - Whale shark (works with spline)
  - Atlantic mackerel
- Started converting spline movement system to "snake-like" movement:
  - Updates relative to the pathed movement of the head node.
  - Will be used for the deformation spline of sharks, dolphins, whales, and other large and long meshes.
- Some reorganization/removal of junk files (removed "Rendering" and "SceneFX" folders to merge into PostFX, etc.)
- Updated README/Thumbnails.
- Adjusted particles materials for transparency culling.
- FPS metrics now uses coroutines to update instead of every frame.
- Added post processing FX for underwater environment:
  - Fog color now changes based on the y position of the pixel using a gradient.
  - Visibility now changes based on the distance from the camera (fixed falloff distance)
- Started working on a new version of the boids simulation for small groups of fish.
- Started adding simulation code for jumping/diving animals (dolphins/birds):
  - No flocking behaviour yet.
- Started overhauling the player controller (PlayerController2 class): 
  - Rotation glitches are now fixed.
  - Speed is now based on acceleration with proper state-logging.
  - Boosting now works.
  - Idle state rotation is in-progress.
  - Animation controller works with new player controller.
  - Supports both keyboard and mouse (bo touch yet).
- Started overhauling the camera/UI controls.
  - Now panels will only show up in the proper context (ex. only observe if an observable creature is on the screen and nearby).

## Version 0.2.4 | May 31, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-2-4.png)
**A prototype of the animation system for using a manta ray.**
- Panels and buttons can now be added to the UI manager and can be toggled as well.
- Added rideable creature tag:
  - For creatures within the viewport and sufficiently close to the player, the nearest rideable creature can be interacted with using a button.
- Added snake path follower script (nodes follow the general position history of the initial node).
- Added the ability to switch camera targets to/from the player (distance based currently).
- Added a whale shark mesh for procedural animation testing.
- Began completion of the spline-curve based procedural animation system.
- Removed old manta ray prototyping.
- Added a screenshake function to the camera (called when gates are opening).

## Version 0.2.3 | May 30, 2023
- Made UI manager on Camera globally accesible and initialized at start.
- Added the ability to hide/show labels.
- Added a pause button (doesn't do anything currently).
- Added an interact button to the UI (will be renamed) to toggle when entering/leaving a trigger zone to open the door.
- Some renaming/reorganization + changed README description.

## Version 0.2.2 | May 30, 2023
- Camera controller now has access to UI manager:
  - Set the label on text for species' english and latin names.
- Sound and camera files moved to their own folders.
- Added a thumbnails folder + thumbnail to README.

## Version 0.2.1 | May 30, 2023
- Temporarily removed the deadzones from the player controller input.
- Added a custom path system that uses spline paths:
  - Allows a designated target to move on the path.
  - Target has controllable offset and speed.
- Xcode publishing and macOS compatibility changes for better performance:
  - No anti-aliasing
  - Windowed mode now allowed.
  - IL2CPP used for runtime over Mono.
- Lighting changed to mixed from realtime.
- Changed update notes to be in reverse chronological order.
- Some project reorginzation/renaming.
- Updated manta ray procedural animation:
  - Vertices now pivot around the center point based on a gradient and an angle.

# Milestone Version 0.2 | May 30, 2023
- Added (N^2) boids behaviour:
  - Center of mass
  - Match velocity
  - Avoidance from:
    - Other boids
    - Obstacle colliders
    - Player
    - Predators
- Added path history script.
- Added camera controller:
  - Can follow any target with a path history
- Added smoke stacks and smoke particle VFX
- Created a player prefab
- Added a player controller with:
  - Deadzones for ignoring input at certain positions.
  - Adjustable rotation speed, max speed, boosting, acceleration
  - No touch controls yet only mouse controls
  - Changeable player states (idle, swimming, boost, etc.)
  - Collision detection
- Some renaming/reorganization and a test scene for VFX.
- Added a movement target gizmo with adjustable position/direction.
- Fixed a bug where the animation spline used global position instead of local position.
- Added incomplete procedural animations for the Manta ray (in C#)
- Door trigger now opens door on player collision
- Added ambient sounds and door opening sounds
- Started working on a snake-style target following script for large creatures:
  - Each spline point is a segment.
  - Distance between each segment is fixed.
  - Look at and move towards previous segment (except head).
  - Head copies creature position (with some offset).
- Added y-depth based fog color changing to global fullscreen shader.
- Added ambient particle VFX.

## Version 0.1.6 | May 23, 2023
- Spline interpolation for procedural mesh animation now works properly (using 1 catmull rom spline)
  - Scaling spline down to initial mesh length still incomplete.
- Added universal static Interpolate for all custom lerps (from 0 to 1):
  - Splines:
    - Catmull-rom position, forward, x/y tangents
  - Polynomials:
    - Linear
    - Quadratic
    - Cubic
- Added TextMeshPro package.

## Version 0.1.5 | May 23, 2023
- Added performance metrics calculator (currently only FPS):
  - Displays average FPS on a label (supports min/max samples and rounding).
- Added Attributions.md page.
- Minor reorganization / renaming.
- More work on spline interpolation for procedural mesh animation.

## Version 0.1.4 | May 23, 2023
- Successful (partial) underwater fog render pass:
  - Fog opacity using cubic function now calculated/applied properly.
  - Fog still does not have a seperate value for falloff distance (uses camera zFar).
- Started working on player controller.
- Added AnimationSpline interpolation calculator script.
- (Not working) Added placeholder mesh for procedural mesh animation using a spline.
- Added placeholder UI for interacting with a checkpoint.

## Version 0.1.3 | May 23, 2023
- Started working on fullscreen render pass shader for underwater fog. In-progress:
  - Fog opacity using a custom cubic function with linear (0-1) z-depth.
- Minor renaming/reorganization to files/assets.
- Minor changes to test level.

## Version 0.1.2 | May 22, 2023
- Created a basic “example level” testing scene (replacing sample scene)
- Added a basic environment with materials for floor, walls, etc.
- Added a door which can animate sliding open.
- Added an animated trigger object which can open a target door on interaction
- Added emissive lanterns which can flicker on/off with intervals and intensity.
- Added UI and post-processing VFX placeholders.
- Added checkpoint/interaction placeholders.
- Added player placeholder.
- Updated volume profile.

## Version 0.1.1 | May 19, 2023
- Set up sample scene.
- Added a shader to cover all points below a gradient:
  - Will be used for a future water line at the water surface.

# Version 0.1 (Project Creation) | May 18, 2023
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/thumbnail.png)