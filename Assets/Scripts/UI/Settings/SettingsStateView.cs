using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
    public class SettingsStateView : MonoBehaviour
    {
        public Toggle AutofocusToggle;
        public Toggle OccluderToggle;
        public Toggle GPSToggle;
        public Toggle WriteLogsInFileToggle;

        public Button CloseButton;

        public event System.Action OnCloseButtomClicked;
        public event System.Action<bool> OnAutofocusToggleValueChanged;
        public event System.Action<bool> OnOccluderToggleValueChanged;
        public event System.Action<bool> OnGPSToggleValueChanged;
        public event System.Action<bool> OnWriteLogsInFileToggleValueChanged;

        private void Awake()
        {
            CloseButton?.onClick.AddListener(() => OnCloseButtomClicked?.Invoke());
            AutofocusToggle.onValueChanged.AddListener((value) => OnAutofocusToggleValueChanged?.Invoke(value));
            OccluderToggle.onValueChanged.AddListener((value) => OnOccluderToggleValueChanged?.Invoke(value));
            GPSToggle.onValueChanged.AddListener((value) => OnGPSToggleValueChanged?.Invoke(value));
            WriteLogsInFileToggle.onValueChanged.AddListener((value) => OnWriteLogsInFileToggleValueChanged?.Invoke(value));
        }

        public void SetAutofocusToggleValue(bool value)
        {
            AutofocusToggle.isOn = value;
        }

        public void SetOccluderToggleValue(bool value)
        {
            OccluderToggle.isOn = value;
        }

        public void SetGPSToggleValue(bool value)
        {
            GPSToggle.isOn = value;
        }

        public void SetWriteLogsInFileToggleValue(bool value)
        {
            WriteLogsInFileToggle.isOn = value;
        }
    }
}
