using System.Collections;
using UnityEngine;

public class SnakeAnimationCurve : MonoBehaviour
{
    public float scale = 1;

    private Transform[] nodes;
    private SnakeAnimationSpline[] splines;

    private float cachedLength;
    private Vector3 cachedCenter;

    public void Start()
    {
        GetNodes();
        CreateSplines();
        UpdateCaches();
        // StartCoroutine(LogApproximateLengths());
    }

    public void GetNodes()
    {
        nodes = new Transform[transform.childCount];
        for (var i = 0; i < transform.childCount; i++)
        {
            nodes[i] = transform.GetChild(i);
        }
        CreateSplines();
    }

    public void CreateSplines()
    {
        splines = new SnakeAnimationSpline[transform.childCount - 3];
        for (var i = 0; i < nodes.Length - 3; i++)
        {
            splines[i] = new(new Transform[] { nodes[i], nodes[i + 1], nodes[i + 2], nodes[i + 3] });
        }
    }

    private IEnumerator LogApproximateLengths()
    {
        var i = 0;
        // Debug.ClearDeveloperConsole();
        while (true)
        {
            // Debug.ClearDeveloperConsole();
            yield return new WaitForSeconds(0.25f);
            Debug.Log(i + " Approximate Length: " + CalculateApproximateLength());
            Debug.Log(i + "Approximate Scaled Length: " + CalculateApproximateLengthScaled(ScaledPositions()));
            // Debug.Log("Length after updating: " + ApproximateLength());
            i++;
            yield return new WaitForSeconds(1.5f);
        }
    }

    private Vector3[] ScaledPositions()
    {
        var scaledPositions = new Vector3[nodes.Length];
        var center = CalculateTrueCenter();

        for (var i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            var positionDifference = node.position - center;
            var distance = positionDifference.magnitude;
            scaledPositions[i] = center + positionDifference * scale;
        }
        return scaledPositions;
    }

    public float CalculateApproximateLength()
    {
        float length = 0;
        foreach (var spline in splines)
        {
            length += spline.CalculateApproximateLength(0.025f);
        }
        return length;
    }

    public float CalculateApproximateLengthScaled(Vector3[] positions)
    {
        float length = 0;
        for (var i = 0; i < positions.Length - 3; i++)
        {
            length += SnakeAnimationSpline.CalculateApproximateLength(positions[i], positions[i + 1], positions[i + 2], positions[i + 3], 0.025f);
        }
        return length;
    }

    public void OnDrawGizmos()
    {
        if (nodes == null || splines == null) return;

        foreach (var spline in splines)
        {
            // spline.DrawGizmo();
        }

        Gizmos.color = Color.white;
        for (float t = -0.1f; t < 1.1f; t += 0.025f)
        {
            var a = GetPoint(t);
            var b = GetPoint(t + 0.025f);
            /*
            if ((a - b).magnitude > 0.75f)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(a, b);
                Gizmos.color = Color.cyan;
                Debug.Log("Error at t: " + t);
                continue;
            }
            */
            Gizmos.DrawLine(a, b);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(CalculateTrueCenter(), 1f);
    }

    private Vector3 CalculateTrueCenter()
    {
        if (nodes.Length % 2 == 0)
        {
            var centerA = nodes[nodes.Length / 2 - 1].position;
            var centerB = nodes[nodes.Length / 2].position;
            return (centerA + centerB) / 2;
        }
        return nodes[nodes.Length / 2].position;
    }

    public void UpdateCaches()
    {
        this.cachedCenter = CalculateTrueCenter();

        float approximateLength = 0;
        for (var i = 0; i < splines.Length; i++)
        {
            splines[i].UpdateCaches(0.02f);
        }
        foreach (var spline in splines)
        {
            spline.UpdateCaches(0.02f);
            approximateLength += spline.CachedLength();
        }
        this.cachedLength = approximateLength;
    }

    public float CachedLength()
    {
        return cachedLength;
    }

    public Vector3 GetPoint(float t)
    {
        float curveT = 0;
        var i = 0;
        var spline = splines[i];

        if (t < 0)
        {
            splines[0].UpdateCaches(0.025f);
            var newT = t * cachedLength / splines[0].CachedLength();
            // Debug.Log(newT + "Cached Length: " + cachedLength + " Spline Length: " + splines[0].CachedLength());
            return splines[0].Position(newT) - cachedCenter;
        }
        else if (t == 0)
        {
            return splines[0].Position(0) - cachedCenter;
        }

        while (i < splines.Length && curveT < t)
        {
            spline = splines[i];
            var splineLength = spline.CachedLength();
            var splineRelativeLength = splineLength / cachedLength;
            curveT += splineRelativeLength;
            i++;
        }
        var curveRemainder = curveT - t; // The remaining length relative to the curve.
        // Multiply the remainder by ratio of the curve's length to the spline's length to get the spline remaider (spline t).
        var splineRemainder = curveRemainder * (cachedLength / spline.CachedLength());
        var splineT = 1f - splineRemainder;
        return spline.Position(splineT) - cachedCenter;
    }

    public Vector3 GetScaledPoint(float t, float expectedCurveLength)
    {
        var point = GetPoint(t);
        var ratio = expectedCurveLength / cachedLength;
        return point * ratio;
    }
}

public class SnakeAnimationSpline
{
    private Transform a, b, c, d;
    private float cachedLength = 0;

    public SnakeAnimationSpline(Transform[] points)
    {
        a = points[0];
        b = points[1];
        c = points[2];
        d = points[3];
    }

    public void UpdateCaches(float tInterval)
    {
        this.cachedLength = CalculateApproximateLength(tInterval);
    }

    public float CachedLength()
    {
        return cachedLength;
    }

    public Vector3 Position(float t)
    {
        var position = Interpolate.Spline.Position(a.position, b.position, c.position, d.position, t);
        return position;
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