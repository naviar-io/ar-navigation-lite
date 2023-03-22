package com.naviar.permissions;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import android.util.Log;

public class OverrideUnityActivity extends UnityPlayerActivity {
    public OverrideUnityActivity() {
    }

    public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        switch(requestCode) {
            case 0:
                if (grantResults[0] == 0) {
                    this.SendRequestResultToUnity("OnAllow");
                } else 
					if (this.shouldShowRequestPermissionRationale(permissions[0])) {
						this.SendRequestResultToUnity("OnDeny");
					} else {
						this.SendRequestResultToUnity("OnDenyAndNeverAskAgain");
					}
            default:
        }
    }

    private void SendRequestResultToUnity(String result) {
        UnityPlayer.UnitySendMessage("AndroidPermissionsProvider", result, "");
    }
}
