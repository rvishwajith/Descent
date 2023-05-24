# Update Notes

Project and repository created on May 18, 2023.

# Version History

## Version 0.1.1 | Published May 19, 2023
- Setup sample scene
- Added a shader to cover all points below a gradient (for a future water line at the water surface)

## Version 0.1.2 | Published May 22, 2023
- Created a basic “example level” testing scene (replacing sample scene)
- Added a basic environment with materials for floor, walls, etc.
- Added a door which can animate sliding open .
- Added an animated trigger object which can open a target door on interaction
- Added emissive lanterns which can flicker on/off with intervals and intensity.
- Added UI and post-processing VFX placeholders.
- Added checkpoint/interaction placeholders.
- Added player placeholder.
- Updated volume profile.

## Version 0.1.3 | Published May 23, 2023
- Started working on fullscreen render pass shader for underwater fog. In-progress:
  - Fog opacity using a custom cubic function with linear (0-1) z-depth
- Minor renaming/reorganization to files/assets
- Minor changes to test level

## Version 0.1.4 | Published May 23, 2023
- Successful (partial) underwater fog render pass:
  - Fog opacity using cubic function now calculated/applied properly.
  - Fog still does not have a seperate value for falloff distance (uses camera zFar).
- Started working on player controller.
- Added AnimationSpline interpolation calculator script.
- (Not working) Added placeholder mesh for procedural mesh animation using a spline.
- Added placeholder UI for interacting with a checkpoint.

## Version 0.1.5 | Published May 23, 2022.
- Added performance metrics calculator (currently only FPS):
  - Displays average FPS on a label (supports min/max samples and rounding).
- Added Attributions.md page.
- Minor reorganization / renaming.
- More work on spline interpolation for procedural mesh animation.

## Version 0.1.6 | In-progress
- Spline interpolation for procedural mesh animation now works properly (using 1 catmull rom spline)
  - Scaling spline down to initial mesh length still incomplete
- Added universal static Interpolate for all custom lerps (from 0 to 1):
  - Splines:
    - Catmull-rom position, forward, x/y tangents
  - Polynomials:
    - Quadratic
    - Cubic
  - Other
    - Smoothstep
    - Ease (ease in/out/in-out)

## Version 0.1.7 | Planned
- New underwater fog render pass features:
  - Fog opacity has a seperate adjustable value for falloff distance.
  - Fog color now changes based on the y-depth of the camera using a gradient.