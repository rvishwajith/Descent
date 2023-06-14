using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpLeafDeformation : MonoBehaviour
{
    public VerletKelpLeaf leafSimulation = null;
    private Vector3[] positions = null;

    private Mesh mesh;
    private float minX, maxX;

    private Vector3[] originalVertices;
    private Vector3[] vertices;
    private bool initialized = false;

    void Start()
    {
        mesh = this.GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic();

        originalVertices = mesh.vertices;
        vertices = new Vector3[originalVertices.Length];
        minX = mesh.bounds.min.x;
        maxX = mesh.bounds.max.z;

        initialized = true;
    }

    void FixedUpdate()
    {
        if (leafSimulation == null) return;
        // TODO: Convert the positions from world to local space and account
        // for orientation (?)
        positions = leafSimulation.GetPositions();

        DeformVertices();
    }

    void DeformVertices()
    {
        for (var i = 0; i < originalVertices.Length; i++)
            vertices[i] = DeformVertex(originalVertices[i]);
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
    }

    Vector3 DeformVertex(Vector3 pos)
    {
        var relativeDist = Mathf.InverseLerp(minX, maxX, pos.x);
        float indexAndRemainder = relativeDist * (positions.Length - 1);

        int index = (int)indexAndRemainder;
        float remainder;

        if (relativeDist >= 1)
            remainder = 0;
        else
            remainder = indexAndRemainder % index;

        if (remainder == 0)
        {
            pos.x = positions[index].x;
            // pos.z = positions[index].z;
        }
        else
        {
            pos.x = Mathf.Lerp(positions[index].x, positions[index + 1].x, remainder);
            // pos.z = Mathf.Lerp(positions[index].z, positions[index + 1].z, remainder);
        }
        // Debug.Log("relative X: " + relativeX);
        return pos;
    }

    private void OnDrawGizmos()
    {
        if (initialized)
        {
            Gizmos.DrawMesh(mesh);
        }
    }
}
