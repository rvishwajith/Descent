using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Targets")]
    public Transform doorTarget;
    public Transform animationTarget;

    private float zRotationSpeed = 90f, // Degrees per second.
        xRotationSpeed = 30f,
        yRotationSpeed = 15f;

    private float yPositionCenter = 0f, // Meters
        yPositionAmp = 0.3f,
        yPositionSpeed = 1f;

    void Update()
    {
        var yPosition = yPositionCenter + Mathf.Sin(Time.time * yPositionSpeed) * yPositionAmp;
        animationTarget.localPosition = new(0, yPosition, 0);

        var xRotation = xRotationSpeed * Time.time;
        var yRotation = yRotationSpeed * Time.time;
        var zRotation = zRotationSpeed * Time.time;
        animationTarget.localEulerAngles = new Vector3(xRotation, yRotation, zRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided: " + collision.collider.transform.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered: " + other.transform.name);
    }

    void Activate()
    {
        doorTarget.GetComponent<Door>().Open();
    }
}
