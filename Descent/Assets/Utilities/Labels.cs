using UnityEngine;

namespace Utilities
{
    public static class Labels
    {

        public static Color color
        {
            set
            {
#if UNITY_EDITOR
                UnityEditor.Handles.color = value;
#endif
            }
            get
            {
#if UNITY_EDITOR
                return UnityEditor.Handles.color;
#else
                return Color.black;
#endif
            }
        }

        public static void OnScreen(string text, Vector2 position)
        {
            AtWorld(text, Camera.main.ViewportToWorldPoint(position));
        }

        public static void AtWorld(string text, Vector3 position)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.Label(position, text);
#endif
        }
    }
}