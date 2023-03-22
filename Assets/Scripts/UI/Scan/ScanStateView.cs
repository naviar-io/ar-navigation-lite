using System.Collections;
using System.Collections.Generic;
using naviar.VPSService;
using UnityEngine;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
    public class ScanStateView : MonoBehaviour
    {
        public Button CancelButton;
        public Animator animController;

        public event System.Action OnCancelPressed;

        public float sucsessTimeout = 1f;

        private void Awake()
        {
            CancelButton?.onClick.AddListener(() => OnCancelPressed?.Invoke());
        }

        public void ShowSuccessState(System.Action completeCallback)
        {
            StartCoroutine(ShowAloneSuccessRoutine(completeCallback));
        }

        private IEnumerator ShowAloneSuccessRoutine(System.Action completeCallback)
        {
            animController.SetTrigger("Success");
            yield return new WaitForSeconds(sucsessTimeout);
            completeCallback();
        }
    }
}