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

    [HideInInspector] public int mode;
    [HideInInspector] public float screenShakeStrength = 0f;
    [HideInInspector] public float lensForwardScale = 1.5f;

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
        if (target.name.ToLower() == "player")
            target.GetComponent<PlayerController2>().mode = 0;
        else if (TargetName() == "player")
            this.target.GetComponent<PlayerController2>().mode = 9;

        hasPathHistory = target.TryGetComponent<CreaturePathHistory>(out pathHistory);
        if (hasPathHistory)
        {
            this.target = target;
            UpdateUI();
        }
        else Debug.Log("CameraController.SetTarget() - Error: Target has no path history!");
    }

    void UpdateUI()
    {
        var name = TargetName();
        if (name == "player")
        {
            uiManager.SetPanel("SpeciesName", false);
            uiManager.SetPanel("ExitFollow", false);
            uiManager.SetPanel("FollowCreature", true);
            uiManager.SetPanel("DoorInteraction", false);
        }
        else if (name == "mantaray") // is creature
        {
            uiManager.SetPanel("ExitFollow", true);
            uiManager.SetPanel("SpeciesName", true);
            uiManager.SetPanel("FollowCreature", false);
            uiManager.SetPanel("DoorInteraction", false);

            uiManager.SetText("SpeciesEnglish", "Giant Oceanic Manta Ray");
            uiManager.SetText("SpeciesLatin", "Mobula Birostris");
        }
    }

    private string TargetName()
    {
        return target.name.Replace(" ", "").ToLower();
    }

    private void Update()
    {
        if (TargetName() == "player")
            CheckForNearbyCreatures();
        else if (Input.GetKeyDown(KeyCode.B))
            SetTarget(GameObject.Find("Player").transform);
        if (hasPathHistory)
            FollowPathHistory();
    }

    void CheckForNearbyCreatures()
    {
        bool creaturesInView = false;
        Transform nearestCreature = null;
        float nearestCreatureDist = float.MaxValue;

        foreach (var creature in GameObject.FindGameObjectsWithTag("RideableCreature"))
        {
            var dist = (target.position - creature.transform.position).magnitude;
            if (dist < 6)
            {
                creaturesInView = true;
                if (dist < nearestCreatureDist)
                {
                    nearestCreature = creature.transform;
                    nearestCreatureDist = dist;
                }
            }
        }
        uiManager.SetPanel("FollowCreature", creaturesInView);
        if (creaturesInView && Input.GetKeyDown(KeyCode.F))
            SetTarget(nearestCreature.transform);
    }

    void FollowPathHistory()
    {
        var currentCameraPos = transform.position;
        var pos = Vector3.Lerp(currentCameraPos, cameraPosTarget, Time.deltaTime * 3);
        transform.position = pos;

        var currentLensPos = transform.position + transform.forward * lensForwardScale;
        var lensPos = Vector3.Lerp(currentLensPos, lensPosTarget, Time.deltaTime * 3);
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