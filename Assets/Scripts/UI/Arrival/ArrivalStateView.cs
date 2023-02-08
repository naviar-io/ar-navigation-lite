using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
    public class ArrivalStateView : MonoBehaviour
    {
        public event System.Action OnOkButtonClicked;

        public Button okButton;
        public Text destinationText;

        private void Awake()
        {
            okButton?.onClick.AddListener(() => OnOkButtonClicked?.Invoke());
        }

        public void SetDestination(string pointId)
        {
            destinationText.text = pointId;
        }
    }
}