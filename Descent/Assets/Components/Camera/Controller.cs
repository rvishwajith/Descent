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
        public CameraSettings settings;
        private Transform targetInternal;

        public Transform target
        {
            get { return targetInternal; }
            set
            {
                if (value != targetInternal)
                {
                    targetInternal = value;
                    OnTargetChange();
                }
            }
        }
        private CameraState state;
        private Transform orbitTransform;
        private Transform cameraTransform;

        private List<ObservableSpecies> animals = new();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            cameraTransform = UnityEngine.Camera.main.transform;

            void InstantiateOrbitTransform()
            {
                orbitTransform = new GameObject("Camera Orbit").transform;
                orbitTransform.position = cameraTransform.position;
                orbitTransform.rotation = cameraTransform.rotation;

                cameraTransform.SetParent(orbitTransform);
                cameraTransform.localPosition = Vector3.back * settings.defaultOrbitDistance;
                cameraTransform.localRotation = Quaternion.identity;

                // Debug.Log("Camera.Controller.Init(): Created camera orbit.");
            }
            void GetAllAnimalsInScene()
            {
                var observableSpecies = FindObjectsByType<ObservableSpecies>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                animals.AddRange(observableSpecies);
            }

            InstantiateOrbitTransform();
            GetAllAnimalsInScene();
            OnTargetChange();
        }

        private void OnTargetChange()
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
            state = CameraState.Transitioning;

            var startPosition = orbitTransform.position;
            DOVirtual.Float(0, 1, settings.transitionDuration, value =>
            {
                orbitTransform.position = Vector3.Lerp(startPosition, target.position, value);
            }).SetEase(Ease.InOutCubic).OnComplete(() => { DidChangeTarget(targetType); });

            /*
            float threshold = 0.025f;
            Tweener tweener = orbitTransform.DOMove(target.position, duration).SetSpeedBased(true);
            tweener.OnUpdate(delegate ()
            {
                // If the tween isn't close enough to the target, set the end position to the target again
                if (Vector3.Distance(orbitTransform.position, target.position) > threshold)
                {
                    tweener.ChangeEndValue(target.position, true);
                }
            });
            */
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
            if (state == CameraState.Transitioning)
                return;
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
                target = animals[0].transform;
        }

        private void Update()
        {
            if (state != CameraState.Transitioning)
            {
                if (Input.GetKeyUp(settings.observationPreviousTargetKey) || Input.GetKeyUp(settings.observationNextTargetKey))
                    TrySwitchToRandomSpecies();
            }

            if (state == CameraState.FollowAnimal)
                orbitTransform.position = target.position;
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