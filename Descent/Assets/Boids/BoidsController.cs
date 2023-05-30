using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Boids : MonoBehaviour
{
    // [Header("Simulation Settings")]
    // private bool use2D = false;

    [Header("Instantiation")]
    public Transform prefab = null;
    public int spawnCount = 300;
    public float spawnRadius = 10;
    public float spawnRadiusNoise = 1.5f;

    [Header("Detection")]
    public float neighborDetection;
    public LayerMask obstacleMask;
    public float obstacleDetection;

    [Header("Movement")]
    public float speed = 7;
    public float acceleration = 5;

    private BoidSimulation simulation = null;

    private void SetData()
    {
        // Movement
        BoidData.Movement.SPEED = speed;
        BoidData.Movement.ACCELERATION = acceleration;

        // Avoidance/Detection
        BoidData.Obstacles.MASK = obstacleMask;
        BoidData.Detection.NEIGHBOR_DIST = neighborDetection;
        BoidData.Detection.OBSTACLE_DIST = obstacleDetection;
    }

    void SpawnBoids()
    {
        var transforms = new Transform[spawnCount];
        for (var i = 0; i < spawnCount; i++)
        {
            var radius = spawnRadius + (Random.value) * spawnRadiusNoise;
            var position = Random.onUnitSphere * radius;
            position += this.transform.position;

            var boidTransform = GameObject.Instantiate(prefab, position, Quaternion.identity).transform;
            boidTransform.LookAt(Vector3.up * boidTransform.position.y);
            boidTransform.forward = boidTransform.right;
            transforms[i] = boidTransform;
        }
        simulation = new(transforms, this.transform);
    }

    void Start()
    {
        SetData();
        SpawnBoids();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        if (simulation != null) simulation.DrawGizmo();
    }

    void FixedUpdate()
    {
        simulation.Simulate(Time.fixedDeltaTime);
    }
}