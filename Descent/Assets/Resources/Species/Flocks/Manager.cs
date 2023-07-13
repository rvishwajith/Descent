using UnityEngine;

namespace Species
{
    namespace Flocks
    {
        public class Manager : MonoBehaviour
        {
            [HideInInspector]
            public enum InstanceMode
            {
                TransformOnly,
                MatrixOnly,
                Hybrid,
            }

            [Header("Instancing")]
            public InstanceMode instanceMode = InstanceMode.Hybrid;
            public Mesh mesh;
            public Material meshMaterial;

            [Header("Settings")]
            public Transform target = null;
            public FlockingSettings settingsAsset;

            [Header("Compute Shader")]
            public ComputeShader computeShader;
            public int computeThreadGroupSize = 1024; // Default: 1024

            private Boid[] boids;
            private Matrix4x4[] matrices;

            private void Start()
            {
                Spawner spawner;
                if (TryGetComponent<Spawner>(out spawner))
                    boids = spawner.SpawnBoids();
                else
                    boids = FindObjectsOfType<Boid>();

                foreach (Boid b in boids)
                    b.Initialize(settingsAsset, target);
                matrices = new Matrix4x4[boids.Length];
            }

            private void Update()
            {
                var flockData = new BoidData[boids.Length];
                for (int i = 0; i < boids.Length; i++)
                {
                    flockData[i].position = boids[i].position;
                    flockData[i].direction = boids[i].forward;
                }

                ComputeBuffer boidBuffer = new(
                    count: boids.Length,
                    stride: BoidData.Size);
                boidBuffer.SetData(flockData);

                computeShader.SetBuffer(0, "boids", boidBuffer);
                computeShader.SetInt("numBoids", boids.Length);
                computeShader.SetFloat("viewRadius", settingsAsset.perceptionRadius);
                computeShader.SetFloat("avoidRadius", settingsAsset.avoidanceRadius);

                int threadGroups = Mathf.CeilToInt(boids.Length / (float)computeThreadGroupSize);
                computeShader.Dispatch(0, threadGroups, 1, 1);
                boidBuffer.GetData(flockData);

                for (int i = 0; i < boids.Length; i++)
                {
                    boids[i].avgFlockHeading = flockData[i].flockHeading;
                    boids[i].centreOfFlockmates = flockData[i].flockCentre;
                    boids[i].avgAvoidanceHeading = flockData[i].avoidanceHeading;
                    boids[i].numPerceivedFlockmates = flockData[i].numFlockmates;
                    boids[i].UpdateBoid();
                }
                boidBuffer.Release();
                Render();
            }

            private void Render()
            {
                if (mesh == null || meshMaterial == null)
                {
                    Debug.Log("Manager.Render(): ERROR - Mesh or material is null.");
                    return;
                }
                for (int i = 0; i < boids.Length; i++)
                {
                    matrices[i] = boids[i].transform.localToWorldMatrix;
                }
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

            private void OnDrawGizmosSelected()
            {
                var targetColor = Color.cyan;
                targetColor.a = 0.3f;

                Gizmos.color = targetColor;
                Gizmos.DrawSphere(target.position, 0.5f);
            }
        }
    }
}

namespace Species
{
    namespace Flocks
    {
        public struct BoidData
        {
            public Vector3 position;
            public Vector3 direction;

            public Vector3 flockHeading;
            public Vector3 flockCentre;
            public Vector3 avoidanceHeading;
            public int numFlockmates;

            public static int Size
            {
                get { return sizeof(float) * 3 * 5 + sizeof(int); }
            }
        }
    }
}