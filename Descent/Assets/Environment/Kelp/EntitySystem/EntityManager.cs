namespace Kelp
{
    using UnityEngine;
    using System;

    namespace Advanced
    {
        public class EntityManager : MonoBehaviour
        {
            [Serializable]
            public struct SpawnData
            {
                public Vector3 position;
                public float height;

                public void DrawGizmos()
                {
                    if (height == 0) height = 15;

                    var radius = 0.25f;
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(position, radius);

                    Gizmos.color = Color.white;
                    Gizmos.DrawRay(position, Vector3.up * height);
                }
            }

            [Header("Spawning")]
            public SpawnData[] spawners;

            [Header("Collisions")]
            public LayerMask leafCollisionLayers;
            public Collider[] colliders;

            private Entity[] entities;
            private bool initialized = false;

            private void Start()
            {
                CreateEntities();
                initialized = true;
            }

            void CreateEntities()
            {
                entities = new Entity[spawners.Length];
                for (var i = 0; i < spawners.Length; i++)
                {
                    var spawner = spawners[i];
                    var transform = new GameObject("Kelp Entity").transform;
                    transform.position = spawner.position;
                    entities[i] = new Entity(transform, spawner.height);
                }
            }

            private void FixedUpdate()
            {
                foreach (var entity in entities)
                    entity.Update(Time.fixedDeltaTime, colliders);
            }

            private void OnDrawGizmos()
            {
                if (!initialized)
                {
                    foreach (var spawner in spawners)
                        spawner.DrawGizmos();
                }
                else
                {
                    foreach (var entity in entities)
                        entity.DrawGizmos();
                }
            }
        }
    }
}