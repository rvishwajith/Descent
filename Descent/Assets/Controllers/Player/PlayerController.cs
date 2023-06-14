using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // MODES:
    // 9 - Disabled, 0 - Idle, 1 - Swimming, 2 - Boosting
    [HideInInspector] public int mode = 0;
    [HideInInspector] public float speed = 0, speedMax = 0;

    // SWIMMING (Regular Mode)
    [Header("Regular Movement")]
    public float swimSpeedMax;
    public float swimAcceleration;

    // SWIMMING (Boost Mode)
    [Header("Boosted Movement")]
    public float boostSpeedMax;
    public float boostAcceleration;

    // ROTATION
    // Note: Diving is on the y axis and turning is on the x axis.
    private float turnAngle = 0,
        diveAngle = 0, diveAngleMin = -70, diveAngleMax = 88;
    [Header("Rotation")]
    public float turnSensitivity = 1;
    public float diveSensitivity = 1;

    [Header("Collisions")]
    public LayerMask terrainCollisionMask;
    public LayerMask boundsCollisionMask;

    private void Update()
    {
        if (mode == 9)
            return;
        if (mode != 8)
        {
            CheckForModeChange();
            // OutOfBoundsCheck();
        }

        if (mode == 0)
        {
            Idle();
        }
        else if (mode == 1)
        {
            SwimmingSpeed();
            SwimmingRotation();
            SwimmingTryBoost();
        }
        else if (mode == 2)
        {
            BoostingSpeed();
            SwimmingRotation();
        }
        Move();
    }

    void CollisionCheck()
    {
        RaycastHit hit;
        float castDist = 2.5f;
        if (Physics.Raycast(transform.position, transform.forward, out hit, castDist, terrainCollisionMask))
        {
            var inverseDistFactor = 1 - (hit.distance / castDist);
            var distFactor = Mathf.Pow(inverseDistFactor * 0.85f, 2.2f);
            transform.forward = Vector3.Lerp(transform.forward, -transform.forward, distFactor);
        }
    }

    void OutOfBoundsCheck()
    {
        RaycastHit hit;
        float castDist = 0.5f;
        if (Physics.Raycast(transform.position, transform.forward, out hit, castDist, boundsCollisionMask))
        {
            mode = 8;
            StartCoroutine(PlayerIsOutOfBounds());
        }
    }

    IEnumerator PlayerIsOutOfBounds()
    {
        Debug.Log("Out of bounds!");
        float rotation = 0;
        float finalRotation = 120;
        float timeStep = 0.02f;
        float rotationStep = 1.25f;
        speed = swimSpeedMax;

        while (rotation < finalRotation)
        {
            transform.eulerAngles += new Vector3(0, rotationStep, 0);
            rotation += rotationStep;
            yield return new WaitForSeconds(timeStep);
        }
        yield return new WaitForSeconds(1.5f);
        mode = 1;
        Debug.Log("No longer out of bounds.");
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2.5f);
    }

    public void CheckForModeChange()
    {
        if (mode != 2 && ShouldMove())
            mode = 1;
        else if ((mode != 0) && !ShouldMove())
            mode = 0;
    }

    public void Idle()
    {
        speed -= 2 * swimAcceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, speedMax);
    }

    public void Move()
    {
        speed = Mathf.Clamp(speed, 0, speedMax);
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void SwimmingSpeed()
    {
        speedMax = swimSpeedMax;
        speed += swimAcceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, speedMax);
    }

    public void SwimmingRotation()
    {
        var speedMultiplier = Mathf.Clamp(Mathf.InverseLerp(0, speedMax, speed * 1.5f), 0, 1);
        var input = RotationInput() * speedMultiplier;

        turnAngle += input.x * (Time.deltaTime * 90);
        // Debug.Log(turnAngle);
        diveAngle += input.y * (Time.deltaTime * 90);
        diveAngle = Mathf.Clamp(diveAngle, diveAngleMin, diveAngleMax);
        transform.rotation = Quaternion.Euler(diveAngle, turnAngle, 0);
    }

    public void SwimmingTryBoost()
    {
        bool canBoost = speed >= swimSpeedMax && Input.GetKeyDown(KeyCode.Space);
        if (canBoost)
        {
            mode = 2;
            StartCoroutine(BoostDeceleration());
        }
    }

    private IEnumerator BoostDeceleration()
    {
        Debug.Log("Started boosting!");
        yield return new WaitForSeconds(1.5f);
        while (speedMax > swimSpeedMax)
        {
            speedMax -= 0.2f;
            yield return new WaitForSeconds(0.05f);
        }
        mode = 1;
        Debug.Log("Stopped boosting!");
        yield return null;
    }

    public void BoostingSpeed()
    {
        speedMax = boostSpeedMax;
        speed += boostAcceleration * Time.deltaTime;
    }

    private bool ShouldMove()
    {
        return ShouldMoveMouse() || ShouldMoveKeyboard();
    }

    private bool ShouldMoveMouse()
    {
        return Input.GetKey(KeyCode.Mouse0);
    }

    private bool ShouldMoveKeyboard()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private Vector2 RotationInput()
    {
        return Vector2.Scale(RotationInputMouse(), new(turnSensitivity, diveSensitivity));
    }

    private Vector2 RotationInputMouse()
    {
        Vector2 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // Convert from viewport position (0 to 1) to quadrant position (-1 to 1)
        Vector2 screenSpacePos = viewportPos * 2 - Vector2.one;
        screenSpacePos.y *= -1;
        return screenSpacePos;
    }

    private Vector2 RotationInputKeyboard()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            input.x = -1;
        else if (Input.GetKey(KeyCode.D))
            input.x = 1;
        if (Input.GetKey(KeyCode.W))
            input.y = -1;
        else if (Input.GetKey(KeyCode.S))
            input.y = 1;
        return input.normalized;
    }
}
