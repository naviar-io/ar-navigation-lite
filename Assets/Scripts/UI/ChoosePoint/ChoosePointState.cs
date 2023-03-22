using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace naviar.ARNavigation.UI
{
	public class ChoosePointState : State
	{
        public ChoosePointStateView view;
        public static event System.Action<string> OnButtonEventInvoked;

        private System.Action OnChoosePointCallback;
        private System.Action OnCancelCallback;

        private void Awake()
		{
            view.OnChoosePoint += OnChoosePoint;
        }

        public void Init(List<PlanPoint> pointIds, System.Action OnChoosePointCallback, System.Action OnCancelCallback)
        {
            view.CreatePointsUI(pointIds);
            this.OnChoosePointCallback = OnChoosePointCallback;
            this.OnCancelCallback = OnCancelCallback;
        }

        private void OnChoosePoint(string pointId)
        {
            AppModel.Instance.SetTargetPoint(pointId);
            if (OnChoosePointCallback == null)
            {
                AppRouter.Instance.SwitchToScan();
            }
            else
            {
                OnChoosePointCallback();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (OnCancelCallback == null)
                {
                    AppRouter.Instance.QuitApplication();
                }
                else
                {
                    OnCancelCallback();
                }
            }
        }
    }
}
