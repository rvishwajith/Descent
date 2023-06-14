using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinController : MonoBehaviour
{
    /*
    int state;
    Vector3 prevTailPos;
    float swimmingSpeed = 4f;
    float jumpingSpeed = 8f;

    void Start()
    {
        state = 1;
        prevTailPos = TailPos();
    }

    Vector3 TailPos()
    {
        return transform.position - transform.forward * transform.localScale.z / 2;
    }

    void FixedUpdate()
    {
        bool inAir = TailPos().y >= 0;

        if (inAir)
        {
            if (prevTailPos.y < 0)
                Debug.Log("Just jumped!");

            var velocity = transform.forward.normalized * jumpingSpeed * Time.fixedDeltaTime;
            velocity += Vector3.down * 10f * (Time.fixedDeltaTime * Time.fixedDeltaTime);

            transform.forward = velocity;
            transform.position += velocity;
            return;
        }
        else if (prevTailPos.y > 0)
        {
            Debug.Log("Just landed.");
            state = 0;
            Invoke("SwitchToJumping", 2.5f);
        }

        if (state == 0)
        {
            // transform.forward += Vector3.forwar * Time.fixedDeltaTime;
            transform.position += transform.forward * Time.fixedDeltaTime * swimmingSpeed;
        }
        else if (state == 1)
        {
            transform.forward += (Vector3.forward + Vector3.up) * Time.fixedDeltaTime;
            transform.position += transform.forward.normalized * Time.fixedDeltaTime * jumpingSpeed;
        }
        prevTailPos = TailPos();
    }

    void SwitchToJumping()
    {
        Debug.Log("Switching back to jumping.");
        state = 1;
    }
    */

    private void Update()
    {
        var time = Time.time;
        var amp = 40f;
        var ampOffset = 0f;
        var speed = 1.25f;
        var progressShift = 0.25f;

        var angleT = Mathf.Sin(time * speed) + progressShift; // Shift up so that the up motion takes longer.
        if (angleT > progressShift)
            angleT /= (1 + progressShift);
        else
            angleT /= (1 - progressShift);
        var angle = amp * angleT + ampOffset;

        transform.eulerAngles = Vector3.right * angle;
    }
}
