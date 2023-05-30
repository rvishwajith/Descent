using System;
using UnityEngine;
public class Door : MonoBehaviour
{
    [Header("Panels")]
    public Transform right;
    public Transform left;

    private bool opening = false, didOpen = false;
    private float openStartTime = -1, openAnimTime = 6.3f;
    private float startPosX = 0.05f, endPosX = 3.6f;

    public void Start()
    {
        Close();
    }

    public void Close()
    {
        right.localPosition = new(startPosX, right.localPosition.y, right.localPosition.z);
        left.localPosition = new(-startPosX, left.localPosition.y, left.localPosition.z);
    }

    public void Open(float delay)
    {
        if (opening || didOpen)
        {
            Debug.Log("Door.Open() - Ignoring, door is opening or open.");
            return;
        }
        Debug.Log("Door.Open() - Started opening.");
        openStartTime = Time.time + delay;
    }

    void PlayOpeningSound()
    {
        var audioSource = transform.GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.Play();
    }

    public void DidOpen()
    {
        Debug.Log("Door.DidOpen() - Finished opening.");
        didOpen = true;
        opening = false;
    }

    public void Update()
    {
        if (!opening)
        {
            if (didOpen) return;
            else if (openStartTime > 0 && Time.time > openStartTime)
            {
                opening = true;
                PlayOpeningSound();
            }
        }
        else // Opening
        {
            if (!didOpen) OpeningAnimation();
        }
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

