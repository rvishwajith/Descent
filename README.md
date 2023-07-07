![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/thumbnail.png)

# About
Descent is an underwater exploration game that I'm solo developing using Unity.
The goal of this project is to amplify people's interests with one of my personal passions, marine biology through games. Instead of focusing on horror or survival, as underwater games often do, Descent is designed to be serene and beautiful.

# Development
Descent is being developed using Unity 2022.2.9 (URP) on macOS. The most recent major update is **Version 0.5.0** (see UpdateNotes.md).

## Gallery
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-4-0-fog2.png)
Custom fog system with height-based color and an adjustable falloff curve (v0.4).
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-2-4.png)
A prototype of the custom animation system for large creatures using a manta ray (v0.2.4).
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-5-0-boids.png)
Dynamic flocking (400 object instances) running on a compute shader with collision avoidance in realtime (v0.5).
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-4-1-godrays.png)
Faking lightrays using simple particles with a mask and height-based culling (v0.4.1).
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/spline-animation-demo.png)
The custom procedural deformation system using a set of catmull-rom splines (v0.3.0).
![](https://raw.githubusercontent.com/rvishwajith/Descent/main/Thumbnails/v0-4-0-verlet-kelp2.png)
Procedural kelp interacting with large animals using verlet integration (v0.4.0).

The main languages used in development are:
- C# (Both Mono & .NET)
- HLSL (Shading Language).
  - Some HLSL files were also generated using Shader Graph.
- Swift (used in SceneKit testing).
- Metal (used in SceneKit testing).

## Development Updates
All documented updates to the Master branch can be found at the **UpdateNotes.md** file.

## Target Platforms
The target platforms for release are **iOS**, **iPadOS**, & **macOS**. Standard mouse and touch input are used, so Android and Windows are possible release targets as well.

# Attributions
Attributions will be added soon and will be found at **Attributions.md**.