using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target = null;
    private bool hasPathHistory = false;
    private CreaturePathHistory pathHistory = null;
    private Vector3 lensPosTarget, cameraPosTarget;
    private Vector3 originalCameraPos;

    private UIInfoManager uiManager;

    public int mode;
    public float screenShakeStrength = 0f;

    void Start()
    {
        this.uiManager = GetComponent<UIInfoManager>();

        if (target == null)
        {
            Debug.Log("CameraController.Start() - No target found.");
            return;
        }
        SetTarget(target);
        UpdateTargetPositions();
    }

    public void SetTarget(Transform target)
    {
        if (target.TryGetComponent<CreaturePathHistory>(out pathHistory))
        {
            hasPathHistory = true;
            // Debug.Log("CameraController.SetTarget() - Target has a path history.");
            this.target = target;
        }
        else
        {
            hasPathHistory = false;
            pathHistory = null;
        }
        ShowTargetInfo();
    }

    void ShowTargetInfo()
    {
        var targetName = target.name.Replace(" ", "").ToLower();
        if (targetName == "player")
        {
            uiManager.SetPanelVisible("SpeciesNamePanel", false);
        }
        else if (targetName == "mantaray") // is creature
        {
            uiManager.SetPanelVisible("SpeciesNamePanel", true);
            uiManager.SetLabelText("SpeciesEnglish", "Giant Oceanic Manta Ray");
            uiManager.SetLabelText("SpeciesLatin", "Mobula Birostris");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && target.name != "Player")
        {
            SetTarget(GameObject.Find("Player").transform);
        }
        if (hasPathHistory)
        {
            FollowPathHistory();
        }
        ApplyScreenShake();
    }

    void FollowPathHistory()
    {
        var currentCameraPos = transform.position;
        var pos = Vector3.Lerp(currentCameraPos, cameraPosTarget, Time.deltaTime * 2);
        transform.position = pos;

        var currentLensPos = transform.position + transform.forward;
        var lensPos = Vector3.Lerp(currentLensPos, lensPosTarget, Time.deltaTime * 2);
        transform.LookAt(lensPos);

        originalCameraPos = transform.position;
    }

    void FixedUpdate() { UpdateTargetPositions(); }

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

    void ApplyScreenShake()
    {
        if (screenShakeStrength == 0) return;
        float shakeAmp = 0.05f;
        transform.localPosition = originalCameraPos + Random.insideUnitSphere * (shakeAmp * screenShakeStrength);
    }
}

public static class CameraModes
{
    public static int
        FOLLOW_PLAYER = 0,
        FOLLOW_CREATURE_SMALL = 1,
        FOLLOW_CREATURE_LARGE = 2,
        CUTSCENE = 3;
}