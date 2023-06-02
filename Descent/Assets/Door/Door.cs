using System;
using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Panels")]
    public Transform right;
    public Transform left;

    private int state; // Closed
    private float closedDist = 0.1f, openDist = 3.2f;
    private float timeToOpen = 6.8f;

    public void Start()
    {
        Close();
    }

    public void Close()
    {
        state = 0;
        right.localPosition = Vector3.right * closedDist;
        left.localPosition = Vector3.left * closedDist;
    }

    public void Open(float delay)
    {
        Invoke("Open", delay);
    }

    public void Open()
    {
        state = 1;
        Camera.main.GetComponent<CameraController>().screenShakeStrength = 0.3f;
        transform.GetComponent<AudioSource>().Play();
        Delegates.Animation.Animate(
            right,
            property: "LocalPosition",
            start: right.localPosition,
            end: Vector3.right * openDist,
            duration: timeToOpen);
        Delegates.Animation.Animate(
            left,
            property: "LocalPosition",
            start: left.localPosition,
            end: Vector3.left * openDist,
            duration: timeToOpen);
        Invoke("DidOpen", timeToOpen);
    }

    void DidOpen()
    {
        Camera.main.GetComponent<CameraController>().screenShakeStrength = 0f;
        Debug.Log("Door opened.");
        state = 2;
    }
}

