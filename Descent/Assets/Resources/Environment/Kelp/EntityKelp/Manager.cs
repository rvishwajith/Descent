namespace Kelp
{
    using UnityEngine;
    using System;

    namespace Advanced
    {
        public class Manager : MonoBehaviour
        {
            private Entity[] entities;
            private bool initialized = false;

            private void Start()
            {
                CreateRandomEntities(transform.position, 8, 30);
                initialized = true;
            }

            private void CreateRandomEntities(Vector3 center, float radius, int count)
            {
                entities = new Entity[count];
                for (var i = 0; i < entities.Length; i++)
                {
                    Vector3 offset = UnityEngine.Random.insideUnitCircle;
                    offset.z = offset.y;
                    offset.y = 0;

                    var transform = new GameObject("Kelp Entity").transform;
                    transform.position = center + offset * radius;
                    entities[i] = new(transform, UnityEngine.Random.Range(25, 40));
                }
            }

            private void FixedUpdate()
            {
                bool useFrustumCulling = true;

                if (useFrustumCulling)
                {
                    int entitiesInFrustum = 0;
                    var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

                    foreach (var entity in entities)
                    {
                        if (entity.InFrustum(planes))
                        {
                            entity.Update(Time.fixedDeltaTime);
                            entitiesInFrustum++;
                        }
                    }
                    // Debug.Log("Entities in frustum: " + entitiesInFrustum);
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        entity.Update(Time.fixedDeltaTime);
                    }
                }
            }

            private void OnDrawGizmos()
            {
                if (initialized)
                {
                    int entitiesInFrustum = 0;
                    var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

                    foreach (var entity in entities)
                    {
                        if (entity.InFrustum(planes))
                        {
                            entity.DrawGizmos();
                            entitiesInFrustum++;
                        }
                    }
                }
            }
        }
    }
}