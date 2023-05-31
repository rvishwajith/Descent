using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantaRayProceduralAnimation : MonoBehaviour
{
    [Header("Masks")] public Gradient wingMask;
    [Range(0.1f, 10.0f)] public float timeScale;

    private Mesh mesh;
    private Vector3[] initialVertices;
    private Vector3[] vertices;
    private float xMin, xMax, zMin, zMax;

    void Start()
    {
        Setup();
    }

    void Setup()
    {
        this.mesh = GetComponent<MeshFilter>().mesh;
        initialVertices = this.mesh.vertices;
        vertices = new Vector3[initialVertices.Length];

        xMin = this.mesh.bounds.min.x;
        xMin = this.mesh.bounds.min.z;

        xMax = this.mesh.bounds.max.x;
        zMax = this.mesh.bounds.max.z;
    }

    void FixedUpdate()
    {
        Animate(Time.time);
    }

    void Animate(float t)
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] = Deform(initialVertices[i], t);
        }
        mesh.vertices = vertices;
    }

    Vector3 Deform(Vector3 pos, float t)
    {
        var wingPivot = new Vector3(0, pos.y, pos.z);
        var relativeX = Mathf.InverseLerp(0, xMax, Mathf.Abs(pos.x));
        var wingStrength = wingMask.Evaluate(relativeX).grayscale;
        var wingAngle = wingStrength * 35 * Mathf.Sin(t * timeScale) + 15;

        var wingRotationAxis = Vector3.forward;
        if (pos.x < 0)
        {
            wingRotationAxis = Vector3.back;
        }
        var wingPos = RotatePointAroundPivot(pos, wingPivot, wingRotationAxis * wingAngle);
        return wingPos;
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
