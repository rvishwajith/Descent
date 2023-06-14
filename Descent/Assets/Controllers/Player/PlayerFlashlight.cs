using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    public float distance = 6f;
    public float height = 1f;

    private void OnDrawGizmos()
    {
        var origin = transform.position;
        var endPointADir = (transform.forward * distance + transform.up * height).normalized;
        var endPointBDir = (transform.forward * distance - transform.up * height).normalized;

        var endPointA = origin + endPointADir * distance;
        var endPointB = origin + endPointBDir * distance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, endPointA);
        Gizmos.DrawLine(origin, endPointB);
        Gizmos.DrawLine(endPointA, endPointB);

        float radius = 0.1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, radius);
        Gizmos.DrawWireSphere(endPointA, radius);
        Gizmos.DrawWireSphere(endPointB, radius);
    }
}
