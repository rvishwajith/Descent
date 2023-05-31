# Update Notes
A complete version history is listed from newest-to-oldest.

## Version 0.2.2 | In-progress
- Camera controller now has access to UI manager:
  - Set the label on text for species' english and latin names.
- Sound and camera files moved to their own folders.
- Added a thumbnails folder + thumbnail to README.

## Version 0.2.1 | Published May 30, 2023
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

## Version 0.2.0 | Published May 30, 2023
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

# Major Update - Version 0.2 Release

## Version 0.1.6 | Published May 23, 2023
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

## Version 0.1.5 | Published May 23, 2023
- Added performance metrics calculator (currently only FPS):
  - Displays average FPS on a label (supports min/max samples and rounding).
- Added Attributions.md page.
- Minor reorganization / renaming.
- More work on spline interpolation for procedural mesh animation.

## Version 0.1.4 | Published May 23, 2023
- Successful (partial) underwater fog render pass:
  - Fog opacity using cubic function now calculated/applied properly.
  - Fog still does not have a seperate value for falloff distance (uses camera zFar).
- Started working on player controller.
- Added AnimationSpline interpolation calculator script.
- (Not working) Added placeholder mesh for procedural mesh animation using a spline.
- Added placeholder UI for interacting with a checkpoint.

## Version 0.1.3 | Published May 23, 2023
- Started working on fullscreen render pass shader for underwater fog. In-progress:
  - Fog opacity using a custom cubic function with linear (0-1) z-depth.
- Minor renaming/reorganization to files/assets.
- Minor changes to test level.

## Version 0.1.2 | Published May 22, 2023
- Created a basic “example level” testing scene (replacing sample scene)
- Added a basic environment with materials for floor, walls, etc.
- Added a door which can animate sliding open.
- Added an animated trigger object which can open a target door on interaction
- Added emissive lanterns which can flicker on/off with intervals and intensity.
- Added UI and post-processing VFX placeholders.
- Added checkpoint/interaction placeholders.
- Added player placeholder.
- Updated volume profile.

## Version 0.1.1 | Published May 19, 2023
- Set up sample scene.
- Added a shader to cover all points below a gradient:
  - Will be used for a future water line at the water surface.

# Project Creation | May 18, 2023.
Repository, Unity/VS projects, etc. created.


