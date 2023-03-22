using System;
using UnityEngine;

public class AndroidPermissionsProvider : MonoBehaviour
{
    const string PackageName = "com.naviar.permissions.PermissionManager";

    static Action onAllowCallback;
    static Action onDenyCallback;
    static Action onDenyAndNeverAskAgainCallback;

    public static bool IsPermitted(AndroidPermission permission)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        using (var permissionManager = new AndroidJavaClass(PackageName))
        {
            return permissionManager.CallStatic<bool>("hasPermission", GetPermissionStr(permission));
        }
#else
        return true;
#endif
    }

    public static void RequestPermission(AndroidPermission permission, Action onAllow = null, Action onDeny = null, Action onDenyAndNeverAskAgain = null)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        using (var permissionManager = new AndroidJavaClass(PackageName))
        {
            permissionManager.CallStatic("requestPermission", GetPermissionStr(permission));
            onAllowCallback = onAllow;
            onDenyCallback = onDeny;
            onDenyAndNeverAskAgainCallback = onDenyAndNeverAskAgain;
        }
#else
        Debug.LogWarning("AndroidPermissionsProvider works only Androud Devices.");
#endif
    }

    private static string GetPermissionStr(AndroidPermission permission)
    {
        return "android.permission." + permission.ToString();
    }

    private void OnAllow()
    {
        if (onAllowCallback != null)
        {
            onAllowCallback();
        }
        ResetAllCallBacks();
    }

    private void OnDeny()
    {
        if (onDenyCallback != null)
        {
            onDenyCallback();
        }
        ResetAllCallBacks();
    }

    private void OnDenyAndNeverAskAgain()
    {
        if (onDenyAndNeverAskAgainCallback != null)
        {
            onDenyAndNeverAskAgainCallback();
        }
        ResetAllCallBacks();
    }

    private void ResetAllCallBacks(){
        onAllowCallback = null;
        onDenyCallback = null;
        onDenyAndNeverAskAgainCallback = null;
    }
}
public enum AndroidPermission
{
    ACCESS_FINE_LOCATION,
    CAMERA
}
