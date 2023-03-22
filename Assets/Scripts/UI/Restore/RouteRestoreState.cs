using System.Collections;
using System.Collections.Generic;
using naviar.VPSService;
using UnityEngine;

namespace naviar.ARNavigation.UI
{
    public class RouteRestoreState : State
    {
        public RouteRestoreStateView view;

        private System.Action OnRestoreCallback;
        private System.Action OnCancelCallback;

        private void Awake()
        {
            view.OnRestoreButtonPressed += OnRestoreButtonPressed;
            view.OnCancelButtonPressed += OnCancelButtonPressed;
        }

        private void OnCancelButtonPressed()
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

        private void OnRestoreButtonPressed()
        {
            if (OnRestoreCallback == null)
            {
                AppRouter.Instance.SwitchToScan();
            }
            else
            {
                OnRestoreCallback();
            }
        }

        public void Init(PlanPoint point, System.Action OnRestoreCallback, System.Action OnCancelCallback)
        {
            view.SetDestination(point);
            this.OnRestoreCallback = OnRestoreCallback;
            this.OnCancelCallback = OnCancelCallback;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancelButtonPressed();
            }
        }
    }
}