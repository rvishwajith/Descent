using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseCursor : MonoBehaviour
{
    void Update()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.y = 0;
        transform.position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
