using System;
using UnityEngine;

public class FollowTargetSnake : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target;
    public float
        speed = 3f,
        segmentDistance = 3;
    private Transform[] segments;

    void Start()
    {
        segments = transform.Find("Spline").GetComponentsInChildren<Transform>()[1..];
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].localPosition = new(0, 0, i * 0.2f * -segmentDistance);
        }
    }

    private void Update()
    {
        MoveTowardsTarget();
        MoveSegments();
    }

    void MoveTowardsTarget()
    {
        var deltaPos = target.position - transform.position;
        if (deltaPos.magnitude > 0)
        {
            transform.LookAt(target.position);
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    void MoveSegments()
    {
        // Head.
        segments[0].localPosition = Vector3.zero;

        // Followers.
        for (var i = 1; i < segments.Length; i++)
        {
            Transform
                segment = segments[i],
                segmentTarget = segments[i - 1];

            var deltaPos = segmentTarget.position - segment.position;
            if (deltaPos.magnitude > segmentDistance)
            {
                segment.LookAt(segmentTarget.position);
                segment.position += segment.forward * Time.deltaTime * speed;
                Debug.DrawLine(segment.position, segmentTarget.position, Color.yellow);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }
}