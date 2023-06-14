using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DynamicFollowers : MonoBehaviour
{
    public Transform target;
    public DynamicFollower[] followers;
    public float speed = 8f;

    private Transform head;
    private Vector3[] positionHistory;
    private bool initialized = false;

    private void Start()
    {
        head = new GameObject("Head").transform;
        head.position = transform.position;
        head.forward = transform.forward;

        float totalDistance = 0;
        for (var i = 0; i < followers.Length; i++)
        {
            totalDistance += followers[i].distance;
            followers[i].transform = new GameObject("Follower " + (i + 1)).transform;
            followers[i].transform.position = transform.position - transform.forward * totalDistance;
            followers[i].index = DistanceToIndex(totalDistance);
            Debug.Log("Index: " + DistanceToIndex(totalDistance));
        }

        positionHistory = new Vector3[DistanceToIndex(totalDistance) + 1];
        for (var i = 0; i < positionHistory.Length; i++)
            positionHistory[i] = transform.position - (transform.forward * speed * Time.fixedDeltaTime);
        initialized = true;
    }

    private int DistanceToIndex(float distance)
    {
        return (int)(distance / (speed * Time.fixedDeltaTime));
    }

    private void FixedUpdate()
    {
        if (!MoveHead()) return;
        UpdateHistory();
        MoveFollowers();
    }

    private bool MoveHead()
    {
        var posDiff = target.position - head.position;
        var distance = posDiff.magnitude;

        if (distance == 0)
            return false;
        else if (distance < speed * Time.fixedDeltaTime)
            head.position = target.position;
        else
        {
            head.LookAt(target.position);
            head.position += posDiff.normalized * speed * Time.fixedDeltaTime;
        }
        return true;
    }

    void UpdateHistory()
    {
        // Shift all of the positions to the right (newest position is first).
        for (var i = positionHistory.Length - 1; i > 0; i--)
            positionHistory[i] = positionHistory[i - 1];
        // Set the current position as the newest position.
        positionHistory[0] = head.position;
    }

    void MoveFollowers()
    {
        foreach (var follower in followers)
            follower.transform.position = positionHistory[follower.index];
    }

    private void OnDrawGizmos()
    {
        if (!initialized)
        {
            DrawPreviewGizmo();
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(head.position, 0.5f);
        Gizmos.DrawRay(head.position, head.forward * speed / 4);

        Gizmos.color = Color.white;
        foreach (var follower in followers)
            Gizmos.DrawWireSphere(follower.transform.position, 0.4f);

        Gizmos.color = Color.gray;
        var interval = 4;
        for (var i = 0; i < positionHistory.Length - interval; i += interval)
        {
            Gizmos.DrawLine(positionHistory[i], positionHistory[i + interval]);
        }
    }

    private void DrawPreviewGizmo()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawRay(transform.position, transform.forward * speed);

        Gizmos.color = Color.white;
        float totalDistance = 0;
        foreach (var follower in followers)
        {
            totalDistance += follower.distance;
            var position = transform.position - transform.forward * totalDistance;
            Gizmos.DrawWireSphere(position, 0.4f);
        }

        Gizmos.color = Color.gray;
        totalDistance = 0;
        foreach (var follower in followers)
        {
            var initialDistance = totalDistance;
            totalDistance += follower.distance;
            var initialPosition = transform.position - transform.forward * initialDistance;
            var position = transform.position - transform.forward * totalDistance;
            Gizmos.DrawLine(initialPosition, position);
        }
    }
}

[Serializable]
public struct DynamicFollower
{
    public float distance;
    [HideInInspector] public Transform transform;
    [HideInInspector] public int index;
}