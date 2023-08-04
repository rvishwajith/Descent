/* Components -> Performance -> PerformanceController
 * 
 * Class Description:
 * Dynamically chooses performance settings at runtime based on the device types, which are
 * determined by the following (from UnityEngine -> SystemInfo/Application):
 * 1. If the device is a mobile device.
 * 2. The device's model name.
 * 3. The device's refresh rate.
 * A target frame rate is set based on the device profile. Graphics settings, such as spawn counts
 * for flocks that require compute shaders, may be added in the future.
 */

using UnityEngine;

namespace Components.Performance
{
    public class PerformanceController : MonoBehaviour
    {
        private void Awake() { ApplyPerformanceSettings(); }

        private void ApplyPerformanceSettings()
        {
            /*
            Debug.Log("PerformanceController.ApplyPerformanceSettings() Runtime Info:" +
                "\n\tDevice Model Name: " + GetDeviceModelName() +
                "\n\tPerformance Profile: " + GetDeviceProfile() +
                "\n\tRefresh Rate: " + GetDeviceRefreshRate());
            */

            var deviceProfile = GetDeviceProfile();
            if (deviceProfile == DeviceProfile.GenericMobile)
            {
                Application.targetFrameRate = 60;
                // Debug.Log("PerformanceController.Awake(): Mobile device, set target FPS to 60.");
            }
            else if (deviceProfile == DeviceProfile.iPhone || deviceProfile == DeviceProfile.iPad)
            {
                Application.targetFrameRate = 60;
                // Debug.Log("PerformanceController.Awake(): Generic iOS/iPad, set target FPS to 60.");
            }
            else if (deviceProfile == DeviceProfile.iPadPro)
            {
                Application.targetFrameRate = (int)GetDeviceRefreshRate();
                // Debug.Log("PerformanceController.Awake(): Generic iOS/iPad, set target FPS to " + (int)GetDeviceRefreshRate());
            }
            else if (deviceProfile == DeviceProfile.GenericDesktop)
            {
                // Debug.Log("PerformanceController.Awake(): Generic desktop, did not set target FPS.");
            }
        }

        public static DeviceProfile GetDeviceProfile()
        {
            var deviceModelName = GetDeviceModelName();

            if (Application.isMobilePlatform)
            {
                if (deviceModelName.Contains("IPHONE"))
                {
                    return DeviceProfile.iPhone;
                }
                else if (deviceModelName.Contains("IPAD"))
                {
                    if (deviceModelName.Contains("PRO") && deviceModelName.Contains("GEN"))
                    {
                        return DeviceProfile.iPadPro;
                    }
                    else
                    {
                        return DeviceProfile.iPad;
                    }
                }
                else
                {
                    return DeviceProfile.GenericMobile;
                }
            }
            return DeviceProfile.GenericDesktop;
        }

        public static string GetDeviceModelName()
        {
            return SystemInfo.deviceModel.Trim().Replace("\n", "").Replace(" ", "").ToUpper();
        }

        public static float GetDeviceRefreshRate()
        {
            return (float)Screen.currentResolution.refreshRateRatio.value;
        }
    }
}