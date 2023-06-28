using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum GizmoType { Never, SelectedOnly, Always }
    public GizmoType showSpawnRegion;

    public Boid prefab;
    public float spawnRadius = 10;
    public int spawnCount = 10;
    public Color gizmoColor;

    [HideInInspector] public Boid[] boids;

    void Awake()
    {
        boids = new Boid[spawnCount];

        for (int i = 0; i < spawnCount; i++)
        {
            var boid = Instantiate(prefab);
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            boid.transform.position = pos;
            boid.transform.forward = Random.insideUnitSphere;
            boids[i] = boid;
        }
    }

    private void OnDrawGizmos()
    {
        if (showSpawnRegion == GizmoType.Always)
            DrawGizmos();
    }

    void OnDrawGizmosSelected()
    {
        if (showSpawnRegion == GizmoType.SelectedOnly)
            DrawGizmos();
    }

    void DrawGizmos()
    {
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.3f);
        Gizmos.DrawSphere(transform.position, spawnRadius);
    }

}