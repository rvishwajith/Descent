using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target = null;
    private bool hasPathHistory = false;
    private PathHistory pathHistory = null;

    private Vector3
        lensPosTarget,
        cameraPosTarget;

    void Start()
    {
        SetTarget(target);
        UpdateTargetPositions();
        // transform.position = cameraPosTarget;
        // transform.LookAt(lensPosTarget);
    }

    void SetTarget(Transform target)
    {
        if (target.TryGetComponent<PathHistory>(out pathHistory))
        {
            hasPathHistory = true;
            Debug.Log("CameraController.SetTarget() - Target has a path history.");
            this.target = target;
        }
        else
        {
            hasPathHistory = false;
            pathHistory = null;
        }

        if (target.name.Replace(" ", "") == "MantaRay")
        {
            this.GetComponent<UpdateUIElements>().SetLabel("SpeciesEnglish", "Giant Oceanic Manta Ray");
            this.GetComponent<UpdateUIElements>().SetLabel("SpeciesLatin", "Mobula Birostris");
        }
    }

    void Update()
    {
        if (hasPathHistory)
        {
            var currentCameraPos = transform.position;
            var pos = Vector3.Lerp(currentCameraPos, cameraPosTarget, Time.deltaTime * 2);
            transform.position = pos;

            var currentLensPos = transform.position + transform.forward;
            var lensPos = Vector3.Lerp(currentLensPos, lensPosTarget, Time.deltaTime * 2);
            transform.LookAt(lensPos);
        }
    }

    void FixedUpdate()
    {
        UpdateTargetPositions();
    }

    void UpdateTargetPositions()
    {
        if (hasPathHistory && pathHistory != null)
        {
            var cameraPosOffset = Vector3.up * 1.2f;
            cameraPosTarget = pathHistory.EndPoint() + cameraPosOffset;

            // transform.LookAt(target.position + offset, Vector3.up);
            var lensPosOffset = target.forward * 1.5f + Vector3.up * 0.3f;
            lensPosTarget = target.position + lensPosOffset;
        }
    }

    void OnDrawGizmos()
    {
        if (target == null) return;
        else if (pathHistory == null) return;

        if (hasPathHistory)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(pathHistory.EndPoint(), transform.position);
            Gizmos.DrawLine(target.position + target.forward * 1f + 0.5f * Vector3.up, transform.position);
        }
    }
}