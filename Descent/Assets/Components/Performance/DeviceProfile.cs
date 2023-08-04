using UnityEngine;

namespace Components.Performance
{
    public enum DeviceProfile
    {
        iPhone, // Always target 60 FPS.
        iPad, // A regular iPad or an older iPad Pro, target 60 FPS.
        iPadPro, // A >=2020  iPad Pro in the name, target FPS = screen's refresh rate (60 or 120 FPS).
        GenericMobile, // Always target 60 FPS.
        GenericDesktop, // Target maximum possible refresh rate (-1), don't use Application.targetFrameRate
        Unknown
    }
}

