using UnityEngine;
using UnityEditor;

namespace Utilities
{
    public static class Labels
    {
        public static Color color
        {
            set { Handles.color = value; }
            get { return Handles.color; }
        }

        public static void Screen(string text, Vector2 position)
        {
            World(text, Camera.main.ViewportToWorldPoint(position));
        }

        public static void World(string text, Vector3 position)
        {
            UnityEditor.Handles.Label(position, text);
        }
    }
}
