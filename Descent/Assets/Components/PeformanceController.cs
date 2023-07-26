using UnityEngine;

/* 
 * Components > PerformanceController
 */
namespace Components
{
    public class PerformanceController : MonoBehaviour
    {
        void Awake()
        {
            var deviceType = GetDeviceType();
            var deviceModel = SystemInfo.deviceModel;
            Debug.Log("PerformanceController.Awake(): Device info:\nName: " + deviceModel + "\nType: " + deviceType);

            if (deviceType == DeviceType.Mobile)
            {
                Application.targetFrameRate = 60;
                Debug.Log("PerformanceController.Awake(): Mobile device detected, target frame rate set to 60.");
            }
        }

        private DeviceType GetDeviceType()
        {
            if (Application.isMobilePlatform)
                return DeviceType.Mobile;
            else
                return DeviceType.Desktop;
        }

        private int GetTargetFrameRate(DeviceType deviceType)
        {
            if (deviceType == DeviceType.Mobile || deviceType == DeviceType.iPhone)
                return 60;
            else if (deviceType == DeviceType.iPad)
                return 60;
            else if (deviceType == DeviceType.iPadPro)
                return 60;
            else if (deviceType == DeviceType.Desktop)
                return -1;

            Debug.Log("PerformanceController.GetTargetFrameRate() ERROR: Device type is unknown.");
            return -1;
        }
    }

    public enum DeviceType
    {
        iPhone, // Always target 60 FPS.
        iPad, // A regular iPad or an older iPad Pro, target 60 FPS.
        iPadPro, // A >=2020  iPad Pro in the name, target FPS = screen's refresh rate (60 or 120 FPS).
        Mobile, // Always target 60 FPS.
        Desktop // Target maximum possible refresh rate (-1), don't use Application.targetFrameRate
    }
}