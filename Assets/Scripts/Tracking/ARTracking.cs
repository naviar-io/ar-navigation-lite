using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

// Track user position and rotation using AR Foundation and check hiding the application
public class ARTracking : MonoBehaviour
{
    public ARSessionOrigin sessionOrigin;
    public KeyCode ResetKeyCode;

    public event System.Action OnTrackingLost;

    public Vector3 GetPlayerPosition()
    {
        return sessionOrigin.camera.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(ResetKeyCode))
            OnTrackingLost?.Invoke();
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause || Application.isEditor)
            return;

        OnTrackingLost?.Invoke();
    }
}