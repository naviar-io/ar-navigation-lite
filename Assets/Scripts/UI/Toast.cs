using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace naviar.ARNavigation.UI
{
    public class Toast : MonoBehaviour
    {
        private static Toast _instance;

        public static Toast Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        
        private bool Active = false;
        public float Timeout;

        public GameObject MessageBox;
        public Text text;

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            MessageBox.SetActive(false);
        }

        public void Show(string mes)
        {
            if (Active)
            {
                StopAllCoroutines();
                MessageBox.SetActive(false);
            }

            MessageBox.SetActive(true);

            Active = true;
            text.text = mes;
            animator.SetTrigger("On");

            StartCoroutine(WaitToHide());
        }

        IEnumerator WaitToHide()
        {
            yield return new WaitForSeconds(Timeout);
            animator.SetTrigger("Off");
            Active = false;
        }
    }
}
