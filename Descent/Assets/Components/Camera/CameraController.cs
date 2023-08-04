using System.Collections.Generic;
using Components.Species;
using Components.Player;
using DG.Tweening;
using UnityEngine;
using Utilities;

/* Components -> Camera -> Controller */

namespace Components.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] public Transform target;
        [SerializeField] public CameraSettings settings;

        public TargetType targetType { get { return GetTargetType(); } }
        private TrackingMode trackingMode;
        private Transform targetTracker;

        private OrbitAndZoomController orbitAndZoomController;

        private float zoom
        {
            get { return -transform.localPosition.z; }
            set { transform.localPosition = -Vector3.forward * value; }
        }

        private Vector3 velocity = Vector3.zero;
        private float smoothing = 0.5f;

        private void Start()
        {
            targetTracker = new GameObject("Camera Target Tracker").transform;
            targetTracker.position = transform.position;
            targetTracker.rotation = transform.rotation;

            orbitAndZoomController = new()
            {
                transform = new GameObject("Camera Orbit/Zoom").transform,
                controller = this
            };
            orbitAndZoomController.transform.SetParent(targetTracker);
            orbitAndZoomController.transform.localPosition = Vector3.zero;
            orbitAndZoomController.transform.localRotation = Quaternion.identity;

            transform.SetParent(orbitAndZoomController.transform);
            transform.localPosition = Vector3.back * settings.defaultOrbitDistance;
            transform.localRotation = Quaternion.identity;

            WillChangeTarget();
        }

        private void SetTarget(Transform newTarget)
        {
            target = newTarget;
            WillChangeTarget();
        }

        private void WillChangeTarget()
        {
            if (targetType == TargetType.NoTarget || targetType == TargetType.Unknown)
            {
                // Debug.Log("Camera.Controller.DidChangeTarget(): Target null or unknown.");
                trackingMode = TrackingMode.DoNothing;
            }
            else
            {
                // Debug.Log("Camera.Controller.DidChangeTarget(): Switching target to known type.");
                StartTransitionToNewTarget(targetType);
            }
        }

        public TargetType GetTargetType()
        {
            if (target == null)
            {
                // Debug.Log("Camera.Controller.IdentifyTarget(): Type = Null");
                return TargetType.NoTarget;
            }
            else if (target.TryGetComponent<PlayerController>(out _) || target.name == "Player")
            {
                Debug.Log("Camera.Controller.IdentifyTarget(): Type = Player");
                return TargetType.Player;
            }
            else if (target.GetComponentsInChildren<ObservableSpecies>().Length != 0)
            {
                // targetInternal = target.GetComponentInChildren<ObservableSpecies>().transform;
                // Debug.Log("Camera.Controller.IdentifyTarget(): Type = Animal");
                return TargetType.Species;
            }
            Debug.Log("Camera.Controller.GetTargetType() ERROR: Unknown target type.");
            return TargetType.NoTarget;
        }

        private void StartTransitionToNewTarget(TargetType targetType)
        {
            velocity = Vector3.zero;
            trackingMode = TrackingMode.Transition;

            if (targetType == TargetType.Species || targetType == TargetType.Player)
            {
                var initialOrbitPosition = targetTracker.position;
                var initialOrbitForward = targetTracker.forward;
                var initialZoom = zoom;

                if (targetType == TargetType.Player)
                {
                    Find.UI.ObservationMode.Hide();
                }

                DOVirtual.Float(0, 1, settings.observation.transitionTime, value =>
                {
                    if (targetType == TargetType.Species)
                    {
                        targetTracker.position = Vector3.Lerp(initialOrbitPosition, target.position, value);
                        targetTracker.forward = Vector3.Slerp(initialOrbitForward, target.right, value);
                        var speciesInfo = target.GetComponent<ObservableSpecies>().infoSheet;
                        zoom = Mathf.Lerp(initialZoom, speciesInfo.defaultCameraZoom, value);
                    }
                    else if (targetType == TargetType.Player)
                    {
                        targetTracker.position = Vector3.Lerp(initialOrbitPosition, target.position + Vector3.up, value);
                        targetTracker.forward = Vector3.Slerp(initialOrbitForward, target.forward, value);
                        var playerController = target.GetComponent<PlayerController>();
                        zoom = Mathf.Lerp(initialZoom, 6.5f, value);
                    }
                }).SetEase(Ease.InOutCubic).OnComplete(() => { DidChangeTarget(targetType); });
            }
        }

        private void DidChangeTarget(TargetType targetType)
        {
            if (targetType == TargetType.Species)
            {
                trackingMode = TrackingMode.FollowAnimal;
                Find.UI.ObservationMode.DisplaySpeciesInfo(target.GetComponent<ObservableSpecies>().infoSheet);
            }
            else if (targetType == TargetType.Player)
            {
                trackingMode = TrackingMode.FollowPlayer;
            }
        }

        public void TrySwitchToRandomSpecies()
        {
            if (trackingMode == TrackingMode.Transition)
                return;

            var species = Find.Species.InScene();
            if (species.Length == 0)
                return;
            else if (species.Length == 1 && target == species[0])
                return;

            while (true)
            {
                var randomSpecies = species[Random.Range(0, species.Length)].transform;
                if (randomSpecies != target)
                {
                    SetTarget(randomSpecies);
                    return;
                }
            }
        }

        private void Update()
        {
            if (trackingMode != TrackingMode.Transition)
            {
                if (Input.GetKeyUp(settings.observationPreviousTargetKey) || Input.GetKeyUp(settings.observationNextTargetKey))
                {
                    TrySwitchToRandomSpecies();
                }
            }

            if (trackingMode == TrackingMode.FollowAnimal)
            {
                targetTracker.position = target.position;
                var currentForward = targetTracker.forward;
                var desiredForward = target.right;
                targetTracker.forward = Vector3.SmoothDamp(currentForward, desiredForward, ref velocity, settings.observation.smoothingFactor);
            }

            if (trackingMode == TrackingMode.FollowPlayer)
            {
                var desiredPos = target.position + Vector3.up;
                targetTracker.position = Vector3.SmoothDamp(targetTracker.position, desiredPos, ref velocity, 0.35f);

                var currentForward = targetTracker.forward;
                var desiredForward = target.forward;
                targetTracker.forward = Vector3.SmoothDamp(currentForward, desiredForward, ref velocity, 0.75f);
            }
        }
    }
}

namespace Components.Camera
{
    public class Target
    {
        public Transform transform;
        public TargetType type = TargetType.NoTarget;
        public Vector3 position { get { return transform != null ? transform.position : Vector3.zero; } }

        public Player.PlayerController playerController = null;
        public Species.ObservableSpecies observableSpecies = null;

        public void Set(Transform t)
        {
            if (t == null)
            {
                type = TargetType.NoTarget;
                transform = null;
            }
            else if (t.TryGetComponent(out playerController))
            {
                type = TargetType.Player;
                transform = t;
            }
            else if (t.GetComponentInChildren<ObservableSpecies>() != null)
            {
                type = TargetType.Species;
                transform = t;
            }
            else
            {
                type = TargetType.Unknown;
                Debug.Log("Camera.Target.Set() ERROR: Unknown target type.");
            }
        }
    }

    public enum TargetType
    {
        NoTarget,
        Player,
        Species,
        Cutscene,
        Unknown
    }
}