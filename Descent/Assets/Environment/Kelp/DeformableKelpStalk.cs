using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformableKelpStalk : MonoBehaviour
{
    public VerletKelp simulation;
    private bool isRendered = false;

    private Mesh mesh;
    private Vector3[] originalVertices, vertices;
    private Vector3 boundsMin, boundsMax;

    private void Start()
    {
        mesh = transform.GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic();

        originalVertices = mesh.vertices;
        vertices = new Vector3[originalVertices.Length];

        boundsMin = mesh.bounds.min;
        boundsMax = mesh.bounds.max;
    }

    private void Update()
    {
        if (isRendered)
        {
            var verletPoints = simulation.GetPositions();
            Deform(verletPoints);
            mesh.SetVertices(vertices);
        }
    }

    void Deform(Vector3[] verletPoints)
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            var pos = originalVertices[i];
            var t = Mathf.InverseLerp(boundsMin.y, boundsMax.y, pos.y);
            var pivot = FindPointOnRope(verletPoints, t);
            vertices[i] = pivot + new Vector3(pos.x, 0, pos.z);
        }
    }

    private Vector3 FindPointOnRope(Vector3[] points, float t)
    {
        float preciseIndex = t * (points.Length - 1);
        float remainder = preciseIndex % 1;
        int index = (int)preciseIndex;

        // Instead do t-1 for next in the future.
        Vector3 pos;
        if (index < 0)
            index = 0;
        if (index >= points.Length - 1)
            pos = points[points.Length - 1];
        else
            pos = Vector3.Lerp(points[index], points[index + 1], remainder);
        return Vector3.Scale(pos, Vector3.one + Vector3.up);
    }

    private void OnBecameVisible()
    {
        isRendered = true;
    }

    private void OnBecameInvisible()
    {
        isRendered = false;
    }
}
