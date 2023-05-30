using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowTargetSnake : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target;
    public float
        speed = 5f,
        rotationSpeed = 5f;
    [Header("Segments")]
    public float segmentDistance = 3f;

    private Transform[] segments = null;

    void Start()
    {
        segments = transform.GetComponentsInChildren<Transform>()[1..];
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].localPosition = new(0, 0, i * -segmentDistance);
        }
    }

    private void Update()
    {
        MoveHead();
        UpdateSegments();
    }

    void MoveHead()
    {
        var head = segments[0];
        /*
        var deltaPos = target.position - head.position;
        if (deltaPos.magnitude > -1) // -1 = never stop
        {
            // head.LookAt(target.position);
        }
        */
        //find the vector pointing from our position to the target
        //create the rotation we need to be in to look at the target
        var direction = (target.position - head.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        head.position += head.forward * Time.deltaTime * speed;
    }

    void UpdateSegments()
    {
        for (var i = 1; i < segments.Length; i++) // Followers.
        {
            Transform
                segment = segments[i],
                segmentTarget = segments[i - 1];
            segment.LookAt(segmentTarget.position);
            segment.position += segment.forward * Time.deltaTime * speed * i;

            var deltaPos = segmentTarget.position - segment.position;
            if (deltaPos.magnitude < segmentDistance)
            {
                // segment.position = segmentTarget.position + deltaPos.normalized * segmentDistance;
            }
            else if (deltaPos.magnitude > segmentDistance)
            {

            }
            segment.position = segmentTarget.position +
                    (segment.position - segmentTarget.position).normalized * segmentDistance;
        }
    }

    private void OnDrawGizmos()
    {
        if (segments != null && segments.Length > 0)
        {
            for (var i = 1; i < segments.Length; i++)
            {
                Transform
                    segment = segments[i],
                    segmentTarget = segments[i - 1];
                Gizmos.color = Color.yellow;
                var deltaPos = segmentTarget.position - segment.position;
                if (deltaPos.magnitude > segmentDistance)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawLine(segment.position, segmentTarget.position);
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(MedianPosition(), Vector3.one * 0.5f);
        }
    }

    private Vector3 MedianPosition()
    {
        int mid = segments.Length / 2;
        bool isEven = segments.Length % 2 == 0;
        if (isEven) // Find the average position of the two center nodes.
        {
            return (segments[mid - 1].position + segments[mid].position) / 2;
        }
        return segments[mid].position;
    }

    private Vector3 CenterOfMass()
    {
        var centerOfMass = Vector3.zero;
        foreach (var segment in segments)
        {
            centerOfMass += segment.position;
        }
        centerOfMass /= segments.Length;
        return centerOfMass;
    }
}