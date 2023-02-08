using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace naviar.ARNavigation.UI
{
    public class ArrivalState : State
    {
		public ArrivalStateView view;

        private System.Action OnOkCallback;

        public void Init(PlanPoint point, System.Action OnOkCallback)
        {
            view.SetDestination(point.PointID);
            this.OnOkCallback = OnOkCallback;
        }

        private void Awake()
        {
            view.OnOkButtonClicked += OnOkButtonClicked;
        }

        private void OnOkButtonClicked()
        {
            if (OnOkCallback == null)
            {
                AppRouter.Instance.SwitchToChoosePoint();
            }
            else
            {
                OnOkCallback();
            }

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnOkButtonClicked();
            }
        }
    }
}