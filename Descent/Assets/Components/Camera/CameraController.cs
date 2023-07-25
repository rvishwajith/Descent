using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Components.Species;
using Utilities;

namespace Components.Camera
{
    public class Controller : MonoBehaviour
    {
        [Header("Settings")]
        public Transform initialTarget;
        public CameraSettings settings;

        private Transform targetInternal;
        public Transform target
        {
            get { return targetInternal; }
            set
            {
                if (value == null)
                    targetInternal = null;
                else if (value != targetInternal)
                    targetInternal = value;
                TargetDidChange();
            }
        }
        private CameraState state;
        private Transform orbitParent;
        private Transform cameraTransform;
        private float zoom
        {
            get { return -cameraTransform.localPosition.z; }
            set { cameraTransform.localPosition = Vector3.back * value; }
        }

        private Vector3 velocity = Vector3.zero;
        private float smoothing = 0.5f;
        private List<ObservableSpecies> animals = new();

        private void Start()
        {
            Init();
            target = initialTarget;
        }

        private void Init()
        {
            cameraTransform = UnityEngine.Camera.main.transform;

            void InstantiateOrbitTransform()
            {
                orbitParent = new GameObject("Camera Orbit").transform;
                orbitParent.position = cameraTransform.position;
                orbitParent.rotation = cameraTransform.rotation;

                cameraTransform.SetParent(orbitParent);
                cameraTransform.localPosition = Vector3.back * settings.defaultOrbitDistance;
                cameraTransform.localRotation = Quaternion.identity;
            }
            void GetAllAnimalsInScene()
            {
                var observableSpecies = FindObjectsByType<ObservableSpecies>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                animals.AddRange(observableSpecies);
            }
            InstantiateOrbitTransform();
            GetAllAnimalsInScene();
        }

        private void TargetDidChange()
        {
            var targetType = GetTargetType();
            if (targetType == CameraTargetType.Null || targetType == CameraTargetType.Unknown)
            {
                // Debug.Log("Camera.Controller.DidChangeTarget(): Target null or unknown.");
                state = CameraState.Null;
            }
            else
            {
                // Debug.Log("Camera.Controller.DidChangeTarget(): Switching target to known type.");
                WillTransitionToNewTarget(targetType);
            }
        }

        private CameraTargetType GetTargetType()
        {
            if (target == null)
            {
                // Debug.Log("Camera.Controller.IdentifyTarget(): Type: Null");
                return CameraTargetType.Null;
            }
            else if (target.TryGetComponent<Player.Controller>(out _) || target.name == "Player")
            {
                // Debug.Log("Camera.Controller.IdentifyTarget(): Type: Player");
                return CameraTargetType.Player;
            }
            else if (target.GetComponentsInChildren<ObservableSpecies>().Length > 0)
            {
                targetInternal = target.GetComponentInChildren<ObservableSpecies>().transform;
                // Debug.Log("Camera.Controller.IdentifyTarget(): Type: Animal");
                return CameraTargetType.Animal;
            }
            else
            {
                Debug.Log("Camera.Controller.IdentifyTarget(): Type: Unknown");
                return CameraTargetType.Unknown;
            }
        }

        private void WillTransitionToNewTarget(CameraTargetType targetType)
        {
            velocity = Vector3.zero;
            state = CameraState.Transitioning;

            if (targetType == CameraTargetType.Animal || targetType == CameraTargetType.Player)
            {
                var initialOrbitPosition = orbitParent.position;
                var initialOrbitForward = orbitParent.forward;
                var initialZoom = zoom;

                DOVirtual.Float(0, 1, settings.observation.transitionTime, value =>
                {
                    orbitParent.position = Vector3.Lerp(initialOrbitPosition, target.position, value);
                    if (targetType == CameraTargetType.Animal)
                    {
                        orbitParent.forward = Vector3.Slerp(initialOrbitForward, target.right, value);
                        var speciesInfo = target.GetComponent<ObservableSpecies>().infoSheet;
                        zoom = Mathf.Lerp(initialZoom, speciesInfo.defaultCameraZoom, value);
                    }
                }).SetEase(Ease.InOutCubic).OnComplete(() => { DidChangeTarget(targetType); });
            }
        }

        private void DidChangeTarget(CameraTargetType targetType)
        {
            if (targetType == CameraTargetType.Animal)
            {
                state = CameraState.FollowAnimal;
                Find.UI.ObservationMode.DisplaySpeciesInfo(target.GetComponent<ObservableSpecies>().infoSheet);
            }
            else if (targetType == CameraTargetType.Player)
            {
                state = CameraState.FollowPlayer;
            }
        }

        public void TrySwitchToRandomSpecies()
        {
            if (state == CameraState.Transitioning) return;

            if (animals.Count > 1)
            {
                while (true)
                {
                    var randomTarget = animals[UnityEngine.Random.Range(0, animals.Count)].transform;
                    if (target != randomTarget)
                    {
                        target = randomTarget;
                        return;
                    }
                }
            }
            else if (animals.Count == 1 && target != animals[0])
            {
                target = animals[0].transform;
            }
        }

        private void Update()
        {
            if (state != CameraState.Transitioning)
            {
                if (Input.GetKeyUp(settings.observationPreviousTargetKey) || Input.GetKeyUp(settings.observationNextTargetKey))
                    TrySwitchToRandomSpecies();
            }
            if (state == CameraState.FollowAnimal)
            {
                orbitParent.position = target.position;
                var currentForward = orbitParent.forward;
                var desiredForward = target.right;
                orbitParent.forward = Vector3.SmoothDamp(currentForward, desiredForward, ref velocity, settings.observation.smoothingFactor);
            }
        }
    }

    public enum CameraState
    {
        Null,
        FollowPlayer,
        FollowAnimal,
        Transitioning
    }
}