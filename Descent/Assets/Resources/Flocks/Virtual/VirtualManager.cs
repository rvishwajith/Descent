using System.Collections.Generic;
using UnityEngine;
using Components;

namespace Species
{
    namespace Flocks
    {
        public class VirtualManager : MonoBehaviour
        {
            private VirtualBoid[] boids;
            private int frameCount = 0;

            [Header("Instancing")]
            public Mesh mesh;
            public Material meshMaterial;

            [Header("Settings")]
            public int spawnCount = 600;
            public float spawnRadius = 5;
            public Transform target = null;
            public FlockingSettings settingsAsset;

            [Header("Compute Shader")]
            public ComputeShader computeShader;
            public int computeThreadGroupSize = 1024;

            public int frameSplitFactor = 1;

            private void Start()
            {
                // Debug.Log("Current refresh rate: " + Screen.currentResolution.refreshRateRatio + "\nRendering mode: " + SystemInfo.renderingThreadingMode);
                // Application.targetFrameRate = 120;
                SpawnBoids();
            }

            private void SpawnBoids()
            {
                boids = new VirtualBoid[spawnCount];
                for (int i = 0; i < boids.Length; i++)
                {
                    VirtualTransform vT = new VirtualTransform();
                    vT.position = transform.position + Random.onUnitSphere * spawnRadius;
                    vT.forward = Random.onUnitSphere;
                    vT.scale = Vector3.one * 0.6f;

                    VirtualBoid boid = new(vT, settingsAsset);
                    boid.target = target;
                    boid.settings = settingsAsset;
                    boids[i] = boid;
                }
            }

            private void Update()
            {
                var bufferData = new VirtualBoidData[boids.Length];
                for (int i = 0; i < boids.Length; i++)
                {
                    bufferData[i].position = boids[i].position;
                    bufferData[i].direction = boids[i].forward;
                }

                ComputeBuffer buffer = new(count: boids.Length, stride: VirtualBoidData.Size);
                buffer.SetData(bufferData);

                computeShader.SetBuffer(0, "boids", buffer);
                computeShader.SetInt("numBoids", boids.Length);
                computeShader.SetBool("evenOdd", frameCount % 2 == 0);
                computeShader.SetFloat("viewRadius", settingsAsset.perceptionRadius);
                computeShader.SetFloat("avoidRadius", settingsAsset.avoidanceRadius);

                int threadGroups = Mathf.CeilToInt(boids.Length / (float)computeThreadGroupSize);
                computeShader.Dispatch(0, threadGroups, 1, 1);
                buffer.GetData(bufferData);

                for (int i = 0; i < boids.Length; i++)
                {
                    boids[i].avgFlockHeading = bufferData[i].flockHeading;
                    boids[i].centreOfFlockmates = bufferData[i].flockCentre;
                    boids[i].avgAvoidanceHeading = bufferData[i].avoidanceHeading;
                    boids[i].numPerceivedFlockmates = bufferData[i].numFlockmates;
                    boids[i].UpdateForces();

                    if (i % frameSplitFactor == frameCount % frameSplitFactor)
                        boids[i].UpdateCollisionAvoidance();
                }
                foreach (var boid in boids)
                {
                    boid.Move(Time.deltaTime);
                }
                buffer.Release();
                Render();
                frameCount++;
            }

            private void Render()
            {
                if (mesh == null || meshMaterial == null)
                {
                    Debug.Log("Manager.Render(): ERROR - Mesh or material is null.");
                    return;
                }

                var matrices = FrustumCulledMatrices();
                Graphics.DrawMeshInstanced(
                    mesh: mesh,
                    submeshIndex: 0,
                    material: meshMaterial,
                    matrices: matrices,
                    count: matrices.Length,
                    properties: null,
                    castShadows: UnityEngine.Rendering.ShadowCastingMode.On,
                    receiveShadows: true
                );
            }

            private Matrix4x4[] FrustumCulledMatrices()
            {
                List<Matrix4x4> matrices = new();
                float boundsRadius = 2f;
                var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

                Bounds bounds = new(Vector3.zero, Vector3.one * boundsRadius);
                int culledMeshes = 0;
                bool useFrustumCulling = false;

                foreach (var boid in boids)
                {
                    bounds.center = boid.position;
                    bool inFrustum = true;
                    if (useFrustumCulling)
                        inFrustum = GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);

                    if (inFrustum)
                        matrices.Add(boid.transform.matrix);
                    else
                        culledMeshes++;
                }
                // Debug.Log("VirtualManager.CulledMatrices(): " + culledMeshes + "meshes culled.");
                return matrices.ToArray();
            }

            private void OnDrawGizmosSelected()
            {
                var targetColor = Color.cyan;
                targetColor.a = 0.3f;

                Gizmos.color = targetColor;
                Gizmos.DrawSphere(target.position, spawnRadius);
            }
        }
    }
}