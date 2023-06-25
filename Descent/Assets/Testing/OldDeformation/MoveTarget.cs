using System;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [Header("Path Options")]
    public Vector3 scales = new(15, 5, 15);
    public Vector3 speeds = new(1, 1, 0.5f);
    private Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
        Vector3 position = GetPosition(Time.time);
        Vector3 target = GetPosition(Time.time + 0.1f);
        transform.LookAt(target, Vector3.up);
        transform.position = position;
    }

    private void Update()
    {
        Vector3 position = GetPosition(Time.time);
        Vector3 target = GetPosition(Time.time + Time.deltaTime);
        transform.LookAt(target, Vector3.up);
        transform.position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float step = Mathf.PI * 2 / 50f;
        for (float t = -Mathf.PI; t < Mathf.PI * 2f; t += step)
        {
            Gizmos.DrawLine(GetPosition(t), GetPosition(t + step));
        }
    }

    Vector3 GetPosition(float t)
    {
        t += Mathf.PI;
        return Vector3.Scale(scales, new Vector3(
                Mathf.Cos(t * speeds.x), // * Mathf.Sin(t * speeds.x),
                Mathf.Cos(t * speeds.y) + scales.y,
                Mathf.Sin(t * speeds.z))) + initialPosition;
    }
}

