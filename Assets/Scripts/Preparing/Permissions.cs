using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Android;
using System;

public enum AuthorizationStatus { DENIED = -1, NOT_DETERMINED = 0, AUTHORIZED = 1, UNKNOWN = 2 }

public static class Permissions
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int _ex_callCheckCameraPermission();
    [DllImport("__Internal")]
    private static extern void _ex_callRequestCameraPermission();
    [DllImport("__Internal")]
    private static extern int _ex_callCheckLocationPermission();
    [DllImport("__Internal")]
    private static extern void _ex_callRequestLocationPermission();
    [DllImport("__Internal")]
    private static extern void _ex_callOpenAppSettings();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
    static bool camIsNeverAskAgain = false;
    static bool gpsIsNeverAskAgain = false;
#endif

    public static AuthorizationStatus CheckCameraPermission()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return (AuthorizationStatus)_ex_callCheckCameraPermission();
#elif UNITY_ANDROID && !UNITY_EDITOR
        if (AndroidPermissionsProvider.IsPermitted(AndroidPermission.CAMERA))
            return AuthorizationStatus.AUTHORIZED;
        else if (camIsNeverAskAgain)
            return AuthorizationStatus.DENIED;
        else
            return AuthorizationStatus.NOT_DETERMINED;
#elif UNITY_EDITOR
        return AuthorizationStatus.AUTHORIZED;
#else
        return AuthorizationStatus.UNKNOWN;
#endif
    }

    public static void RequestCameraPermission()
    {
#if UNITY_IOS && !UNITY_EDITOR
        _ex_callRequestCameraPermission();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidPermissionsProvider.RequestPermission(AndroidPermission.CAMERA, null, null, () => camIsNeverAskAgain = true);
#endif
    }

    public static AuthorizationStatus CheckLocationPermission()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return (AuthorizationStatus)_ex_callCheckLocationPermission();
#elif UNITY_ANDROID && !UNITY_EDITOR
        if (AndroidPermissionsProvider.IsPermitted(AndroidPermission.ACCESS_FINE_LOCATION))
            return AuthorizationStatus.AUTHORIZED;
        else if (gpsIsNeverAskAgain)
            return AuthorizationStatus.DENIED;
        else
            return AuthorizationStatus.NOT_DETERMINED;
#elif UNITY_EDITOR
        return AuthorizationStatus.AUTHORIZED;
#else
        return AuthorizationStatus.UNKNOWN;
#endif
    }

    public static void RequestLocationPermission()
    {
#if UNITY_IOS && !UNITY_EDITOR
        Input.location.Start(0f, 0f);
        Input.location.Stop();
        _ex_callRequestLocationPermission();
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidPermissionsProvider.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION, null, null, () => gpsIsNeverAskAgain = true);
#endif
    }

    public static void OpenAppSettings()
    {
        try
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string packageName = currentActivityObject.Call<string>("getPackageName");

                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
#elif UNITY_IOS && !UNITY_EDITOR
        _ex_callOpenAppSettings();
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}

