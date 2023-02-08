using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
    public class WalkToStateView : MonoBehaviour
    {
        public Text destinationText;
        public Button cancelButton;

        public PathProgressView pathProgress;
        public Text RemainDistanceText;

        public event System.Action OnCancelButtonPressed;

        private void Awake()
        {
            cancelButton?.onClick.AddListener(() => OnCancelButtonPressed?.Invoke());
        }

        public void SetDestinationName(string pointId)
        {
            destinationText.text = pointId;
        }

        public void SetProgress(float percent)
        {
            pathProgress.SetProgress(percent);
        }

        public void SetRemainDistance(float distance)
        {
            RemainDistanceText.text = ((int)distance).ToString() + " m";
        }
    }
}