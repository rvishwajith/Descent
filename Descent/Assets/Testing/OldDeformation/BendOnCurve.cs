using UnityEngine;
using System.Collections;
using static Interpolate;
using System.Security.Cryptography;

/* New deformation algorithm:
 * 
 * For each vertex:
 * Get T: Get the position of the vertex's z position relative to the mesh bounds.
 * 
 * We need two get 2 vectors from the curve:
 * 1. The pivot point at T:
 * - This is the point we will rotate about.
 * - This must be scaled to fit the actual mesh's length.
 * - Should be relative to the center point's position.
 * 2. The forward vector at T:
 * - This will be used to find the x and y tangents, and offset the vertex position from the pivot point accordingly.
 * - Should be relative to the forward vector.
 */
// What is the difference in forward between the center forward and the actual forward?
// That is the amount to rotate (this way transformations on the actual curve have no affect).

public class BendOnCurve : MonoBehaviour
{
    public Transform curveContainer;
    private BendableCurve curve;

    private Mesh mesh;
    private Vector3[] originalVertices, vertices;
    private float meshLength, maxZ, minZ;

    public void Start()
    {
        curve = new(curveContainer);

        mesh = transform.GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        vertices = new Vector3[mesh.vertexCount];

        meshLength = mesh.bounds.size.z;
        maxZ = mesh.bounds.max.z;
        minZ = mesh.bounds.min.z;
    }

    private void FixedUpdate()
    {
        curve.BuildCache();
        for (var i = 0; i < originalVertices.Length; i++)
        {
            vertices[i] = Deform(originalVertices[i]);
        }
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
    }

    private Vector3 Deform(Vector3 pos)
    {
        var t = Mathf.InverseLerp(maxZ, minZ, pos.z);
        var pivot = curve.AdjustedPosition(t, meshLength);
        var forward = PivotToForward(pivot, t);

        var right = ForwardToRight(forward);
        var up = ForwardToUp(forward);
        if (t > 0.5)
        {
            right = -right;
            up = -up;
        }
        var tangent = right * pos.x + up * pos.y;
        return pivot + tangent;
    }

    private void OnDrawGizmos()
    {
        if (curve == null) return;
        curve.DrawGizmo();

        // Adjusted data.
        Gizmos.color = Color.grey;
        for (var t = 0f; t < 1 - 0.025f; t += 0.025f)
        {
            Gizmos.color = Color.Lerp(Color.cyan, Color.yellow, t);

            var pos = curve.AdjustedPosition(t, meshLength);
            var pos2 = curve.AdjustedPosition(t + 0.025f, meshLength);
            Gizmos.DrawLine(pos, pos2);

            var forward = PivotToForward(pos, t);
            var right = ForwardToRight(forward);
            var up = ForwardToUp(forward);
            if (t > 0.5)
            {
                right = -right;
                up = -up;
            }
            Gizmos.DrawRay(pos, right);
            Gizmos.DrawRay(pos, up);
        }
        Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(curve.AdjustedPosition(0.5f, meshLength), 1f);
        Gizmos.color = Color.cyan;
    }

    private Vector3 ForwardToRight(Vector3 forward)
    {
        return Vector3.Cross(-forward, Vector3.up);
    }

    private Vector3 ForwardToUp(Vector3 forward)
    {
        return Vector3.Cross(forward, Vector3.right);
    }

    private Vector3 PivotToForward(Vector3 pivot, float t)
    {
        return (curve.AdjustedPosition(t - 0.005f) - pivot).normalized;
    }
}