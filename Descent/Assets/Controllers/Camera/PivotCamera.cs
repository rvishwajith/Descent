using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotCamera : MonoBehaviour
{
    public Transform target;
    private Vector3 targetOffset = Vector3.zero;

    private Transform pivot;
    private Vector2 pivotRotation = Vector2.zero;

    /*
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    void Update()
    {
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
    }
    */

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
        /*
        if (Input.touchCount == 0) return;

        var touch = Input.touches[0];
        if (touch.phase != TouchPhase.Moved) return;

        var deltaPos = touch.deltaPosition;
        var deltaTime = touch.deltaTime;

        pivot.eulerAngles += sensitivity / deltaTime * new Vector3(
            -deltaPos.y / Screen.width,
            deltaPos.x / Screen.height,
            0
        );
        */
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

    /*
    void ClampPivotAngles()
    {
        Vector3 rot = new Vector3(pivot.eulerAngles.x, pivot.eulerAngles.y, 0f);
        rot.x = Mathf.Clamp(rot.x, -80, 80);
        pivot.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
    }
    */

    void CameraRotation()
    {
        transform.LookAt(target.position + targetOffset);
    }
}
