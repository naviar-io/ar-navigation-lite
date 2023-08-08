using System.Collections;
using System.Collections.Generic;
using naviar.VPSService;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace naviar.ARNavigation.UI
{
    public class SettingsState : MonoBehaviour
    {
        public SettingsStateView view;
        public GameObject UserCanvas;

        public GameObject Root;
        public ARCameraManager cameraManager;
        public GameObject OccluderModel;
        public VPSLocalisationService VPS;

        public float PressTime = 2f;
        private float mouseDeltaTime = 0;
        bool isDevOn = false;

        private void Awake()
        {
            view.OnCloseButtomClicked += OnCloseButtomClicked;
            view.OnAutofocusToggleValueChanged += OnAutofocusToggleValueChanged;
            view.OnOccluderToggleValueChanged += OnOccluderToggleValueChanged;
            view.OnGPSToggleValueChanged += OnGPSToggleValueChanged;
            view.OnWriteLogsInFileToggleValueChanged += OnWriteLogsInFileToggleValueChanged;
        }

        private void Start()
        {
            view.SetAutofocusToggleValue(cameraManager.autoFocusRequested);
            view.SetOccluderToggleValue(Application.isEditor);
            OccluderModel.SetActive(Application.isEditor);
            view.SetGPSToggleValue(VPS.SendGPS);
            view.SetWriteLogsInFileToggleValue(VPSLogger.WriteLogsInFile);
        }

        public void OnCloseButtomClicked()
        {
            Root.gameObject.SetActive(false);
            UserCanvas.SetActive(true);
        }

        public void OnAutofocusToggleValueChanged(bool value)
        {
            cameraManager.autoFocusRequested = value;
        }

        public void OnOccluderToggleValueChanged(bool value)
        {
            OccluderModel.SetActive(value);
        }

        public void OnGPSToggleValueChanged(bool value)
        {
            VPS.SendGPS = value;
        }

        public void OnWriteLogsInFileToggleValueChanged(bool value)
        {
            VPSLogger.WriteLogsInFile = value;
        }

        public void OnActivateButtonPressed()
        {
            isDevOn = !Root.activeInHierarchy;
            UpdateUI(isDevOn);
            Handheld.Vibrate();
        }

        private void UpdateUI(bool isDevOn)
        {
            Root.gameObject.SetActive(isDevOn);
            UserCanvas.SetActive(!isDevOn);
        }
    }
}