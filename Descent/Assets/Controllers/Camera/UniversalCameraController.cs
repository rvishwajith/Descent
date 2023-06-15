using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCameraController : MonoBehaviour
{
    public Transform target = null;

    private string mode = "N/A";
    private string[] modes = { "N/A", "Player", "Observation", "Cutscene" };

    CameraTargetTracking tracking;

    void Start()
    {
        if (target == null)
        {
            if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
                SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    void SetTarget(Transform newTarget)
    {
        if (target == newTarget)
        {
            Debug.Log("CameraController.SetTarget: Can't switch to the same target.");
            return;
        }

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
            else if (newTarget.tag == "CutScene")
            {
                Debug.Log("CameraController.SetTarget: Target is a custscene: " + newTarget.name);
                mode = "Cutscene";
            }
        }
        target = newTarget;
    }

    void DisablePlayer()
    {
        Debug.Log("CameraController: Disabling player.");
        // Disable the player controller.
    }

    void Update()
    {

    }
}

class CameraTargetTracking
{

}