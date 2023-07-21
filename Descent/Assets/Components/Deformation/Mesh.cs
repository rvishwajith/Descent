using UnityEngine;
using Utilities;

public class MeshController
{
    private Mesh mesh;
    private Vector3[] vertexIn;
    private Vector3[] vertexOut;

    public float initialLength;
    public float currentLength;
    private Vector3 currentScale;
    public Vector3 scale
    {
        get { return currentScale; }
        set { SetScale(value); }
    }
    public bool initialized = false;

    public MeshController(MonoBehaviour mb, bool shiftZ = false)
    {
        MeshFilter mf;
        if (mb.TryGetComponent(out mf))
        {
            Init(mf, shiftZ);
            return;
        }
        Debug.Log("MeshController() ERROR: No mesh filter is attached to MonoBehaviour");
    }

    private void Init(MeshFilter mf, bool shiftZ = false)
    {
        mesh = mf.mesh;
        mesh.MarkDynamic();

        vertexIn = mesh.vertices;
        vertexOut = new Vector3[vertexIn.Length];

        currentScale = Vector3.one;
        initialLength = mesh.bounds.size.z;

        if (shiftZ)
        {
            var offset = Vector3.forward * (mesh.bounds.max.z);
            for (var i = 0; i < vertexIn.Length; i++)
            {
                vertexIn[i] -= offset;
            }
            mesh.SetVertices(vertexIn);
            Debug.Log("MeshController.Init(): Shifted vertices by " + Format.Vector(offset));
        }
        currentLength = mesh.bounds.size.z;
        initialized = true;
    }

    private void SetScale(Vector3 newScale)
    {
        var relativeScale = new Vector3(
            newScale.x / currentScale.x,
            newScale.y / currentScale.y,
            newScale.z / currentScale.z);
        var center = mesh.bounds.center;

        for (var i = 0; i < vertexIn.Length; i++)
        {
            var offset = vertexIn[i] - center;
            var newOffset = Vector3.Scale(offset, relativeScale);
            vertexIn[i] = newOffset + center;
        }
        currentLength = mesh.bounds.size.z;
        currentScale = newScale;
    }

    public void DrawGizmos()
    {
        Vector3 origin = mesh.bounds.center, offset = Vector3.up * 1f;
        var text = "Mesh Data:"
            + "\nScale: " + Format.Vector(currentScale, 3)
            + "\nLength: " + Format.Float(currentLength, 3) + " (Initial: " + Format.Float(initialLength, 3) + ")";
        Gizmo.color = Color.gray;
        Gizmo.Arrow(origin + offset, -offset);
        Labels.World(text, origin + offset);
    }
}