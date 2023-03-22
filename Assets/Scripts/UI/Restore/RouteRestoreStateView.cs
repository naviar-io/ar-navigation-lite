using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
    public class RouteRestoreStateView : MonoBehaviour
    {
        [Multiline]
        public string destinationTemplate;

		public Text destinationText;
        public Button restoreButton;
        public Button cancelButton;

        public event System.Action OnRestoreButtonPressed;
        public event System.Action OnCancelButtonPressed;

        private void Awake()
        {
            restoreButton?.onClick.AddListener(() => OnRestoreButtonPressed?.Invoke());
            cancelButton?.onClick.AddListener(() => OnCancelButtonPressed?.Invoke());
        }

        public void SetDestination(PlanPoint planPoint)
        {
            string destinationPointName = planPoint.PointID;
            destinationText.text = string.Format(destinationTemplate, destinationPointName);
        }
    }
}