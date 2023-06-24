using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCameraController : MonoBehaviour
{
    private Transform gimbal;
    private Vector3 angles = Vector3.zero;
    private float zoom = 5f, zoomMin = 2f, zoomMax = 8f;

    private CameraTargetTracker targetTracker = new();
    public Transform target = null;
    private PlayerController playerController = null;

    private string mode = "N/A";
    private string[] modes = { "N/A", "Player", "Observation", "Cutscene", "Inventory" };

    void Start()
    {
        gimbal = new GameObject("Gimbal").transform;
        transform.parent = gimbal;

        if (target == null)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
                SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
            else if (GameObject.Find("Player") != null)
                SetTarget(GameObject.Find("Player").transform);
            else
            {
                Debug.Log("UniversalCameraController.Start(): No target found.");
                return;
            }
        }

        if (target.tag == "Player")
        {
            if (target.TryGetComponent<PlayerController>(out playerController))
                Debug.Log("UniversalCameraController.Start(): Player controller found.");
        }
        SetTarget(target);
        MoveGimbal();
        MoveCamera();
    }

    void Update()
    {
        if (target == null) return;
        MoveGimbal();
        MoveCamera();
    }

    void MoveGimbal()
    {
        gimbal.position = target.position;
        gimbal.forward = target.forward;
    }

    void MoveCamera()
    {
        transform.localPosition = Vector3.back * zoom;
        transform.LookAt(gimbal.position);
    }

    void SetTarget(Transform newTarget)
    {
        if (target == newTarget)
        {
            Debug.Log("CameraController.SetTarget: Can't switch to the same target.");
            return;
        }
        target = newTarget;
        if (newTarget.tag == "Player")
        {
            Debug.Log("CameraController.SetTarget: Target is the player.");
            mode = "Player";
        }
        else
        {
            DisablePlayer();
            if (newTarget.tag == "LargeCreature")
            {
                Debug.Log("CameraController.SetTarget: Target is a creature: " + newTarget.name);
                mode = "Observation";
            }
            else if (newTarget.tag == "Cutscene")
            {
                Debug.Log("CameraController.SetTarget: Target is a custscene: " + newTarget.name);
                mode = "Cutscene";
            }
        }
    }

    void DisablePlayer()
    {
        Debug.Log("CameraController: Disabling player.");
        // Disable the player controller.
    }
}

class CameraTargetTracker
{
    Transform target;

    void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {

    }
}