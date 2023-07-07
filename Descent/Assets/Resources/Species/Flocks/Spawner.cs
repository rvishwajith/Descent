using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life
{
    namespace Flocks
    {
        public class Spawner : MonoBehaviour
        {
            public enum GizmoType { Never, SelectedOnly, Always }

            [Header("Spawn Settings")]
            public Boid prefab;
            public int count = 300;
            public int radius = 10;

            [Header("Gizmo Settings")]
            public Color color = Color.black;
            public float opacity = 0.3f;
            public GizmoType settings;

            public Boid[] SpawnBoids()
            {
                var boids = new Boid[count];
                for (int i = 0; i < count; i++)
                {
                    var boid = Instantiate(prefab);
                    Vector3 pos = transform.position + Random.insideUnitSphere * radius;
                    boid.transform.position = pos;
                    boid.transform.forward = Random.onUnitSphere;
                    boids[i] = boid;
                }
                return boids;
            }

            private void OnDrawGizmos()
            {
                if (settings == GizmoType.Always)
                    DrawGizmo();
            }

            private void OnDrawGizmosSelected()
            {
                if (settings == GizmoType.SelectedOnly)
                    DrawGizmo();
            }

            private void DrawGizmo()
            {
                color.a = opacity;
                Gizmos.color = color;
                Gizmos.DrawSphere(transform.position, radius);
            }
        }
    }
}

