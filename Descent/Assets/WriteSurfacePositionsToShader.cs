using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteSurfacePositionsToShader : MonoBehaviour
{
    public Transform waterSurface;
    private Mesh waterSurfaceMesh;

    private void Start()
    {
        waterSurfaceMesh = waterSurface.GetComponent<MeshFilter>().mesh;
    }

    private void OnDrawGizmos()
    {
        if (waterSurfaceMesh != null)
        {
            Gizmos.color = Color.cyan;
            var vertices = waterSurfaceMesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                // If we move or scale the plane, we need to adjust for that
                // by calculating the vertex's vertex position, not local.
                var vertex = vertices[i];
                var point = vertex;
                // ToWorldPoint: Returns (x, y) ranging from 0 - 1.0
                // (instead of pixels like ToScreenPoint)
                var viewportPos = Camera.main.WorldToViewportPoint(point);
                bool inViewport = (viewportPos.x >= 0
                    && viewportPos.x <= 1
                    && viewportPos.y >= 0
                    && viewportPos.y <= 1);
                if (inViewport)
                {
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }
    }

    private void Update()
    {
        var vertices = waterSurfaceMesh.vertices;
        for (var i = 0; i < vertices.Length; i++)
        {
            // If we move or scale the plane, we need to adjust for that
            // by calculating the vertex's vertex position, not local.
            var vertex = vertices[i];
            var point = vertex;
            // ToWorldPoint: Returns (x, y) ranging from 0 - 1.0
            // (instead of pixels like ToScreenPoint)
            var viewportPos = Camera.main.WorldToViewportPoint(point);
            bool inViewport = (viewportPos.x >= 0
                && viewportPos.x <= 1
                && viewportPos.y >= 0
                && viewportPos.y <= 1);
            if (inViewport)
            {
                WritePointToShader(point);
            }
        }
    }

    private void WritePointToShader(Vector3 point)
    {
        // Write point.
    }
}
