using System;
using UnityEngine;

public class BendableCurve
{
    private Transform[] points;
    private TransformSpline[] splines;

    private float cachedLength = 0;
    private Vector3
        cachedCenterPosition = Vector3.zero,
        cachedCenterDirection = Vector3.zero;

    public BendableCurve(Transform parent)
    {
        points = new Transform[parent.childCount];
        for (var i = 0; i < parent.childCount; i++)
        {
            points[i] = parent.GetChild(i);
        }
        CreateSplines();
        BuildCache();
        // Debug.Log("Center Pos: " + WorldPosition(0.5f));
        // Debug.Log("Forward Offset: " + cachedForwardOffset);
    }

    public void CreateSplines()
    {
        splines = new TransformSpline[points.Length - 3];
        for (var i = 0; i < splines.Length; i++)
        {
            splines[i] = new(new Transform[] { points[i], points[i + 1], points[i + 2], points[i + 3] });
        }
    }

    public void BuildCache()
    {
        float approximateLength = 0;
        foreach (var spline in splines)
        {
            spline.UpdateCache(0.02f);
            approximateLength += spline.cachedLength;
        }
        cachedLength = approximateLength;

        this.cachedCenterPosition = Position(0.5f);
        this.cachedCenterDirection = (cachedCenterPosition - Position(0.51f)).normalized;
    }

    public Vector3 Position(float t)
    {
        float curveT = 0;
        var i = 0;
        var spline = splines[i];

        if (t < 0)
        {
            splines[0].UpdateCache(0.025f);
            var newT = t * cachedLength / splines[0].cachedLength;
            return splines[0].Position(newT);
        }
        else if (t == 0)
        {
            return splines[0].Position(0);
        }
        while (i < splines.Length && curveT < t)
        {
            spline = splines[i];
            var splineLength = spline.cachedLength;
            var splineRelativeLength = splineLength / cachedLength;
            curveT += splineRelativeLength;
            i++;
        }
        var curveRemainder = curveT - t; // Remaining length relative to curve.
        var splineRemainder = curveRemainder * (cachedLength / spline.cachedLength); // Remaining length relative to spline.
        var splineT = 1 - splineRemainder; // Go backwards since it's extra.
        return spline.Position(splineT);
    }

    public Vector3 AdjustedPosition(float t)
    {
        // 1. Get a point on the curve.
        // 2. Rotate about the center point based on the center point's direction.
        // 3. Subtract the center point position.
        // 4. Scale the position down to the desired length if necessary.
        var initialPosition = Position(t);


        var rotation = Quaternion.FromToRotation(cachedCenterDirection, Vector3.forward);
        var dir = initialPosition - cachedCenterPosition;
        dir = rotation * dir;
        initialPosition = dir; // + cachedCenterPosition;
        return initialPosition;
    }

    public Vector3 AdjustedPosition(float t, float desiredLength)
    {
        return AdjustedPosition(t) * desiredLength / cachedLength;
    }

    public void DrawGizmo()
    {
        BuildCache();
        if (points == null || splines == null) return;

        // World data.
        Gizmos.color = Color.yellow;
        for (var t = 0f; t < 1 - 0.025f; t += 0.025f)
        {
            Gizmos.DrawLine(Position(t), Position(t + 0.025f));
        }
        Gizmos.DrawWireSphere(cachedCenterPosition, 1f);
        Gizmos.DrawRay(cachedCenterPosition, cachedCenterDirection * 2.5f);
    }
}