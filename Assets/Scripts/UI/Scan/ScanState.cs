using System;
using System.Collections;
using System.Collections.Generic;
using naviar.VPSService;
using UnityEngine;

namespace naviar.ARNavigation.UI
{
    public class ScanState : State
    {
        public ScanStateView view;

        private System.Action OnLocalizeCallback;
        private System.Action OnCancelCallback;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancelClicked();
            }
        }

        private void Awake()
		{
            view.OnCancelPressed += OnCancelClicked;
        }

        private void OnCancelClicked()
		{
            if (OnCancelCallback == null)
            {
                AppRouter.Instance.SwitchToChoosePoint();
            }
            else
            {
                OnCancelCallback();
            }
        }

        public override void OnStatePreHidden()
        {
            base.OnStatePreHidden();
            ARNavigationSession.Instance.OnVpsRequestSuccess -= OnLocalized;
        }

        private void OnLocalized()
        {
            view.ShowSuccessState(SwitchState);
        }

        public void Init(System.Action OnLocalizeCallback, System.Action OnCancelCallback)
        {
            ARNavigationSession.Instance.OnVpsRequestSuccess += OnLocalized;
            // uncomment if you want to react to vps fails
            // AppRouter.Instance.OnVpsRequestFailed += OnFail;

            this.OnLocalizeCallback = OnLocalizeCallback;
            this.OnCancelCallback = OnCancelCallback;
    }

        private void SwitchState()
        {
            if (OnLocalizeCallback == null)
            {
                AppRouter.Instance.SwitchToWalk();
            }
            else
            {
                OnLocalizeCallback();
            }
        }
    }
}
