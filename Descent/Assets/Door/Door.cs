using System;
using UnityEngine;
public class Door : MonoBehaviour
{
    [Header("Panels")]
    public Transform right;
    public Transform left;

    private bool opening = false, didOpen = false;
    private float openStartTime = -1, openAnimTime = 6;
    private float startPosX = 0.05f, endPosX = 3.6f;

    public void Start()
    {
        Open();
    }

    public void Open()
    {
        if (opening || didOpen)
        {
            Debug.Log("Door.Open() - Ignoring, door is opening or open.");
            return;
        }
        Debug.Log("Door.Open() - Started opening.");
        opening = true;
        openStartTime = Time.time;
    }

    public void DidOpen()
    {
        Debug.Log("Door.DidOpen() - Finished opening.");
        didOpen = true;
        opening = false;
    }

    public void Update()
    {
        if (!opening || didOpen)
        {
            return;
        }
        if (opening && !didOpen) { OpeningAnimation(); }
    }

    public void OpeningAnimation()
    {
        var timeSinceAnimationStart = Time.time - openStartTime;
        var t = Mathf.Clamp(timeSinceAnimationStart / openAnimTime, 0, 1);
        if (t == 1) { DidOpen(); }

        var x = Mathf.Lerp(startPosX, endPosX, t);
        right.localPosition = new(x, right.localPosition.y, right.localPosition.z);
        left.localPosition = new(-x, left.localPosition.y, left.localPosition.z);
    }
}

