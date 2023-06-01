using UnityEngine;

public class CreatureProceduralAnimation : MonoBehaviour
{
    private AnimationSpline spline;
    private Mesh mesh;
    private Vector3[]
        originalVertices,
        vertices;
    private float maxZ, minZ;
    private float length;

    void Start()
    {
        spline = new AnimationSpline(this.transform.Find("Spline"));
        mesh = this.GetComponent<MeshFilter>().mesh;

        maxZ = mesh.bounds.max.z;
        minZ = mesh.bounds.min.z;
        length = Mathf.Abs(maxZ - minZ);
        originalVertices = mesh.vertices;
        vertices = new Vector3[mesh.vertices.Length];

        Debug.Log("Length: " + length);
        Debug.Log("Spline Length: " + spline.ApproximateLength());

        var scaleRatio = length / spline.ApproximateLength();
        spline.scaler.localScale = Vector3.one * scaleRatio;
        Debug.Log("Scaled spline length: " + spline.ApproximateLength());
    }

    void OnDrawGizmos()
    {
        if (spline != null)
        {
            spline.DrawGizmo();
        }
    }

    void FixedUpdate()
    {
        // 1. Scale the spline down so that the curve length matches the length of the mesh.
        // var scaleRatio = length / spline.ApproximateLength();
        // spline.scaler.localScale = Vector3.one * scaleRatio;

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
        mesh.RecalculateNormals();
    }
}

public class AnimationSpline
{
    public Transform scaler;
    private Transform pointA, pointB, pointC, pointD;

    public AnimationSpline(Transform spline)
    {
        scaler = spline;
        pointA = spline.Find("Start");
        pointB = spline.Find("1");
        pointC = spline.Find("2");
        pointD = spline.Find("End");
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
            p0 = pointA.position - scaler.position,
            p1 = pointB.position - scaler.position,
            p2 = pointC.position - scaler.position,
            p3 = pointD.position - scaler.position,
            a = 2 * p1,
            b = (-1 * p0 + p2) * t,
            c = (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t,
            d = (-1 * p0 + 3 * p1 - 3 * p2 + p3) * t * t * t,
            point = 0.5f * (a + b + c + d);
        return point;
    }

    public void DrawGizmo()
    {
        float t = 0f, tInterval = 0.04f;
        while (t < 1)
        {
            Vector3 start = Position(t) + scaler.position, end = Position(t + tInterval) + scaler.position;

            Gizmos.color = Color.Lerp(Color.yellow, Color.blue, t);
            Gizmos.DrawLine(start, end);

            Vector3 forward = start - end;
            Gizmos.DrawLine(start, TangentX(forward) + start);
            Gizmos.DrawLine(start, TangentY(forward) + start);

            t += tInterval;
        }
    }

    public float ApproximateLength()
    {
        float t = 0f, tInterval = 0.02f;
        float length = 0;
        while (t < 1)
        {
            Vector3 a = Position(t);
            Vector3 b = Position(t + tInterval);
            float segmentLength = (a - b).magnitude;
            length += segmentLength;
            t += tInterval;
        }
        return length;
    }

    public static Vector3 TangentX(Vector3 forward)
    {
        return Vector3.Cross(forward, Vector3.up).normalized;
    }

    public static Vector3 TangentY(Vector3 forward)
    {
        return Vector3.Cross(Vector3.right, forward).normalized;
    }
}