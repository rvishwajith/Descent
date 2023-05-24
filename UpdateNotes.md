# Update Notes

## Version 0 | Published May 18, 2023.
Repository, unity project, etc. created.

# Version History

## Version 0.1.1 | Published May 19, 2023
- Set up sample scene.
- Added a shader to cover all points below a gradient:
  - Will be used for a future water line at the water surface.

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

## Version 0.1.3 | Published May 23, 2023
- Started working on fullscreen render pass shader for underwater fog. In-progress:
  - Fog opacity using a custom cubic function with linear (0-1) z-depth.
- Minor renaming/reorganization to files/assets.
- Minor changes to test level.

## Version 0.1.4 | Published May 23, 2023
- Successful (partial) underwater fog render pass:
  - Fog opacity using cubic function now calculated/applied properly.
  - Fog still does not have a seperate value for falloff distance (uses camera zFar).
- Started working on player controller.
- Added AnimationSpline interpolation calculator script.
- (Not working) Added placeholder mesh for procedural mesh animation using a spline.
- Added placeholder UI for interacting with a checkpoint.

## Version 0.1.5 | Published May 23, 2023
- Added performance metrics calculator (currently only FPS):
  - Displays average FPS on a label (supports min/max samples and rounding).
- Added Attributions.md page.
- Minor reorganization / renaming.
- More work on spline interpolation for procedural mesh animation.

## Version 0.1.6 | Published May 23, 2023
- Spline interpolation for procedural mesh animation now works properly (using 1 catmull rom spline)
  - Scaling spline down to initial mesh length still incomplete.
- Added universal static Interpolate for all custom lerps (from 0 to 1):
  - Splines:
    - Catmull-rom position, forward, x/y tangents
  - Polynomials:
    - Quadratic
    - Cubic
  - Other
    - Smoothstep
    - Ease (ease in/out/in-out)

## Version 0.1.7 | In-progress
- Added prefabs for frame rate / debugging.
- Some renaming/reorganization.
- Added TextMeshPro package.
- Added a movement target gizmo with adjustable position/direction.
- Fixed a bug where the animation spline used global position instead of local position.
- Started working on a target following script for large creatures that adds snake-like movement:
  - Each spline point is a segment
  - Distance between each segment is fixed
  - Look at and move towards previous segment (except head)
  - Head copies creature position (with some offset)

## Future Plans
- Testing for procedural animation on actual meshes.
- Underwater fog render pass features:
  - Fog opacity has a seperate adjustable value for falloff distance.
  - Fog color now changes based on the y-depth of the camera using a gradient.
- When a door opens:
  - Play a sound.
  - Add particle system (and a second particle system for when the door finishes opening).
- Add ambient music.
- Add mouse-based controls to player controller.