using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotCamera : MonoBehaviour
{
    public Transform target;
    private Vector3 targetOffset = Vector3.zero;

    private Transform pivot;
    private Vector2 pivotRotation = Vector2.zero;

    private float sensitivity = 2f;
    private float minAngleX = -70f, maxAngleX = 70f;

    private void Start()
    {
        pivot = transform.parent;
        pivotRotation = pivot.eulerAngles;
    }

    void Update()
    {
        PivotRotation();
        CameraRotation();
    }

    void PivotRotation()
    {
        pivotRotation += GetSwipeInput() * sensitivity;
        pivotRotation.y = Mathf.Clamp(pivotRotation.y, -80, 80);
        var xQuat = Quaternion.AngleAxis(pivotRotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(pivotRotation.y, Vector3.left);
        pivot.localRotation = xQuat * yQuat;
    }

    Vector2 GetSwipeInput()
    {
        if (Input.touchCount != 1) return Vector2.zero;

        var touch = Input.touches[0];
        return new Vector2(-touch.deltaPosition.x, touch.deltaPosition.y) / touch.deltaTime;
    }

    void CameraRotation()
    {
        transform.LookAt(target.position + targetOffset);
    }
}
