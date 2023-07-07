using UnityEngine;
using System.Collections;
using System;

public class WaterSurface : MonoBehaviour
{
    private Mesh mesh = null;
    private Vector3[] initialVertices;
    private Vector3[] vertices;

    private const int waveCount = 8;
    public float speed = 0.4f;
    public float maxAmplitude = 0.03f; // Wave Amplitude (Min <= Ai <= Max)
    public float minAmplitude = 0.001f;
    private float[] Ai = new float[waveCount];
    private float[] lambda = new float[waveCount]; // Length of the waves
    public float maxLambda = 8.0f;
    private float[] ki = new float[waveCount]; // Magnitude
    private float[] frequencies = new float[waveCount];
    public float waterDepth = 1000.0f;
    public bool useRandomDirections = true;

    // Wave vector -> Point where the waves will move towards
    // Given this point we approximate 100 slightly off directions
    // (looks more natural)
    public Vector2 targetPoint = new Vector2(1.0f, 1.0f);
    private Vector2[] directions = new Vector2[waveCount];


    private float GRAVITY = 9.81f;


    private void Start()
    {
        mesh = transform.GetComponent<MeshFilter>().mesh;
        initialVertices = mesh.vertices;
        vertices = new Vector3[initialVertices.Length];
        mesh.MarkDynamic();

        // Initialize Parameters
        for (var i = 0; i < waveCount; i++)
        {
            lambda[i] = UnityEngine.Random.Range(2.0f, maxLambda);
            ki[i] = (float)(2.0f * Math.PI / lambda[i]);
            frequencies[i] = (float)(Math.Sqrt(GRAVITY * ki[i] * Math.Tanh(ki[i] * waterDepth)));
            Ai[i] = UnityEngine.Random.Range(minAmplitude, maxAmplitude);

            if (useRandomDirections)
                directions[i] = new Vector2(UnityEngine.Random.Range(-2.0f, 2.0f), UnityEngine.Random.Range(-2.0f, 2.0f));
            else
                directions[i] = GenerateTargetDirections();
        }
    }

    /// <summary>
    /// First call the HandleChanges Method and check if anything
    /// changes since the last callt to update. Then initialize the mesh-filter and
    /// get the planes (surface) mesh, as well as its vertices.
    /// 
    /// The formulas to calculate the new positions of the vertices was gotten
    /// from graphics.ucsd.edu/courses/rendering/2005/jdewall/tessendorf.pdf
    /// </summary>
    private void Update()
    {
        if (initialVertices == null) return;

        for (int i = 0; i < initialVertices.Length; i++)
        {
            Vector3 pos = initialVertices[i];
            Vector2 oldXZ = new Vector2(pos.x, pos.z);
            float y0 = pos.y;
            float t = Time.time * speed;
            float phi = (float)Math.PI;

            Vector2 xSum = new Vector2(0.0f, 0.0f);
            float y = 0.0f;
            for (int j = 0; j < waveCount; j++)
            {
                xSum += (directions[j] / ki[j]) * Ai[j] * (float)(Math.Sin(Vector2.Dot(directions[j], oldXZ) - frequencies[j] * t + phi));
                y += (float)(Ai[j] * Math.Cos(phi * Vector2.Dot(directions[j], oldXZ) - frequencies[j] * t));
            }

            Vector2 newXZ = (oldXZ - xSum);
            vertices[i] = new(newXZ.x, y, newXZ.y);
        }
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
    }

    private Vector2 GenerateTargetDirections()
    {
        float offsetX = targetPoint.x / 5.0f;
        float offsetY = targetPoint.y / 5.0f;
        return new Vector2(UnityEngine.Random.Range(targetPoint.x - offsetX, targetPoint.x + offsetX),
                           UnityEngine.Random.Range(targetPoint.y - offsetY, targetPoint.y + offsetY));
    }
}

