using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class State : MonoBehaviour {

    public bool disableWhenHidden = true;
    [HideInInspector]
    public UIStateSwitcherManager stateSwitcher;

    private RectTransform rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    public virtual void OnStatePreShown()
    {
        if (disableWhenHidden)
           gameObject.SetActive(true);
    }

    public virtual void OnStateReady()
    {
        if (disableWhenHidden)
            gameObject.SetActive(true);
    }

    public virtual void OnStatePreHidden()
    {

    }

    public virtual void OnStateHidden()
    {
        if (disableWhenHidden)
            gameObject.SetActive(false);
    }

    public virtual void OnHideImidiately()
    {
        if (disableWhenHidden)
            gameObject.SetActive(false);
    }
}
