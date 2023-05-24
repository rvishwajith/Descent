using UnityEngine;

public class Animation : MonoBehaviour
{
    [Header("Spline")]
    public Transform splineObject;
    private AnimationSpline spline = null;

    private Mesh mesh = null;
    private Vector3[] originalVertices = null,
        vertices = null;

    private float zStart, zEnd;

    void Start()
    {
        spline = new(splineObject);
        mesh = this.GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices; // Convert to world position?
        vertices = new Vector3[mesh.vertices.Length];

        zStart = mesh.bounds.max.z;
        zEnd = mesh.bounds.min.z;
        Debug.Log("Start: " + zStart + ", End: " + zEnd);
    }

    private void OnDrawGizmos()
    {
        if (mesh != null)
        {

        }
        if (spline != null)
        {
            spline.DrawGizmo();
        }
    }

    float VertexT(Vector3 vertex)
    {
        return Mathf.InverseLerp(zStart, zEnd, vertex.z);
    }

    void FixedUpdate()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            var t = VertexT(originalVertices[i]);
            var pivot = spline.Position(t);
            var forward = spline.Forward(t, pivot);

            float distX = originalVertices[i].x,
                distY = originalVertices[i].y;
            Vector3 tangentX = AnimationSpline.TangentX(forward) * distX;
            Vector3 tangentY = AnimationSpline.TangentY(forward) * distY;

            // var xyOffset = new Vector3(originalVertices[i].x, originalVertices[i].y, 0);
            var finalPos = pivot + (tangentX + tangentY);
            // Debug.DrawLine(pivot, finalPos, Color.yellow);

            vertices[i] = finalPos;
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
        Vector3 p0 = pointA.position, // Convert to local position?
            p1 = pointB.position,
            p2 = pointC.position,
            p3 = pointD.position;

        var a = 2 * p1;
        var b = (-1 * p0 + p2) * t;
        var c = (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t;
        var d = (-1 * p0 + 3 * p1 - 3 * p2 + p3) * t * t * t;
        var point = 0.5f * (a + b + c + d);
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