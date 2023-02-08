package com.naviar.permissions;

import android.content.Context;
import android.os.Build.VERSION;
import com.unity3d.player.UnityPlayer;

public class PermissionManager {
    public PermissionManager() {
    }

    public static void requestPermission(String permissionStr) {
        if (!hasPermission(permissionStr)) {
            UnityPlayer.currentActivity.requestPermissions(new String[]{permissionStr}, 0);
        }

    }

    public static boolean hasPermission(String permissionStr) {
        if (VERSION.SDK_INT < 23) {
            return true;
        } else {
            Context context = UnityPlayer.currentActivity.getApplicationContext();
            return context.checkCallingOrSelfPermission(permissionStr) == 0;
        }
    }
}