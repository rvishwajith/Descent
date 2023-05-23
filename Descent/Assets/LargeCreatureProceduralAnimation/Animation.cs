using UnityEngine;

public class Animation : MonoBehaviour
{
    [Header("Spline")]
    public Transform splineObject;
    private AnimationSpline spline = null;

    private Mesh mesh = null;
    private Vector3[] originalVertices = null;

    void Start()
    {
        spline = new(splineObject);
        mesh = this.GetComponent<MeshFilter>().mesh;
        originalVertices = this.mesh.vertices;
        // Convert to world position?
    }

    private void OnDrawGizmos()
    {
        if (mesh != null)
        {

        }
        if (spline != null)
        {
            // spline.DrawGizmo();
        }
    }

    void VertexT(Vector3 vertex)
    {

    }

    void FixedUpdate()
    {

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

    public Vector3 Interpolate(float t)
    {
        Vector3 p0 = pointA.position,
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

        while (t < 1)
        {
            float edgeStart = t, edgeEnd = edgeStart + tInterval;
            if (edgeEnd > 1) Debug.Log("AnimationSpline.DrawGizmos() ISSUE: Edge > 1");

            Vector3 edgeA = Interpolate(edgeStart), edgeB = Interpolate(edgeEnd);
            Gizmos.color = Color.Lerp(Color.yellow, Color.cyan, t);
            Gizmos.DrawLine(edgeA, edgeB);

            t += tInterval;
        }
    }
}