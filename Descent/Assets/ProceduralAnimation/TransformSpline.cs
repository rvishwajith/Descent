using System;
using UnityEngine;

public class TransformSpline
{
    private Transform a, b, c, d;
    public float cachedLength = 0;

    public TransformSpline(Transform[] points)
    {
        a = points[0];
        b = points[1];
        c = points[2];
        d = points[3];
    }

    public void UpdateCache(float tInterval)
    {
        this.cachedLength = CalculateApproximateLength(tInterval);
    }

    public float CalculateApproximateLength(float tInterval)
    {
        float length = 0, t = 0;
        while (t < 1)
        {
            var start = Interpolate.Spline.Position(a.position, b.position, c.position, d.position, t);
            var end = Interpolate.Spline.Position(a.position, b.position, c.position, d.position, t + tInterval);
            var segmentLength = (start - end).magnitude;
            length += segmentLength;
            t += tInterval;
        }
        return length;
    }

    public Vector3 Position(float t)
    {
        var position = Interpolate.Spline.Position(a.position, b.position, c.position, d.position, t);
        return position;
    }

    public static float CalculateApproximateLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float tInterval)
    {
        float length = 0;
        for (float t = 0; t < 1; t += tInterval)
        {
            var start = Interpolate.Spline.Position(p0, p1, p2, p3, t);
            var end = Interpolate.Spline.Position(p0, p1, p2, p3, t + tInterval);
            length += (start - end).magnitude;
        }
        return length;
    }

    public void DrawGizmo()
    {
        float t = 0, tInterval = 0.05f;
        while (t < 1)
        {
            var start = Interpolate.Spline.Position(a.position, b.position, c.position, d.position, t);
            var end = Interpolate.Spline.Position(a.position, b.position, c.position, d.position, t + tInterval);
            Gizmos.color = Color.Lerp(Color.yellow, Color.cyan, t);
            Gizmos.DrawLine(start, end);
            t += tInterval;
        }
    }
}