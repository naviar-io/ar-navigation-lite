using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARVRLab.ARNavigation.MockupV3
{
    public class NotArSupportStateView : MonoBehaviour
    {
        public Button CloseButton;
        public event System.Action OnCloseButtonPressed;

        private void Awake()
        {
            if (CloseButton != null)
            {
                CloseButton.onClick.AddListener(() =>
                {
                    OnCloseButtonPressed?.Invoke();
                });
            }
        }
    }
}
