using System;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [Header("Path Options")]
    public Vector3 scales = Vector3.one * 10f;
    public Vector3 speeds = Vector3.one * 0.5f;

    private Transform target;
    private Vector3 initialPosition;

    void Start()
    {
        target = this.transform;
        initialPosition = this.transform.position;
    }

    private void Update()
    {
        Vector3 position = Vector3.Scale(scales,
            new Vector3(
                Mathf.Sin(Time.time * speeds.x),
                Mathf.Sin(Time.time * speeds.y),
                Mathf.Cos(Time.time * speeds.z)));
        target.position = position + initialPosition;
    }
}

