using UnityEngine;
using Components.Species;

namespace Utilities
{
    public static class Find
    {
        static Find() { Recache(); }

        public static void Recache() { }

        public static Components.Camera.CameraController CameraController
        {
            get { return GameObject.FindAnyObjectByType<Components.Camera.CameraController>(); }
        }

        public static class UI
        {
            public static UserInterface.ObservationModeUI ObservationMode
            {
                get { return GameObject.FindAnyObjectByType<UserInterface.ObservationModeUI>(); }
            }
        }

        public static class Species
        {
            public static ObservableSpecies[] InScene(
                FindObjectsInactive includeActive = FindObjectsInactive.Exclude,
                FindObjectsSortMode sortMode = FindObjectsSortMode.None)
            {
                return GameObject.FindObjectsByType<ObservableSpecies>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            }
        }
    }
}