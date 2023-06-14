using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletKelp : MonoBehaviour
{
    public int pointCount = 20;
    public float pointDistance = 1f; // Why is this double?
    public Vector3 spawnDirection;
    private LayerMask collisionMask;

    private Vector3 resetForce = Vector3.up * 5f;
    private float friction = 5f;
    private int integrations = 10;

    private VerletKelpPoint[] points;
    private VerletKelpSegment[] segments;
    private List<VerletKelpCollider> colliders = new();
    private bool initialized = false;

    void Start()
    {
        BuildPoints();
        GetColliders();
        initialized = true;
    }

    void BuildPoints()
    {
        spawnDirection = spawnDirection.normalized;
        points = new VerletKelpPoint[pointCount];
        segments = new VerletKelpSegment[pointCount - 1];

        for (var i = 0; i < points.Length; i++)
        {
            points[i] = new();
            var position = transform.position + spawnDirection * pointDistance * i;
            points[i].position = position;
            points[i].prevPosition = position;
            points[i].locked = false;
        }
        points[0].locked = true;

        for (var i = 0; i < segments.Length; i++)
        {
            segments[i].pointA = points[i];
            segments[i].pointB = points[i + 1];
            segments[i].length = pointDistance;
        }
    }

    void GetColliders()
    {
        collisionMask = LayerMask.GetMask("Player", "LargeAnimals");

        var allObjects = FindObjectsOfType<GameObject>();
        for (var i = 0; i < allObjects.Length; i++)
        {
            if (collisionMask == (collisionMask | (1 << allObjects[i].layer)))
            {
                var collider = new VerletKelpCollider();
                collider.transform = allObjects[i].transform;

                if (allObjects[i].name == "Player")
                    collider.radius = 2f;
                else if (allObjects[i].name.Contains("MantaRay"))
                    collider.radius = 3f;
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
        var frictionFactor = 1 - (friction * Time.fixedDeltaTime);
        var gravityFactor = resetForce * Time.fixedDeltaTime * Time.fixedDeltaTime;

        for (var i = 0; i < points.Length; i++)
        {
            if (!points[i].locked)
            {
                var initialPos = points[i].position;
                points[i].position += (initialPos - points[i].prevPosition) * frictionFactor;
                points[i].position += gravityFactor;
                points[i].prevPosition = initialPos;
                CheckCollisions(points[i]);
            }
        }
    }

    void CheckCollisions(VerletKelpPoint point)
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

    private void OnDrawGizmos()
    {
        if (!initialized)
        {
            var dir = spawnDirection.normalized * pointDistance;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.3f);

            Gizmos.color = Color.white;
            for (var i = 0; i < pointCount; i++)
            {
                var position = transform.position + dir * pointDistance * i;
                Gizmos.DrawWireSphere(position, 0.05f);
            }
            Gizmos.color = Color.gray;
            for (var i = 1; i < pointCount; i++)
            {
                var initialPosition = transform.position + dir * pointDistance * (i - 1);
                var position = transform.position + dir * pointDistance * i;
                Gizmos.DrawLine(initialPosition, position);
            }
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);

        var radius = 0.05f;
        foreach (var segment in segments)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(segment.pointA.position, radius);
            Gizmos.DrawWireSphere(segment.pointB.position, radius);

            Gizmos.color = Color.gray;
            Gizmos.DrawLine(segment.pointA.position, segment.pointB.position);
        }

        Gizmos.color = Color.cyan;
        foreach (var collider in colliders)
            Gizmos.DrawWireSphere(collider.position, collider.radius);
    }

    public Vector3[] GetPositions()
    {
        var positions = new Vector3[pointCount];
        var offset = transform.position;
        for (var i = 0; i < points.Length; i++)
            positions[i] = points[i].position - offset;
        return positions;
    }
}

class VerletKelpPoint
{
    public Vector3 position;
    public Vector3 prevPosition;
    public bool locked;
}

struct VerletKelpSegment
{
    public VerletKelpPoint pointA;
    public VerletKelpPoint pointB;
    public float length;
}

struct VerletKelpCollider
{
    public Transform transform;
    public Renderer renderer;

    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public float radius;
}