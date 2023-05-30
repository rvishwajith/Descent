using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProceduralAnimation : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[]
        originalVertices,
        vertices;
    // private float maxZ, minZ;
    private float minX, maxX;

    void Start()
    {
        // spline = new AnimationSpline(this.transform.Find("Spline"));
        mesh = this.GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        vertices = new Vector3[mesh.vertices.Length];
        // maxZ = mesh.bounds.max.z;
        // minZ = mesh.bounds.min.z;
        minX = 0.5f;
        maxX = mesh.bounds.max.x;
    }

    void FixedUpdate()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            var offset = GetOffsetSin(originalVertices[i]);
            vertices[i] = originalVertices[i] + offset;
        }
        mesh.vertices = vertices;
    }

    Vector3 GetOffsetSin(Vector3 pos)
    {
        float
            sinAmp = 1.2f,
            sinFreq = 1.2f,
            sinShiftY = 0.2f,
            sin = sinAmp * Mathf.Sin(Time.time * sinFreq) + sinShiftY;

        float strengthX = Mathf.InverseLerp(minX, maxX, Mathf.Abs(pos.x));

        Vector3 offset = new(0, Mathf.Pow(strengthX, 2) * sin, 0);
        return offset;
    }
}
