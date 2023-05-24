using UnityEngine;

public class CreatureProceduralAnimation : MonoBehaviour
{
    [Header("Spline")]
    private AnimationSpline spline;
    private Mesh mesh;
    private Vector3[]
        originalVertices,
        vertices;
    private float maxZ, minZ;

    void Start()
    {
        spline = new AnimationSpline(this.transform.Find("Spline"));
        mesh = this.GetComponent<MeshFilter>().mesh;

        originalVertices = mesh.vertices;
        vertices = new Vector3[mesh.vertices.Length];
        maxZ = mesh.bounds.max.z;
        minZ = mesh.bounds.min.z;
        // Debug.Log("Max (Start): " + maxZ + ", Min (End): " + minZ);
    }

    void OnDrawGizmos()
    {
        if (spline != null) spline.DrawGizmo();
    }

    void FixedUpdate()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            float
                t = Mathf.InverseLerp(maxZ, minZ, originalVertices[i].z),
                offsetDistX = originalVertices[i].x,
                offsetDistY = originalVertices[i].y;
            Vector3
                pivot = spline.Position(t),
                forward = spline.Forward(t, pivot);
            Vector3
                offsetX = AnimationSpline.TangentX(forward) * offsetDistX,
                offsetY = AnimationSpline.TangentY(forward) * offsetDistY,
                pivotOffset = offsetX + offsetY;
            vertices[i] = pivot + pivotOffset;
        }
        mesh.vertices = vertices;
    }
}

public class AnimationSpline
{
    Transform pointA, pointB, pointC, pointD;

    public AnimationSpline(Transform splineObject)
    {
        pointA = splineObject.Find("Start");
        pointB = splineObject.Find("1");
        pointC = splineObject.Find("2");
        pointD = splineObject.Find("End");
    }

    public static Vector3 TangentX(Vector3 forward)
    {
        return Vector3.Cross(Vector3.up, forward).normalized;
    }

    public static Vector3 TangentY(Vector3 forward)
    {
        return Vector3.Cross(forward, Vector3.right).normalized;
    }

    public Vector3 Forward(float t, Vector3 start)
    {
        float precision = 0.01f;
        Vector3 end = Position(t - precision);
        Vector3 forward = start - end;
        return forward;
    }

    public Vector3 Position(float t)
    {
        Vector3
            p0 = pointA.localPosition,
            p1 = pointB.localPosition,
            p2 = pointC.localPosition,
            p3 = pointD.localPosition;
        Vector3
            a = 2 * p1,
            b = (-1 * p0 + p2) * t,
            c = (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t,
            d = (-1 * p0 + 3 * p1 - 3 * p2 + p3) * t * t * t,
            point = 0.5f * (a + b + c + d);
        return point;
    }

    public void DrawGizmo()
    {
        float t = 0f, tInterval = 0.02f;

        while (t < 1 - tInterval)
        {
            float startT = t, endT = startT + tInterval;
            if (endT > 1) Debug.Log("AnimationSpline.DrawGizmos() ISSUE: EdgeEnd > 1 (value: " + endT + ")");
            t += tInterval;

            Vector3 start = Position(startT), end = Position(endT);
            Gizmos.color = Color.Lerp(Color.yellow, Color.cyan, t);
            Gizmos.DrawLine(start, end);
            // Vector3 forward = start - end;
            // Gizmos.DrawLine(start, TangentX(forward) + start);
            // Gizmos.DrawLine(start, TangentY(forward) + start);
        }
    }
}