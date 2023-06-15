using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoidsECS : MonoBehaviour
{
    public Transform prefab;
    public LayerMask collisionLayers;
    public bool use2D = true;

    // Spawning
    public int spawnCount = 100;
    public float spawnRadius = 5f;
    private bool initialized = false;
    private FastBoid[] boids;

    // Movement and detection.
    public float moveSpeed = 7f;
    private float
        collisionDist = 1.5f,
        neighborDist = 3f,
        predatorDist = 10f,
        centerAttractMinDist = 8f,
        centerAvoidMaxDist = 5f;

    // Grid allocation.
    private Dictionary<Vector3, BoidCell> cellMap = new();
    private float cellSize;

    private void Start()
    {
        cellSize = neighborDist;
        boids = new FastBoid[spawnCount];
        SpawnBoids();
        initialized = true;
    }

    private void SpawnBoids()
    {
        for (var i = 0; i < boids.Length; i++)
        {
            boids[i] = new();
            boids[i].id = i;
            boids[i].transform = Instantiate(prefab);
            boids[i].transform.parent = transform;
            boids[i].transform.name = "Boid " + i;

            var position = transform.position + Random.insideUnitSphere * spawnRadius;
            var forward = Random.onUnitSphere;
            if (use2D)
            {
                position.y = transform.position.y;
                forward.y = 0;
            }
            boids[i].position = position;
            boids[i].forward = forward;
        }
    }

    private void FixedUpdate() { UpdateSimulation(Time.fixedDeltaTime); }

    private void UpdateSimulation(float deltaTime)
    {
        ResetCells();
        PopulateCells();
        UpdateCells(deltaTime);
        MoveBoids(deltaTime);
        WrapBoidPositions();
    }

    private void ResetCells()
    {
        // Clear all of the cell lists.
        foreach (var cell in cellMap.Values)
            cell.Reset();
    }

    private void PopulateCells()
    {
        BoidCell currentCell;
        foreach (var boid in boids)
        {
            var roundedPos = MathUtil.RoundFloor(boid.position, cellSize);

            // Does the cell exist already? If it does not, create and add it.
            if (!cellMap.TryGetValue(roundedPos, out currentCell))
            {
                currentCell = new();
                cellMap.Add(roundedPos, currentCell);
                // Debug.Log("BoidsECS.PopulateCells(): Added key: " + roundedPos + " for position: " + boid.position);
            }
            currentCell.Add(boid);
        }
    }

    private void UpdateCells(float deltaTime)
    {
        foreach (var key in cellMap.Keys)
        {
            var cell = cellMap.GetValueOrDefault(key);
            cell.Update(deltaTime);
        }
    }

    private void MoveBoids(float deltaTime)
    {
        foreach (var boid in boids)
            boid.position += boid.forward * moveSpeed * deltaTime;
    }

    private void WrapBoidPositions()
    {
        foreach (var boid in boids)
        {
            if (boid == null)
                continue;

            if (Mathf.Abs(boid.position.z) >= 14)
                boid.position = Vector3.Scale(boid.position, new(1, 1, -1));
            if (Mathf.Abs(boid.position.x) >= 14)
                boid.position = Vector3.Scale(boid.position, new(-1, 1, 1));
            if (!use2D && Mathf.Abs(boid.position.y) >= 14)
                boid.position = Vector3.Scale(boid.position, new(1, -1, 1));
        }
    }

    private void OnDrawGizmos()
    {
        if (!initialized)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
            return;
        }

        Handles.color = Color.white;
        foreach (var position in cellMap.Keys)
        {
            var center = position;
            if (use2D)
                center += (Vector3.forward + Vector3.right) * cellSize / 2;
            else
                center += Vector3.one * cellSize / 2;

            Gizmos.color = Color.gray;
            var count = cellMap.GetValueOrDefault(position).Count();
            if (count > 0)
            {
                Gizmos.color = Color.white;
                Handles.Label(center, "" + count);
            }
            Gizmos.DrawWireCube(center, Vector3.one * cellSize);
            // Gizmos.DrawRay(position, Vector3.right * cellSize);
            // Gizmos.DrawRay(position, Vector3.forward * cellSize);
            // Gizmos.DrawRay(position + Vector3.forward * cellSize, Vector3.right * cellSize);
            // Gizmos.DrawRay(position + Vector3.right * cellSize, Vector3.forward * cellSize);
        }
    }
}

class FastBoid
{
    public Transform transform;
    public int id;

    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Vector3 forward
    {
        get { return transform.forward; }
        set { transform.forward = value; }
    }
}

class BoidCell
{
    public List<FastBoid> boids = new();
    public LayerMask collisionMask = LayerMask.GetMask("Boids", "Player");

    public void Update(float deltaTime)
    {
        RaycastHit hit;
        if (Count() == 0) return;

        foreach (var boid in boids)
        {
            if (Physics.Raycast(boid.position, boid.forward, out hit, 3f, collisionMask))
            {
                boid.forward = boid.transform.right;
            }
        }
    }

    public int Count()
    {
        return boids.Count;
    }

    public void Add(FastBoid boid) { boids.Add(boid); }

    public void Reset() { boids.Clear(); }
}