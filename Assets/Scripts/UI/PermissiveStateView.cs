using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARVRLab.ARNavigation.MockupV3
{
    public class PermissiveStateView : MonoBehaviour
    {
        public Toggle CameraToggle;
        public Toggle GPSToggle;
        public Button ContinueButton;

        public Image CameraToggleBackground;
        public Image GPSToggleBackground;

        public Texture GrayButton;
        public Texture BlueButton;

        public event System.Action OnContinuePressed;
        public event System.Action OnTryCameraEnable;
        public event System.Action OnTryGPSEnable;

        private void Awake()
        {
            ContinueButton?.onClick.AddListener(() => OnContinuePressed?.Invoke());

            CameraToggle?.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    CameraToggleBackground.enabled = !value;
                    OnTryCameraEnable?.Invoke();
                }
            });

            GPSToggle?.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    GPSToggleBackground.enabled = !value;
                    OnTryGPSEnable?.Invoke();
                }
            });
        }

        public void SetCameraToggleValue(bool value)
        {
            CameraToggle.isOn = value;
            CameraToggle.interactable = !value;
            CameraToggleBackground.enabled = !value;
        }

        public void SetGPSToggleValue(bool value)
        {
            GPSToggle.isOn = value;
            GPSToggle.interactable = !value;
            GPSToggleBackground.enabled = !value;
        }

        public void SetContinueButtonEnable(bool value)
        {
            ContinueButton.interactable = value;
            if (value)
            {
                ContinueButton.image.sprite = Sprite.Create((Texture2D)BlueButton, new Rect(0, 0, BlueButton.width, BlueButton.height), Vector2.zero);
            }
            else
            {
                ContinueButton.image.sprite = Sprite.Create((Texture2D)GrayButton, new Rect(0, 0, GrayButton.width, GrayButton.height), Vector2.zero);
            }
        }
    }
}
