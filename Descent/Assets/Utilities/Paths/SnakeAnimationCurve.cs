using System.Collections;
using UnityEngine;

public class SnakeAnimationCurve : MonoBehaviour
{
    public float scale = 1;

    private Transform[] nodes;
    private TransformSpline[] splines;

    private float cachedLength;
    private Vector3 cachedForward;
    private Vector3 cachedCenter;

    public void Start()
    {
        GetNodes();
        CreateSplines();
        UpdateCaches();
        // StartCoroutine(LogApproximateLengths());
    }

    private void FixedUpdate()
    {
        // MoveNodesSinusoid();
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
        splines = new TransformSpline[transform.childCount - 3];
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
            length += TransformSpline.CalculateApproximateLength(positions[i], positions[i + 1], positions[i + 2], positions[i + 3], 0.025f);
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
            Gizmos.DrawLine(a, b);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetPoint(0.5f), CachedForward() * 5f);

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
            splines[i].UpdateCache(0.02f);
        }
        foreach (var spline in splines)
        {
            spline.UpdateCache(0.02f);
            approximateLength += spline.cachedLength;
        }
        this.cachedForward = CalculateCenterForward();
        this.cachedLength = approximateLength;
    }

    public float CachedLength()
    {
        return cachedLength;
    }

    public Vector3 CachedForward()
    {
        return cachedForward;
    }

    public Vector3 CalculateCenterForward()
    {
        return (GetPoint(0.5f) - GetPoint(0.51f)).normalized;
    }

    public Vector3 GetPoint(float t)
    {
        float curveT = 0;
        var i = 0;
        var spline = splines[i];

        if (t < 0)
        {
            splines[0].UpdateCache(0.025f);
            var newT = t * cachedLength / splines[0].cachedLength;
            return splines[0].Position(newT) - cachedCenter;
        }
        else if (t == 0)
        {
            return splines[0].Position(0) - cachedCenter;
        }

        while (i < splines.Length && curveT < t)
        {
            spline = splines[i];
            var splineLength = spline.cachedLength;
            var splineRelativeLength = splineLength / cachedLength;
            curveT += splineRelativeLength;
            i++;
        }
        var curveRemainder = curveT - t; // The remaining length relative to the curve.
        // Multiply the remainder by ratio of the curve's length to the spline's length to get the spline remaider (spline t).
        var splineRemainder = curveRemainder * (cachedLength / spline.cachedLength);
        var splineT = 1f - splineRemainder;
        return spline.Position(splineT) - cachedCenter;
    }

    public Vector3 GetScaledPoint(float t, float expectedCurveLength)
    {
        var point = GetPoint(t);
        var ratio = expectedCurveLength / cachedLength;
        return point * ratio;
    }

    void MoveNodesSinusoid()
    {
        foreach (var node in nodes)
        {
            var angle = 0.9f * Mathf.Sin(Time.time * 1.4f + node.position.z) * Mathf.Pow(node.position.z / 11, 2);
            node.position = new(angle, node.position.y, node.position.z);
        }
    }
}

