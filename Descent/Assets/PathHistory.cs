using System.Collections.Generic;
using UnityEngine;

public class PathHistory : MonoBehaviour
{
    [Header("Follow Distance")]
    public float maxLength = 5f; // In meters, 5 by default.

    private LinkedList<Vector3> positions = new(); // Oldest position is first.
    private float
        minSampleInterval = 0.03f, // In meters/second.
        length; // In meters, gets set in Start().
    private int maxSamples = 1000;

    private void Start()
    {
        float step = maxLength / 100.0f;
        for (float i = maxLength; i > 0; i -= step)
        {
            var pos = transform.position - transform.forward * i;
            AddToHistory(pos);
        }
        positions.AddLast(transform.position);
        length = maxLength;
        Debug.Log("PathHistory.Start(): Positions - " + positions.Count + ", Oldest = " + positions.First.Value + ", Newest = " + positions.Last.Value);
    }

    private void OnDrawGizmos()
    {
        if (positions.Count < 5) return;

        Gizmos.color = Color.yellow;
        var node = positions.First;

        float i = 0;
        while (i < positions.Count - 5)
        {
            var t = i / positions.Count;
            Gizmos.color = Color.Lerp(Color.red, Color.cyan, t);
            Gizmos.DrawLine(node.Value, node.Next.Next.Next.Next.Value);
            node = node.Next.Next.Next.Next;
            i += 4;
        }
    }

    private void FixedUpdate()
    {
        AddToHistory(transform.position);
        RemoveExtraPathSegments();
    }

    void AddToHistory(Vector3 pos)
    {
        if (positions.Count == 0)
        {
            positions.AddLast(pos);
            return;
        }
        Vector3 prevPos = positions.Last.Value;
        var distTravelled = (pos - prevPos).magnitude;
        if (distTravelled >= minSampleInterval)
        {
            positions.AddLast(pos);
            length += distTravelled;
        }
    }

    void RemoveExtraPathSegments()
    {
        while (positions.Count > maxSamples)
        {
            Debug.Log("t = " + Time.fixedDeltaTime + " exceeded max samples.");
            RemoveOldPoint();
        }
        while (length > maxLength)
        {
            // Debug.Log("t = " + Time.fixedDeltaTime + " exceeded max length.");
            RemoveOldPoint();
        }
    }

    void RemoveOldPoint()
    {
        if (positions.Count < 2) return;

        var pos = positions.First.Value;
        var nextPos = positions.First.Next.Value;
        var segmentLength = (pos - nextPos).magnitude;
        length -= segmentLength;
        positions.RemoveFirst();
    }

    public Vector3 EndPoint()
    {
        if (positions.Count < 1) return Vector3.zero;
        return positions.First.Value;
    }
}
