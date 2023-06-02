using UnityEngine;
using System.Collections;

public class WhaleSharkProceduralAnimation : MonoBehaviour
{
    public SnakeAnimationCurve curve;

    private Mesh mesh;
    private Vector3[] originalVertices, vertices;
    private float meshLength, maxZ, minZ;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        vertices = new Vector3[mesh.vertexCount];

        meshLength = mesh.bounds.size.z;
        maxZ = mesh.bounds.max.z;
        minZ = mesh.bounds.min.z;

        StartCoroutine(Logging());
    }

    private void FixedUpdate()
    {
        curve.UpdateCaches();

        for (var i = 0; i < originalVertices.Length; i++)
        {
            vertices[i] = Deform(originalVertices[i]);
        }
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
    }

    private Vector3 Deform(Vector3 pos)
    {
        var curveT = Mathf.InverseLerp(maxZ, minZ, pos.z);
        var forwardT = curveT + 0.01f;

        var pivot = curve.GetScaledPoint(curveT, meshLength); // The pivot point.
        var forward = curve.GetScaledPoint(forwardT, meshLength) - pivot; // Forward -> Forward(A) = B - A
        // var offset = new Vector3(pos.x, pos.y, 0);

        var tangentX = Vector3.Cross(forward, Vector3.up).normalized * pos.x;
        var tangentY = Vector3.Cross(Vector3.right, forward).normalized * pos.y;
        var tangent = tangentX + tangentY;
        pos = pivot + tangent;
        return pos;
    }

    private IEnumerator Logging()
    {
        var i = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            // Debug.Log("Cached Length: " + curve.CachedLength() + ", Mesh Length: " + meshLength);
            i++;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && curve != null && mesh != null)
        {
            Gizmos.color = Color.yellow;
            // curve.UpdateCaches();
            for (float t = -0f; t < 1f; t += 0.02f)
            {
                var a = curve.GetScaledPoint(t, meshLength) + transform.position;
                var b = curve.GetScaledPoint(t + 0.02f, meshLength) + transform.position;
                Gizmos.DrawLine(a, b);
            }
            Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(curve.GetScaledPoint(-0.2f, meshLength) + transform.position, 0.4f);
            Gizmos.color = Color.cyan;
            // Gizmos.DrawWireSphere(curve.GetScaledPoint(0, meshLength) + transform.position, 0.4f);
            Gizmos.color = Color.red;
            // Gizmos.DrawWireSphere(curve.GetScaledPoint(1, meshLength) + transform.position, 0.4f);
            Gizmos.color = Color.white;
            // Gizmos.DrawWireSphere(curve.GetScaledPoint(1.2f, meshLength) + transform.position, 0.4f);
        }
    }
}