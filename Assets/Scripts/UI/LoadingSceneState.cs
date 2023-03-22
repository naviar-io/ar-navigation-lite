using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace ARVRLab.ARNavigation.MockupV3
{
    public class LoadingSceneState : State
    {
        private System.Action OnNotARSupportCallback;
        private System.Action OnNotPermissionsCallback;
        private System.Action OnSuccessCallback; 

        public void Init(System.Action OnNotARSupportCallback = null, System.Action OnNotPermissionsCallback = null,
            System.Action OnSuccessCallback = null)
        {
            this.OnNotARSupportCallback = OnNotARSupportCallback;
            this.OnNotPermissionsCallback = OnNotPermissionsCallback;
            this.OnSuccessCallback = OnSuccessCallback;
        }

        private void OnEnable()
        {
            StartCoroutine(StartChecking());
        }

        private IEnumerator StartChecking()
        {
            yield return null;
            while (true)
            {
                if ((ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.CheckingAvailability))
                {
                    yield return ARSession.CheckAvailability();
                    continue;
                }

                if (ARSession.state == ARSessionState.NeedsInstall)
                {
                    yield return ARSession.Install();
                    continue;
                }

                if (ARSession.state == ARSessionState.Unsupported || ARSession.state == ARSessionState.NeedsInstall)
                {
                    if (OnNotARSupportCallback == null)
                    {
                        PermissionAppRouter.Instance.SwitchToNotARSupport();
                    }
                    else
                    {
                        OnNotARSupportCallback();
                    }
                    yield break;
                }

                if (Permissions.CheckCameraPermission() != AuthorizationStatus.AUTHORIZED || Permissions.CheckLocationPermission() != AuthorizationStatus.AUTHORIZED)
                {
                    if (OnNotPermissionsCallback == null)
                    {
                        PermissionAppRouter.Instance.SwitchToPermissive();
                    }
                    else
                    {
                        OnNotPermissionsCallback();
                    }
                    yield break;
                }
                else
                {
                    yield return null;
                    if (OnSuccessCallback == null)
                    {
                        AppRouter.Instance.SwitchToChoosePoint();
                    }
                    else
                    {
                        OnSuccessCallback();
                    }
                    yield break;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PermissionAppRouter.Instance.QuitApplication();
            }
        }
    }
}



