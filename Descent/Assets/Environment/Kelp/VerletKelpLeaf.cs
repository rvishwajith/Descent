using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletKelpLeaf : MonoBehaviour
{
    private LayerMask collisionLayers;

    // Spawning
    private Vector3 spawnDirection = Vector3.forward + Vector3.right;
    private int numPoints = 5;
    private float segmentLength = 0.4f;

    private float friction = 6f;
    private Vector3 resistanceForce;
    private int integrations = 15;

    private List<VerletLeafCollider> colliders = new();
    private VerletLeafPoint[] points;
    private VerletLeafSegment[] segments;
    private bool initialized = false;

    void Start()
    {
        collisionLayers = LayerMask.GetMask("LargeAnimals", "Player");

        spawnDirection = (transform.right + 0.5f * transform.forward).normalized;
        resistanceForce = transform.right * 20f;

        BuildPoints();
        GetColliders();
        initialized = true;
    }

    private void BuildPoints()
    {
        spawnDirection = spawnDirection.normalized;
        points = new VerletLeafPoint[numPoints];
        segments = new VerletLeafSegment[numPoints - 1];

        for (var i = 0; i < points.Length; i++)
        {
            points[i] = new();
            var position = transform.position + spawnDirection * segmentLength * i;
            points[i].position = position;
            points[i].prevPosition = position;
            points[i].locked = false;
        }
        points[0].locked = true;

        for (var i = 0; i < segments.Length; i++)
        {
            segments[i] = new();
            segments[i].pointA = points[i];
            segments[i].pointB = points[i + 1];
            segments[i].length = segmentLength;
        }
    }

    void GetColliders()
    {
        var allObjects = FindObjectsOfType<GameObject>();
        for (var i = 0; i < allObjects.Length; i++)
        {
            if (collisionLayers == (collisionLayers | (1 << allObjects[i].layer)))
            {
                var collider = new VerletLeafCollider();
                collider.transform = allObjects[i].transform;
                collider.radius = collider.transform.localScale.x / 2;
                colliders.Add(collider);
            }
        }
    }

    void FixedUpdate()
    {
        UpdatePoints(Time.fixedDeltaTime);
        UpdateSegments();
    }

    void UpdatePoints(float deltaT)
    {
        var frictionFactor = 1 - (friction * deltaT);
        var gravityFactor = resistanceForce * deltaT * deltaT;

        for (var i = 0; i < points.Length; i++)
        {
            if (!points[i].locked)
            {
                var initialPos = points[i].position;
                points[i].position += (initialPos - points[i].prevPosition) * frictionFactor;
                points[i].position += gravityFactor;
                points[i].prevPosition = initialPos;
                ApplyPointConstraints(points[i]);
            }
        }
        points[0].position = transform.position;
    }

    void ApplyPointConstraints(VerletLeafPoint point)
    {
        foreach (var collider in colliders)
        {
            var posDiff = collider.position - point.position;
            if (posDiff.magnitude < collider.radius)
                point.position = collider.position - posDiff.normalized * collider.radius;
        }
    }

    void UpdateSegments()
    {
        bool useRandom = true;
        for (var integration = 0; integration < integrations; integration++)
        {
            int offset = 0;
            if (useRandom)
                offset = useRandom ? (int)(UnityEngine.Random.value * (segments.Length - 1)) : 0;
            for (var i = 0; i < segments.Length; i++)
            {
                var index = MathUtil.Wrap(i + offset, segments.Length);
                var center = (segments[index].pointA.position + segments[index].pointB.position) / 2;
                var dir = (segments[index].pointA.position - segments[index].pointB.position).normalized;
                var length = segments[index].length;
                if (!segments[index].pointA.locked)
                    segments[index].pointA.position = center + dir * length / 2;
                if (!segments[index].pointB.locked)
                    segments[index].pointB.position = center - dir * length / 2;
            }
        }
    }

    void OnDrawGizmos()
    {
        spawnDirection = spawnDirection.normalized;
        float pointRadius = 0.05f,
            lockedRadius = 0.1f;
        Color segmentColor = Color.white,
            pointColor = Color.white,
            lockedColor = Color.green;

        if (initialized)
        {
            Gizmos.color = pointColor;
            foreach (var point in points)
            {
                if (point.locked)
                {
                    Gizmos.color = lockedColor;
                    Gizmos.DrawWireSphere(point.position, lockedRadius);
                    Gizmos.color = pointColor;
                }
                else
                    Gizmos.DrawWireSphere(point.position, pointRadius);
            }

            Gizmos.color = segmentColor;
            foreach (var segment in segments)
                Gizmos.DrawLine(segment.pointA.position, segment.pointB.position);
            return;
        }

        var dir = (transform.right + 0f * transform.forward).normalized;
        Gizmos.color = pointColor;
        for (var i = 0; i < numPoints; i++)
        {
            var position = transform.position + dir * segmentLength * i;
            if (i == 0)
            {
                Gizmos.color = lockedColor;
                Gizmos.DrawWireSphere(position, lockedRadius);
                Gizmos.color = pointColor;
            }
            else
                Gizmos.DrawWireSphere(position, pointRadius);
        }
        Gizmos.color = segmentColor;
        Gizmos.DrawRay(transform.position, (numPoints - 1) * dir * segmentLength);
    }

    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[points.Length];
        for (var i = 0; i < points.Length; i++)
        {
            positions[i] = points[i].position;
        }
        return positions;
    }
}

class VerletLeafPoint
{
    public Vector3 position;
    public Vector3 prevPosition;
    public bool locked;
}

class VerletLeafSegment
{
    public VerletLeafPoint pointA;
    public VerletLeafPoint pointB;
    public float length;
}

struct VerletLeafCollider
{
    public Transform transform;
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public float radius;
}