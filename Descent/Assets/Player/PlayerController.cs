using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cursorDebug;
    private int mode = MovementModes.IDLE;

    private Vector3
        rotationDir = new(0, 0, 0),
        deadZone = new(0f, 0f, 0),
        rotationSpeed = new(40, 70, 0);

    private float // 1 m/s = 2.24 mph
        speed = 0,
        speedMax = 6f, // 17 mph
        acceleration = 2f; // m/s^2
    private float // Boosting
        boostSpeedMax = 13f,
        boostDuration = 2f,
        boostAcceleration = 10f,
        boolEndTime = 0;
    private bool boosting = false;
    public LayerMask mask;

    private void Update()
    {
        SelectMovementMode();
        HandleMovementMode();
    }

    private void FixedUpdate()
    {
        // Debug.Log(Time.fixedDeltaTime);
    }

    public float GetSpeed() { return speed; }

    public int GetMode() { return mode; }

    void SelectMovementMode()
    {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.LeftShift))
        {
            mode = MovementModes.MOVE_NORMAL;
            rotationDir = GetMouseDirection();
        }
        else if (Input.touchCount > 0)
        {
            mode = MovementModes.MOVE_NORMAL;
        }
        else
        {
            mode = MovementModes.IDLE;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchTargets();
        }
    }

    void SwitchTargets()
    {
        var allRideableCreatures = GameObject.FindGameObjectsWithTag("RideableCreature");
        foreach (var creature in allRideableCreatures)
        {
            var distance = (transform.position - creature.transform.position).magnitude;
            if (distance < 5)
            {
                Camera.main.GetComponent<CameraController>().SetTarget(creature.transform);
            }
        }
    }

    void HandleMovementMode()
    {
        if (mode == MovementModes.MOVE_NORMAL)
        {
            speed += acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, speedMax);
            // Debug.Log(speed);
            Turn();
            Move();
        }
        else if (mode == MovementModes.MOVE_BOOST)
        {
            speed += boostAcceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, boostSpeedMax);
            Turn();
            Move();
        }
        else if (mode == MovementModes.IDLE)
        {
            rotationDir = Vector3.zero;
            speed -= 3 * acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, speedMax);
            Move();
        }
    }

    private Vector3 GetMouseDirection()
    {
        var mouseDirection = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseDirection -= new Vector3(0.5f, 0.5f, 0);
        mouseDirection *= 2; // Now from [-1, 1] for x and y (relative to viewport).

        cursorDebug.localPosition = new Vector3(
            mouseDirection.x,
            mouseDirection.y * (1 / Camera.main.aspect),
            cursorDebug.localPosition.z);

        if (Mathf.Abs(mouseDirection.x) < deadZone.x)
        {
            mouseDirection.x = 0;
        }
        if (Mathf.Abs(mouseDirection.y) < deadZone.y)
        {
            mouseDirection.y = 0;
        }
        return mouseDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, transform.forward * speed);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.up * speed);
    }

    void Turn()
    {
        Vector3 rotationScale = rotationSpeed * Time.deltaTime;

        var rotation = new Vector3(-rotationDir.y, rotationDir.x, 0);
        if (rotation.magnitude > 1)
        {
            rotation = rotation.normalized;
        }
        // Debug.Log(Vector3.Dot(transform.up, Vector3.up));

        transform.Rotate(Vector3.Scale(rotation, rotationScale));
        if (Mathf.Abs(Vector3.Dot(transform.forward, Vector3.up)) > 0.7f)
        {
            transform.forward = Vector3.Slerp(transform.forward,
                Vector3.Scale(transform.forward, new Vector3(1, 0.7f, 1)), Time.deltaTime);

            // rotation.y *= -1;
            // Debug.Log(Vector3.Dot(transform.up, Vector3.up));
        }
    }

    void Move()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        float rayDist = (speed / 2) + 1.5f;
        if (Physics.Raycast(ray, out hit, rayDist, mask))
        {
            speed /= (1 + Time.deltaTime);
            return;
        }
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    void AnimateFlippers()
    {

    }
}

public static class MovementModes
{
    public static int IDLE = 0;
    public static int MOVE_NORMAL = 1;
    public static int MOVE_BOOST = 2;
    public static int REDIRECT = 3;
    public static int CUTSCENE = 4;
}