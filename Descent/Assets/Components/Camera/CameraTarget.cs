using UnityEngine;
using Components.Species;

namespace Components.Camera
{
    public class CameraTarget
    {
        private Transform internalTransform;
        public Transform transform
        {
            get { return internalTransform; }
            set
            {
                if (value != internalTransform)
                {
                    internalTransform = value;
                    TargetDidChange();
                }
            }
        }
        public CameraTargetType type;

        private CameraTargetType IdentifyTarget(Transform t)
        {
            if (t == null)
            {
                Debug.Log("Camera.Target.IdentifyTarget(): Type: Null");
                return CameraTargetType.Null;
            }
            else if (t.TryGetComponent<Components.Player.Controller>(out _) || t.name == "Player")
            {
                Debug.Log("Camera.Target.IdentifyTarget(): Type: Player");
                return CameraTargetType.Player;
            }
            else if (t.GetComponentsInChildren<ObservableSpecies>().Length > 0)
            {
                internalTransform = t.GetComponentInChildren<ObservableSpecies>().transform;
                Debug.Log("Camera.Target.IdentifyTarget(): Type: Animal");
                return CameraTargetType.Animal;
            }
            Debug.Log("Camera.Controller.IdentifyTarget(): Type: Unknown");
            return CameraTargetType.Unknown;
        }

        public void TargetDidChange()
        {
            type = IdentifyTarget(internalTransform);
            Debug.Log("Camera.Controller.IdentifyTarget(): Target changed.");
        }
    }

    public enum CameraTargetType
    {
        Unknown,
        Null,
        Player,
        Animal
    }
}

