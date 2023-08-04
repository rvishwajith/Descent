using UnityEngine;

namespace Utilities
{
    public static class Draw
    {
        static Draw()
        {
            Style.backgroundColor = Color.white;
        }

        public static class Style
        {
            public static int fontSize = 24;
            public static Color textColor = Color.black;

            private static Texture2D backgroundTexture;
            public static Color backgroundColor
            {
                set
                {
                    if (value == Color.white)
                        backgroundTexture = Texture2D.whiteTexture;
                    else if (value == Color.black)
                        backgroundTexture = Texture2D.blackTexture;
                    else
                        backgroundTexture = Texture2D.grayTexture;
                }
            }

            public static GUIStyle Label
            {
                get
                {
                    GUIStyle style = new(GUI.skin.label);
                    style.fontSize = fontSize;
                    style.normal.background = backgroundTexture;
                    style.normal.textColor = textColor;
                    return style;
                }
            }
        }

        public static void Label(Rect position, string content, GUIStyle style = null)
        {
            style = style != null ? style : Style.Label;
            GUI.Label(position, content, style);
        }
    }
}