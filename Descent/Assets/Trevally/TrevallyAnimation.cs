using UnityEngine;
using System.Collections;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class TrevallyAnimation : MonoBehaviour
{
    [Header("Y Pivot")]
    public Gradient yPivotMask;
    public float yPivotAmp = 30f;
    public float yPivotSpeed = 2f;
    public float zCenter;

    private Mesh mesh;
    private Vector3[] initialVertices, vertices;
    private float zMin, zMax;

    void Start()
    {
        this.mesh = GetComponent<MeshFilter>().mesh;
        initialVertices = this.mesh.vertices;
        vertices = new Vector3[initialVertices.Length];

        zMin = this.mesh.bounds.min.z;
        zMax = this.mesh.bounds.max.z;
    }

    void Update() { Animate(); }

    void Animate()
    {
        float t = Time.time;
        for (var i = 0; i < mesh.vertexCount; i++)
        {
            vertices[i] = Deform(initialVertices[i], t);
        }
        mesh.SetVertices(vertices);
    }

    private Vector3 Deform(Vector3 pos, float t)
    {
        var distFromCenter = (pos.z - zCenter);
        var yPivotStrength = Mathf.Sin(t * yPivotSpeed);
        var yPivotAngle = yPivotStrength * distFromCenter * yPivotAmp;

        pos = RotateAbout(pos, Vector3.forward * zCenter, Vector3.up * yPivotAngle);
        return pos;
    }

    private Vector3 RotateAbout(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}

