using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurfaceVertexDeformation : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        mesh = transform.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        RandomDeform();
    }

    void RandomDeform()
    {
        float yNoiseAmp = 1;
        for (var i = 0; i < vertices.Length; i++)
        {
            float yNoise = (Random.value - 0.5f) * yNoiseAmp;
            vertices[i].y += yNoise;
        }
        mesh.vertices = vertices;
    }
}
