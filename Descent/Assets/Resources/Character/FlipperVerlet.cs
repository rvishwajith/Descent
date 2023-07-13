using UnityEngine;
using Utilities;

public class FlipperVerlet : MonoBehaviour
{
    public Transform[] flipperTransforms;

    private FlipperPoint[] points;
    private FlipperSegment[] segments;

    void Start()
    {
        CreatePointsAndSegments();
    }

    void CreatePointsAndSegments()
    {
        points = new FlipperPoint[flipperTransforms.Length];
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = new(flipperTransforms[i]);
        }
        points[0].locked = true;

        segments = new FlipperSegment[points.Length - 1];
        for (var i = 0; i < segments.Length; i++)
        {
            segments[i] = new();
            segments[i].a = points[i];
            segments[i].b = points[i + 1];
        }
    }

    void FixedUpdate()
    {
        points[0].transform.LookAt(Vector3.forward * 2);
        var gravityDir = -1 * points[0].transform.forward;

        for (var i = 0; i < points.Length; i++)
        {
            points[i].gravityDir = gravityDir;
            points[i].Update(Time.fixedDeltaTime);
        }

        int numIntegrations = 10;
        for (var integrations = 0; integrations < numIntegrations; integrations++)
        {
            var offset = Random.Range(0, segments.Length - 1);
            for (var i = 0; i < segments.Length; i++)
            {
                // var index = Math.Arrays.Wrap(i + offset, segments.Length);
                segments[i].Update(Time.fixedDeltaTime);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (flipperTransforms == null || flipperTransforms.Length < 2)
            return;
        else if (points == null || segments == null)
            CreatePointsAndSegments();

        Gizmos.color = Color.yellow;
        foreach (var point in points)
            point.DrawGizmo();

        Gizmos.color = Color.yellow;
        foreach (var segment in segments)
            segment.DrawGizmo();
    }
}

public class FlipperPoint
{
    public bool locked = false;
    public Vector3 gravityDir;

    public Vector3 position
    {
        get { return transform.position; }
        set
        {
            prevPosition = transform.position;
            transform.position = value;
        }
    }
    public Vector3 prevPosition;
    public Transform transform;
    private float decelerationFactor = 0.9f;

    public FlipperPoint(Transform t)
    {
        this.transform = t;
        prevPosition = t.position;
    }

    public void DrawGizmo()
    {
        Gizmos.DrawWireSphere(position, 0.05f);
    }

    public void Update(float dT)
    {
        if (locked) return;
        /*
        var deltaPos = prevPosition - position;
        position += deltaPos * decelerationFactor;
        position += gravityDir * dT * dT;
        */
        position += gravityDir * dT;
    }
}

public class FlipperSegment
{
    public FlipperPoint a, b;

    public void DrawGizmo()
    {
        Gizmos.DrawLine(a.position, b.position);
    }

    public void Update(float dT)
    {
        var segmentLength = 0.5f;
        var dir = (a.position - b.position).normalized;
        var center = (a.position + b.position) / 2;

        if (!a.locked)
        {
            a.position = center + dir * segmentLength / 2;
        }
        if (!b.locked)
        {
            b.position = center - dir * segmentLength / 2;
        }
    }
}