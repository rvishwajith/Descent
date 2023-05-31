using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPath : MonoBehaviour
{
    public Transform target;
    public int targetIndex = 0;
    public float targetT = 0;
    public float moveSpeed = 0.035f;

    private Transform[] points;

    void Start()
    {
        points = transform.GetComponentsInChildren<Transform>()[1..];
    }

    void Update()
    {
        targetT += moveSpeed * Time.deltaTime;
        if (targetT >= 1)
        {
            targetIndex++;
            targetT -= 1;
        }

        if (target == null) return;
        target.position = PositionAtSpline(targetIndex, targetT);

        var forwardIndex = targetIndex;
        var forwardT = targetT + moveSpeed * Time.deltaTime;
        if (forwardT > 1)
        {
            forwardIndex++;
            forwardT -= 1;
        }
        target.LookAt(PositionAtSpline(forwardIndex, forwardT));
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor) points = transform.GetComponentsInChildren<Transform>()[1..];
        if (points.Length < 2) return;

        Gizmos.color = Color.yellow;
        float tInterval = 0.05f;
        for (var i = 0; i < points.Length; i++)
        {
            for (float t = 0; t < 1; t += tInterval)
            {
                var start = PositionAtSpline(i, t);
                var end = PositionAtSpline(i, t + tInterval);
                Gizmos.DrawLine(start, end);
            }
            Gizmos.DrawWireSphere(points[i].position, 0.3f);
        }

        Gizmos.color = Color.cyan;
        var targetPos = PositionAtSpline(targetIndex, targetT);
        var targetDir = (PositionAtSpline(targetIndex, targetT + 0.05f) - targetPos).normalized;
        Gizmos.DrawWireSphere(targetPos, 0.6f);
        Gizmos.DrawRay(targetPos, targetDir * 2f);
    }

    private Vector3 PositionAtSpline(int i, float t)
    {
        var a = points[Wrap(i - 1)].position;
        var b = points[Wrap(i)].position;
        var c = points[Wrap(i + 1)].position;
        var d = points[Wrap(i + 2)].position;
        return Interpolate.Spline.Position(a, b, c, d, t);
    }

    private int Wrap(int i)
    {
        if (i >= points.Length)
        {
            i -= points.Length;
        }
        else if (i < 0)
        {
            i += points.Length;
        }
        return i;
    }
}
