using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using naviar.VPSService;

namespace naviar.ARNavigation.UI
{
    public class WalkToState : State
    {
        public WalkToStateView view;

        private System.Action OnCancelCallback;
        private System.Action OnPathFindingFailCallback;
        private System.Action OnPointReachedCallback;

        public void Init(string pointId, System.Action OnCancelCallback, System.Action OnPathFindingFailCallback, System.Action OnPointReachedCallback)
        {
            view.SetDestinationName(pointId);

            ARNavigationSession.Instance.OnPointReached += OnPointReachedHandler;
            ARNavigationSession.Instance.OnPathFindingFailed += OnPathfindingFailedHandler;
            ARNavigationSession.Instance.OnProgressUpdated += OnProgressUpdated;

            this.OnCancelCallback = OnCancelCallback;
            this.OnPathFindingFailCallback = OnPathFindingFailCallback;
            this.OnPointReachedCallback = OnPointReachedCallback;
        }

        public override void OnStatePreHidden()
        {
            base.OnStatePreHidden();
            ARNavigationSession.Instance.OnPointReached -= OnPointReachedHandler;
            ARNavigationSession.Instance.OnPathFindingFailed -= OnPathfindingFailedHandler;
            ARNavigationSession.Instance.OnProgressUpdated -= OnProgressUpdated;
        }

        private void Awake()
        {
            view.OnCancelButtonPressed += OnCancelButtonClicked;
        }

        private void OnCancelButtonClicked()
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancelButtonClicked();
            }
        }

        private void OnPointReachedHandler(PlanPoint destinationPoint)
        {
            if (OnPointReachedCallback == null)
            {
                AppRouter.Instance.SwitchToArrival();
            }
            else
            {
                OnPointReachedCallback();
            }
        }

        private void OnPathfindingFailedHandler()
        {
            if (OnPathFindingFailCallback == null)
            {
                AppRouter.Instance.SwitchToScan();
            }
            else
            {
                OnPathFindingFailCallback();
            }
        }

        private void OnProgressUpdated(float progress, float distance)
        {
            view.SetProgress(progress);
            view.SetRemainDistance(distance);
        }
    }
}