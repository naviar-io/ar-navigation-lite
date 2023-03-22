using System;
using System.Collections;
using System.Collections.Generic;
using naviar.VPSService;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

namespace ARVRLab.ARNavigation.MockupV3
{
    public class PermissiveState : State
    {
        public PermissiveStateView view;

        private System.Action OnContinueCallback;

        public void Init(System.Action OnContinueCallback = null)
        {
            this.OnContinueCallback = OnContinueCallback;
        }

        private void Awake()
        {
            view.OnContinuePressed += OnContinueClicked;
            view.OnTryCameraEnable += TryCameraEnable;
            view.OnTryGPSEnable += TryGPSEnable;
        }

        public override void OnStatePreShown()
        {
            base.OnStatePreShown();
            UpdateTogglesValues();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                UpdateTogglesValues();
            }
        }

        private void UpdateTogglesValues()
        {
            bool cameraEnable = Permissions.CheckCameraPermission() == AuthorizationStatus.AUTHORIZED;
            view.SetCameraToggleValue(cameraEnable);
            bool gpsEnable = Permissions.CheckLocationPermission() == AuthorizationStatus.AUTHORIZED;
            view.SetGPSToggleValue(gpsEnable);

            view.SetContinueButtonEnable(cameraEnable && gpsEnable);
        }

        private void OnContinueClicked()
        {
            if (OnContinueCallback == null)
            {
                PermissionAppRouter.Instance.SwitchToLoading();
            }
            else
            {
                OnContinueCallback();
            }
        }

        private void TryCameraEnable()
        {
            switch(Permissions.CheckCameraPermission())
            {
                case AuthorizationStatus.NOT_DETERMINED:
                    Permissions.RequestCameraPermission();
                    break;
                case AuthorizationStatus.DENIED:
                    Permissions.OpenAppSettings();
                    break;
                default:
                    break;
            }
        }

        private void TryGPSEnable()
        {
            switch (Permissions.CheckLocationPermission())
            {
                case AuthorizationStatus.NOT_DETERMINED:
                    Permissions.RequestLocationPermission();
                    break;
                case AuthorizationStatus.DENIED:
                    Permissions.OpenAppSettings();
                    break;
                default:
                    break;
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
