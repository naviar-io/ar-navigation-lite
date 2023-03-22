using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVRLab.ARNavigation.MockupV3
{
    public class NotArSupportState : State
    {
        public NotArSupportStateView view;

        private System.Action OnCloseCallback;

        public void Init(System.Action OnCloseCallback = null)
        {
            this.OnCloseCallback = OnCloseCallback;
        }

        private void Awake()
        {
            view.OnCloseButtonPressed += OnCloseButtonClicked;
        }

        private void OnCloseButtonClicked()
        {
#if UNITY_EDITOR
            if (OnCloseCallback == null)
            {
                AppRouter.Instance.SwitchToChoosePoint();
            }
            else
            {
                OnCloseCallback();
            }
#else
            if (OnCloseCallback == null)
            {
                PermissionAppRouter.Instance.QuitApplication();
            }
            else
            {
                OnCloseCallback();
            }
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCloseButtonClicked();
            }
        }
    }
}
