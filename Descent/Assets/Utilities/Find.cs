using System;
using UnityEngine;

namespace Utilities
{
    public static class Find
    {
        static Find() { Recache(); }

        public static void Recache() { }

        public static Components.Camera.Controller CameraController
        {
            get { return GameObject.FindAnyObjectByType<Components.Camera.Controller>(); }
        }

        public static class UI
        {
            public static UserInterface.ObservationModeUI ObservationMode
            {
                get { return GameObject.FindAnyObjectByType<UserInterface.ObservationModeUI>(); }
            }
        }
    }
}