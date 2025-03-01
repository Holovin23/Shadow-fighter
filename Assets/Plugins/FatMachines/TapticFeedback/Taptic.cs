using System.Runtime.InteropServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Taptic : MonoBehaviour
{

    private static float tapticDelay = .5f;
    private static float tapticNextTime;
    private static bool tapticReady = true;

#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _PlayTaptic(string type);
    [DllImport("__Internal")]
    private static extern void _PlayTaptic6s(string type);
#endif

    public static bool TapticOn = true;

    private static bool TapticReady
    {
        get
        {
            tapticReady = false;
            if (Time.time > tapticNextTime)
            {
                tapticReady = true;
                tapticNextTime = Time.time + tapticDelay;
            }
            return tapticReady;
        }
    }


    public static void Warning()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("warning");
        }
        else
        {
            _PlayTaptic("warning");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.Warning);
#endif
    }
    public static void Failure()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("failure");
        }
        else
        {
            _PlayTaptic("failure");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.Failure);
#endif
    }
    public static void Success()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("success");
        }
        else
        {
            _PlayTaptic("success");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.Success);
#endif
    }
    public static void Light()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("light");
        }
        else
        {
            _PlayTaptic("light");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.LightImpact);
#endif
    }
    public static void Medium()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("medium");
        }
        else
        {
            _PlayTaptic("medium");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.MediumImpact);
#endif
    }
    public static void Heavy()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("heavy");
        }
        else
        {
            _PlayTaptic("heavy");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.HeavyImpact);
#endif
    }
    public static void Default()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS || UNITY_ANDROID
        Handheld.Vibrate();
#endif
    }
    public static void Vibrate()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("medium");
        }
        else
        {
            _PlayTaptic("medium");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Vibrate();
#endif
    }
    public static void Selection()
    {
        if (!TapticOn || !TapticReady || Application.isEditor)
            return;
#if UNITY_IOS
        if (iPhone6s())
        {
            _PlayTaptic6s("selection");
        }
        else
        {
            _PlayTaptic("selection");
        }
#elif UNITY_ANDROID
        AndroidTaptic.Haptic(HapticTypes.Selection);
#endif
    }

    static bool iPhone6s()
    {
        return SystemInfo.deviceModel == "iPhone8,1" || SystemInfo.deviceModel == "iPhone8,2";
    }
}